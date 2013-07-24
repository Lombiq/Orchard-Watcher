using System;
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
    public class WatchController : Controller
    {
        private readonly IOrchardServices _orchardServices;

        public Localizer T { get; set; }


        public WatchController(IOrchardServices orchardServices)
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

            var ids = item.As<WatchablePart>().WatcherIds;
            var userId = _orchardServices.WorkContext.CurrentUser.ContentItem.Id;
            if (!ids.Contains(userId)) ids.Add(userId);

            return Json(new WatchResponse { WatcherCount = ids.Count, Message = T("You're now watching this item.").Text });
        }

        [HttpPost]
        public ActionResult UnWatch(int itemId)
        {
            ContentItem item;
            ActionResult invalidResult;
            if (!IsValid(itemId, out item, out invalidResult)) return invalidResult;

            var ids = item.As<WatchablePart>().WatcherIds;
            var userId = _orchardServices.WorkContext.CurrentUser.ContentItem.Id;
            if (ids.Contains(userId)) ids.Remove(userId);

            return Json(new WatchResponse { WatcherCount = ids.Count, Message = T("You don't watch this item anymore.").Text });
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
                invalidResult = HttpNotFound();
                return false;
            }

            invalidResult = null;
            return true;
        }
    }
}