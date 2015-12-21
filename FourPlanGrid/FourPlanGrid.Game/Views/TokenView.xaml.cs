namespace FourPlanGrid.Game.Views
{
    using System.Windows.Controls;
    /// <summary>
    /// Interaction logic for TokenView.xaml
    /// </summary>
    public partial class TokenView : UserControl
    {
        public TokenView()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.TokenViewModel(ViewModels.ApplicationService.Instance.EventAggregator);
        }
    }
}
