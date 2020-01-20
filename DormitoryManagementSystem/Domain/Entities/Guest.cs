namespace Domain.Entities
{
    public class Guest
    {
        public string Id { get; set; }

        public string IdCardNumber { get; set; }

        public string DormitoryCardNumber { get; set; }

        public string RoomNumber { get; set; }


        /// <summary>
        /// Distance in kilometers from home
        /// </summary>
        public int DistanceFromHome { get; set; }

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
