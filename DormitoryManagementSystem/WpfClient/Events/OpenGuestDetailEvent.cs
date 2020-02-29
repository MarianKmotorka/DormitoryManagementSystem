namespace WpfClient.Events
{
    public class OpenGuestDetailEvent
    {
        public string GuestId { get; }

        public object Sender { get; }

        public OpenGuestDetailEvent(object sender, string guestId)
        {
            GuestId = guestId;
            Sender = sender;
        }
    }
}
