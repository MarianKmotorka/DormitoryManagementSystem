using System.Collections.Generic;

namespace Domain.Entities
{
    public class Repairer
    {
        public string Id { get; set; }

        public ICollection<RepairRequest> RepairRequests { get; set; }

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

        public Repairer()
        {
            RepairRequests = new List<RepairRequest>();
        }
    }
}
