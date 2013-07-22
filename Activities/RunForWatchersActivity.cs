using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.Localization;
using Orchard.Workflows.Models;
using Orchard.Workflows.Services;

namespace Lombiq.Watcher.Activities
{
    public class RunForWatchersActivity : Task
    {
        public Localizer T { get; set; }


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

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            var counter = GetState(workflowContext, activityContext);
            if (counter < 3)
            {
                SetState(workflowContext, activityContext, ++counter);
                yield return T("Run for the next watcher");
            }
            else if (counter == 3)
            {
                SetState(workflowContext, activityContext, ++counter);
                yield return T("Done");
            }
            else
            {
                yield break;
            }
        }


        private int GetState(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            if (!workflowContext.HasStateFor(activityContext.Record, Name + ".State"))
            {
                SetState(workflowContext, activityContext, 0);
                return 0;
            }

            return workflowContext.GetStateFor<int>(activityContext.Record, Name + ".State");
        }

        private void SetState(WorkflowContext workflowContext, ActivityContext activityContext, int state)
        {
            workflowContext.SetStateFor(activityContext.Record, Name + ".State", state);
        }
    }
}