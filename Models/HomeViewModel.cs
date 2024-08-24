using HotelReservation.Models;
using static iTextSharp.text.pdf.AcroFields;

namespace HotelReservation.Models
{
    public class HomeViewModel
    {
        public Customer Customer { get; set; }
        public IEnumerable<Hotel> Hotel { get; set; }
        public Contact Contact { get; set; }
        public IEnumerable<Testimonial> Testimonial { get; set; }
    }

}
