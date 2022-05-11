using PMS.Model;

namespace PMS.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string EmployeeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public int UserAccountId { get; set; }
    }
}
