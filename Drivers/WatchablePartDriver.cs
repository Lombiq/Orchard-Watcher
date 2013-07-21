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

        protected override DriverResult Editor(WatchablePart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_WatchablePart_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts.WatchablePart",
                    Model: part,
                    Prefix: Prefix));
        }

        protected override DriverResult Editor(WatchablePart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(WatchablePart part, ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("WatcherIds", string.Join(",", part.WatcherIds));
        }

        protected override void Importing(WatchablePart part, ImportContentContext context)
        {
            var partName = part.PartDefinition.Name;

            context.ImportAttribute(partName, "WatcherIds", value => part.WatcherIds = value.Split(',').Select(id => int.Parse(id)));
        }
    }
}