namespace FourPlanGrid.Game.Views
{
    using System.Windows;
    using FourPlanGrid.Game.ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        /// <summary>
        /// Initializes the player settings VMs and sets the player number and binds them to the player settings views
        /// </summary>
        public MainWindowView()
        {
            InitializeComponent();

            MainWindowViewModel mainWindowVM = new MainWindowViewModel();
            PlayerSettingsViewModel playerOneSettingsVM = new PlayerSettingsViewModel(ApplicationService.Instance.EventAggregator);
            PlayerSettingsViewModel playerTwoSettingsVM = new PlayerSettingsViewModel(ApplicationService.Instance.EventAggregator);

            playerOneSettingsVM.Player = 1;
            playerTwoSettingsVM.Player = 2;

            this.DataContext = mainWindowVM;

            PlayerOneSettingsView.DataContext = playerOneSettingsVM;
            PlayerTwoSettingsView.DataContext = playerTwoSettingsVM;
        }
    }
}
