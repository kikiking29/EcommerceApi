namespace EcommerceApi.Models
{
    public class addressModels
    {
        public int id_address { get; set; }
        public int id_users { get; set; }
        public string? a_province { get; set; }
        public string? a_district { get; set; }
        public string? a_subdistrict { get; set; }
        public int a_postalcode { get; set; }
        public string? a_streetname { get; set; }
        public string? a_building { get; set; }
        public string? a_housenumber { get; set; }
        public string? a_alley { get; set; }
        public string? a_intersection { get; set; }
        public string? a_locationurl { get; set; }
        public string? a_details { get; set; }
    }
    public class newaddressModels
    {
        public int id_users { get; set; }
        public string? a_province { get; set; }
        public string? a_district { get; set; }
        public string? a_subdistrict { get; set; }
        public int a_postalcode { get; set; }
        public string? a_streetname { get; set; }
        public string? a_building { get; set; }
        public string? a_housenumber { get; set; }
        public string? a_alley { get; set; }
        public string? a_intersection { get; set; }
        public string? a_locationurl { get; set; }
        public string? a_details { get; set; }
    }
}
