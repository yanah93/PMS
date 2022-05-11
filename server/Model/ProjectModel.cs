using System.ComponentModel.DataAnnotations;

namespace PMS.Model
{
    public class ProjectModel : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
    }
}
