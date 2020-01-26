namespace Domain.Entities
{
    public class Officer
    {
        private AppUser _appUser;


        public string Id { get; set; }

        public string IdCardNumber { get; set; }

        public string OfficeNumber { get; set; }

        public AppUser AppUser
        {
            get => _appUser;
            set
            {
                if (value != null) Id = value.Id;
                _appUser = value;
            }
        }

    }
}
