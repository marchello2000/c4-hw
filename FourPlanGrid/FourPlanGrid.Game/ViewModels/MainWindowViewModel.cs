using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace FourPlanGrid.Game.ViewModels
{
    using FourPlanGrid.Windows;
    using System.ComponentModel;
    class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel()
        {
            hiButtonCommand = new RelayCommand(ShowMessage, param => this.canExecute);
        }

        public void ColorOneChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        public void ColorTwoChanged(object sender, PropertyChangedEventArgs e)
        {
            PlayerSettingsViewModel playerTwoSettingsVM = sender as PlayerSettingsViewModel;
            if (playerTwoSettingsVM != null)
            {
                ColorTwo = Color.FromArgb(0,0,0,0);
            }
        }


        private Color _colorOne, _colorTwo;
        public Color ColorOne
        {
            get
            {
                return _colorOne;
            }
            set
            {
                _colorOne = value;
                OnPropertyChanged("ColorOne");
                OnPropertyChanged("RedOne");
                OnPropertyChanged("BlueOne");
                OnPropertyChanged("GreenOne");
                OnPropertyChanged("AlphaOne");
            }
        }
        public Color ColorTwo
        {
            get
            {
                return _colorTwo;
            }
            set
            {
                _colorTwo = value;
                OnPropertyChanged("ColorTwo");
            }
        }

        



















        private bool canExecute = true;
        private ICommand hiButtonCommand;

        public ICommand HiButtonCommand
        {
            get
            {
                return hiButtonCommand;
            }
            set
            {
                hiButtonCommand = value;
            }
        }

        public string HiButtonContent
        {
            get
            {
                return "click to hi";
            }
        }

        public bool CanExecute
        {
            get
            {
                return this.canExecute;
            }

            set
            {
                if (this.canExecute == value)
                {
                    return;
                }

                this.canExecute = value;
            }
        }
        private void ShowMessage(object obj)
        {
            MessageBox.Show(obj.ToString());
        }
    }
}
