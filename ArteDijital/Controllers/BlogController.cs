using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArteDijital.Models.Entity;
using System.Data.Entity;
using System.Net;
using System.Net.Mail;

namespace ArteDijital.Controllers
{
    public class BlogController : Controller
    {
        artedijitalEntities db = new artedijitalEntities();
        // GET: Blog
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Kategori(int id)
        {

            var degerler = (from s in db.posttables
                            where s.isaktif == 1 && s.categoryid == id
                            orderby s.possttime descending
                            select s).ToList();


            //string strName = db.menulers.Where(x => x.id == id).SingleOrDefault().menuname;
            //string nameFetchedId = list.SingleOrDefault(obj => obj.Id == 2)?.Name;
            //string str = db.menulers.SingleOrDefault(x => x.id == id)?.menuname.ToString();
            //ViewBag.baslik = str;
            return View(degerler);
        }

        public ActionResult GirisYap()
        {
            ViewBag.alert = TempData["alert"];
            return View();
        }

        public ActionResult KayitOl()
        {
            ViewBag.alert = TempData["message"];
            return View();
        }

        [HttpPost]
        public ActionResult KayitOl(string username, string usersurname, string useremail, string userpassword, string userpasswordagain, string usernick)
        {
            if (userpassword == userpasswordagain)
            {
                var kullanicinick = db.usertables.FirstOrDefault(x => x.usernick == usernick);
                var kullanici = db.usertables.FirstOrDefault(x => x.useremail == useremail);
                if (kullanici != null)
                {
                    ViewBag.Alert = "mail";
                    return View("KayitOl");
                }
                else
                {
                    if (kullanicinick != null)
                    {
                        ViewBag.Alert = "nick";
                        return View("KayitOl");
                    }
                    else
                    {
                        var ekle = new usertable
                        {
                            username = username,
                            usersurname = usersurname,
                            userpassword = userpassword,
                            useremail = useremail,
                            usernick = usernick,
                            userauthorityID = 3,
                            isaktif = 1,
                        };

                        if (Request.Files.Count > 0)
                        {
                            var image = Request.Files[0];

                            if ((5 * 1024 * 1024) < image.ContentLength) //5MB
                                throw new Exception("Yüksek Boyut!");

                            var fileInfo = new FileInfo(image.FileName);
                            var pic = "UserPhotos_" + DateTime.Now.Ticks + fileInfo.Extension; //new file name
                            var filePath = "/Photos/UserPhotos/" + pic; //sen bunu db'ye yaz..
                            var tempFilePath = Server.MapPath("~\\Photos\\UserPhotos\\" + pic);
                            image.SaveAs(tempFilePath);
                            ekle.userphotourl = filePath;


                        }
                        db.usertables.Add(ekle);
                        db.SaveChanges();
                        TempData["message"] = string.Format("ok");
                        return RedirectToAction("KayitOl");

                    }
                }
            }
            else
            {
                ViewBag.Alert = "sifre";
                return View("KayitOl");
            }
        }

        [HttpPost]
        public ActionResult GirisYap(usertable model)
        {
            var ArteDijitalUser = db.usertables.FirstOrDefault(x => x.useremail == model.useremail && x.userpassword == model.userpassword);
            if (ArteDijitalUser != null)
            {
                Session["MySessionUser"] = ArteDijitalUser;
                TempData["alert"] = string.Format("ok");
                return RedirectToAction("GirisYap");
            }
            else
            {
                TempData["alert"] = string.Format("no");
                return RedirectToAction("GirisYap");
            }
        }

        public ActionResult CikisYap()
        {

            Session.Remove("MySessionUser");
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("GirisYap");

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


        public ActionResult BlogOku(string posturl)
        {
            var post = db.posttables.Include(a => a.comenttables).FirstOrDefault(a => a.posturl == posturl);
            if (post == null)
            {
                return RedirectToAction("Hata", "Blog");
            }
            post.views += 1;
            db.SaveChanges();
            return View("BlogOku", post);
        }

        public ActionResult Hata()
        {

            return View();
        }

        public ActionResult PostLike(int id)
        {
            var post = db.posttables.FirstOrDefault(a => a.id == id);

            post.postlike += 1;
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());



        }

