namespace EcommerceApi.Models
{
    public class ordersModels
    {
        public int id_orders { get; set; }
        public int id_users { get; set; }
        public int o_total { get; set; }
        public DateTime o_datetime { get; set; }
        public string? o_status { get; set; }
    }
}
