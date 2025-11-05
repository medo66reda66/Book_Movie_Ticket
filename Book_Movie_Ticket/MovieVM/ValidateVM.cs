using System.ComponentModel.DataAnnotations;

namespace Book_Movie_Ticket.MovieVM
{
    public class ValidateVM
    {
        public int Id { get; set; }

        [Required]
        public string OTP { get; set; } = string.Empty;

        public string ApplicationUserId { get; set; } = string.Empty;
    }
}
