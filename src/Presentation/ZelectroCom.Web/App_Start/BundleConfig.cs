﻿using System.Web;
using System.Web.Optimization;

namespace ZelectroCom.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/formValidation").Include(
            //          "~/Scripts/FormValidation/formValidation.min.js",
            //          "~/Scripts/FormValidation/bootstrap.min.js",
            //          "~/Scripts/FormValidation/formValidationru_RU.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/postcss").Include(
                      "~/Scripts/ckeditor/plugins/codesnippet/lib/highlight/styles/github-gist.css")
                      .Include("~/Scripts/ckeditor/plugins/spoiler/css/spoiler.css", new CssRewriteUrlTransform()));
                      
            //Member bundles
            bundles.Add(new StyleBundle("~/Content/membercss").Include(
                "~/Content/font-awesome.min.css",
                "~/Areas/Member/Content/style.css")
                .Include("~/Scripts/BForms/Bundles/css/*.css", new CssRewriteUrlTransform()));
        }
    }
}
