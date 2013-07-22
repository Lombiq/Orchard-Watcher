using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using Orchard.Core.Common.Utilities;
using Orchard.Data.Conventions;

namespace Lombiq.Watcher.Models
{
    public class WatchablePart : ContentPart<WatchablePartRecord>
    {
        private readonly LazyField<ObservableCollection<int>> _watcherIds = new LazyField<ObservableCollection<int>>();
        internal LazyField<ObservableCollection<int>> WatcherIdsField { get { return _watcherIds; } }
        public IList<int> WatcherIds
        {
            get { return _watcherIds.Value; }
            set
            {
                WatcherIdsField.Value.Clear();

                // Adding items one by one is not particularly nice, but simple and the setter will be used only by import anyway.
                foreach (var id in value)
                {
                    WatcherIdsField.Value.Add(id);
                }
            }
        }

    }


    public class WatchablePartRecord : ContentPartRecord
    {
        [StringLengthMax]
        public virtual string WatcherIdsSerialized { get; set; }
    }
}