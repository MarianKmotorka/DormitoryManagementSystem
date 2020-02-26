using System.Collections.Generic;

namespace Domain.Entities
{
    public class Office
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public int Capacity { get; set; }

        public ICollection<Officer> Officers { get; set; }

        public Office()
        {
            Officers = new List<Officer>();
        }
    }
}
