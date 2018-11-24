using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HanimeliManti.Common;
using HanimeliManti.Entities;

namespace HanimeliManti.WebApp.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        {
            if (HttpContext.Current.Session["login"] != null)
            {
                HanimeliUser user = HttpContext.Current.Session["login"] as HanimeliUser;
                return user.Username;
            }

            return "system";
        }
    }
}