namespace FourPlanGrid.Game.ViewModels
{
    using Prism.Events;
    using System.Windows.Media;
    class PlayerSettingsViewModel : FourPlanGrid.Windows.ObservableObject
    {

        protected readonly IEventAggregator _eventAggregator;
        public PlayerSettingsViewModel(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
        }

        
        private Color color;
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                OnPropertyChanged("Color");
                OnPropertyChanged("Red");
                OnPropertyChanged("Blue");
                OnPropertyChanged("Green");
                OnPropertyChanged("Alpha");

                PublishColorChanged();
            }
        }
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
                OnPropertyChanged("Color");
                PublishColorChanged();
            }
        }
        public byte Blue
        {
            get
            {
                return color.B;
            }
            set
            {
                color.B = value;
                OnPropertyChanged("Blue");
                OnPropertyChanged("Color");
                PublishColorChanged();
            }
        }
        public byte Green
        {
            get
            {
                return color.G;
            }
            set
            {
                color.G = value;
                OnPropertyChanged("Green");
                OnPropertyChanged("Color");
                PublishColorChanged();
            }
        }
        public byte Alpha
        {
            get
            {
                return color.A;
            }
            set
            {
                color.A = value;
                OnPropertyChanged("Alpha");
                OnPropertyChanged("Color");
                PublishColorChanged();
            }
        }
        public int Player { get; set; }


        private void PublishColorChanged()
        {
            PlayerColor pc = new PlayerColor();
            pc.color = Color;
            pc.player = Player;
            _eventAggregator.GetEvent<PlayerColorChangedEvent>().Publish(pc);
        }

    }
}
