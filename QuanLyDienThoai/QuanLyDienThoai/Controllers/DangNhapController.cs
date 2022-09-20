using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyDienThoai.Models;
using System.Web.Security;

namespace QuanLyDienThoai.Controllers
{
    public class DangNhapController : Controller
    {
        QLDTDataContext db = new QLDTDataContext();

        [HttpPost]
        public ActionResult XuLyDangNhap(FormCollection c)
        {
            String userName = c["Name"].ToString().Trim().ToLower();
            String passWord = c["Password"].ToString();

            TaiKhoan tk = db.TaiKhoans.Where(t => t.TaiKhoan1.ToLower().Trim() == userName && t.MatKhau == passWord).FirstOrDefault();
            if(tk != null)
            {
                NhanVien nv = db.NhanViens.Where(s => s.TaiKhoan.ToLower().Trim() == userName).FirstOrDefault();
                Session["nhanvien"] = nv;
                Session["dangnhap"] = nv.TenNV;
                //if (nv.MaLoaiNV == 1)
                //    Session["quyen"] = "admin";
                //    //admin
                //    //TempData["message"] = "<script>alert('admin');</script>";
                //if (nv.MaLoaiNV == 2)
                //    Session["quyen"] = "khachhang";
                //    //khachhang
                //    //TempData["message"] = "<script>alert('kh');</script>";

                IEnumerable<LoaiNhanVien_Quyen> lstQuyen = db.LoaiNhanVien_Quyens.Where(s => s.MaLoaiNV == nv.MaLoaiNV);
                string Quyen = "";
                foreach (var item in lstQuyen)
                {
                    Quyen += item.Quyen.MaQuyen + ",";
                }
                Quyen = Quyen.Substring(0, Quyen.Length - 1);
                PhanQuyen(userName, Quyen);
                if (nv.MaLoaiNV == 1)
                    return RedirectToAction("QuanLyLoaiThietBi", "Admin");
                return RedirectToAction("ShowListSanPham", "SanPham");
            }
            else
            {
                setAlert("Tên đăng nhập hoặc mật khẩu không chính xác, Vui lòng kiểm tra lại!!", "warning");
            }
            return RedirectToAction("ShowListSanPham", "SanPham");
        }

        public void PhanQuyen(string taiKhoan, string quyen)
        {
            FormsAuthentication.Initialize();

            var ticket = new FormsAuthenticationTicket(1, taiKhoan, DateTime.Now, DateTime.Now.AddHours(3), false, quyen, FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
        }

        private bool kiemTraTaiKhoanTonTai(string tenTK)
        {
            TaiKhoan tk = db.TaiKhoans.Where(t => t.TaiKhoan1 == tenTK).FirstOrDefault();
            if (tk != null)
                return true;
            return false;
        }

        [HttpPost]
        public ActionResult XuLyDangKy(FormCollection c)
        {
            var userName = c["Name"].Trim();
            var hoten = c["FullName"].Trim();
            var email = c["Email"].Trim();
            var diachi = c["Address"].Trim();
            var sodt = c["Tel"].Trim();
            var matkhau = c["Password"].Trim();
            if (!kiemTraTaiKhoanTonTai(userName))
            {
                NhanVien nv = new NhanVien();
                TaiKhoan tk = new TaiKhoan();
                try
                {
                    tk.TaiKhoan1 = userName;
                    tk.MatKhau = matkhau;
                    tk.NgayDangKy = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    tk.TrangThai = true;

                    nv.TaiKhoan = userName;
                    nv.DiaChi = diachi;
                    nv.DienThoai = sodt;
                    nv.Email = email;
                    nv.TenNV = hoten;
                    nv.MaLoaiNV = 2;

                    db.TaiKhoans.InsertOnSubmit(tk);
                    db.NhanViens.InsertOnSubmit(nv);
                    db.SubmitChanges();

                    setAlert("Đăng ký thành công", "success");
                    return RedirectToAction("ShowListSanPham", "SanPham");
                }
                catch (Exception ex)
                {
                    setAlert("Đăng ký không thành công, đã xảy ra lỗi trong quá trình thêm dữ liệu", "error");
                }

            }
            else
            {
                setAlert("Tài khoản đã được sử dụng, vui lòng chọn tài khoản khác", "error");
            }
            return RedirectToAction("ShowListSanPham", "SanPham");
        }

        public ActionResult XuLyDangXuat()
        {
            Session["dangnhap"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("ShowListSanPham", "SanPham");
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