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
using HanimeliManti.WebApp.Models;

namespace HanimeliManti.WebApp.Controllers
{
    [Exc]
    public class FoodController : Controller
    {
        FoodManager foodManager = new FoodManager();
        LikedManager likedManager = new LikedManager();

        [Auth]
        [AuthAdmin]
        public ActionResult Index()
        {
            var foods = foodManager.ListQueryable().OrderByDescending(x => x.ModifiedOn);
            return View(foods.ToList());
        }

        [Auth]
        [AuthAdmin]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Food food = foodManager.Find(x => x.Id == id.Value);

            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        [Auth]
        [AuthAdmin]
        public ActionResult Create()
        {
            return View();
        }

        [Auth]
        [AuthAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Food food)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                food.Owner = CurrentSession.User;
                foodManager.Insert(food);

                return RedirectToAction("Index");
            }

            return View(food);
        }

        [Auth]
        [AuthAdmin]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = foodManager.Find(x => x.Id == id.Value);

            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        [Auth]
        [AuthAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Food food)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                Food db_food = foodManager.Find(x => x.Id == food.Id);
                db_food.Name = food.Name;
                db_food.Description = food.Description;
                db_food.Price = food.Price;

                foodManager.Update(db_food);

                return RedirectToAction("Index");
            }
            return View(food);
        }

        [Auth]
        [AuthAdmin]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = foodManager.Find(x => x.Id == id.Value);
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        [Auth]
        [AuthAdmin]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Food food = foodManager.Find(x => x.Id == id);
            foodManager.Delete(food);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GetLiked (int[] ids)
        {

            if (CurrentSession.User != null)
            {
                List<int> likedFoodIds = likedManager.List(
                    x => x.LikedUser.Id == CurrentSession.User.Id && ids.Contains(x.Food.Id)).Select(
                    x => x.Food.Id).ToList();

                return Json(new { result = likedFoodIds });
            }
            else
            {
                return Json(new { result = new List<int>() });
            }
        }

        [HttpPost]
        public ActionResult SetLikeState(int foodid, bool liked)
        {
            int res = 0;

            if (CurrentSession.User == null)
                return Json(new { hasError = true, errorMessage = "Beğenme işlemi için giriş yapmalısınız.", result = 0 });

            Liked like = likedManager.Find(x => x.Food.Id == foodid && x.LikedUser.Id == CurrentSession.User.Id);

            Food food = foodManager.Find(x => x.Id == foodid);

            if (like != null && liked == false)
            {
                res = likedManager.Delete(like);
            }
            else if (like == null && liked == true)
            {
                res = likedManager.Insert(new Liked()
                {
                    LikedUser = CurrentSession.User,
                    Food = food
                });
            }

            if (res > 0)
            {
                if (liked)
                {
                    food.LikeCount++;
                }
                else
                {
                    food.LikeCount--;
                }

               res = foodManager.Update(food);
                return Json(new
                {
                    hasError = false,
                    errorMessage = string.Empty,
                    result = food.LikeCount
                });
            }

            return Json(new
            {
                hasError = true,
                errorMessage = " Beğenme işlemi başarısız",
                result = food.LikeCount
            });
        }


    }
}
