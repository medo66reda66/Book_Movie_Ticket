using Book_Movie_Tickets.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Net.Http.Headers;

namespace Book_Movie_Ticket.Models
{
    public class Promotion
    {
        [ValidateNever]
        public int id { get; set; }
        public int Movieid { get; set; }
        [ValidateNever]
        public Movie Movie { get; set; }
        public DateTime Validto { get; set; }
        public DateTime PublishAt { get; set; }= DateTime.Now;
        public bool Isvalid { get; set; }=true;
        public string Code { get; set; }
        public decimal Discount { get; set; }

    }
}
