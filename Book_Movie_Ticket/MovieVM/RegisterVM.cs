using System.ComponentModel.DataAnnotations;

namespace Book_Movie_Ticket.MovieVM
{
    public class RegisterVM
    {
        public int id { get; set; }
        [Required]
        public string Firstname { get; set; } = string.Empty;
        [Required]
        public string Lastname { get; set; } = string.Empty;
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Phone { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string? Confirmpasswood { get; set; }

    }
}
