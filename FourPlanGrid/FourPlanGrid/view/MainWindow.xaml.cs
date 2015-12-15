using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FourPlanGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            TheGrid.ShowGridLines = true;

            // save off the move ellipses
            _moveEllipsesList = new Ellipse[7];
            this.TheGrid.Children.CopyTo(_moveEllipsesList,0);
        }

        private int _curPlayer = 1;
        private Ellipse[] _moveEllipsesList;
        private const int NumberOfRows = 6;
        private const int NumberOfColumns = 7;
        private const double TokenMarginFactor = 0.1;

        private void Player1NameGotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Resources["rowHeight"] = new GridLength(50);
        }

        private void MoveEllipse_MouseEnter(object sender, MouseEventArgs e)
        {
            Ellipse moveEllipse = sender as Ellipse;
            if (!moveEllipse.IsEnabled) return;
            moveEllipse.Fill = new SolidColorBrush(Color.FromArgb(150, 153, 153, 153));
        }
        private void MoveEllipse_MouseLeave(object sender, MouseEventArgs e)
        {
            Ellipse moveEllipse = sender as Ellipse;
            if (!moveEllipse.IsEnabled) return;
            moveEllipse.Fill = new SolidColorBrush(Color.FromArgb(20, 153, 153, 153));
        }

        private FPGModel.IFPGBoard _model = new FPGModel.CEzBoard(6, 7);

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private void MoveEllipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Ellipse clickable = sender as Ellipse;
            int col = Grid.GetColumn(clickable);

            _model.Drop(col, _curPlayer);
            if (!_model.CanDrop(col))
            {
                DisableMoveEllipse(clickable);
            }
            clickable.IsEnabled = _model.CanDrop(col);
            

            //create an ellipse
            Ellipse ep = new Ellipse();
            ep.SetBinding(Ellipse.WidthProperty, new Binding()
            {
                Path = new PropertyPath("TokenSize"),
                Source = this,
            });
            ep.SetBinding(Ellipse.HeightProperty, new Binding()
            {
                Path = new PropertyPath("TokenSize"),
                Source = this,
            });
            ep.Fill = new SolidColorBrush(getPlayerColor());
            Grid.SetColumn(ep, col);
            Grid.SetRow(ep, getGridRow(_model.Top(col)));
            this.TheGrid.Children.Add(ep);

            // check for winner
            if (_model.IsWinner(_model.Top(col) - 1, col))
            {
                MessageBox.Show(string.Format("Player {0} wins!", _curPlayer));
                SetEnableMoveEllipses(false);
            }

            _curPlayer = (_curPlayer == 1 ? 2 : 1);
        }

        private Color getPlayerColor()
        {
            return getPlayerColor(_curPlayer);
        }
        private Color getPlayerColor(int player)
        {
            if (player == 1)
            {
                return Color.FromArgb(255, 255, 0, 0);
            }

            return Color.FromArgb(255, 0, 0, 0);
        }

        public double TokenSize
        {
            get { return (double?)this.Resources["TokenSize"] ?? 0.0; }
            set
            {
                this.Resources["TokenSize"] = value;
                OnPropertyChanged("TokenSize");
            }
        }

        private int getGridRow(int modelRow)
        {
            return (NumberOfRows + 1) - (modelRow - 1);
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void MoveGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double curWidth = (sender as Grid).ColumnDefinitions.First().ActualWidth;
            double maxHeight = (sender as Grid).RowDefinitions.First().ActualHeight;
            if (curWidth > 0)
            {
                TokenSize = (maxHeight < curWidth ? maxHeight : curWidth);
                TokenSize *= (1 - TokenMarginFactor);
            }
        }

        private void MoveEllipse_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            _model.Reset();
            this.TheGrid.Children.Clear();
            _curPlayer = 1;
            SetEnableMoveEllipses(true);
        }

        private void SetEnableMoveEllipses(bool enable)
        {
            for (int i = 0; i < NumberOfColumns; i++)
            {
                if (enable) EnableMoveEllipse(_moveEllipsesList[i]);
                else DisableMoveEllipse(_moveEllipsesList[i]);
            }
        }

        private void EnableMoveEllipse(Ellipse ep)
        {
            ep.IsEnabled = true;
            ep.Fill = new SolidColorBrush(Color.FromArgb(20, 153, 153, 153));
            ep.Stroke = new SolidColorBrush(Colors.DarkGray);
            this.TheGrid.Children.Add(ep);
        }
        private void DisableMoveEllipse(Ellipse ep)
        {
            ep.IsEnabled = false;
            ep.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            ep.Stroke = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            this.TheGrid.Children.Remove(ep);
        }
    }
}
