using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyDienThoai.Models;

namespace QuanLyDienThoai.Controllers
{
    public class ThanhToanController : Controller
    {
        QLDTDataContext db = new QLDTDataContext();
        public ActionResult ThanhToan()
        {
            if (Session["nhanvien"] != null)
            {
                NhanVien a = Session["nhanvien"] as NhanVien;
                NhanVien nv = db.NhanViens.FirstOrDefault(s => s.MaNV == a.MaNV);
                return View(nv);
            }
            setAlert("Vui lòng đăng nhập để mua hàng!!", "warning");
            return RedirectToAction("GioHang", "GioHang");
        }

        public ActionResult XacNhanThanhToan(FormCollection c)
        {
            if (Session["nhanvien"] == null)
            {
                setAlert("Vui lòng đăng nhập để mua hàng!!", "warning");
                return RedirectToAction("GioHang", "GioHang");
            }
            try
            {
                NhanVien kh = Session["nhanvien"] as NhanVien;

                string hoten = c["name"].ToString();
                string diachi = c["Address"].ToString();
                string sdt = c["Tel"].ToString();
                string email = c["email"].ToString();

                List<GioHang> lst = LayGioHang();
                NhanVien nv = db.NhanViens.FirstOrDefault(s => s.MaNV == kh.MaNV);
                HoaDon hd = new HoaDon();
                GiaoHang gh = new GiaoHang();
                if(nv != null)
                {
                    nv.TenNV = hoten;
                    nv.DiaChi = diachi;
                    nv.DienThoai = sdt;
                    nv.Email = email;
                    db.SubmitChanges();

                    hd.MaNV = nv.MaNV;
                    hd.NgayHD = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    hd.TongTien = (decimal)TongThanhTien();
                    db.HoaDons.InsertOnSubmit(hd);
                    db.SubmitChanges();

                    gh.MaHD = hd.MaHD;
                    gh.TenNguoiNhan = hoten;
                    gh.DiaChi1 = diachi;
                    gh.SoDT1 = sdt;
                    gh.Email = email;
                    db.GiaoHangs.InsertOnSubmit(gh);
                    db.SubmitChanges();

                    foreach (var sp in lst)
                    {
                        if (ktSL(sp.iMaSP, sp.iSoLuong))
                        {
                            CTHoaDon ct = new CTHoaDon();
                            ChiTietThietBi cttb = db.ChiTietThietBis.FirstOrDefault(s => s.MaTB == sp.iMaSP);
                            ct.MaHD = hd.MaHD;
                            ct.MaChiTiet = cttb.MaChiTiet;
                            ct.SoLuong = sp.iSoLuong;
                            ct.ThanhTien = (decimal)sp.dDonGia;
                            db.CTHoaDons.InsertOnSubmit(ct);
                            db.SubmitChanges();
                            
                            
                            cttb.SoLuong = cttb.SoLuong - sp.iSoLuong;
                            db.SubmitChanges();
                        }
                        else
                        {
                            setAlert("Đặt hàng thất bại!!!", "error");
                            return RedirectToAction("ThanhToan", "ThanhToan");
                        }
                    }

                    Session["dangnhap"] = nv.TenNV;

                    setAlert("Đặt hàng thành công!!!", "success");
                    return RedirectToAction("ShowListSanPham", "SanPham");
                }
                return RedirectToAction("ShowListSanPham", "SanPham");
            }
            catch
            {
                setAlert("Đặt hàng thất bại!!!", "error");
                return RedirectToAction("ShowListSanPham", "SanPham");
            }

        }

        private bool ktSL(int ma, int sl)
        {
            ChiTietThietBi a = db.ChiTietThietBis.Where(t => t.MaTB == ma).FirstOrDefault();
            if (a != null)
            {
                if (a.SoLuong >= sl)
                {
                    return true;
                }
            }
            return false;
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