using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyDienThoai.Models;

namespace QuanLyDienThoai.Controllers
{
    public class PhanLoaiController : Controller
    {
        QLDTDataContext db = new QLDTDataContext();
        public ActionResult MenuDienThoaiPartial()
        {
            var menuDT = db.LoaiThietBis.Where(s => s.Note == "DT").ToList();
            return View(menuDT);
        }

        public ActionResult MenuPhuKienPartial()
        {
            var menuPK = db.LoaiThietBis.Where(s => s.Note == "PK").ToList();
            return View(menuPK);
        }
	}
}