        public ActionResult PostDislike(int id)
        {
            var post = db.posttables.FirstOrDefault(a => a.id == id);

            post.postdislike += 1;
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult Comment(int postid, string title, string comentcontent)
        {
            usertable currentUser = (usertable)Session["MySessionUser"];
            var ekle = new comenttable
            {
                postid = postid,
                title = title,
                comentcontent = comentcontent,
                userid = currentUser.id,
                isaktif = 1,
                commenttime = DateTime.Now,
            };
            db.comenttables.Add(ekle);
            db.SaveChanges();
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
        }

        public ActionResult CommentDelete(int id)
        {
            var commentdel = db.comenttables.Find(id);
            db.comenttables.Remove(commentdel);
            db.SaveChanges();
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());

        }

        public ActionResult Search(string search)
        {
            var post = from x in db.posttables
                       select x;
            if (!String.IsNullOrEmpty(search))
            {
                post = post.Where(s => s.title.Contains(search)
                || s.spottext.Contains(search)
                || s.postcontent.Contains(search)
                || s.usertable.usernick.Contains(search));
            }
            return View(post.ToList());
        }

        public ActionResult Guncel()
        {
            var degerler = (from s in db.posttables
                            where s.isaktif == 1
                            orderby s.possttime descending
                            select s
                            ).Take(20);

            return View(degerler);


        }

        public ActionResult Yazilim()
        {
            var degerler = (from s in db.posttables
                            where s.isaktif == 1 && s.category.menuler.id == 1
                            orderby s.possttime descending
                            select s
                           ).ToList();

            return View(degerler);

        }

        public ActionResult Donanim()
        {
            var degerler = (from s in db.posttables
                            where s.isaktif == 1 && s.category.menuler.id == 2
                            orderby s.possttime descending
                            select s
                           ).ToList();

            return View(degerler);

        }

        public ActionResult Teknoloji()
        {
            var degerler = (from s in db.posttables
                            where s.isaktif == 1 && s.category.menuler.id == 3
                            orderby s.possttime descending
                            select s
                           ).ToList();

            return View(degerler);

        }

        public ActionResult Oyun()
        {
            var degerler = (from s in db.posttables
                            where s.isaktif == 1 && s.category.menuler.id == 4
                            orderby s.possttime descending
                            select s
                           ).ToList();

            return View(degerler);

        }

        public ActionResult KullaniciYazilari(int id)
        {

            var degerler = (from s in db.posttables
                            where s.isaktif == 1 && s.userid == id
                            orderby s.possttime descending
                            select s
                           ).ToList();


            return View(degerler);
        }

        public ActionResult Kurallar()
        {

            return View();
        }

        public ActionResult EditorOl()
        {

            return View();
        }

        public ActionResult SSS()
        {

            return View();
        }

        [HttpGet]
        public ActionResult SifremiUnuttum()
        {

            return View();
        }

        [HttpPost]
        public ActionResult SifremiUnuttum(usertable k){
            var mailadresi = db.usertables.Where(x => x.useremail == k.useremail).FirstOrDefault();           
            if (mailadresi != null)
            {
                Guid rastgele = Guid.NewGuid();
                mailadresi.userpassword = rastgele.ToString().Substring(0, 8);
                db.SaveChanges();

                SmtpClient client = new SmtpClient("mail.artedijital.com", 587);

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("info@artedijital.com", "Şifre Sıfırlama - Arte Dijital");
                mail.To.Add(mailadresi.useremail);
                mail.IsBodyHtml = true;
                mail.Subject = "Şifre Değiştirme İsteği - Arte Dijital";
                mail.Body += "Merhaba " + mailadresi.usernick + "<br/>" + "<b>Kullanıcı Mail Adresi :</b> " + mailadresi.useremail + "<br/>"
                    + " <b>Şifreniz :</b> " + mailadresi.userpassword
                    + "<br /> <br />" + "Eğer şifrenin başkası tarafından değiştirildiğini düşünüyorsan bizimle iletişime geçebilirsin. </br> info@artedijital.com <br/><br/><br/>"
                    + "<a href='https://artedijital.com/'>Arte Dijital</a> <br /> <br /> <img src='https://www.artedijital.com/images/artelogo.png' height='100px' />"
                    ;

                NetworkCredential net = new NetworkCredential("info@artedijital.com", "Artedijital1234!");
                client.Credentials = net;
                client.Send(mail);

                return RedirectToAction("Index");

            }
            else
            {
                var HataMesaji = " Girmiş olduğunuz E-posta sistemimizde kayıtlı değil.";
                ViewBag.mesaj = HataMesaji;
                return View();
            }
        }
    }
}