using Microsoft.AspNetCore.Identity;

namespace Book_Movie_Ticket.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Firestname { get; set; }=string.Empty;
        public string Lasttname { get; set; }=string.Empty ;
        public string? Addrress { get; set; }
    }
}
