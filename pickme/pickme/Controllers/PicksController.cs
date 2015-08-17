using System;
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

        // GET: Picks/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pick pick = db.Picks.Find(id);
            if (pick == null)
            {
                return HttpNotFound();
            }
            return View(pick);
        }

        // GET: Picks/Create
        //[Authorize]
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Picks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(PickUploadVM newPick)
        //{
        //    ApplicationUser currentUser = new ApplicationUser();
        //    if (User.Identity.Name == "")
        //    {
        //        currentUser = db.Users.FirstOrDefault(x => x.UserName == newPick.YourName);
        //    }
        //    else
        //    {
        //        currentUser = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
        //    }

        //    using (var ms = new MemoryStream())
        //    {
        //        newPick.File.InputStream.CopyTo(ms);

        //        var pi = new Pick
        //        {
        //            PostedBy = currentUser,
        //            Image = Pick.ScaleImage(ms.ToArray(), 50, 50),
        //            PostedOn = DateTime.Now,
        //            Description = newPick.Description,
        //            PictureUrl = newPick.Url
        //        };

        //        db.Picks.Add(pi);
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("PickList");

        //add pick to db from angular
        //[Authorize]
        public ActionResult Create(PickUploadVM pick)
        {
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            if (pick.File != null)
            {
                using (var ms = new MemoryStream())
                {
                    pick.File.InputStream.CopyTo(ms);

                    Pick pi = new Pick
                    {
                        PostedBy = currentUser,
                        Image = Pick.ScaleImage(ms.ToArray(), 100, 100),
                        PostedOn = DateTime.Now,
                        Description = pick.Description,
                        PictureUrl = pick.Url
                    };
                    db.Picks.Add(pi);
                }
            }
           else
            {

                Pick pi = new Pick
                {
                    PostedBy = currentUser,
                    PostedOn = DateTime.Today,
                    Description = pick.Description,
                    PictureUrl = pick.Url,
                };
                pi.Image = pi.GetBytes(pi.PictureUrl);
                db.Picks.Add(pi);
            }

            db.SaveChanges();

            var myPicks = from p in db.Picks.ToList()
                          where p.PostedBy == currentUser
                          orderby p.PostedOn descending
                          select new { Id = p.Id, Description = p.Description, PostedOn = p.PostedOn.ToString(), PostedBy = p.PostedBy.UserName };

            var myLastPick = myPicks.FirstOrDefault();


            return Json(myLastPick, JsonRequestBehavior.AllowGet);
        }


        // GET: Picks/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pick pick = db.Picks.Find(id);
            if (pick == null)
            {
                return HttpNotFound();
            }

            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

            if (currentUser != pick.PostedBy)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EditVM changePick = new EditVM();
            changePick.Id = pick.Id;
            changePick.Description = pick.Description;
            changePick.Url = pick.PictureUrl;
            
            return View(changePick);
        }

        // POST: Picks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditVM changePick)
        {
            Pick pick = db.Picks.FirstOrDefault(x => x.Id == changePick.Id);
            pick.Description = changePick.Description;
            pick.PictureUrl = changePick.Url;
            if (ModelState.IsValid)
            {
                
                db.Entry(pick).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(changePick);
        }

        // GET: Picks/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pick pick = db.Picks.Find(id);
            if (pick == null)
            {
                return HttpNotFound();
            }

            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

            if (currentUser != pick.PostedBy)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(pick);
        }

        // POST: Picks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pick pick = db.Picks.Find(id);
            db.Picks.Remove(pick);
            db.SaveChanges();
            return RedirectToAction("Index");
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
        public ActionResult Main1()
        {
            return View("Main1");
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
