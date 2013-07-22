﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard;
using Orchard.Localization;
using Orchard.ContentManagement;
using Lombiq.Watcher.Models;

namespace Lombiq.Watcher.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOrchardServices _orchardServices;

        public Localizer T { get; set; }


        public HomeController(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;

            T = NullLocalizer.Instance;
        }
	
			
        [HttpPost]
        public ActionResult Watch(int itemId)
        {
            ContentItem item;
            ActionResult invalidResult;
            if (!IsValid(itemId, out item, out invalidResult)) return invalidResult;

            item.As<WatchablePart>().AddWatcher(_orchardServices.WorkContext.CurrentUser.ContentItem.Id);

            return Json(new WatchResponse { Message = T("You're now watching this item.").Text });
        }

        [HttpPost]
        public ActionResult UnWatch(int itemId)
        {
            ContentItem item;
            ActionResult invalidResult;
            if (!IsValid(itemId, out item, out invalidResult)) return invalidResult;

            return Json(new WatchResponse { Message = T("You don't watch this item anymore.").Text });
        }


        private bool IsValid(int itemId, out ContentItem item, out ActionResult invalidResult)
        {
            if (_orchardServices.WorkContext.CurrentUser == null || !_orchardServices.Authorizer.Authorize(Permissions.WatchItems))
            {
                invalidResult = new HttpUnauthorizedResult();
                item = null;
                return false;
            }

            item = _orchardServices.ContentManager.Get(itemId);

            if (item == null)
            {
                invalidResult = HttpNotFound();
                return false;
            }

            if (!_orchardServices.Authorizer.Authorize(Orchard.Core.Contents.Permissions.ViewContent))
            {
                invalidResult = new HttpUnauthorizedResult();
                return false;
            }

            if (!item.Has<WatchablePart>())
            {
                invalidResult = Json(new WatchResponse { Message = T("You can't watch this item.").Text });
                return false;
            }

            invalidResult = null;
            return true;
        }
    }
}