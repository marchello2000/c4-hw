﻿namespace FourPlanGrid.Game.Views
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for GameBoardView.xaml
    /// </summary>
    public partial class GameBoardView : UserControl
    {
        public GameBoardView()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.GameBoardViewModel(ViewModels.ApplicationService.Instance.EventAggregator);
            InitBoard();
        }

        /// <summary>
        /// Creates the TokenView and adds them to the game board. I think it's ok mvvm for the board view to know about 
        /// the token views to this limited extent.
        /// </summary>
        private void InitBoard()
        {
            
            for (int row = 0; row < ViewModels.GameBoardViewModel.NumberOfRows; row++)
            {
                for (int column = 0; column < ViewModels.GameBoardViewModel.NumberOfColumns; column++)
                {
                    TokenView tokenV = new TokenView();
                    GameGrid.Children.Add(tokenV);
                    // in the view, we bind the grid.row/column properties to the VM Row/Column properties in a 
                    // style setter (needed for attached properties). UpdateSourceTrigger on property change and
                    // OneWayToSource so the default values in the VM don't update the view. If we ever want the
                    // view to reflect row/col changes in the VM/Model then we need a different approach here.
                    tokenV.SetValue(Grid.RowProperty, row);
                    tokenV.SetValue(Grid.ColumnProperty, column);
                }
            }
        }
    }
}
