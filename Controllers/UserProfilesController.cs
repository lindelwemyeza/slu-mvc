using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentLinkUp_MVC_;

namespace StudentLinkUp_MVC_.Controllers
{
    public class UserProfilesController : Controller
    {
        private StudentLinkUpDBEntities db = new StudentLinkUpDBEntities();

        // GET: UserProfiles
        public ActionResult Index()
        {
            var userProfiles = db.UserProfiles.Include(u => u.Campu).Include(u => u.PayType1).Include(u => u.UserType1);
            return View(userProfiles.ToList());
        }

        // GET: UserProfiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProfile userProfile = db.UserProfiles.Find(id);
            if (userProfile == null)
            {
                return HttpNotFound();
            }
            return View(userProfile);
        }

        // GET: UserProfiles/Create
        public ActionResult Create()
        {
            ViewBag.campusID = new SelectList(db.Campus, "campusID", "campusName");
            ViewBag.payType = new SelectList(db.PayTypes, "ID", "payType1");
            ViewBag.userType = new SelectList(db.UserTypes, "ID", "userType1");
            return View();
        }

        // POST: UserProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userID,username,userFName,userSName,userPassword,studentNum,campusID,areaCode,cellNum,ban,yearOfStudy,payType,userType")] UserProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                db.UserProfiles.Add(userProfile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.campusID = new SelectList(db.Campus, "campusID", "campusName", userProfile.campusID);
            ViewBag.payType = new SelectList(db.PayTypes, "ID", "payType1", userProfile.payType);
            ViewBag.userType = new SelectList(db.UserTypes, "ID", "userType1", userProfile.userType);
            return View(userProfile);
        }

        // GET: UserProfiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProfile userProfile = db.UserProfiles.Find(id);
            if (userProfile == null)
            {
                return HttpNotFound();
            }
            ViewBag.campusID = new SelectList(db.Campus, "campusID", "campusName", userProfile.campusID);
            ViewBag.payType = new SelectList(db.PayTypes, "ID", "payType1", userProfile.payType);
            ViewBag.userType = new SelectList(db.UserTypes, "ID", "userType1", userProfile.userType);
            return View(userProfile);
        }

        // POST: UserProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userID,username,userFName,userSName,userPassword,studentNum,campusID,areaCode,cellNum,ban,yearOfStudy,payType,userType")] UserProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userProfile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","LearningMaterials");
            }
            ViewBag.campusID = new SelectList(db.Campus, "campusID", "campusName", userProfile.campusID);
            ViewBag.payType = new SelectList(db.PayTypes, "ID", "payType1", userProfile.payType);
            ViewBag.userType = new SelectList(db.UserTypes, "ID", "userType1", userProfile.userType);
            return View(userProfile);
        }

        // GET: UserProfiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProfile userProfile = db.UserProfiles.Find(id);
            if (userProfile == null)
            {
                return HttpNotFound();
            }
            return View(userProfile);
        }

        // POST: UserProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserProfile userProfile = db.UserProfiles.Find(id);
            db.UserProfiles.Remove(userProfile);
            db.SaveChanges();
            return RedirectToAction("Index");
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
