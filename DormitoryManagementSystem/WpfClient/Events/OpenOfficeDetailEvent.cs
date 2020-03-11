namespace WpfClient.Events
{
    public class OpenOfficeDetailEvent
    {
        public object Sender { get; }

        public int Id { get; }

        public OpenOfficeDetailEvent(object sender, int id)
        {
            Sender = sender;
            Id = id;
        }

    }
}
