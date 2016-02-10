using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZelectroCom.Data.Models;
using ZelectroCom.Service;
using ZelectroCom.Web.Infrastructure.Helpers;

namespace ZelectroCom.Web.Controllers
{
    public class OldMediaController : BaseWebController
    {
        private readonly IOldMediaService _oldMediaService;
        public OldMediaController(IOldMediaService oldMediaService)
        {
            _oldMediaService = oldMediaService;
        }

        public ActionResult Index(string url)
        {
            var oldMediaUrlsDict = MemoryCacheHelper.GetCachedData(MemoryCacheHelper.CacheConsts.OldMediaUrls,
                () => _oldMediaService.GetAll().ToDictionary(x => x.OldPath));
            if (oldMediaUrlsDict.ContainsKey(url))
            {
                return RedirectPermanent(oldMediaUrlsDict[url].NewPath);
            }
            
            return HttpNotFound();
        }
    }
}