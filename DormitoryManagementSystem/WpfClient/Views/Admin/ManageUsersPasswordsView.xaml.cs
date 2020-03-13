using System.Windows.Controls;
using WpfClient.ViewModels.Admin;

namespace WpfClient.Views.Admin
{
    /// <summary>
    /// Interaction logic for ManageUsersPasswordsView.xaml
    /// </summary>
    public partial class ManageUsersPasswordsView : UserControl
    {
        public ManageUsersPasswordsView()
        {
            InitializeComponent();
        }

        private async void UsersComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            await (DataContext as ManageUsersPasswordsViewModel)?.LoadUsers();
            e.Handled = true;
        }
    }
}
