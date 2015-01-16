using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lombiq.Watcher.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;

namespace Lombiq.Watcher.Drivers
{
    public class WatchablePartDriver : ContentPartDriver<WatchablePart>
    {
        protected override string Prefix
        {
            get { return "Lombiq.Watcher.WatchablePart"; }
        }


        protected override DriverResult Display(WatchablePart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_WatchablePart",
                () => shapeHelper.Parts_WatchablePart());
        }

        protected override void Exporting(WatchablePart part, ExportContentContext context)
        {
            context.Element(part.PartDefinition.Name).SetAttributeValue("WatcherIdsSerialized", part.WatcherIdsSerialized);
        }

        protected override void Importing(WatchablePart part, ImportContentContext context)
        {
            context.ImportAttribute(part.PartDefinition.Name, "WatcherIdsSerialized", value => part.WatcherIdsSerialized = value);
        }
    }
}