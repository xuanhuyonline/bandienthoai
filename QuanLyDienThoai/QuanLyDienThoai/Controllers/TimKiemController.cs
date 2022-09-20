using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyDienThoai.Models;
using PagedList;

namespace QuanLyDienThoai.Controllers
{
    public class TimKiemController : Controller
    {
        QLDTDataContext db = new QLDTDataContext();

        [HttpPost]
        public ActionResult KQTimKiemTen(string sTuKhoa, int? page)
        {
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            var listSP = db.ChiTietThietBis.Where(n => n.ThietBi.TenTB.Contains(sTuKhoa));
            ViewBag.listSPTK = listSP;
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(listSP.OrderByDescending(s => s.MaTB).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult KQTimKiemGia(int GiaTien1, int GiaTien2, int? page)
        {
            var listSP = db.ChiTietThietBis.Where(n => n.GiaTien >= GiaTien1 && n.GiaTien <= GiaTien2);
            ViewBag.listSPTKG = listSP;
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(listSP.OrderByDescending(s => s.MaTB).ToPagedList(pageNumber, pageSize));
        }
	}
}