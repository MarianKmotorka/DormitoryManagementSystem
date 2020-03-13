using System.Collections.Generic;
using Library.Models.Officers;

namespace Library.Models.Offices
{
    public class OfficeModel
    {
        public int Id { get; set; }

        public string OfficeNumber { get; set; }

        public int FreeTables { get; set; }

        public int Capacity { get; set; }

        public IEnumerable<OfficerLookup> Officers { get; set; }
    }
}
