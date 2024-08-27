using HotelReservation.Models;

namespace HotelReservation.Models
{
    public class ReportViewModel
    {
        public IEnumerable<JoinTables> ReportData { get; set; }
        public TestimonialChartViewModel TestimonialChart { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


    }
}
