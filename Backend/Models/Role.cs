using System.ComponentModel.DataAnnotations;

namespace QuanLyBenhVien.API.Models;

public class Role
{
    [Key]
    public int RoleID { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string? Description { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
