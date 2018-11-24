using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HanimeliManti.BusinessLayer;
using HanimeliManti.Entities;
using HanimeliManti.Entities.ValueObjects;
using HanimeliManti.WebApp.Filters;
using HanimeliManti.WebApp.ViewModels;

namespace HanimeliManti.WebApp.Controllers
{
    [Exc]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            FoodManager fm = new FoodManager();

            return View(fm.GetAllFood().OrderByDescending(x => x.ModifiedOn).ToList());
            //return View(fm.GetAllFoodQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            FoodManager fm = new FoodManager();
            return View("Index", fm.GetAllFood().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [Auth]
        public ActionResult ShowProfile()
        {
            HanimeliUser currentUser = Session["login"] as HanimeliUser;

            HanimeliUserManager hum = new HanimeliUserManager();
            BusinessLayerResult<HanimeliUser> res = hum.GetUserById(currentUser.Id);

            if (res.Errors.Count>0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);

            }

            return View(res.Result);
        }
        [Auth]
        public ActionResult EditProfile()
        {
            HanimeliUser currentUser = Session["login"] as HanimeliUser;

            HanimeliUserManager hum = new HanimeliUserManager();
            BusinessLayerResult<HanimeliUser> res = hum.GetUserById(currentUser.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);

            }

            return View(res.Result);
        }

        [Auth]
        [HttpPost]
        public ActionResult EditProfile(HanimeliUser model, HttpPostedFileBase ProfileImage )
        {
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                    (ProfileImage.ContentType == "image/jpeg" ||
                     ProfileImage.ContentType == "image/jpg" ||
                     ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFilename = filename;
                }

                HanimeliUserManager hum = new HanimeliUserManager();
                BusinessLayerResult<HanimeliUser> res = hum.UpdateProfile(model);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi",
                        RedirectingUrl = "/Home/EditProfile"
                    };

                    return View("Error", errorNotifyObj);
                }

                Session["login"] = res.Result;

                return RedirectToAction("ShowProfile");

            }

            return View(model);
        }

        [Auth]
        public ActionResult RemoveProfile()
        {
            HanimeliUser currentUser = Session["login"] as HanimeliUser;

            HanimeliUserManager hum = new HanimeliUserManager();
            BusinessLayerResult<HanimeliUser> res = hum.RemoveUserById(currentUser.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi.",
                    RedirectingUrl = "/Home/ShowProfile"
                };

                return View("Error", errorNotifyObj);
            }

            Session.Clear();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                HanimeliUserManager hum = new HanimeliUserManager();
                BusinessLayerResult<HanimeliUser> res = hum.LoginUser(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));


                    return View(model);
                }

                Session["login"] = res.Result;
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                HanimeliUserManager hum = new HanimeliUserManager();
                BusinessLayerResult<HanimeliUser> res = hum.RegisterUser(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login"
                };

                notifyObj.Items.Add("Kayıt olma işleminizi başarıyla tamamladınız. Artık yemekleri yorumlayabilir veya beğenebilirsiniz. İyi Eğlenceler..");

                return View("Ok", notifyObj);
            }

            return View(model);
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult HasError()
        {
            return View();
        }
    }
}