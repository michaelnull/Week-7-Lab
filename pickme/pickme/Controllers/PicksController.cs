﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using pickme.Models;
using System.IO;

namespace pickme.Controllers
{
    public class PicksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        //move data about pictures into app.js for display on main webpage
        [Authorize]
        public ActionResult DisplayPicks(int? id)
        {
            if (id == null || id < 1)
            {
                id = 1;
            }
            int picksToSkip = Convert.ToInt32(id - 1) * 12;

            var query = from p in db.Picks
                        orderby p.PostedOn descending
                        select new { Id = p.Id, Description = p.Description, PostedOn = p.PostedOn.ToString(), PostedBy = p.PostedBy.UserName };


            return Json(query.ToList().Skip(picksToSkip).Take(12),
                JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public ActionResult Create(PickUploadVM pick)
        {
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

            Pick pi = new Pick
            {
                PostedBy = currentUser,
                PostedOn = DateTime.Now,
                Description = pick.Description,
                PictureUrl = pick.Url,
            };

            if (pick.File != null)
            {
                using (var ms = new MemoryStream())
                {
                    pick.File.InputStream.CopyTo(ms);
                    pi.Image = Pick.ScaleImage(ms.ToArray(), 150, 150);
                }
            }
            else
            {
                pi.Image = pi.GetBytes(pi.PictureUrl);
            }


            db.Picks.Add(pi);
            db.SaveChanges();


            var result = new { Id = pi.Id, Description = pi.Description, PostedOn = pi.PostedOn.ToString(), PostedBy = pi.PostedBy.UserName };



            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetImage(int id)
        {

            var dbRow = db.Picks.Find(id);
            if (dbRow.Image == null)
            {
                Pick noImage = new Pick();
                noImage.Image = noImage.GetBytes("https://encrypted-tbn1.gstatic.com/images?q=tbn:ANd9GcTLna360D4lNuRMj_2nBHnO-vtIh9QhpDXJPnfadeaMQEPJbGNoLdSZ4A");
                return File(noImage.Image, "image");
            }
            return File(dbRow.Image, "image");

        }
        public ActionResult Main()
        {
            return View("Main");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



    }
}
