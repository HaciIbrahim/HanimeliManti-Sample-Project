using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HanimeliManti.BusinessLayer;
using HanimeliManti.Entities;
using HanimeliManti.WebApp.Filters;

namespace HanimeliManti.WebApp.Controllers
{
    [Auth]
    [AuthAdmin]
    [Exc]
    public class HanimeliUserController : Controller
    {
        private HanimeliUserManager hanimeliUserManager = new HanimeliUserManager();

        public ActionResult Index()
        {
            return View(hanimeliUserManager.List());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            HanimeliUser hanimeliUser = hanimeliUserManager.Find(x => x.Id == id.Value);

            if (hanimeliUser == null)
            {
                return HttpNotFound();
            }

            return View(hanimeliUser);
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( HanimeliUser hanimeliUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<HanimeliUser> res = hanimeliUserManager.Insert(hanimeliUser);

                if (res.Errors.Count>0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("",x.Message));
                    return View(hanimeliUser);
                }
                
                return RedirectToAction("Index");
            }

            return View(hanimeliUser);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HanimeliUser hanimeliUser = hanimeliUserManager.Find(x => x.Id == id.Value);

            if (hanimeliUser == null)
            {
                return HttpNotFound();
            }

            return View(hanimeliUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( HanimeliUser hanimeliUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<HanimeliUser> res = hanimeliUserManager.Update(hanimeliUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(hanimeliUser);
                }

                return RedirectToAction("Index");
            }
            return View(hanimeliUser);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HanimeliUser hanimeliUser = hanimeliUserManager.Find(x => x.Id == id.Value);

            if (hanimeliUser == null)
            {
                return HttpNotFound();
            }
            return View(hanimeliUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HanimeliUser hanimeliUser = hanimeliUserManager.Find(x => x.Id == id);
            hanimeliUserManager.Delete(hanimeliUser);

            return RedirectToAction("Index");
        }

    }
}
