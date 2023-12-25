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
    public class LoginController : Controller
    {
        private StudentLinkUpDBEntities db = new StudentLinkUpDBEntities();

        // GET: Login
        public ActionResult Index()
        {
            var userProfiles = db.UserProfiles.Include(u => u.Campu).Include(u => u.PayType1).Include(u => u.UserType1);
            return View(userProfiles.ToList());
        }

        //Get Login page
        public ActionResult LogIn()
        {
            return View();
        }

        //POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn([Bind(Include = "userID,username,userPassword,signedIn")] UserProfile userProfile,String username,String Password)
        {
            int? id=0;
            string usrpswrd = "";

            foreach(UserProfile user in db.UserProfiles)
            {
                if(user.username.Equals(username))
                {
                    id = user.userID;
                    usrpswrd = user.userPassword;
                    break;
                }
                else
                {
                    id = null;
                }
            }
            /*if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/
            userProfile = db.UserProfiles.Find(id);

            if (userProfile == null)
            {
                return HttpNotFound();
            }

            if(ModelState.IsValid)
            {
                if (usrpswrd.Equals(Password))
                {
                    userProfile.signedIn = true;
                    SignInManager.signedIn.Enqueue(userProfile);
                    return RedirectToAction("Index", "LearningMaterials");
                }
            }

            return View(userProfile);
        }

        //Post: LogOut
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut(UserProfile user)
        {
                user.signedIn = false;
                SignInManager.Empty();
                return RedirectToAction("Login");
        }

        // GET: Login/Details/5
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

        // GET: Login/Create
        public ActionResult Create()
        {
            ViewBag.campusID = new SelectList(db.Campus, "campusID", "campusName");
            ViewBag.payType = new SelectList(db.PayTypes, "ID", "payType1");
            ViewBag.userType = new SelectList(db.UserTypes, "ID", "userType1");
            return View();
        }

        // POST: Login/Create
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
                return RedirectToAction("LogIn");
            }

            ViewBag.campusID = new SelectList(db.Campus, "campusID", "campusName", userProfile.campusID);
            ViewBag.payType = new SelectList(db.PayTypes, "ID", "payType1", userProfile.payType);
            ViewBag.userType = new SelectList(db.UserTypes, "ID", "userType1", userProfile.userType);
            return View(userProfile);
        }

        // GET: Login/Edit/5
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

        // POST: Login/Edit/5
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

        // GET: Login/Delete/5
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

        // POST: Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserProfile userProfile = db.UserProfiles.Find(id);
            db.UserProfiles.Remove(userProfile);
            db.SaveChanges();
            return RedirectToAction("LogOut");
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
