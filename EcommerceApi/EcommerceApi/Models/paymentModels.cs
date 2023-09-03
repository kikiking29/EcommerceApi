namespace EcommerceApi.Models
{
    public class paymentModels
    {
        public int id_payment { get; set; }
        public int id_users { get; set; }
        public int id_orders { get; set; }
        public DateTime p_datetime { get; set; }
        public string? p_status { get; set; }
    }
}
