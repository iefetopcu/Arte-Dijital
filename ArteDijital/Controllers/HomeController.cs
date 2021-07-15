using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArteDijital.Models.Entity;

namespace ArteDijital.Controllers
{
    public class HomeController : Controller
    {

        artedijitalEntities db = new artedijitalEntities();
        public ActionResult Index()
        {
            ViewBag.footermessage = TempData["footermessage"];
            
            return View();
        }

        public ActionResult ArteDijital()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult Projelerimiz()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult Referanslarimiz()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult Amacimiz()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult isortaklarimiz()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult Hizmetler()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }
        public ActionResult YazilimCozumleri()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult WebYazilim()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult MobilYazilim()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult MasaustuYazilim()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult YapayZeka()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult VeriAnalitigi()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult WebCozumleri()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult Eticaret()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult TanitimSitesi()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult BlogSitesi()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult DijitalPazarlama()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult NetworkCozumleri()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult SistemYaziciDestegi()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult NetworkSunucuKurulumlari()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult IpTabanliTumCihazlar()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult GrafikTasarim()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

       

        

        public ActionResult GrafikTasarimPaketleri()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult GrafikTasarimHizmetleri()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult FotografVideo()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult FotografCekimi()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult VideoCekimi()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        
        public ActionResult Ceviri()
        {
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }
        public ActionResult EdebiTercume()
        {
            ViewBag.footermessage = TempData["footermessage"];
            ViewBag.Message = TempData["message"];
            return View();
        }
        public ActionResult SimultaneCeviri()
        {
            ViewBag.footermessage = TempData["footermessage"];
            ViewBag.Message = TempData["message"];
            return View();
        }
        public ActionResult TeknikTercume()
        {
            ViewBag.footermessage = TempData["footermessage"];
            ViewBag.Message = TempData["message"];
            return View();
        }
        public ActionResult TicariTercume()
        {
            ViewBag.footermessage = TempData["footermessage"];
            ViewBag.Message = TempData["message"];
            return View();
        }
        public ActionResult WebSitesiCeviri()
        {
            ViewBag.footermessage = TempData["footermessage"];
            ViewBag.Message = TempData["message"];
            return View();
        }


        [HttpGet]
        public ActionResult FreelancerOl()
        {
            List<SelectListItem> job = (from i in db.FreelanceJobs
                                             select new SelectListItem
                                             {
                                                 Text = i.jobname,
                                                 Value = i.id.ToString()
                                             }).ToList();

            ViewBag.jobs = job;

            ViewBag.footermessage = TempData["footermessage"];
            ViewBag.Freelancer = TempData["freelancer"];
            
            return View();
        }

        [HttpPost]
        public ActionResult FreelancerOl(string name, string surname, string email, string gsm, string websiteurl , int jobid , string jobexplanation, string userphotourl, string city, string explanation)
        {
           
            var NewFreelancer = db.Freelancers.FirstOrDefault(x => x.email == email);

            if(NewFreelancer != null)
            {
                TempData["freelancer"] = string.Format("no");
                return Redirect(Request.UrlReferrer.ToString());

            }
            else
            {
                var ekle = new Freelancer
                {
                    name = name,
                    surname = surname,
                    email = email,
                    gsm = gsm,
                    websiteurl = websiteurl,
                    jobid = jobid,
                    jobexplanation = jobexplanation,
                    city = city,
                    explanation = explanation,
                    isaktif = 1,
                    time = DateTime.Now,
                };

                if(Request.Files.Count > 0)
                {
                    var image = Request.Files[0];

                    if ((5 * 1024 * 1024) < image.ContentLength) //5MB
                        throw new Exception("hata mesaaji");

                    var fileInfo = new FileInfo(image.FileName);
                    var pic = "freelancerpic_" + DateTime.Now.Ticks + fileInfo.Extension;
                    var filePath = "/Photos/Freelancers/" + pic;
                    var tempFilePath = Server.MapPath("~\\Photos\\Freelancers\\" + pic);
                    image.SaveAs(tempFilePath);
                    ekle.userphotourl = filePath;
                }

                db.Freelancers.Add(ekle);
                db.SaveChanges();

                TempData["freelancer"] = string.Format("ok");


                return Redirect(Request.UrlReferrer.ToString());
            }

        }


        public ActionResult iletisim()
        {
            ViewBag.Message = TempData["message"];
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }

        public ActionResult kvkk()
        {
            ViewBag.Message = TempData["message"];
            ViewBag.footermessage = TempData["footermessage"];
            return View();
        }






    }
}