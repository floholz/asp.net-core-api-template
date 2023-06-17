using System.ComponentModel.DataAnnotations;
using asp.net_core_api_template.Models.Enums;
using asp.net_core_api_template.Models.Responses;

namespace asp.net_core_api_template.Models;

public class User: IModel<UserResponse>
{
    public int Id { get; set; }
    [EmailAddress]
    public string Email { get; set; } = null!;
    public Role UserRole { get; set; }
    public string Password { get; set; } = null!;
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhotoBase64 { get; set; }
    
    /* Base Properties */
    public DateTime CreatedAt { get; set; }
    public int? CreatedById { get; set; }
    public User? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedById { get; set; }
    public User? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public int? DeleteById { get; set; }
    public User? DeleteBy { get; set; }

    public UserResponse Map()
    {
        return new UserResponse(this);
    }
    
    /* Custom Properties */
    public string FullName => LastName + (FirstName == null ? "" : $" {FirstName}");
}