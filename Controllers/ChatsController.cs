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
    public class ChatsController : Controller
    {
        private StudentLinkUpDBEntities db = new StudentLinkUpDBEntities();

        // GET: Chats
        public ActionResult Index()
        {
            var userMessages = db.UserMessages.Include(u => u.UserProfile).Include(u => u.UserProfile1);
            return View(userMessages.ToList());
        }

        // GET: Chats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMessage userMessage = db.UserMessages.Find(id);
            if (userMessage == null)
            {
                return HttpNotFound();
            }
            return View(userMessage);
        }

        // GET: Chats/Create
        public ActionResult Create()
        {
            ViewBag.recieverID = new SelectList(db.UserProfiles, "userID", "username");
            ViewBag.senderID = new SelectList(db.UserProfiles, "userID", "username");
            return View();
        }

        // POST: Chats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "messageID,senderID,recieverID,containedText,messageDate,messageTime")] UserMessage userMessage)
        {
            if (ModelState.IsValid)
            {
                db.UserMessages.Add(userMessage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.recieverID = new SelectList(db.UserProfiles, "userID", "username", userMessage.recieverID);
            ViewBag.senderID = new SelectList(db.UserProfiles, "userID", "username", userMessage.senderID);
            return View(userMessage);
        }

        // GET: Chats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMessage userMessage = db.UserMessages.Find(id);
            if (userMessage == null)
            {
                return HttpNotFound();
            }
            ViewBag.recieverID = new SelectList(db.UserProfiles, "userID", "username", userMessage.recieverID);
            ViewBag.senderID = new SelectList(db.UserProfiles, "userID", "username", userMessage.senderID);
            return View(userMessage);
        }

        // POST: Chats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "messageID,senderID,recieverID,containedText,messageDate,messageTime")] UserMessage userMessage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userMessage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.recieverID = new SelectList(db.UserProfiles, "userID", "username", userMessage.recieverID);
            ViewBag.senderID = new SelectList(db.UserProfiles, "userID", "username", userMessage.senderID);
            return View(userMessage);
        }

        // GET: Chats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMessage userMessage = db.UserMessages.Find(id);
            if (userMessage == null)
            {
                return HttpNotFound();
            }
            return View(userMessage);
        }

        // POST: Chats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserMessage userMessage = db.UserMessages.Find(id);
            db.UserMessages.Remove(userMessage);
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
