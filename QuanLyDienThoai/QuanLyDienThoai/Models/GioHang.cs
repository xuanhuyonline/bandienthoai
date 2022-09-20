using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuanLyDienThoai.Models;

namespace QuanLyDienThoai.Models
{
    public class GioHang
    {
        QLDTDataContext db = new QLDTDataContext();

        public int iMaSP { get; set; }
        public string sTenSP { set; get; }
        public string sAnh { set; get; }
        public double dDonGia { set; get; }
        public int iSoLuong { set; get; }
        public int iTongSL { set; get; }
        public double dThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }

        public GioHang(int MaSP)
        {
            iMaSP = MaSP;
            ChiTietThietBi sp = db.ChiTietThietBis.Single(s => s.MaTB == iMaSP);
            sTenSP = sp.ThietBi.TenTB;
            sAnh = sp.ThietBi.HinhAnh1;
            dDonGia = double.Parse(sp.GiaTien.ToString());
            iSoLuong = 1;
            iTongSL = sp.SoLuong;
        }
    }
}