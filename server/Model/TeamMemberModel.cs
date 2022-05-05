namespace PMS.Model
{
    public class TeamMemberModel
    {
        public int Id { get; set; }
        //public int TeamId { get; set; }
        public TeamModel? Team { get; set; }
        //public int RoleId { get; set; }
        public RoleModel? Role { get; set; }
        //public int EmployeeId { get; set; }
        public EmployeeModel? Employee { get; set; }

    }
}
