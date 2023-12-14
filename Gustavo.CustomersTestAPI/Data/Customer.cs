namespace Gustavo.CustomersTestAPI.Data
{
    public class Customer
    {
        public int Id { get; set; }
        public char ClientType { get; set; }
        public string? CPF { get; set; } = null!;
        public string? CNPJ { get; set; } = null!;
        public string? FullName { get; set; } = null!;
        public string? CompanyName { get; set; } = null!;
        public string? TradeName {  get; set; } = null!;

    }
}
