namespace Domain.Entities
{
    public class Repairer
    {
        public string Id { get; set; }

        //TODO add history of repairs

        public AppUser AppUser
        {
            get => _appUser;
            set
            {
                if (value != null) Id = value.Id;
                _appUser = value;
            }
        }

        private AppUser _appUser;
    }
}
