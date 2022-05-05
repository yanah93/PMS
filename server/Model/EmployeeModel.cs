namespace PMS.Model
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public string EmployeeeCode { get; set; }
        public string EmployeeName { get; set; }
        //public int UserAccountId { get; set; }
        public UserAccountModel? UserAccount { get; set; }
    }
}
