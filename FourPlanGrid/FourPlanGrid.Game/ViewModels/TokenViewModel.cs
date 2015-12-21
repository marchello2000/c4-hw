namespace FourPlanGrid.Game.ViewModels
{
    using System.Windows.Media;
    using FourPlanGrid.Windows;
    using FourPlanGrid.Game.Models;
    using Prism.Events;
    class TokenViewModel : ObservableObject
    {
        #region Fields
        private TokenModel tokenModel;
        private Color color;
        #endregion

        #region Constructors
        protected readonly IEventAggregator _eventAggregator;
        public TokenViewModel(IEventAggregator eventAggregator)
        {
            TokenModel = new TokenModel();
            this._eventAggregator = eventAggregator;
            this._eventAggregator.GetEvent<PlayerColorChangedEvent>()
            .Subscribe(pc => { if (pc.player == this.Player) this.PlayerColor = pc.color; });
            this._eventAggregator.GetEvent<PlayerColorChangedEvent>().Publish(this);
        }

        
       
        #endregion

        #region Properties

        public TokenModel TokenModel
        {
            get
            {
                return tokenModel;
            }
            set
            {
                tokenModel = value;
                OnPropertyChanged("TokenModel");
            }
        }
        public int Row
        {
            get
            {
                return tokenModel.Row;
            }
            set
            {
                tokenModel.Row = value;
                OnPropertyChanged("Row");
            }
        }
        public int Column
        {
            get
            {
                return tokenModel.Column;
            }
            set
            {
                tokenModel.Column = value;
                OnPropertyChanged("Column");
            }
        }
        public int Player
        {
            get
            {
                return tokenModel.Player;
            }
            set
            {
                tokenModel.Player = value;
                OnPropertyChanged("Player");    // do we need this? we only need to fire property changed 
                                                //events if the view is bound to that property (directly or indirectly)
                OnPropertyChanged("Fill");      // depends on player 
            }
        }
        public TokenState State
        {
            get
            {
                return tokenModel.State;
            }
            set
            {
                tokenModel.State = value;
                OnPropertyChanged("State");
                OnPropertyChanged("Fill");
            }
        }

        public Brush Fill
        {
            get
            {
                Brush brush;
                switch (State)
                {
                    case TokenState.Empty:
                        brush = new SolidColorBrush(Color.FromArgb(100,100,100,100));
                        break;
                    case TokenState.Hover:
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
            }
        }

        #endregion

        #region Helpers


        #endregion
    }
}
