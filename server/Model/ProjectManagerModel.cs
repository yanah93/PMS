using System.ComponentModel.DataAnnotations;

namespace PMS.Model
{
    public class ProjectManagerModel
    {
        [Key]
        public int Id { get; set; }
        public int? ProjectId { get; set; }
        public int? UserAccountId { get; set; }
        public UserAccountModel? UserAccount { get; set; }
    }
}
