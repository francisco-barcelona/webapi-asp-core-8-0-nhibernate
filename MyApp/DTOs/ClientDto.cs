namespace MyApp.DTOs
{
    public class ClientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<SalesDto> Sales { get; set; }
    }
}
