namespace FourPlanGrid.Game.ViewModels
{
    using System.Windows;
    using System.Windows.Media;
    using Prism.Events;
    using FourPlanGrid.Windows;
    using FourPlanGrid.Game.Models;
    
    
    class TokenViewModel : ObservableObject, Logic.IPlayer
    {
        #region Fields
        private Color color, colorOne, colorTwo;

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

            // some default values for the properties
            Player = 0;
            State = TokenState.Empty;

            this.eventAggregator = eventAggregator;
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
                PlayerColor = GetPlayerColor();
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
                        brush = new SolidColorBrush(PlayerColor);
                        break;
                    case TokenState.Winner:
                    case TokenState.NotWinner:
                    default:
                        brush = new SolidColorBrush(Color.FromArgb(0,0,0,0));
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
                        brush = new SolidColorBrush(Color.FromArgb(100, 100, 100, 100));
                        break;
                    case TokenState.Hover:
                        brush = new SolidColorBrush(PlayerColor);
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

        /// <summary>
        /// Property to trake the player color setting. Value is set when PlayerColorChangedEvent is 
        /// recieved. This will fire property changed "Fill" and "Stroke" since they depend on the 
        /// player color.
        /// </summary>
        public Color PlayerColor
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                OnPropertyChanged("Fill");
                OnPropertyChanged("Stroke");
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
                    PlayerColor = colorOne; // update placed tokens immediately
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
                    PlayerColor = colorTwo;
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

        private Color GetPlayerColor()
        {
            if (Player == 1) return PlayerOneColor;
            else if (Player == 2) return PlayerTwoColor;

            return Color.FromArgb(100, 100, 100, 100); // ugly gray color
        }

        #endregion
    }
}
