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
        private readonly IWorkContextAccessor _wca;

        public Localizer T { get; set; }


        public RunForWatchersActivity(IWorkContextAccessor wca)
        {
            _wca = wca;
        }



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
            var counter = GetState();
            if (counter < 3)
            {
                SetState(++counter);
                yield return T("Run for the next watcher");
            }
            else if (counter == 3)
            {
                SetState(++counter);
                yield return T("Done");
            }
            else
            {
                yield break;
            }
        }


        private int GetState()
        {
            var wc = _wca.GetContext();
            var state = wc.GetState<int?>(Name + ".State");

            if (state == null)
            {
                SetState(0);
                return 0;
            }

            return state.Value;
        }

        private void SetState(int state)
        {
            _wca.GetContext().SetState<int?>(Name + ".State", state);
        }
    }
}