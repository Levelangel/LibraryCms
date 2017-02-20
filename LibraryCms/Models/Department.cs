namespace LibraryCms.Models
{
    public enum DepartmentType
    {
        X,//0
        B,//1
        A//2
    }
    public class Department
    {
        public string DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public DepartmentType DepartmentType { get; set; }
    }
}