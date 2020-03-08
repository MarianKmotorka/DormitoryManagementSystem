using System.Windows.Controls;
using WpfClient.ViewModels.AccomodationRequests;

namespace WpfClient.Views.AccomodationRequests
{
    /// <summary>
    /// Interaction logic for RespondToAccomodationRequestView.xaml
    /// </summary>
    public partial class RespondToAccomodationRequestView : UserControl
    {
        public RespondToAccomodationRequestView()
        {
            InitializeComponent();
        }

        private async void RoomsComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            await (DataContext as RespondToAccomodationRequestViewModel)?.LoadRooms();
            e.Handled = true;
        }
    }
}
