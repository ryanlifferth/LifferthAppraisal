using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LifferthAppraisal.Common
{
    public class MyRazorViewEngine : RazorViewEngine
    {

        private static string[] NewPartialViewFormats = new[] { 
                "~/Views/{1}/PartialViews/{0}.cshtml",                
                "~/Views/Shared/PartialViews/{0}.cshtml"
        };

        public MyRazorViewEngine()
        {
            base.PartialViewLocationFormats = base.PartialViewLocationFormats.Union(NewPartialViewFormats).ToArray();
        }
    }
}
