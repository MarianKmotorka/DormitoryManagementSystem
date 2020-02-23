using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfClient.Views.Guests
{
    /// <summary>
    /// Interaction logic for GuestRegistrationView.xaml
    /// </summary>
    public partial class GuestRegistrationView : UserControl
    {
        public GuestRegistrationView()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
