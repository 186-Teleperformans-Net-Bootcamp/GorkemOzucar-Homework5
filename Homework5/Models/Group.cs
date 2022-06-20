using Microsoft.AspNetCore.Identity;

namespace Homework5.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<IdentityUser> GroupMembers { get; set; }
        public IdentityUser GroupOwner { get; set; }
    }
}
