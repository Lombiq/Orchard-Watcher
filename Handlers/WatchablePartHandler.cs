using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lombiq.Watcher.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Services;

namespace Lombiq.Watcher.Handlers
{
    public class WatchablePartHandler : ContentHandler
    {
        public WatchablePartHandler(IRepository<WatchablePartRecord> repository, IJsonConverter jsonConverter)
        {
            Filters.Add(StorageFilter.For(repository));

            OnActivated<WatchablePart>((ctx, part) =>
                {
                    part.WatcherIdsField.Loader(() => jsonConverter.Deserialize<IEnumerable<int>>(part.Record.WatcherIdsSerialized));
                    part.WatcherIdsField.Setter(ids =>
                        {
                            part.Record.WatcherIdsSerialized = jsonConverter.Serialize(ids);
                            return ids;
                        });
                });
        }
    }
}