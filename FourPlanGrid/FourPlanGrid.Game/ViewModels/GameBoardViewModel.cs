namespace FourPlanGrid.Game.ViewModels
{
    using Prism.Events;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Input;
    using Windows;
    class GameBoardViewModel : Logic.IBoardWalker<TokenViewModel>
    {

        #region Fields
        /// <summary>
        /// ICommand object to hold the RelayCommand for the token placed user action
        /// </summary>
        
        protected readonly IEventAggregator eventAggregator;
        private Hashtable board;
        private List<TokenViewModel> tokenVMs;
        private int currentPlayer;

        #endregion


        #region Constructors
        public GameBoardViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<TokenViewModelCreatedEvent>()
            .Subscribe(tVM => AddTokenVM(tVM));
            this.eventAggregator.GetEvent<NewGameEvent>()
            .Subscribe(obj => NewGame(obj));
            this.eventAggregator.GetEvent<TokenPlacedEvent>()
            .Subscribe(obj => PlaceToken(obj));

            board = new Hashtable();
            tokenVMs = new List<TokenViewModel>();
            
        }
        #endregion

        #region Properties
        
        public int CurrentPlayer
        {
            get
            {
                return currentPlayer;
            }
            set
            {
                currentPlayer = value;
                this.eventAggregator.GetEvent<CurrentPlayerChangedEvent>().Publish(currentPlayer);
            }
        }
        
        #endregion


        #region Private Methods
        
        private void PlaceToken(object obj)
        {
            #region Parameter Validation
            if (obj == null)
            {
                throw new System.ArgumentNullException("FourPlanGrid.Game.ViewModels.GameBoardViewModel.IsPlaceable");
            }

            TokenViewModel tokenVM = obj as TokenViewModel;
            if (tokenVM == null)
            {
                throw new System.ArgumentException("FourPlanGrid.Game.ViewModels.GameBoardViewModel.IsPlaceable unable to cast to TokenViewModel");
            }
            #endregion

            // we already know the token is in the Ready state (RelayCommand)
            tokenVM.State = Models.TokenState.Placed;
            tokenVM.Player = CurrentPlayer;

            CurrentPlayer = (CurrentPlayer == 1 ? 2 : 1);

            TokenViewModel tVMAbove = GetUp(tokenVM);
            if (tVMAbove != null)
            {
                tVMAbove.State = Models.TokenState.Ready;
            }

        }
        #endregion


        private void AddTokenVM(TokenViewModel tokenVM)
        {
            tokenVMs.Add(tokenVM);
        }

        private TokenViewModel GetTokenVM(int row, int column)
        {
            return tokenVMs.Find(tk => tk.Row == row && tk.Column == column);
        }

        private void NewGame(object obj)
        {
            CurrentPlayer = 1;
            foreach (TokenViewModel tokenVM in tokenVMs)
            {
                tokenVM.State = (tokenVM.Row == 5 ? Models.TokenState.Ready : Models.TokenState.Empty);
            }
            
        }

        #region IBoardWalker
        public TokenViewModel GetRight(TokenViewModel cur)
        {
            return GetTokenVM(cur.Row, cur.Column + 1);
        }

        public TokenViewModel GetRightUp(TokenViewModel cur)
        {
            return GetTokenVM(cur.Row - 1, cur.Column + 1);
        }

        public TokenViewModel GetUp(TokenViewModel cur)
        {
            return GetTokenVM(cur.Row - 1, cur.Column);
        }

        public TokenViewModel GetLeftUp(TokenViewModel cur)
        {
            return GetTokenVM(cur.Row - 1, cur.Column - 1);
        }

        public TokenViewModel GetLeft(TokenViewModel cur)
        {
            return GetTokenVM(cur.Row, cur.Column - 1);
        }

        public TokenViewModel GetLeftDown(TokenViewModel cur)
        {
            return GetTokenVM(cur.Row + 1, cur.Column - 1);
        }

        public TokenViewModel GetDown(TokenViewModel cur)
        {
            return GetTokenVM(cur.Row + 1, cur.Column);
        }

        public TokenViewModel GetRightDown(TokenViewModel cur)
        {
            return GetTokenVM(cur.Row + 1, cur.Column + 1);
        }
        #endregion
    }
}
