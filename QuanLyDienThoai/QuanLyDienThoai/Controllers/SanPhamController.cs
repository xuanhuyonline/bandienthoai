using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyDienThoai.Models;
using PagedList;
using System.Web.Security;

namespace QuanLyDienThoai.Controllers
{
    public class SanPhamController : Controller
    {
        QLDTDataContext db = new QLDTDataContext();
        //
        // GET: /SanPham/
        public ActionResult ShowListSanPham(int? page)
        {
            var listSP = db.ChiTietThietBis.Where(s=> s.ThietBi.IsDelete == false).ToList();
            ViewBag.listSP = listSP;
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(listSP.OrderByDescending(s => s.MaTB).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ListSanPhamBanChayNhat()
        {
            var listSP = db.ChiTietThietBis.Where(s=> s.ThietBi.IsBestSeller == true && s.ThietBi.IsDelete == false).ToList();
            var a = listSP;
            return View(listSP);
        }

        public ActionResult SanPhamTheoLoai(int maLoai, int? page)
        {
            var listSP = db.ChiTietThietBis.Where(d => d.ThietBi.MaLoai == maLoai && d.ThietBi.IsDelete == false).ToList();
            var LoaiSP = db.ChiTietThietBis.Where(d => d.ThietBi.MaLoai == maLoai && d.ThietBi.IsDelete == false).FirstOrDefault();
            if(LoaiSP != null)
            {
                ViewBag.LoaiSP = LoaiSP.ThietBi.LoaiThietBi.TenLoai.ToString();
            }
            else
            {
                ViewBag.LoaiSP = "";
            }
            ViewBag.listSP = listSP;
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(listSP.OrderByDescending(s => s.MaTB).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult SanPhamPartial()
        {
            return PartialView();
        }
        public ActionResult XemChiTiet(int? maSP)
        {
            if (maSP == null)
                return HttpNotFound();
            ChiTietThietBi ct = db.ChiTietThietBis.SingleOrDefault(n => n.MaTB == maSP);
            return View(ct);
        }
	}
}