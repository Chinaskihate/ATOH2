using Microsoft.AspNetCore.Identity;

namespace ATOH.Domain.Models;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; }

    public Gender Gender { get; set; }

    public DateTime? BirthDay { get; set; }

    public bool IsAdmin { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public DateTime ModifiedOn { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime? RevokedOn { get; set; }

    public string RevokedBy { get; set; }
}

public enum Gender
{
    Female,
    Male,
    Other
}