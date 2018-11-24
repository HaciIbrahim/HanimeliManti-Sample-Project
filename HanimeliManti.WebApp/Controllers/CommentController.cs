using HanimeliManti.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HanimeliManti.BusinessLayer;
using HanimeliManti.WebApp.Models;
using HanimeliManti.WebApp.Filters;

namespace HanimeliManti.WebApp.Controllers
{
    [Exc]
    public class CommentController : Controller
    {
        private FoodManager foodManager = new FoodManager();
        private CommentManager commentManager = new CommentManager();


        public ActionResult ShowFoodComments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest);
            }

            //Food food = foodManager.Find(x => x.Id == id.Value); Bu sorgu iki kez çalışır. Aşağıdaki tekte getirir.

            Food food = foodManager.ListQueryable().Include("Comments").FirstOrDefault(x => x.Id == id.Value);

            if (food == null)
            {
                return HttpNotFound();
            }

            return PartialView("_PartialComments", food.Comments);
        }

        [Auth]
        [HttpPost]
        public ActionResult Edit(int? id, string text)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = commentManager.Find(x => x.Id == id.Value);

            if (comment == null)
            {
                return new HttpNotFoundResult();
            }

            comment.Text = text;

            if (commentManager.Update(comment) > 0)
            {
                return Json(new {result = true}, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = commentManager.Find(x => x.Id == id.Value);

            if (comment == null)
            {
                return new HttpNotFoundResult();
            }

            if (commentManager.Delete(comment) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);

        }

        [Auth]
        [HttpPost]
        public ActionResult Create(Comment comment ,int? foodid)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                if (foodid == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Food food = foodManager.Find(x => x.Id == foodid);

                if (food == null)
                {
                    return new HttpNotFoundResult();
                }

                comment.Food = food;
                comment.Owner = CurrentSession.User;


                if (commentManager.Insert(comment) > 0)
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }

            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

    }
}