namespace LibraryCms.Models
{
    public class Role
    {
        public string RoleId { get; set; }

        public string RoleName { get; set; }

        public Department Department { get; set; }

        public string Rights { get; set; }
    }
}