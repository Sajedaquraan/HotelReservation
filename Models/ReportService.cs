namespace HotelReservation.Models
{
    public class ReportService
    {
        private readonly ModelContext _context;

        public ReportService(ModelContext context)
        {
            _context = context;
        }

        // Monthly Report
        public IEnumerable<ProfitLossReport> GetMonthlyReport()
        {
            var monthlyData = _context.Paymentrooms
                .Where(p => p.Paymentdate.HasValue)
                .GroupBy(p => new
                {
                    Year = p.Paymentdate.Value.Year,
                    Month = p.Paymentdate.Value.Month
                })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalRevenue = g.Sum(e => e.Amountpaid)
                })
                .ToList();

            var expensesData = _context.Expenseamounts
                .Where(e => e.Expensedate.HasValue)
                .GroupBy(e => new
                {
                    Year = e.Expensedate.Value.Year,
                    Month = e.Expensedate.Value.Month
                })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalExpenses = g.Sum(e => e.Amount)
                })
                .ToList();

            var report = from m in monthlyData
                         join e in expensesData on new { m.Year, m.Month } equals new { e.Year, e.Month } into expensesGroup
                         from e in expensesGroup.DefaultIfEmpty()
                         select new ProfitLossReport
                         {
                             Period = $"{m.Year}-{m.Month:D2}",
                             TotalRevenue = m.TotalRevenue,
                             TotalExpenses = e?.TotalExpenses ?? 0,
                             ProfitOrLoss = m.TotalRevenue - (e?.TotalExpenses ?? 0)
                         };

            var cumulativeReport = report.OrderBy(r => r.Period).ToList();
            decimal cumulativeProfitOrLoss = 0;
            foreach (var item in cumulativeReport)
            {
                cumulativeProfitOrLoss += item.ProfitOrLoss;
                item.CumulativeProfitOrLoss = cumulativeProfitOrLoss;
            }

            return cumulativeReport;
        }


        // Annual Report
        public IEnumerable<ProfitLossReport> GetAnnualReport()
        {
            var annualData = _context.Paymentrooms
                .Where(p => p.Paymentdate.HasValue)
                .GroupBy(p => p.Paymentdate.Value.Year)
                .Select(g => new
                {
                    Year = g.Key,
                    TotalRevenue = g.Sum(e => e.Amountpaid)
                })
                .ToList();

            var expensesData = _context.Expenseamounts
                .Where(e => e.Expensedate.HasValue)
                .GroupBy(e => e.Expensedate.Value.Year)
                .Select(g => new
                {
                    Year = g.Key,
                    TotalExpenses = g.Sum(e => e.Amount)
                })
                .ToList();

            var report = from a in annualData
                         join e in expensesData on a.Year equals e.Year into expensesGroup
                         from e in expensesGroup.DefaultIfEmpty()
                         select new ProfitLossReport
                         {
                             Period = a.Year.ToString(),
                             TotalRevenue = a.TotalRevenue,
                             TotalExpenses = e?.TotalExpenses ?? 0,
                             ProfitOrLoss = a.TotalRevenue - (e?.TotalExpenses ?? 0)
                         };

            var cumulativeReport = report.OrderBy(r => r.Period).ToList();
            decimal cumulativeProfitOrLoss = 0;
            foreach (var item in cumulativeReport)
            {
                cumulativeProfitOrLoss += item.ProfitOrLoss;
                item.CumulativeProfitOrLoss = cumulativeProfitOrLoss;
            }

            return cumulativeReport;
        }


    }
}
