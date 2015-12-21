namespace FourPlanGrid.Game.ViewModels
{
    using System.Windows;
    using System.Windows.Media;
    using Prism.Events;
    using FourPlanGrid.Windows;
    using FourPlanGrid.Game.Models;
    using System.Windows.Input;

    class TokenViewModel : ObservableObject, Logic.IPlayer
    {
        #region Fields
        private static int currentPlayer;

        private static Color colorOne, colorTwo;
        private ICommand tokenPlacedCommand;
        private ICommand tokenEnterCommand;
        private ICommand tokenLeaveCommand;
    
        protected readonly IEventAggregator eventAggregator;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates TokenModel, subscribes to PlayerColorChangedEvents and publishes TokenViewModelCreatedEvent
        /// </summary>
        /// <param name="eventAggregator"></param>
        public TokenViewModel(IEventAggregator eventAggregator)
        {
            TokenModel = new TokenModel();
            this.eventAggregator = eventAggregator;

            // some default values for the properties
            Player = 0;
            State = TokenState.Empty;

            tokenPlacedCommand = new RelayCommand( o => this.eventAggregator.GetEvent<TokenPlacedEvent>().Publish(this), 
                                                   o => this.State == Models.TokenState.Ready || this.State == Models.TokenState.Hover);

            tokenEnterCommand = new RelayCommand( o => this.State = Models.TokenState.Hover,
                                                  o => this.State == Models.TokenState.Ready);

            tokenLeaveCommand = new RelayCommand( o => this.State = Models.TokenState.Ready,
                                                  o => this.State == Models.TokenState.Hover);

            this.eventAggregator.GetEvent<PlayerColorChangedEvent>()
            .Subscribe(pc =>
            {
                if (pc.player == 1) PlayerOneColor = pc.color;
                if (pc.player == 2) PlayerTwoColor = pc.color;
            });
            this.eventAggregator.GetEvent<TokenViewModelCreatedEvent>().Publish(this);


            
        }

        #endregion

        #region Properties

        /// <summary>
        /// Private property wrapping the TokenModel
        /// </summary>
        private TokenModel TokenModel { get; set; }

        /// <summary>
        /// Property for the command when a user places a token.
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

        public ICommand TokenEnterCommand
        {
            get
            {
                return tokenEnterCommand;
            }
            set
            {
                tokenEnterCommand = value;
            }
        }

        public ICommand TokenLeaveCommand
        {
            get
            {
                return tokenLeaveCommand;
            }
            set
            {
                tokenLeaveCommand = value;
            }
        }

        /// <summary>
        /// Tracks the row and fires property changed "Row"
        /// </summary>
        public int Row
        {
            get
            {
                return TokenModel.Row;
            }
            set
            {
                TokenModel.Row = value;
                OnPropertyChanged("Row");
            }
        }

        /// <summary>
        /// Tracks the column and fires property changed "Column"
        /// </summary>
        public int Column
        {
            get
            {
                return TokenModel.Column;
            }
            set
            {
                TokenModel.Column = value;
                OnPropertyChanged("Column");
            }
        }

        /// <summary>
        /// Tracks the Player number/id and fires property changed "Player"
        /// </summary>
        public int Player
        {
            get
            {
                return TokenModel.Player;
            }
            set
            {
                TokenModel.Player = value;
                OnPropertyChanged("Stroke");
                OnPropertyChanged("Fill");      // depends on player 
            }
        }

        public static int CurrentPlayer
        {
            get
            {
                return currentPlayer;
            }
            set
            {
                currentPlayer = value;
            }
        }

        /// <summary>
        /// Tracks the underlying TokenModel state and fires property changed "State"
        /// </summary>
        public TokenState State
        {
            get
            {
                return TokenModel.State;
            }
            set
            {
                TokenModel.State = value;
                OnPropertyChanged("Fill");
                OnPropertyChanged("Stroke");
            }
        }

        /// <summary>
        /// Property that determines the fill color for the view shape. This will depend
        /// on the Player and State properties.
        /// </summary>
        public Brush Fill
        {
            get
            {
                Brush brush;
                switch (State)
                {
                    case TokenState.Empty:
                        brush = new SolidColorBrush(Color.FromArgb(0,0,0,0));
                        break;
                    case TokenState.Hover:
                    case TokenState.Ready:
                        brush = new SolidColorBrush(Color.FromArgb(100, 100, 100, 100));
                        break;
                    case TokenState.Placed:
                        brush = new SolidColorBrush(GetTokenColor());
                        break;
                    case TokenState.Winner:
                        brush = new RadialGradientBrush(Color.FromArgb(255,255,255,255), GetTokenColor());
                        break;
                    case TokenState.NotWinner:
                    default:
                        brush = new SolidColorBrush(GetTokenColor());
                        break;
                }
                return brush;
            }
        }

        /// <summary>
        /// Property that determines the stroke color for the view shape. This will depend
        /// on the Player and State properties
        /// </summary>
        public Brush Stroke
        {
            get
            {
                Brush brush;
                switch (State)
                {
                    case TokenState.Empty:
                        brush = new SolidColorBrush(Color.FromArgb(50, 50, 50, 50));
                        break;
                    case TokenState.Hover:
                        brush = new SolidColorBrush(GetTokenColor());
                        break;
                    case TokenState.Ready:
                        brush = new SolidColorBrush(Color.FromArgb(200, 100, 100, 100));
                        break;
                    case TokenState.Placed:
                        brush = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0));
                        break;
                    case TokenState.Winner:
                    case TokenState.NotWinner:
                    default:
                        brush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                        break;
                }
                return brush;
            }
        }

        private Color PlayerOneColor
        {
            get
            {
                return colorOne;
            }
            set
            {
                colorOne = value;
                if (Player == 1)
                {
                    OnPropertyChanged("Fill");
                    OnPropertyChanged("Stroke");
                }
            }
        }
        private Color PlayerTwoColor
        {
            get
            {
                return colorTwo;
            }
            set
            {
                colorTwo = value;
                if (Player == 2)
                {
                    OnPropertyChanged("Fill");
                    OnPropertyChanged("Stroke");
                }
            }
        }

        #endregion

        public static int CantorHashCoords(int a, int b)
        {
            return (a + b) * (a + b + 1) / 2 + a;
        }

        public override int GetHashCode()
        {
            return CantorHashCoords(Row, Column);
        }

        #region Helpers

        private Color GetTokenColor()
        {
            if (Player == 1 || Player == 0 && CurrentPlayer == 1) return PlayerOneColor;
            else if (Player == 2 || Player == 0 && CurrentPlayer == 2) return PlayerTwoColor;

            return Color.FromArgb(100, 100, 100, 100); // ugly gray color
        }

        #endregion
    }
}
