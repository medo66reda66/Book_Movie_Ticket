namespace Book_Movie_Ticket.Models
{
    public class ApplicationuserOtp
    {
        public string id { get; set; }
        public string OTP {  get; set; }
        public DateTime Validto { get; set; }
        public DateTime CreateAt { get; set; }
        public bool Isvalid { get; set; }
        public string Applicationuserid { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
