namespace HotelReservation.Models
{
    public class ChartData
    {
        public string Label { get; set; }
        public List<int> Data { get; set; }

        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}
