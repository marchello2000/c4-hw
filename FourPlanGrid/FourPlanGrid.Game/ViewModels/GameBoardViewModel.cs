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
        private ICommand tokenPlacedCommand;
        protected readonly IEventAggregator eventAggregator;
        Hashtable board;
        List<TokenViewModel> tokenVMs;
        #endregion


        #region Constructors
        public GameBoardViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<TokenViewModelCreatedEvent>()
            .Subscribe(tVM => AddTokenVM(tVM));
            this.eventAggregator.GetEvent<NewGameEvent>()
            .Subscribe(obj => NewGame(obj));

            board = new Hashtable();
            tokenVMs = new List<TokenViewModel>();
            tokenPlacedCommand = new RelayCommand(PlaceToken, param => IsPlaceable(param));
        }
        #endregion

        private bool IsPlaceable(object obj)
        {
            TokenViewModel tokenVM = obj as TokenViewModel;

            return true;
        }

        #region Properties
        /// <summary>
        /// Property to wrap the newGameButtonCommand field
        /// </summary>
        public ICommand TokenPlacedCommand
        {
            get
            {
                return tokenPlacedCommand;
            }
            set
            {
                tokenPlacedCommand = value;
            }
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Publishes new game event
        /// </summary>
        /// <param name="obj"></param>
        private void PlaceToken(object obj)
        {
            obj = null;
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
            foreach (TokenViewModel tokenVM in tokenVMs)
            {
                tokenVM.Player = 1;
                tokenVM.State = (tokenVM.Row == 5 ? Models.TokenState.Ready : Models.TokenState.Empty);
            }
            
        }

        #region IBoardWalker
        public TokenViewModel GetRight(TokenViewModel cur)
        {
            return GetTokenVM(cur.Row + 1, cur.Column);
        }

        public TokenViewModel GetRightUp(TokenViewModel cur)
        {
            return GetTokenVM(cur.Row + 1, cur.Column + 1);
        }

        public TokenViewModel GetUp(TokenViewModel cur)
        {
            return GetTokenVM(cur.Row, cur.Column + 1);
        }

        public TokenViewModel GetLeftUp(TokenViewModel cur)
        {
            return GetTokenVM(cur.Row - 1, cur.Column + 1);
        }

        public TokenViewModel GetLeft(TokenViewModel cur)
        {
            return GetTokenVM(cur.Row - 1, cur.Column);
        }

        public TokenViewModel GetLeftDown(TokenViewModel cur)
        {
            return GetTokenVM(cur.Row - 1, cur.Column - 1);
        }

        public TokenViewModel GetDown(TokenViewModel cur)
        {
            return GetTokenVM(cur.Row, cur.Column - 1);
        }

        public TokenViewModel GetRightDown(TokenViewModel cur)
        {
            return GetTokenVM(cur.Row + 1, cur.Column - 1);
        }
        #endregion
    }
}
