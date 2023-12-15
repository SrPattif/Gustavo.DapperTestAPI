namespace Gustavo.CustomersTestAPI.Data
{
    public class CustomerAddress
    {
        public int AddressId {  get; set; }
        public string Address { get; set; }
        public int HouseNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public int CustomerId { get; set; }
    }
}
