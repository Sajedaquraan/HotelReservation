namespace HotelReservation.Models
{
    public class ProfitLossReport
    {
        public string Period { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal ProfitOrLoss { get; set; }
        public decimal CumulativeProfitOrLoss { get; set; }
    }
}
