using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public WatchablePartHandler(
            IRepository<WatchablePartRecord> repository,
            IJsonConverter jsonConverter)
        {
            Filters.Add(StorageFilter.For(repository));

            OnActivated<WatchablePart>((ctx, part) =>
                {
                    part.WatcherIdsField.Loader(() =>
                        {
                            var seed = string.IsNullOrEmpty(part.WatcherIdsSerialized) ? Enumerable.Empty<int>() : jsonConverter.Deserialize<IEnumerable<int>>(part.WatcherIdsSerialized);
                            var collection = new ObservableCollection<int>(seed);
                            collection.CollectionChanged += (sender, e) =>
                                {
                                    part.WatcherIdsSerialized = jsonConverter.Serialize((IEnumerable<int>)collection);
                                };
                            return collection;
                        });
                });
        }
    }
}