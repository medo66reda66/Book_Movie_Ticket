using System.ComponentModel.DataAnnotations;

namespace Book_Movie_Ticket.MovieVM
{
    public class ForgetPasswordVm
    {
        public int Id { get; set; }
        [Required]
        public string UserNameOREmail { get; set; } = string.Empty;
    }
}
