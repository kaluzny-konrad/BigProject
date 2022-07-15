using Microsoft.AspNetCore.Identity;

namespace BigProject.Data.Entities;

public class StoreUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
