using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyDienThoai.Models;
using PagedList;
using System.IO;

namespace QuanLyDienThoai.Controllers
{
    [Authorize(Roles = "QuanLySanPham")]
    public class AdminController : Controller
    {
        QLDTDataContext db = new QLDTDataContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TaoMoiLoaiThietBi()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TaoMoiLoaiThietBi(LoaiThietBi ltb)
        {
            db.LoaiThietBis.InsertOnSubmit(ltb);
            db.SubmitChanges();
            setAlert("Thêm loại thiết bị thành công", "success");
            return View();
        }

        [HttpGet]
        public ActionResult TaoMoiThietBi()
        {
            ViewBag.MaNCC = new SelectList(db.NhaCupCungs.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoai = new SelectList(db.LoaiThietBis.OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            return View();
        }

        [HttpPost]
        public ActionResult TaoMoiThietBi(ThietBi tb, ChiTietThietBi ct, HttpPostedFileBase HinhAnh1, HttpPostedFileBase HinhAnh2, HttpPostedFileBase HinhAnh3)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCupCungs.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoai = new SelectList(db.LoaiThietBis.OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            if (HinhAnh1.ContentLength > 0)
            {
                var fileName = Path.GetFileName(HinhAnh1.FileName);
                var path = Path.Combine(Server.MapPath("~/Assests/HinhSP"), fileName);
                if (System.IO.File.Exists(path))
                {
                    ViewBag.Upload1 = "Hình ảnh đã tồn tại!";
                    return View();
                }
                else
                {
                    HinhAnh1.SaveAs(path);
                    tb.HinhAnh1 = fileName;
                }
            }
            if (HinhAnh2.ContentLength > 0)
            {
                var fileName = Path.GetFileName(HinhAnh2.FileName);
                var path = Path.Combine(Server.MapPath("~/Assests/HinhSP"), fileName);
                if (System.IO.File.Exists(path))
                {
                    ViewBag.Upload2 = "Hình ảnh đã tồn tại!";
                    return View();
                }
                else
                {
                    HinhAnh2.SaveAs(path);
                    tb.HinhAnh2 = fileName;
                }
            }
            if (HinhAnh3.ContentLength > 0)
            {
                var fileName = Path.GetFileName(HinhAnh3.FileName);
                var path = Path.Combine(Server.MapPath("~/Assests/HinhSP"), fileName);
                if (System.IO.File.Exists(path))
                {
                    ViewBag.Upload3 = "Hình ảnh đã tồn tại!";
                    return View();
                }
                else
                {
                    HinhAnh3.SaveAs(path);
                    tb.HinhAnh3 = fileName;
                }
            }
            db.ThietBis.InsertOnSubmit(tb);
            db.SubmitChanges();

            ct.MaTB = tb.MaTB;
            db.ChiTietThietBis.InsertOnSubmit(ct);
            db.SubmitChanges();
            setAlert("Thêm điện thoại thành công", "success");

            return View();
        }

        [HttpGet]
        public ActionResult TaoThanhToan()
        {
            ViewBag.MaHD = new SelectList(db.HoaDons.Where(s => s.IsThanhToan == false).OrderBy(n => n.MaHD), "MaHD", "MaHD");
            return View();
        }

        [HttpPost]
        public ActionResult TaoThanhToan(ThanhToan tt)
        {
            ViewBag.MaHD = new SelectList(db.HoaDons.Where(s => s.IsThanhToan == false).OrderBy(n => n.MaHD), "MaHD", "MaHD");

            HoaDon hd = db.HoaDons.FirstOrDefault(s => s.MaHD == tt.MaHD);

            tt.SoTienThanhToan = (decimal)hd.TongTien;
            db.ThanhToans.InsertOnSubmit(tt);
            hd.IsThanhToan = true;
            db.SubmitChanges();
            setAlert("Thêm thanh toán thành công", "success");
            return View();
        }

        public ActionResult XoaThietBi(int? maTB)
        {
            ThietBi tb = db.ThietBis.FirstOrDefault(s => s.MaTB == maTB);
            if (tb == null)
                return HttpNotFound();
            tb.IsDelete = true;
            db.SubmitChanges();
            setAlert("Xóa thành công", "success");
            return RedirectToAction("QuanLyLoaiThietBi");
        }

        [HttpGet]
        public ActionResult SuaThietBi(int? maTB)
        {
            ChiTietThietBi ct = db.ChiTietThietBis.FirstOrDefault(s => s.MaTB == maTB);
            if (ct == null)
                return HttpNotFound();
            ViewBag.MaNCC = new SelectList(db.NhaCupCungs.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", ct.ThietBi.MaNCC);
            ViewBag.MaLoai = new SelectList(db.LoaiThietBis.OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", ct.ThietBi.MaLoai);
            return View(ct);
        }

        [HttpPost]
        public ActionResult SuaThietBi(ThietBi tb, ChiTietThietBi ct, HttpPostedFileBase HinhAnh1, HttpPostedFileBase HinhAnh2, HttpPostedFileBase HinhAnh3)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCupCungs.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", tb.MaNCC);
            ViewBag.MaLoai = new SelectList(db.LoaiThietBis.OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", tb.MaLoai);
            if (HinhAnh1 != null)
            {
                if (HinhAnh1.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(HinhAnh1.FileName);
                    var path = Path.Combine(Server.MapPath("~/Assests/HinhSP"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Upload1 = "Hình ảnh đã tồn tại!";
                        return View();
                    }
                    else
                    {
                        HinhAnh1.SaveAs(path);
                        tb.HinhAnh1 = fileName;
                    }
                }
            }
            if (HinhAnh2 != null)
            {
                if (HinhAnh2.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(HinhAnh2.FileName);
                    var path = Path.Combine(Server.MapPath("~/Assests/HinhSP"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Upload2 = "Hình ảnh đã tồn tại!";
                        return View();
                    }
                    else
                    {
                        HinhAnh2.SaveAs(path);
                        tb.HinhAnh2 = fileName;
                    }
                }
            }
            if (HinhAnh3 != null)
            {
                if (HinhAnh3.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(HinhAnh3.FileName);
                    var path = Path.Combine(Server.MapPath("~/Assests/HinhSP"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Upload3 = "Hình ảnh đã tồn tại!";
                        return View();
                    }
                    else
                    {
                        HinhAnh3.SaveAs(path);
                        tb.HinhAnh3 = fileName;
                    }
                }
            }
            ThietBi tb1 = db.ThietBis.FirstOrDefault(s => s.MaTB == tb.MaTB);
            ChiTietThietBi ct1 = db.ChiTietThietBis.FirstOrDefault(s => s.MaTB == tb.MaTB);
            tb1.TenTB = tb.TenTB;
            tb1.MaLoai = tb.MaLoai;
            tb1.MaNCC = tb.MaNCC;
            if (tb.HinhAnh1 != null)
                tb1.HinhAnh1 = tb.HinhAnh1;
            if (tb.HinhAnh2 != null)
                tb1.HinhAnh2 = tb.HinhAnh2;
            if (tb.HinhAnh3 != null)
                tb1.HinhAnh3 = tb.HinhAnh3;
            tb1.IsBestSeller = tb.IsBestSeller;

            ct1.SoLuong = ct.SoLuong;
            ct1.GiaTien = ct.GiaTien;
            ct1.RAM = ct.RAM;
            ct1.ManHinh = ct.ManHinh;
            ct1.Pin = ct.Pin;
            ct1.BoNhoTrong = ct.BoNhoTrong;
            ct1.Chip = ct.Chip;
            ct1.CameraSau = ct.CameraSau;
            ct1.CameraTruoc = ct.CameraTruoc;

            db.SubmitChanges();
            setAlert("Sửa thành công", "success");
            return RedirectToAction("QuanLyLoaiThietBi");
        }

        public ActionResult QuanLyLoaiThietBi(int? page)
        {
            var listSP = db.ChiTietThietBis.ToList();
            ViewBag.listSP = listSP;
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(listSP.OrderByDescending(s => s.MaTB).ToPagedList(pageNumber, pageSize));
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