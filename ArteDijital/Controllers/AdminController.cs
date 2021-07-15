using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArteDijital.Models.Entity;
using OfficeOpenXml;
using ArteDijital.Helpers;

namespace ArteDijital.Controllers
{
    public class AdminController : BaseController
    {
        // GET: Admin
        artedijitalEntities db = new artedijitalEntities();
        public ActionResult Index()
        {
            ViewBag.FooterOkunmamis = (from s in db.footermessages
                                       where s.isaktif == 1
                                       select s).Count();

            ViewBag.FooterOkunmus = (from s in db.footermessages
                                     where s.isaktif == 2
                                     select s).Count();

            //ViewBag.HizmetOkunmus = (from s in db.Msgservicerequests
            //                         where s.isaktif == 2
            //                         select s).Count();

            //ViewBag.HizmetOkunmamis = (from s in db.Msgservicerequests
            //                           where s.isaktif == 1
            //                           select s).Count();

            ViewBag.YazilimPost = (from s in db.posttables
                                       where s.isaktif == 1 && s.category.menuid == 1 
                                       select s).Count();

            ViewBag.DonanimPost = (from s in db.posttables
                                   where s.isaktif == 1 && s.category.menuid == 2
                                   select s).Count();

            ViewBag.TeknolojiPost = (from s in db.posttables
                                   where s.isaktif == 1 && s.category.menuid == 3
                                   select s).Count();

            ViewBag.OyunPost = (from s in db.posttables
                                   where s.isaktif == 1 && s.category.menuid == 4
                                   select s).Count();

            ViewBag.Kullanicilar = (from s in db.usertables
                                where s.isaktif == 1 && s.userrole.userrolename == "Kullanici"
                                select s).Count();

            ViewBag.Admin = (from s in db.usertables
                                    where s.isaktif == 1 && s.userrole.userrolename == "Admin"
                                    select s).Count();

            ViewBag.Editor = (from s in db.usertables
                                    where s.isaktif == 1 && s.userrole.userrolename == "Editor"
                                    select s).Count();

            ViewBag.Freelancer = (from s in db.Freelancers
                                  where s.isaktif == 1
                                  select s).Count();


            return View();
        }

        public ActionResult Kategori()
        {

            var kategoriler = (from s in db.categories
                               where s.isaktif == 1
                               select s).ToList();

            return View(kategoriler);
        }

        public ActionResult KategoriSil(int id)
        {
            var kategori = db.categories.Find(id);

            db.categories.Remove(kategori);
            db.SaveChanges();
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());


        }

        public ActionResult YeniKategori()
        {


            List<SelectListItem> menulist = (from i in db.menulers.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.menuname,
                                                 Value = i.id.ToString()
                                             }).ToList();
            ViewBag.menu = menulist;


