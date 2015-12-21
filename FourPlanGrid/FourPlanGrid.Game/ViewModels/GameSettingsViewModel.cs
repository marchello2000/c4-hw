namespace FourPlanGrid.Game.ViewModels
{
    using Prism.Events;
    using System.Windows.Input;
    using Windows;
    class GameSettingsViewModel
    {


        #region Fields
        /// <summary>
        /// ICommand object to hold the RelayCommand for the new game button
        /// </summary>
        private ICommand newGameButtonCommand;

        /// <summary>
        /// Event Aggregator for passing messages to other view models.
        /// </summary>
        protected readonly IEventAggregator eventAggregator;
        #endregion


        #region Constructors
        /// <summary>
        /// Initializes commands and saves off the event aggregator
        /// </summary>
        /// <param name="eventAggregator"></param>
        public GameSettingsViewModel(IEventAggregator eventAggregator)
        {
            newGameButtonCommand = new RelayCommand(ExecuteNewGame);
            this.eventAggregator = eventAggregator;
        }
        #endregion


        #region Properties
        /// <summary>
        /// Property to wrap the newGameButtonCommand field
        /// </summary>
        public ICommand NewGameButtonCommand
        {
            get
            {
                return newGameButtonCommand;
            }
            set
            {
                newGameButtonCommand = value;
            }
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Publishes new game event
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteNewGame(object obj)
        {
            eventAggregator.GetEvent<NewGameEvent>().Publish(obj);
        }
        #endregion
    }
}
