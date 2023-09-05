namespace EcommerceApi.Models
{
    public class shopsModels
    {
        public int id_shops { get; set; }
        public int id_users { get; set; }
        public string? sh_name { get; set; }
        public string? sh_type { get; set; }
        public string? sh_description { get; set; }
    }
    public class newshopsModels
    {
        public int id_users { get; set; }
        public string? sh_name { get; set; }
        public string? sh_type { get; set; }
        public string? sh_description { get; set; }
    }
}
