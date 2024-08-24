namespace HotelReservation.Models
{
    public class ProfitLossReport
    {
        public string Period { get; set; }  // "Month" for monthly report, "Year" for annual report
        public decimal TotalRevenue { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal ProfitOrLoss { get; set; }
    }
}
