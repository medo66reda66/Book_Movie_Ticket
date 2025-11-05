using Book_Movie_Tickets.Models;
using Microsoft.EntityFrameworkCore;

namespace Book_Movie_Ticket.Models
{
    [PrimaryKey(nameof(Movieid), nameof(ApplicationUserId))]
    public class Cart
    {
        public int Movieid { get; set; }
        public Movie Movie { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int count { get; set; }
        public decimal Price { get; set; }
    }
}
