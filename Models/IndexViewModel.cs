namespace HotelReservation.Models
{
    public class IndexViewModel
    {
            public Customer Customer { get; set; }
            public IEnumerable<Hotel> Hotels { get; set; }
            public Contact Contact { get; set; }
        
    }
}
