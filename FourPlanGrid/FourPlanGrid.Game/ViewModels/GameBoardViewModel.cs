namespace FourPlanGrid.Game.ViewModels
{
    using Prism.Events;
    class GameBoardViewModel
    {
        protected readonly IEventAggregator eventAggregator;
        public GameBoardViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<TokenViewModelCreatedEvent>()
            .Subscribe(tVM => AddTokenVM(tVM));
        }



        private void AddTokenVM(TokenViewModel tokenVM)
        {

        }
            
    }
}
