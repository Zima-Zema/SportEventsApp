﻿using System.Web;
using System.Web.Mvc;

namespace SportEventsApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //bearer
            //filters.Add(new AuthorizeAttribute());
        }
    }
}
