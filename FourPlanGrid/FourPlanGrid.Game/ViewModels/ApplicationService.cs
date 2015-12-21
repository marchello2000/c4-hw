using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourPlanGrid.Game.ViewModels
{ 
    using Prism.Events;

    /// <summary>
    /// Stole this from http://blog.magnusmontin.net/2014/02/28/using-the-event-aggregator-pattern-to-communicate-between-view-models/
    /// Useful helper singleton class for using the prism event aggregator pattern.
    /// In MVVM we typically let the views bind themselves to their VMs and initialize any child views. Thus,
    /// we don't have a link between ViewModels and even if we explicitly set them, it would be messy to communicate
    /// between view models in hierarchies. Event aggregator lets us create and pass arbitrary events between 
    /// publishers and subscribers (i.e. different view models)
    /// </summary>
    internal sealed class ApplicationService
    {
        private ApplicationService() { }

        private static readonly ApplicationService _instance = new ApplicationService();

        internal static ApplicationService Instance { get { return _instance; } }

        private IEventAggregator eventAggregator;
        internal IEventAggregator EventAggregator
        {
            get
            {
                if (eventAggregator == null)
                    eventAggregator = new EventAggregator();

                return eventAggregator;
            }
        }
    }
}
