namespace EcommerceApi.Models
{
    public class orderdetailsModels
    {
        public int id_orderdetails { get; set; }
        public int id_orders { get; set; }
        public int id_users { get; set; }
        public int id_stock { get; set; }
        public int odt_among { get; set; }
        public int odt_totalprice { get; set; }
        public DateTime odt_datetime { get; set; }
    }
}
