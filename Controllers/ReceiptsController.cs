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
    public class ReceiptsController : Controller
    {
        private StudentLinkUpDBEntities db = new StudentLinkUpDBEntities();

        // GET: Receipts
        public ActionResult Index()
        {
            var receipts = db.Receipts.Include(r => r.Meeting);
            return View(receipts.ToList());
        }

        // GET: Receipts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receipt receipt = db.Receipts.Find(id);
            if (receipt == null)
            {
                return HttpNotFound();
            }
            return View(receipt);
        }

        // GET: Receipts/Create
        public ActionResult Create()
        {
            ViewBag.meetingID = new SelectList(db.Meetings, "meetingID", "meetingLocation");
            return View();
        }

        // POST: Receipts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "receiptNum,meetingID,itemName,total,meetingDate,meetingTime")] Receipt receipt)
        {
            if (ModelState.IsValid)
            {
                db.Receipts.Add(receipt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.meetingID = new SelectList(db.Meetings, "meetingID", "meetingLocation", receipt.meetingID);
            return View(receipt);
        }

        // GET: Receipts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receipt receipt = db.Receipts.Find(id);
            if (receipt == null)
            {
                return HttpNotFound();
            }
            ViewBag.meetingID = new SelectList(db.Meetings, "meetingID", "meetingLocation", receipt.meetingID);
            return View(receipt);
        }

        // POST: Receipts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "receiptNum,meetingID,itemName,total,meetingDate,meetingTime")] Receipt receipt)
        {
            if (ModelState.IsValid)
            {
                db.Entry(receipt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.meetingID = new SelectList(db.Meetings, "meetingID", "meetingLocation", receipt.meetingID);
            return View(receipt);
        }

        // GET: Receipts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receipt receipt = db.Receipts.Find(id);
            if (receipt == null)
            {
                return HttpNotFound();
            }
            return View(receipt);
        }

        // POST: Receipts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Receipt receipt = db.Receipts.Find(id);
            db.Receipts.Remove(receipt);
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
