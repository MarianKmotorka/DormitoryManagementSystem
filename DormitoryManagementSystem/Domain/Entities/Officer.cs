namespace Domain.Entities
{
    public class Officer
    {
        public string Id { get; set; }

        public string IdCardNumber { get; set; }

        public Office Office { get; set; }

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
