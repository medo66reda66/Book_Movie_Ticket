using System.ComponentModel.DataAnnotations;

namespace Book_Movie_Ticket.MovieVM
{
    public class ResendEmailConfirmVM
    {
        public int Id {  get; set; }
        [Required] 
        public string UsernameORemail { get; set; } = string.Empty;
    }
}
