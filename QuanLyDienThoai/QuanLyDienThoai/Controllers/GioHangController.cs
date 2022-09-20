using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyDienThoai.Models;

namespace QuanLyDienThoai.Controllers
{
    public class GioHangController : Controller
    {
        QLDTDataContext db = new QLDTDataContext();
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int ms, string strURL)
        {
            List<GioHang> lstGioHang = LayGioHang();
            ChiTietThietBi hang = db.ChiTietThietBis.FirstOrDefault(s => s.MaTB == ms);
            GioHang SanPham = lstGioHang.Find(sp => sp.iMaSP == ms);
            if(hang.SoLuong > 0)
            {
                if (SanPham == null)
                {
                    SanPham = new GioHang(ms);
                    lstGioHang.Add(SanPham);
                    return Redirect(strURL);
                }
                else
                {
                    if (ktDuSL(SanPham.iMaSP))
                    {
                        SanPham.iSoLuong++;
                    }
                    setAlert("Sản phẩm không đủ số lượng", "error");
                    return Redirect(strURL);
                }
            }
            setAlert("Sản phẩm không đủ số lượng", "error");
            return Redirect(strURL);
        }

        public ActionResult BotGioHang(int ms, string strURL)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang SanPham = lstGioHang.Find(sp => sp.iMaSP == ms);
            if (SanPham.iSoLuong == 1)
            {
                lstGioHang.Remove(SanPham);
                if(lstGioHang.Count == 0)
                    return RedirectToAction("Index");
                return Redirect(strURL);
            }
            else
            {
                SanPham.iSoLuong--;
                return Redirect(strURL);
            }
        }

        public bool ktDuSL(int maSP)
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            GioHang gh = lstGioHang.Find(n => n.iMaSP == maSP);
            if (gh.iSoLuong >= gh.iTongSL)
                return false;
            return true;
        }

        private int TongSoLuong()
        {
            int tsl = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                tsl += lstGioHang.Sum(sp => sp.iSoLuong);
            }
            return tsl;
        }

        private double TongThanhTien()
        {
            double ttt = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                ttt += lstGioHang.Sum(sp => sp.dThanhTien);
            }
            return ttt;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
                return RedirectToAction("Index");
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TongThanhTien();

            return View(lstGioHang);
        }

        public ActionResult XoaGioHang(int maSP)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang gh = lstGioHang.FirstOrDefault(n => n.iMaSP == maSP);
            if (gh != null)
                lstGioHang.Remove(gh);
            return RedirectToAction("GioHang");
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            return PartialView();
        }

        public void setAlert(string message, string type)
        {
            TempData["message"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }

	}
}