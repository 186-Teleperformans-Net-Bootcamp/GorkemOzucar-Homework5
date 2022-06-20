using Microsoft.AspNetCore.Identity;

namespace Homework5.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public int MessageType { get; set; }
        public string MessageContent { get; set; }
        public bool IsPublicMessage { get; set; }
        public Group? ToGroupId { get; set; }
        public IdentityUser? ToUser { get; set; }
        public virtual IdentityUser FromUser { get; set; }

        
    }
}
