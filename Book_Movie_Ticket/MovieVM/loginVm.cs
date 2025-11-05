using System.ComponentModel.DataAnnotations;

namespace Book_Movie_Ticket.MovieVM
{
    public class loginVm
    {
        public int Id { get; set; }
        [Required]
       public string UsernameORemail { get; set; }=String.Empty;
        [Required,DataType(DataType.Password)]
        public string PaSsword {  get; set; }=String.Empty ;
        public bool Rememberme {  get; set; }
     
    }
}
