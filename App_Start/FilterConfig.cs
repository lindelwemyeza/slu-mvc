﻿using System.Web;
using System.Web.Mvc;

namespace StudentLinkUp_MVC_
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
