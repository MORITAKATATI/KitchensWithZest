using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KitchensWithZest.Models;

namespace KitchensWithZest.Controllers
{
    public class PhotosController : Controller
    {
        private KitchensWithZestEntities db = new KitchensWithZestEntities();

        // GET: Photos
        public ActionResult Index()
        {
            var photos = db.Photos.Include(p => p.Gallery).Include(p => p.Product);
            return View(photos.OrderByDescending(p=>p.PhotoId).ToList());
        }

        // GET: Photos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // GET: Photos/Create
        public ActionResult Create()
        {
            ViewBag.Gallery = new SelectList(db.Galleries, "GalleryId", "Title");
            ViewBag.Product = new SelectList(db.Products, "ProductId", "Title");
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "PhotoId,ProductId,GalleryId,PhotoPath")]*/ Photo photo, IEnumerable<HttpPostedFileBase> PhotoFile)
        {
            if (ModelState.IsValid)
            {
                foreach (var file in photo.PhotoFile)
                {
                    if (file.ContentLength > 0)
                    {
                        string filename = Path.GetFileNameWithoutExtension(file.FileName)
                            + DateTime.Now.ToString("yymmssfff")
                            + Path.GetExtension(file.FileName);
                        photo.PhotoPath = "~/Images/Photos/" + filename;
                        filename = Path.Combine(Server.MapPath("~/Images/Photos/"), filename);
                        file.SaveAs(filename);
                            
                        db.Photos.Add(photo);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }

            ViewBag.GalleryId = new SelectList(db.Galleries, "GalleryId", "Title", photo.GalleryId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Title", photo.ProductId);
            return View(photo);
        }

        // GET: Photos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            ViewBag.GalleryId = new SelectList(db.Galleries, "GalleryId", "Title", photo.GalleryId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Title", photo.ProductId);
            return View(photo);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int PhotoId, Photo photo, HttpPostedFileBase PhotoFiles)
        {
            if (ModelState.IsValid)
            {
                string filename = Path.GetFileNameWithoutExtension(PhotoFiles.FileName)
                    + DateTime.Now.ToString("yymmssfff")
                    + Path.GetExtension(PhotoFiles.FileName);
                photo.PhotoPath = "~/Images/Photos/" + filename;
                PhotoFiles.SaveAs(Path.Combine(Server.MapPath("~/Images/Photos/"), filename));

                db.Entry(photo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GalleryId = new SelectList(db.Galleries, "GalleryId", "Title", photo.GalleryId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Title", photo.ProductId);
            return View(photo);
        }

        // GET: Photos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Photo photo = db.Photos.Find(id);
            db.Photos.Remove(photo);
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
