using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HanimeliManti.Entities;

namespace HanimeliManti.WebApp.Models
{
    public class CurrentSession
    {
        //Property i static yaptım. Çünkü new'lemeden erişmek istiyorum.
        public static HanimeliUser User
        {
            get { return Get<HanimeliUser>("login"); }

        }

        public static void Set<T>(string key, T obj)
        {
            HttpContext.Current.Session[key] = obj;
        }

        public static T Get<T>(string key)
        {
            if (HttpContext.Current.Session[key]!= null)
            {
                return (T)HttpContext.Current.Session[key];
            }

            return default(T);
        }

        public static void Remove(string key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                HttpContext.Current.Session.Remove(key);
            }

        }

        public static void Clear()
        {
             HttpContext.Current.Session.Clear();
        }
    }
}