            return View();
        }

        [HttpPost]
        public ActionResult YeniKategori(string categoryname, int menuid)
        {

            var ekle = new category
            {
                categoryname = categoryname,
                menuid = menuid,
                isaktif = 1,

            };


            db.categories.Add(ekle);
            db.SaveChanges();
            return RedirectToAction("Kategori");

        }

        [HttpGet]
        public ActionResult KategoriDuzenle(int id)
        {

            List<SelectListItem> menulist = (from i in db.menulers.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.menuname,
                                                 Value = i.id.ToString()
                                             }).ToList();
            ViewBag.menu = menulist;

            var category = db.categories.Find(id);

            return View("KategoriDuzenle", category);

        }
        [HttpPost]
        public ActionResult KategoriDuzenle(category p1)
        {
            var category = db.categories.Find(p1.id);

            category.categoryname = p1.categoryname;
            category.menuid = p1.menuid;
            category.isaktif = 1;

            db.SaveChanges();
            return RedirectToAction("Kategori");
        }

        public ActionResult Kullanicilar()
        {
            var kullanicilar = (from s in db.usertables
                                where s.isaktif == 1
                                select s).ToList();

            return View(kullanicilar);
        }
        public ActionResult KullaniciSil(int id)
        {
            var user = db.usertables.Find(id);

            db.usertables.Remove(user);
            db.SaveChanges();
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());


        }
        [HttpGet]
        public ActionResult KullaniciDuzenle(int id)
        {

            List<SelectListItem> roles = (from i in db.userroles.ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.userrolename,
                                               Value = i.id.ToString()
                                           }).ToList();

            ViewBag.role = roles;

            var user = db.usertables.Find(id);

            return View("KullaniciDuzenle", user);

        }

        [HttpPost]
        public ActionResult KullaniciDuzenle(usertable p1)
        {
            var kullanici = db.usertables.Find(p1.id);

            if (p1.userphotourl != null)
            {
                var image = Request.Files[0];

                if ((5 * 1024 * 1024) < image.ContentLength)
                    throw new Exception("hata mesaaji");

                var fileInfo = new FileInfo(image.FileName);
                var pic = "UserPic_" + DateTime.Now.Ticks + fileInfo.Extension; 

                var filePath = "/Photos/UserPhotos/" + pic; 

                var tempFilePath = Server.MapPath("~\\Photos\\UserPhotos\\" + pic);

                image.SaveAs(tempFilePath);

                kullanici.userphotourl = filePath;
            }

            kullanici.userauthorityID = p1.userauthorityID;
            kullanici.useremail = p1.useremail;
            kullanici.usersurname = p1.usersurname;
            kullanici.username = p1.username;
            kullanici.userpassword = p1.userpassword;
            kullanici.usernick = p1.usernick;

            db.SaveChanges();
            return RedirectToAction("Kullanicilar");

        }

        public ActionResult BlogPostlari()
        {
            usertable currentUser = (usertable)Session["MySessionUser"];

            if (currentUser.userauthorityID == 1)
            {
                var postlar = (from s in db.posttables
                               where s.isaktif == 1
                               orderby s.possttime descending
                               select s).ToList();
                return View(postlar);
            }

            if (currentUser.userauthorityID == 2)
            {
                var postlar = (from s in db.posttables
                               where s.isaktif == 1 && s.userid == currentUser.id
                               orderby s.possttime descending
                               select s).ToList();
                return View(postlar);      
            }
            return View("Index");
        }

        [HttpGet]
        public ActionResult YeniIcerikEkle()
        {

            List<SelectListItem> category = (from i in db.categories.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.categoryname,
                                                 Value = i.id.ToString()
                                             }).ToList();

            List<SelectListItem> users = (from i in db.usertables.ToList()
                                          select new SelectListItem
                                          {
                                              Text = i.username + " " + i.usersurname,
                                              Value = i.id.ToString()
                                          }).ToList();

            ViewBag.cat = category;
            ViewBag.usr = users;

            return View();

        }
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult YeniIcerikEkle(string photourl, int categoryid, string description, string hashtags, int userid, string title, string postcontent, string spottext, string possttime, int isaktif)
        {

            string zaman = possttime;
            var url = string.IsNullOrEmpty(title.Trim()) ? Utility.UrlSeo(title.Trim()) : Utility.UrlSeo(title.Trim());
            

            var ekle = new posttable
            {
                categoryid = categoryid,
                description = description,
                hashtags = hashtags,
                userid = userid,
                title = title,
                postcontent = postcontent,
                spottext = spottext,
                possttime = Convert.ToDateTime(zaman),
                postlike = 0,
                postdislike = 0,
                views = 0,
                isaktif = isaktif,
                posturl = url
            };          

            if (photourl != null)
            {
                var image = Request.Files[0];

                if ((5 * 1024 * 1024) < image.ContentLength) //5MB
                    throw new Exception("hata mesaaji");

                var fileInfo = new FileInfo(image.FileName);
                var pic = "PostPic_" + DateTime.Now.Ticks + fileInfo.Extension;
                var filePath = "/Photos/PostPhotos/" + pic;
                var tempFilePath = Server.MapPath("~\\Photos\\PostPhotos\\" + pic);
                image.SaveAs(tempFilePath);
                ekle.photourl = filePath;
            }

            db.posttables.Add(ekle);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult BlogSil(int id)
        {
            var comments = db.comenttables.Where(c => c.postid == id);
            db.comenttables.RemoveRange(comments);
            var postdelete = db.posttables.Find(id);
            db.posttables.Remove(postdelete);
            db.SaveChanges();
            return RedirectToAction("BlogPostlari");
        }

        [HttpGet]
        public ActionResult BlogGuncelle(int id)
        {
            

            List<SelectListItem> category = (from i in db.categories.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.categoryname,
                                                 Value = i.id.ToString()
                                             }).ToList();

            List<SelectListItem> users = (from i in db.usertables.ToList()
                                          select new SelectListItem
                                          {
                                              Text = i.username + " " + i.usersurname,
                                              Value = i.id.ToString()
                                          }).ToList();

            ViewBag.cat = category;
            ViewBag.usr = users;

            var post = db.posttables.Find(id);
            return View("BlogGuncelle", post);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult BlogGuncelle(posttable p1)
        {
            var post = db.posttables.Find(p1.id);
            var zaman = p1.possttime;
            var url = string.IsNullOrEmpty(p1.title.Trim()) ? Utility.UrlSeo(p1.title.Trim()) : Utility.UrlSeo(p1.title.Trim());

            if (p1.photourl != null)
            {
                var image = Request.Files[0];
                if ((5 * 1024 * 1024) < image.ContentLength) //5MB
                    throw new Exception("hata mesaaji");
                var fileInfo = new FileInfo(image.FileName);
                var pic = "PostPic_" + DateTime.Now.Ticks + fileInfo.Extension;
                var filePath = "/Photos/PostPhotos/" + pic;
                var tempFilePath = Server.MapPath("~\\Photos\\PostPhotos\\" + pic);
                image.SaveAs(tempFilePath);
                post.photourl = filePath;
            }
            post.isaktif = p1.isaktif;
            post.categoryid = p1.categoryid;
            post.userid = p1.userid;
            post.title = p1.title;
            post.description = p1.description;
            post.hashtags = p1.hashtags;
            post.spottext = p1.spottext;
            post.postcontent = p1.postcontent;
            post.possttime = Convert.ToDateTime(zaman);
            post.posturl = url;

            db.SaveChanges();
            return RedirectToAction("BlogPostlari");

        }
        public ActionResult Mesajlar()
        {
            ViewBag.FooterOkunmamis = (from s in db.footermessages
                                       where s.isaktif == 1
                                       select s).Count();

            ViewBag.FooterOkunmus = (from s in db.footermessages
                                       where s.isaktif == 2
                                       select s).Count();

            //ViewBag.HizmetOkunmus = (from s in db.Msgservicerequests
            //                         where s.isaktif == 2
            //                         select s).Count();

            //ViewBag.HizmetOkunmamis = (from s in db.Msgservicerequests
            //                           where s.isaktif == 1
            //                           select s).Count();




            return View();
        }

        public ActionResult FooterMesaj()
        {
            var footermesaj = (from s in db.footermessages
                               select s).ToList();

            return View(footermesaj);


        }

        public ActionResult FooterMesajSil(int id)
        {
            var mesaj = db.footermessages.Find(id);

            db.footermessages.Remove(mesaj);
            db.SaveChanges();
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());

        }

        public ActionResult FooterMesajOku(int id)
        {
            var mesaj = db.footermessages.Find(id);
            return View(mesaj);
        }

        public ActionResult FooterMesajOkundu(int id)
        {
            var mesaj = db.footermessages.Find(id);

            mesaj.isaktif = 2;
            db.SaveChanges();
            return RedirectToAction("FooterMesaj");

        }

        public ActionResult FooterMesajOkunmadi(int id)
        {
            var mesaj = db.footermessages.Find(id);

            mesaj.isaktif = 1;
            db.SaveChanges();
            return RedirectToAction("FooterMesaj");

        }

        //public ActionResult HizmetMesaj()
        //{
        //    //var hizmetmesaj = (from s in db.Msgservicerequests
        //    //                   select s).ToList();

        //    return View(hizmetmesaj);

        //}

        //public ActionResult HizmetMesajSil(int id)
        //{
        //    var mesaj = db.Msgservicerequests.Find(id);
        //    db.Msgservicerequests.Remove(mesaj);
        //    db.SaveChanges();
        //    return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());

        //}

        //public ActionResult HizmetMesajOku(int id)
        //{
        //    //var mesaj = db.Msgservicerequests.Find(id);
        //    //return View(mesaj);
        //}

        //public ActionResult HizmetMesajOkundu(int id)
        //{
        //    //var mesaj = db.Msgservicerequests.Find(id);

        //    //mesaj.isaktif = 2;
        //    //db.SaveChanges();
        //    //return RedirectToAction("HizmetMesaj");

        //}

        //public ActionResult HizmetMesajOkunmadi(int id)
        //{
        //    var mesaj = db.Msgservicerequests.Find(id);

        //    mesaj.isaktif = 1;
        //    db.SaveChanges();
        //    return RedirectToAction("HizmetMesaj");

        //}

        public ActionResult Freelancer()
        {

            var freelancers = (from s in db.Freelancers
                               where s.isaktif == 1
                               select s).ToList();

            return View(freelancers);
        }

        public ActionResult FreelancerSil(int id)
        {
            var user = db.Freelancers.Find(id);

            db.Freelancers.Remove(user);
            db.SaveChanges();
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());


        }

        public ActionResult FreelancerGoster(int id)
        {

            var freelancer = db.Freelancers.Find(id);
            return View(freelancer);

        }

        public void FreelancerExcel()
        {
            var degerler = db.Freelancers.ToList();

            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1"].Value = "ID";
            ws.Cells["B1"].Value = "Adı Soyadı";
            ws.Cells["C1"].Value = "Meslek";
            ws.Cells["D1"].Value = "Telefon";
            ws.Cells["E1"].Value = "E-Mail";
            ws.Cells["F1"].Value = "Web site";
            ws.Cells["G1"].Value = "Meslek açıklaması";


            int rowStart = 2;
            foreach (var item in degerler)
            {


                ws.Cells[String.Format("A{0}", rowStart)].Value = item.id;
                ws.Cells[String.Format("B{0}", rowStart)].Value = item.name +" "+ item.surname;
                ws.Cells[String.Format("C{0}", rowStart)].Value = item.FreelanceJob.jobname;
                ws.Cells[String.Format("D{0}", rowStart)].Value = item.gsm;
                ws.Cells[String.Format("E{0}", rowStart)].Value = item.email;
                ws.Cells[String.Format("F{0}", rowStart)].Value = item.websiteurl;
                ws.Cells[String.Format("G{0}", rowStart)].Value = item.jobexplanation;
                rowStart++;

            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=" + "Freelancers_" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ".xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();

        }

        public ActionResult Profil()
        {
            usertable currentUser = (usertable)Session["MySessionUser"];

            if (currentUser != null)
            {
                var user = db.usertables.Find(currentUser.id);
                return View("Profil", user);
            }
            else
            {
                return RedirectToAction("Index");
            }


        }

        [HttpPost]
        public ActionResult ProfilDuzenle(usertable p1)
        {
            usertable currentUser = (usertable)Session["MySessionUser"];
            var kullanici = db.usertables.Find(currentUser.id);
            if (p1.userphotourl != null)
            {
                var image = Request.Files[0];
                if ((5 * 1024 * 1024) < image.ContentLength)
                    throw new Exception("hata mesaaji");
                var fileInfo = new FileInfo(image.FileName);
                var pic = "UserPic_" + DateTime.Now.Ticks + fileInfo.Extension;
                var filePath = "/Photos/UserPhotos/" + pic;
                var tempFilePath = Server.MapPath("~\\Photos\\UserPhotos\\" + pic);
                image.SaveAs(tempFilePath);
                kullanici.userphotourl = filePath;
            }

            kullanici.useremail = p1.useremail;
            kullanici.username = p1.username;
            kullanici.userpassword = p1.userpassword;
            kullanici.usernick = p1.usernick;

            db.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult WorkFlow()
        {

            var projects = (from s in db.ProjectTables                             
                               select s).ToList();

            return View(projects);
        }
        public ActionResult ProjectNext(int id)
        {
            var status = db.ProjectTables.Find(id);
            status.projectstatus += 1;
            db.SaveChanges();
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());

        }

        public ActionResult ProjectBack(int id)
        {
            var status = db.ProjectTables.Find(id);
            status.projectstatus -= 1;
            db.SaveChanges();
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());

        }

        public ActionResult ProjectDelete(int id)
        {
            var project = db.ProjectTables.Find(id);
            db.ProjectTables.Remove(project);
            db.SaveChanges();
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
        }

        public ActionResult TaskList(int id)
        {
            var pname = db.ProjectTables.Where(x => x.id == id).Select(x => x.projectname).SingleOrDefault();

            ViewBag.projectname = pname.ToString();
           

            var tasks = (from s in db.TaskTables
                               where s.projectid == id
                               orderby s.deadline ascending
                               select s).ToList();
            return View(tasks);
        }

    }
}