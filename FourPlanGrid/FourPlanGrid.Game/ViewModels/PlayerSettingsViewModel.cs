namespace FourPlanGrid.Game.ViewModels
{
    using Prism.Events;
    using System.Windows.Input;
    using System.Windows.Media;
    using Windows;
    class PlayerSettingsViewModel : ObservableObject
    {
        #region Fields
        private Color color;
        private string playerName;
        protected readonly IEventAggregator eventAggregator;

        private ICommand aiCheckedCommand;

        #endregion

        #region Contructors
        /// <summary>
        /// Default constructor. Class will public PlayerColorChangedEvents 
        /// </summary>
        /// <param name="eventAggregator"></param>
        public PlayerSettingsViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            //this.aiCheckedCommand = new RelayCommand(o => PublishAIEnabledChanged(), o => true);
        }

        #endregion

        #region Properties

        public ICommand AICheckedCommand
        {
            get
            {
                return aiCheckedCommand;
            }
            set
            {
                aiCheckedCommand = value;
            }
        }

        private bool aiEnabled;
        public bool AIEnabled
        {
            get
            {
                return aiEnabled;
            }
            set
            {
                aiEnabled = value;
                PublishAIEnabledChanged();
            }
        }

        /// <summary>
        /// Background brush property for textbox and boarder background
        /// </summary>
        public Brush Background
        {
            get
            {
                return new SolidColorBrush(Color);
            }
        }

        /// <summary>
        /// private Color property. Just wrapping the underlying data.
        /// </summary>
        private Color Color
        {
            get
            {
                return color;
            }
        }

        /// <summary>
        /// Red scale of color
        /// </summary>
        public byte Red
        {
            get
            {
                return color.R;
            }
            set
            {
                color.R = value;
                OnPropertyChanged("Red");
                OnPropertyChanged("Background");
                PublishColorChanged();
            }
        }

        /// <summary>
        /// Blue scale of color
        /// </summary>
        public byte Blue
        {
            get
            {
                return color.B;
            }
            set
            {
                color.B = value;
                OnPropertyChanged("Background");
                OnPropertyChanged("Blue");
                PublishColorChanged();
            }
        }

        /// <summary>
        /// Green scale of color
        /// </summary>
        public byte Green
        {
            get
            {
                return color.G;
            }
            set
            {
                color.G = value;
                OnPropertyChanged("Background");
                OnPropertyChanged("Green");
                PublishColorChanged();
            }
        }

        /// <summary>
        /// Alpha scale of color
        /// </summary>
        public byte Alpha
        {
            get
            {
                return color.A;
            }
            set
            {
                color.A = value;
                OnPropertyChanged("Background");
                OnPropertyChanged("Alpha");
                PublishColorChanged();
            }
        }

        /// <summary>
        /// Player property. Used for publishing PlayerColorChangedEvent
        /// </summary>
        public int Player { get; set; }

        public string PlayerName
        {
            get
            {
                return playerName;
            }
            set
            {
                playerName = value;
                OnPropertyChanged("PlayerName");
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Publishes a PlayerColorChangedEvent
        /// </summary>
        private void PublishColorChanged()
        {
            PlayerColor pc = new PlayerColor();
            pc.color = Color;
            pc.player = Player;
            eventAggregator.GetEvent<PlayerColorChangedEvent>().Publish(pc);
        }

        private void PublishAIEnabledChanged()
        {
            PlayerAIEnabled pc = new PlayerAIEnabled();
            pc.enabled = AIEnabled;
            pc.player = Player;
            eventAggregator.GetEvent<PlayerAIEnabledChangedEvent>().Publish(pc);
        }

        #endregion

    }
}
