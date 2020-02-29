namespace WpfClient.Events
{
    public class GoBackToEvent
    {
        public object ViewModel { get; }

        public GoBackToEvent(object viewModel)
        {
            ViewModel = viewModel;
        }
    }
}
