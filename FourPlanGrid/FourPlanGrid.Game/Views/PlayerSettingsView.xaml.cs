namespace FourPlanGrid.Game.Views
{
    using System.Windows;
    using System.Windows.Controls;
    /// <summary>
    /// Interaction logic for PlayerSettingsView.xaml
    /// </summary>
    public partial class PlayerSettingsView : UserControl
    {
        public PlayerSettingsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Dependency property so we can pass a "heading" for the group box in the xaml
        /// </summary>
        public static readonly DependencyProperty HeadingProperty = DependencyProperty.Register
            ("Heading",typeof(string), typeof(PlayerSettingsView), new PropertyMetadata(string.Empty));
        public string Heading
        {
            get { return (string)GetValue(HeadingProperty); }
            set { SetValue(HeadingProperty, value); }
        }

    }
}
