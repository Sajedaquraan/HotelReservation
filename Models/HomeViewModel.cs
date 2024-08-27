using HotelReservation.Models;
using static iTextSharp.text.pdf.AcroFields;

namespace HotelReservation.Models
{
  public class HomeViewModel
{
    public Customer User { get; set; }
    public IEnumerable<Hotel> Hotels { get; set; }
    public Contact Contact { get; set; }
    public IEnumerable<Testimonial> Testimonials { get; set; }
    public IEnumerable<Todayspecial> Todayspecials { get; set; }
    public string Gallery1 { get; set; }
    public string Gallery2 { get; set; }
    public string Gallery3 { get; set; }
    public string Gallery4 { get; set; }
    public string Gallery5 { get; set; }
    public string Gallery6 { get; set; }
    public string Gallery7 { get; set; }
    public string Gallery8 { get; set; }
    public string Gallery9 { get; set; }
    public string Gallery10 { get; set; }
}

}
