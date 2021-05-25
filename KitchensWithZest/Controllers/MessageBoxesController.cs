using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KitchensWithZest.Models;

namespace KitchensWithZest.Controllers
{
    public class MessageBoxesController : Controller
    {
        private KitchensWithZestEntities db = new KitchensWithZestEntities();

        // GET: MessageBoxes
        public ActionResult Index()
        {
            return View(db.MessageBoxes.OrderByDescending(x=>x.MessageId).ToList());
        }

        // GET: MessageBoxes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageBox messageBox = db.MessageBoxes.Find(id);
            if (messageBox == null)
            {
                return HttpNotFound();
            }
            return View(messageBox);
        }

        // GET: MessageBoxes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MessageBoxes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "MessageId,FirstName,LastName,Phone,Email,Subject,Content,SubmissionTime")] */MessageBox messageBox)
        {
            //if (messageBox != null)
            //{
            //    messageBox.SubmissionTime = DateTime.Now;
            //}
            if (ModelState.IsValid)
            {
                
                DateTime dateTime = DateTime.Now;
                messageBox.SubmissionTime = dateTime;
                db.MessageBoxes.Add(messageBox);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(messageBox);
        }


        // GET: MessageBoxes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageBox messageBox = db.MessageBoxes.Find(id);
            if (messageBox == null)
            {
                return HttpNotFound();
            }
            return View(messageBox);
        }

        // POST: MessageBoxes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MessageId,FirstName,LastName,Phone,Email,Subject,Content,SubmissionTime")] MessageBox messageBox)
        {
            if (ModelState.IsValid)
            {
                db.Entry(messageBox).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(messageBox);
        }

        // GET: MessageBoxes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageBox messageBox = db.MessageBoxes.Find(id);
            if (messageBox == null)
            {
                return HttpNotFound();
            }
            return View(messageBox);
        }

        // POST: MessageBoxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MessageBox messageBox = db.MessageBoxes.Find(id);
            db.MessageBoxes.Remove(messageBox);
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
