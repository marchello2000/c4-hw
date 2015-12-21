namespace FourPlanGrid.Game.Views
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
        /// Kinda messy, but i'd rather generate the tokenviews in code
        /// </summary>
        private void InitBoard()
        {
            const int NumberOfRows = 6;
            const int NumberOfColumns = 7;

            for (int row = 0; row < NumberOfRows; row++)
            {
                for (int column = 0; column < NumberOfColumns; column++)
                {
                    ViewModels.TokenViewModel tokenVM = new ViewModels.TokenViewModel(ViewModels.ApplicationService.Instance.EventAggregator);
                    TokenView tokenV = new TokenView();
                    tokenV.DataContext = tokenVM;
                    GameGrid.Children.Add(tokenV);

                    tokenVM.Row = row;
                    tokenVM.Column = column;
                    tokenVM.State = Models.TokenState.Placed;
                    tokenVM.Player = row * column % 2 + 1;
                    
                    
                }
            }
        }
    }
}
