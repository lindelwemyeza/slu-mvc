using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentLinkUp_MVC_;

namespace StudentLinkUp_MVC_.Controllers
{
    public class LearningMaterialsController : Controller
    {
        private StudentLinkUpDBEntities db = new StudentLinkUpDBEntities();

        // GET: LearningMaterials
        public ActionResult Index(String searchString,String materialType)
        {
            MeetingsManager.Empty();

            var typeList = new List<String>();
            var qryString = from a in db.LearningMaterials select a;
            var qryString2 = from b in db.LearningMaterials select b;
            var qryString3 = from c in db.LearningMaterials select c;
            var qryString4 = from d in db.LearningMaterials select d;
            var qryString5 = from e in db.LearningMaterials select e;
            var qryString6 = from f in db.LearningMaterials select f;

            typeList.Add("Textbook");
            typeList.Add("Notes");
            ViewBag.materialType = new SelectList(typeList);

            if (!String.IsNullOrEmpty(searchString))
            {
                qryString = qryString.Where(t => t.title.Contains(searchString));
                if (!qryString.Any())
                {
                    qryString2 = qryString2.Where(t => t.author.Contains(searchString));
                    qryString = qryString2;
                    if (!qryString2.Any())
                    {
                        qryString3 = qryString3.Where(t => t.publisher.Contains(searchString));
                        qryString = qryString3;
                        if (!qryString3.Any())
                        {
                            qryString4 = qryString4.Where(t => t.moduleName.Contains(searchString));
                            qryString = qryString4;
                            if (!qryString4.Any())
                            {
                                qryString5 = qryString5.Where(t => t.Module.moduleName.Contains(searchString));
                                qryString = qryString5;
                                if (!qryString5.Any())
                                {
                                    qryString6 = qryString6.Where(t => t.UserProfile.username.Contains(searchString));
                                    qryString = qryString6;
                                }
                            }
                        }
                    }
                }
            }

            if (!String.IsNullOrEmpty(materialType) && materialType.Equals("Textbook"))
            {
                qryString = qryString.Where(t => t.materialType == 1);
            }
            if (!String.IsNullOrEmpty(materialType) && materialType.Equals("Notes"))
            {
                qryString = qryString.Where(t => t.materialType == 2);
            }

            return View(qryString);
        }

        // GET: LearningMaterials/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LearningMaterial learningMaterial = db.LearningMaterials.Find(id);
            if (learningMaterial == null)
            {
                return HttpNotFound();
            }
            return View(learningMaterial);
        }

        // GET: LearningMaterials/Create
        public ActionResult Create()
        {
            ViewBag.condition = new SelectList(db.Conditions, "ID", "condition1");
            ViewBag.materialType = new SelectList(db.ItemTypes, "ID", "itemType1");
            ViewBag.moduleName = new SelectList(db.Modules, "moduleCode", "moduleName");
            return View();
        }

        // POST: LearningMaterials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "materialID,materialType,username,title,author,publisher,edition,price,condition,moduleName,available,Bookimage")] LearningMaterial learningMaterial,HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if(file!=null && file.ContentLength >0)
                {
                    string filename = Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.ToString("yymmssff");
                    filename = filename + Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Image/"), filename);
                    file.SaveAs(path);
                    learningMaterial.Bookimage = filename;
                }
                db.LearningMaterials.Add(learningMaterial);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.condition = new SelectList(db.Conditions, "ID", "condition1", learningMaterial.condition);
            ViewBag.materialType = new SelectList(db.ItemTypes, "ID", "itemType1", learningMaterial.materialType);
            ViewBag.moduleName = new SelectList(db.Modules, "moduleCode", "moduleName", learningMaterial.moduleName);
            ViewBag.username = ((UserProfile)(SignInManager.signedIn.Peek())).userFName + " " + ((UserProfile)(SignInManager.signedIn.Peek())).userSName;

            return View(learningMaterial);
        }

        // GET: LearningMaterials/Edit/5
        public ActionResult Edit(int? id)
        {
            MeetingsManager.Empty();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LearningMaterial learningMaterial = db.LearningMaterials.Find(id);
            if (learningMaterial == null)
            {
                return HttpNotFound();
            }
            ViewBag.condition = new SelectList(db.Conditions, "ID", "condition1", learningMaterial.condition);
            ViewBag.materialType = new SelectList(db.ItemTypes, "ID", "itemType1", learningMaterial.materialType);
            ViewBag.moduleName = new SelectList(db.Modules, "moduleCode", "moduleName", learningMaterial.moduleName);
            return View(learningMaterial);
        }

        // POST: LearningMaterials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "materialID,materialType,username,title,author,publisher,edition,price,condition,moduleName,available,Bookimage")] LearningMaterial learningMaterial,HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string filename = Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.ToString("yymmssff");
                    filename = filename + Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Image/"), filename);
                    file.SaveAs(path);
                    learningMaterial.Bookimage = path;
                }
            
                db.Entry(learningMaterial).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.condition = new SelectList(db.Conditions, "ID", "condition1", learningMaterial.condition);
            ViewBag.materialType = new SelectList(db.ItemTypes, "ID", "itemType1", learningMaterial.materialType);
            ViewBag.moduleName = new SelectList(db.Modules, "moduleCode", "moduleName", learningMaterial.moduleName);
            ViewBag.username = ((UserProfile)(SignInManager.signedIn.Peek())).userFName + " " + ((UserProfile)(SignInManager.signedIn.Peek())).userSName;

            return View(learningMaterial);
        }

        // GET: LearningMaterials/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LearningMaterial learningMaterial = db.LearningMaterials.Find(id);
            if (learningMaterial == null)
            {
                return HttpNotFound();
            }
            return View(learningMaterial);
        }

        // POST: LearningMaterials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LearningMaterial learningMaterial = db.LearningMaterials.Find(id);
            string imgName = learningMaterial.Bookimage;
            string imgPath = Path.Combine(Server.MapPath("~Image"), imgName);
            if(System.IO.File.Exists(imgPath))
            {
                System.IO.File.Delete(imgPath);
            }
            db.LearningMaterials.Remove(learningMaterial);
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
