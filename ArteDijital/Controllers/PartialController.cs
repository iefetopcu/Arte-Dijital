using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArteDijital.Models.Entity;

namespace ArteDijital.Controllers
{
    public class PartialController : Controller
    {

        artedijitalEntities db = new artedijitalEntities();
        // GET: Partial
        public ActionResult hizmetiletisim()
        {

            
            return PartialView();
        }

        //[HttpPost]
        //public ActionResult ServiceRequest(Msgservicerequest ekle)
        //{

        //    ekle.isaktif = 1;
        //    ekle.messagetime = DateTime.Now;
        //    db.Msgservicerequests.Add(ekle);
        //    db.SaveChanges();

        //    TempData["message"] = string.Format("ok");
        //    return Redirect(Request.UrlReferrer.ToString());

        //}
        [HttpPost]
        public ActionResult FooterMessage(footermessage ekle)
        {
            ekle.isaktif = 1;
            ekle.messagetime = DateTime.Now;
            db.footermessages.Add(ekle);
            db.SaveChanges();

            TempData["footermessage"] = string.Format("ok");
            return Redirect(Request.UrlReferrer.ToString());

        }

        public ActionResult menu()
        {

            var degerler = (from s in db.categories
                            where s.isaktif == 1
                            select s
                            ).ToList();

            return PartialView(degerler);
        }

        public ActionResult IndexBlogposts()
        {


            var degerler = (from s in db.posttables
                            where s.isaktif == 1
                            orderby s.possttime descending
                            select s
                            ).Take(3);

            return PartialView(degerler);



        }

        public ActionResult BlogIndexposts()
        {
            var degerler = (from s in db.posttables
                            where s.isaktif == 1
                            orderby s.possttime descending
                            select s
                           ).Take(20);

            return PartialView(degerler);

        }

        public ActionResult SideBar()
        {

            var degerler = (from s in db.posttables
                            where s.isaktif == 1
                            orderby s.views descending
                            select s
                           ).Take(5);

            return PartialView(degerler);
        }
    }
}