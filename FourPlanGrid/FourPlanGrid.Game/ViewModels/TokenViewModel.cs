namespace FourPlanGrid.Game.ViewModels
{
    using System.Windows.Media;
    using Prism.Events;
    using FourPlanGrid.Windows;
    using FourPlanGrid.Game.Models;
    using System.Windows.Input;

    class TokenViewModel : ObservableObject, Logic.IPlayer
    {
        #region Fields
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

            InitializeCommands();
            InitializeEvents();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Private property wrapping the TokenModel
        /// </summary>
        private TokenModel TokenModel { get; set; }

        /// <summary>
        /// Property wrappers for the ICommands.
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
                        brush = new SolidColorBrush(Color.Subtract(GetTokenColor(), Color.FromArgb((byte)(GetTokenColor().A / 2), 0, 0, 0)));
                        break;
                    case TokenState.ReadyAI:
                    case TokenState.Ready:
                        brush = new SolidColorBrush(Color.FromArgb(50, 100, 100, 100));
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
                        brush = new SolidColorBrush(Color.FromArgb(200, 100, 100, 100));
                        break;
                    case TokenState.ReadyAI:
                    case TokenState.Ready:
                        brush = new SolidColorBrush(Color.FromArgb(200, 100, 100, 100));
                        break;
                    case TokenState.Placed:
                        brush = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0));
                        break;
                    case TokenState.Winner:
                        brush = new LinearGradientBrush(Color.FromArgb(255, 255, 255, 255), GetTokenColor(), 75);
                        break;
                    case TokenState.NotWinner:
                    default:
                        brush = new SolidColorBrush(Color.FromArgb(200, 100, 100, 100));
                        break;
                }
                return brush;
            }
        }

        /// <summary>
        /// The point here is that there are only 2 player colors, but each token needs to be
        /// updated when these colors change. The change is captured by a PlayerColorChangedEvent
        /// and one of these 2 propertys will get set. That will trigger "Stroke" and "Fill" property
        /// changed events.
        /// </summary>
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

        /// <summary>
        /// I don't like this. We need to abstract out all this list -> hashtable stuff.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static int GetPairingHash(int row, int col)
        {
            return (GameBoardViewModel.NumberOfColumns + 1) * (row + 1) + col + 1;
        }

        #region Helpers

        /// <summary>
        /// Helper to get the current applicable token color. Once tokens are placed, they have a fixed
        /// player and can get the color based on that. On the other hand, for the Ready and Hover states, 
        /// we need the color to be based on the current player instead. This is expposed through the static property 
        /// GameBoardViewModel.CurrentPlayer
        /// </summary>
        /// <returns></returns>
        private Color GetTokenColor()
        {
            if (Player == 1 || Player == 0 && GameBoardViewModel.CurrentPlayer == 1) return PlayerOneColor;
            else if (Player == 2 || Player == 0 && GameBoardViewModel.CurrentPlayer == 2) return PlayerTwoColor;

            return Color.FromArgb(100, 100, 100, 100); // ugly gray color
        }

        /// <summary>
        /// Set up commands for the token. We need to respond to MouseEnter, MouseLeave and Command (i.e. click).
        /// We are using ICommands instead of the standard event handlers. This lets us handle events in the data context
        /// of the view, the viewmodel, by only exposing a bindable property.
        /// </summary>
        private void InitializeCommands()
        {
            tokenPlacedCommand = new RelayCommand(o => this.eventAggregator.GetEvent<TokenPlacedEvent>().Publish(this),
                                                   o => this.State == Models.TokenState.Ready || this.State == Models.TokenState.Hover);

            tokenEnterCommand = new RelayCommand(o => this.State = Models.TokenState.Hover,
                                                  o => this.State == Models.TokenState.Ready);

            tokenLeaveCommand = new RelayCommand(o => this.State = Models.TokenState.Ready,
                                                  o => this.State == Models.TokenState.Hover);
        }
           
        /// <summary>
        /// Set up subscriptions for prism events. in this case we also publish the 
        /// TokenViewModelCreatedEvent so that our GameBoardViewmodel picks it up and can
        /// save off the reference
        /// </summary>
        private void InitializeEvents()
        {
            this.eventAggregator.GetEvent<PlayerColorChangedEvent>()
            .Subscribe(pc =>
            {
                if (pc.player == 1) PlayerOneColor = pc.color;
                if (pc.player == 2) PlayerTwoColor = pc.color;
            });
            this.eventAggregator.GetEvent<TokenViewModelCreatedEvent>().Publish(this);
        }

        #endregion
    }
}
