using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Org.BouncyCastle.Asn1.Cms;
using StudentLinkUp_MVC_;

namespace StudentLinkUp_MVC_.Controllers
{
    public class MeetingsController : Controller
    {
        private StudentLinkUpDBEntities db = new StudentLinkUpDBEntities();

        // GET: Meetings
        public ActionResult Index()
        {
            var meetings = db.Meetings.Include(m => m.LearningMaterial).Include(m => m.UserProfile).Include(m => m.UserProfile1);
            return View(meetings.ToList());
        }

        // GET: Meetings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meeting meeting = db.Meetings.Find(id);
            if (meeting == null)
            {
                return HttpNotFound();
            }
            return View(meeting);
        }

        // GET: Meetings/Create
        public ActionResult Create()
        {
            ViewBag.materialID = new SelectList(db.LearningMaterials, "materialID", "title");
            return View();
        }

        // POST: Meetings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "meetingID,meetingLocation,meetingDate,meetingTime,success,userID_buyer,userID_seller,materialID")] Meeting meeting)
        {
            if (ModelState.IsValid)
            {
                UserProfile temp = MeetingsManager.GetParticipant();
                meeting.userID_buyer = ((UserProfile)SignInManager.signedIn.Peek()).userID;
                meeting.userID_seller = temp.userID;
                MeetingsManager.AddParticipant(temp);
                meeting.materialID = MeetingsManager.GetItem().materialID;
                db.Meetings.Add(meeting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.materialID = new SelectList(db.LearningMaterials, "materialID", "title", meeting.materialID);
            return View(meeting);
        }

        // GET: Meetings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meeting meeting = db.Meetings.Find(id);
            if (meeting == null)
            {
                return HttpNotFound();
            }
            UserProfile buyer = db.UserProfiles.Find(meeting.userID_buyer);
            UserProfile seller = db.UserProfiles.Find(meeting.userID_seller);
            LearningMaterial material = db.LearningMaterials.Find(meeting.materialID);
            MeetingsManager.AddParticipant(buyer);
            MeetingsManager.AddParticipant(seller);
            MeetingsManager.AddMaterial(material);
            db.Set<Meeting>().AddOrUpdate(meeting);
            return View(meeting);
        }

        // POST: Meetings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "meetingID,meetingLocation,meetingDate,meetingTime,success,userID_buyer,userID_seller,materialID")] Meeting meeting)
        {

            if (ModelState.IsValid)
            {
                LearningMaterial material = MeetingsManager.GetItem();

                if (meeting.success == true)
                {
                    Receipt receipt = new Receipt();

                    material.available = false;
                    
                    //db.SaveChanges();

                    if (ModelState.IsValid)
                    {
                        receipt.meetingID = meeting.meetingID;
                        receipt.itemName = material.materialID;
                        receipt.total = material.price;
                        receipt.meetingDate = meeting.meetingDate;
                        receipt.meetingTime = meeting.meetingTime;
                        db.Receipts.Add(receipt);
                        // db.SaveChanges();
                    }

                    ReceiptsManager.AddReceipt(receipt);
                    ReceiptsManager.AddPendingMaterial(material);
                    MeetingsManager.closeSale();
                }

                MeetingsManager.AddMaterial(material);

                ViewBag.userID_buyer = MeetingsManager.GetParticipant().userID;
                ViewBag.userID_seller = MeetingsManager.GetParticipant().userID;
                ViewBag.materialID = MeetingsManager.GetItem().materialID;

                /*db.Entry(material).State = EntityState.Modified;
                db.Entry(meeting).Property(t => t.meetingTime).IsModified = true;*/
                db.Set<Meeting>().AddOrUpdate(meeting);
                //db.Entry(meeting).State = EntityState.Modified;
                db.SaveChanges();
                MeetingsManager.Empty();
                return RedirectToAction("Index");
            }
           
            return View(meeting);
        }

        // GET: Meetings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meeting meeting = db.Meetings.Find(id);
            if (meeting == null)
            {
                return HttpNotFound();
            }
            return View(meeting);
        }

        // POST: Meetings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Meeting meeting = db.Meetings.Find(id);
            db.Meetings.Remove(meeting);
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
