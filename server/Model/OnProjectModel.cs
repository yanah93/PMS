using System.ComponentModel.DataAnnotations;

namespace PMS.Model
{
    public class OnProjectModel
    {
        [Key]
        public int Id { get; set; }
        public int? ProjectId { get; set; }
        public int? ClientPartnerId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public bool isClient { get; set; }
        public bool isPartner { get; set; }
        public string Description { get; set; }
    }
}
