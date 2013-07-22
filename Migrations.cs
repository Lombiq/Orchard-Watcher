using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lombiq.Watcher.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Lombiq.Watcher
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable(typeof(WatchablePartRecord).Name,
                table => table
                    .ContentPartRecord()
                    .Column<string>("WatcherIdsSerialized", column => column.Unlimited())
				);

            ContentDefinitionManager.AlterPartDefinition(typeof(WatchablePart).Name,
                part => part
                    .Attachable()
                    .WithDescription("Makes a content item watchable, i.e. users can sign up to get notified about events related to the item.")
                );


            return 1;
        }
    }
}