namespace WpfClient.Events
{
    public class GoBackEvent
    {
        public object ViewModel { get; }

        public GoBackEvent(object viewModel)
        {
            ViewModel = viewModel;
        }
    }
}
