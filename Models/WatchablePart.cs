using System;
using System.Collections.Generic;
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
        private readonly LazyField<IEnumerable<int>> _watcherIds = new LazyField<IEnumerable<int>>();
        internal LazyField<IEnumerable<int>> WatcherIdsField { get { return _watcherIds; } }
        public IEnumerable<int> WatcherIds
        {
            get { return _watcherIds.Value; }
            set { _watcherIds.Value = value; }
        }
    }


    public class WatchablePartRecord : ContentPartRecord
    {
        [StringLengthMax]
        public string WatcherIdsSerialized { get; set; }
    }
}