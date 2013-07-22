using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lombiq.Watcher.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Localization;
using Orchard.Security;
using Orchard.Workflows.Models;
using Orchard.Workflows.Services;

namespace Lombiq.Watcher.Activities
{
    public class RunForWatchersActivity : Task
    {
        private readonly IContentManager _contentManager;
        private readonly string _stateKey;

        public override string Name
        {
            get { return "RunForWatchers"; }
        }

        public override LocalizedString Category
        {
            get { return T("Notification"); }
        }

        public override LocalizedString Description
        {
            get { return T("Runs another activity for each of the item's watchers"); }
        }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            return new[] { T("Done"), T("Run for the next watcher") };
        }

        public Localizer T { get; set; }


        public RunForWatchersActivity(IContentManager contentManager)
        {
            _contentManager = contentManager;

            _stateKey = Name + ".WatcherIds";
        }


        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            var nextUserId = PopNextWatcherId(workflowContext, activityContext);

            if (nextUserId != 0)
            {
                workflowContext.Tokens["User"] = _contentManager.Get(nextUserId).As<IUser>();
                yield return T("Run for the next watcher");
            }
            else
            {
                yield return T("Done");
            }
        }


        private int PopNextWatcherId(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            TrySetupWatcherIdsState(workflowContext, activityContext);

            var watcherIdsSerialized = workflowContext.GetStateFor<string>(activityContext.Record, _stateKey);
            var split = watcherIdsSerialized.Split(new[] { ',' }, 2, StringSplitOptions.RemoveEmptyEntries);

            if (split.Length == 0) return 0; // No more IDs

            if (split.Length == 1)
            {
                workflowContext.SetStateFor(activityContext.Record, _stateKey, string.Empty);
            }
            else
            {
                workflowContext.SetStateFor(activityContext.Record, _stateKey, split[1]);
            }

            return int.Parse(split[0]);
        }

        private void TrySetupWatcherIdsState(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            if (workflowContext.HasStateFor(activityContext.Record, _stateKey)) return;

            // It seems that not even arrays are supported to be saved to the state, hence the string
            workflowContext.SetStateFor(activityContext.Record, _stateKey, string.Join(",", CumulatedWatcherIds(workflowContext.Content)));
        }


        /// <summary>
        /// Combining watchers from this item and its containers
        /// </summary>
        private static IEnumerable<int> CumulatedWatcherIds(IContent content)
        {
            var watcherIds = new List<int>();

            for (int i = 0; i < 3; i++) // Only going three levels up
            {
                if (content == null) return watcherIds;

                if (content.Has<WatchablePart>())
                {
                    watcherIds.AddRange(content.As<WatchablePart>().WatcherIds);
                }

                var commonPart = content.As<ICommonPart>();
                if (commonPart == null) return watcherIds;
                content = commonPart.Container;
            }

            return watcherIds;
        }
    }
}