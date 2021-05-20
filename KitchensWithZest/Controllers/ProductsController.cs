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
    public class ProductsController : Controller
    {
        private KitchensWithZestEntities db = new KitchensWithZestEntities();

        // GET: Products
        public ActionResult Index()
        {
            List<Product> products = db.Products.OrderByDescending(x => x.ProductId).ToList();
            return View(products);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            } 
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
        
           // List<Product> products = db.Products.Where(x=>x.ProductId == id).ToList(); 
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, IEnumerable<HttpPostedFileBase> Photos)
        {
            if (ModelState.IsValid) 
            {
                string filename = Path.GetFileNameWithoutExtension(product.MainPhotoFile.FileName) 
                    + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(product.MainPhotoFile.FileName);
                product.MainPhotoPath = "~/Images/Products/" + filename;
                filename = Path.Combine(Server.MapPath("~/Images/Products/"), filename);
                product.MainPhotoFile.SaveAs(filename);

                db.Products.Add(product);

            

                //    if (Photos!=null)
                //    {
                //        var photolist = new List<Photo>();
                //        foreach (var photo in Photos)
                //        {
                //            using (var br = new BinaryReader(photo.InputStream))
                //            {
                //                var data = br.ReadBytes(photo.ContentLength);
                //                var pho = new Photo { ProductId = product.ProductId };
                //                pho.PhotoPath = data;
                //                photolist.Add(pho);
                //            }
                //        }
                //        product.ImagesPath = photolist;
                //    }

                db.SaveChanges();
                return RedirectToAction("Index");
                //return RedirectToAction("Details", new { id = product.ProductId });
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int ProductId, Product product, HttpPostedFileBase MainPhotoFile)
        {
            if (ModelState.IsValid)
            {
                string filename = Path.GetFileNameWithoutExtension(MainPhotoFile.FileName)
                    + DateTime.Now.ToString("yymmssfff")
                    + Path.GetExtension(MainPhotoFile.FileName);
                product.MainPhotoPath = "~/Images/Products/" + filename;
                //filename = Path.Combine(Server.MapPath("~/Images/Products/"), filename);
                MainPhotoFile.SaveAs(Path.Combine(Server.MapPath("~/Images/Products/"), filename));

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
