using ArteDijital.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArteDijital.Controllers
{
    public class AdminPartialController : BaseController
    {
        // GET: AdminPartial
        artedijitalEntities db = new artedijitalEntities();
        public ActionResult ProjectForm()
        {       

            return PartialView();
        }

        [HttpPost]
        public ActionResult ProjectAdd(string projectname)
        {
            
            usertable currentUser = (usertable)Session["MySessionUser"];
            var zaman = DateTime.Now.ToString();
            var add = new ProjectTable
            {
                projectstatus = 1,
                projectname = projectname,
                createtime = Convert.ToDateTime(zaman),
                creatorid = currentUser.id,

            };

            db.ProjectTables.Add(add);
            db.SaveChanges();
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
        }
    }
}