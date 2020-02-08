using System.Collections.Generic;

namespace Domain.Entities
{
    public class Guest
    {
        public string Id { get; set; }

        public string IdCardNumber { get; set; }

        public string DormitoryCardNumber { get; set; }

        public Room Room { get; set; }

        public int DistanceFromHome { get; set; }

        public ICollection<AccomodationRequest> AccomodationRequests { get; set; }

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
