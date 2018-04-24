using System.Globalization;
using System.Windows;
using Fluent;
using Fluent.Localization.Languages;

namespace Client
{
    /// <summary>
    /// Interaction logic for ConcurrentActionsWindow.xaml
    /// </summary>
    public partial class ConcurrentActionsWindow : Window
    {
        public ConcurrentActionsWindow()
        {
            InitializeComponent();
            
            // TODO: hide separators in fluent and action dropdowns if there are no items in galeries
            // TODO: remove context menu when right-clicking fluent ribbon and the possiblity to collapse it (if it's even possible)

            RibbonLocalization.Current.Culture = new CultureInfo("pl-PL", false); // TODO: change this if something becomes annoying AF (e.g. commas in doubles/decimals)
            RibbonLocalization.Current.Localization = new Polish();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
