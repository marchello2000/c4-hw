namespace FourPlanGrid.Game.ViewModels
{
    using Prism.Events;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Windows.Threading;
    class GameBoardViewModel : Logic.IBoardWalker<TokenViewModel>
    {

        #region Fields
        /// <summary>
        /// ICommand object to hold the RelayCommand for the token placed user action
        /// </summary>
        
        protected readonly IEventAggregator eventAggregator;
        private Hashtable board;
        private List<TokenViewModel> tokenVMs;
        private static int currentPlayer;
        private bool playerOneAIEnabled, playerTwoAIEnabled;

        public static int NumberOfRows = 6;
        public static int NumberOfColumns = 7;


        #endregion


        #region Constructors
        public GameBoardViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            
            board = new Hashtable();
            tokenVMs = new List<TokenViewModel>();

            InitializeEvents();

            

        }
        #endregion

        #region Properties
        
        public static int CurrentPlayer
        {
            get
            {
                return currentPlayer;
            }
            private set
            {
                currentPlayer = value;
            }
        }
        private bool PlayerOneAIEnabled
        {
            get
            {
                return playerOneAIEnabled;
            }
            set
            {
                playerOneAIEnabled = value;
                StartTurn(); // will start the AI if we switched mid turn
            }
        }
        private bool PlayerTwoAIEnabled
        {
            get
            {
                return playerTwoAIEnabled;
            }
            set
            {
                playerTwoAIEnabled = value;
                StartTurn(); // will start the AI if we switched mid turn
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
            tokenVM.Player = CurrentPlayer; // <-- only place we should assign a player.
            

            //time to check if we just won
            ICollection<TokenViewModel> winners = Logic.BoardSolver<TokenViewModel>.Solve(tokenVM, this);
            if (winners.Count > 0)
            {
                #region OnWin
                foreach (TokenViewModel tkVM in tokenVMs)
                {
                    if (winners.Contains(tkVM))
                    {
                        tkVM.State = Models.TokenState.Winner;
                    }
                    else
                    {
                        if (tkVM.State == Models.TokenState.Ready || tkVM.State == Models.TokenState.Hover)
                        {
                            tkVM.State = Models.TokenState.Empty;
                        }
                        else if (tkVM.State == Models.TokenState.Placed)
                        {
                            tkVM.State = Models.TokenState.NotWinner;
                        }
                    }
                }
                #endregion
            }
            else // just update for the next turn
            {
                // Token above should now be ready
                TokenViewModel tVMAbove = GetUp(tokenVM);
                if (tVMAbove != null)
                {
                    tVMAbove.State = Models.TokenState.Ready;
                }

                NextPlayer();
                StartTurn();
            }
        }

        /// <summary>
        /// 1 -> 2 and 2 -> 1
        /// </summary>
        private void NextPlayer()
        {
            CurrentPlayer = (CurrentPlayer == 1 ? 2 : 1);
        }

        private void StartTurn()
        {

            // check if the current player is an AI and trigger a move
            if (CurrentPlayer == 1 && PlayerOneAIEnabled || CurrentPlayer == 2 && PlayerTwoAIEnabled)
            {
                foreach (TokenViewModel tokenViewModel in tokenVMs.FindAll(i => i.State == Models.TokenState.Ready || i.State == Models.TokenState.Hover))
                {
                    tokenViewModel.State = Models.TokenState.ReadyAI;
                }

                DispatcherTimer dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Tick += new System.EventHandler(AITakeTurn);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
                dispatcherTimer.Start();
            }
            else
            {
                foreach (TokenViewModel tokenViewModel in tokenVMs.FindAll(i => i.State == Models.TokenState.ReadyAI))
                {
                    tokenViewModel.State = Models.TokenState.Ready;
                }
            }
        }

        private void AITakeTurn(object sender, EventArgs e)
        {

            (sender as DispatcherTimer).Stop();
            

            // just make a random move for now. It would be pretty straight forward to modify this to 
            // loop over our moves and check if they win. if not, check if they win for the opponent. if 
            // not then be cleaver or random.
            List<TokenViewModel> moves = tokenVMs.FindAll(i => i.State == Models.TokenState.ReadyAI);
            Random rnd = new Random();
            if (moves.Count > 0)
                PlaceToken(moves[rnd.Next(moves.Count)]);
        }


        #endregion

        /// <summary>
        /// We store off the TokenViewModels by subscribing to the TokenViewModelCreated event.
        /// This way we always have references to the *child* elements without having to initialize them directly.
        /// </summary>
        /// <param name="tokenVM"></param>
        private void AddTokenVM(TokenViewModel tokenVM)
        {
            tokenVMs.Add(tokenVM);
        }

        /// <summary>
        /// Gets the TokenViewModel object at the specified row and column. Now that the tokens have been initialized
        /// we can build our hash map as needed when retrieving a TokenViewModel.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        private TokenViewModel GetTokenVM(int row, int column)
        {
            if (!board.ContainsKey(TokenViewModel.GetPairingHash(row, column)))
            {
                TokenViewModel tkVM = tokenVMs.Find(tk => tk.Row == row && tk.Column == column);
                if (tkVM != null)
                    board.Add(TokenViewModel.GetPairingHash(row, column), tkVM);
            }
            return board[TokenViewModel.GetPairingHash(row, column)] as TokenViewModel;
        }

        /// <summary>
        /// Handles NewGameEvent. resets the state of the tokens and the current player
        /// </summary>
        /// <param name="obj"></param>
        private void NewGame(object obj)
        {
            CurrentPlayer = 1;
            foreach (TokenViewModel tokenVM in tokenVMs)
            {
                tokenVM.Player = 0;
                tokenVM.State = (tokenVM.Row == NumberOfRows - 1 ? Models.TokenState.Ready : Models.TokenState.Empty);
            }
            Thread.Sleep(500);
            StartTurn();
        }

        /// <summary>
        /// Initialize subscriptions to prism events
        /// </summary>
        private void InitializeEvents()
        {
            
            this.eventAggregator.GetEvent<TokenViewModelCreatedEvent>()
            .Subscribe(tVM => AddTokenVM(tVM));
            this.eventAggregator.GetEvent<NewGameEvent>()
            .Subscribe(obj => NewGame(obj));
            this.eventAggregator.GetEvent<TokenPlacedEvent>()
            .Subscribe(obj => PlaceToken(obj));

            this.eventAggregator.GetEvent<PlayerAIEnabledChangedEvent>()
            .Subscribe(pc =>
            {
                if (pc.player == 1) PlayerOneAIEnabled = pc.enabled;
                if (pc.player == 2) PlayerTwoAIEnabled = pc.enabled;
            });
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
