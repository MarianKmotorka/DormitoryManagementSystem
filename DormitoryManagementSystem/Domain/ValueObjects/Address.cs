namespace Domain.ValueObjects
{
    public class Address
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string PostCode { get; set; }

        public static Address Empty
        {
            get
            {
                return new Address
                {
                    City = "",
                    Country = "",
                    HouseNumber = "",
                    PostCode = "",
                    Street = ""
                };
            }
        }

        public override string ToString()
        {
            return $"{Street} {HouseNumber}, {City} {PostCode}, {Country}";
        }
    }
}
