using System.ComponentModel.DataAnnotations;

namespace PMS.Model
{
    public class ClientPartnerModel
    {
        [Key]
        public int Id { get; set; }
        public string CpName { get; set; }
        public string CpAddress { get; set; }
        public string CpDetails { get; set; }
    }
}
