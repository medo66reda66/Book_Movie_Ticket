namespace Book_Movie_Ticket.FilterMonieVM
{
    public record Filtermovie(
    string name,double? minprice, double? maxprice, int? categoryid , int? cinemaid ,bool status
    );
}
