using HotelReservation.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Controllers
{
    public class ChartController : Controller
    {
        private readonly ModelContext _context;

        public ChartController(ModelContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Example data - replace with data from your database
            
            var registrations = new List<ChartData>
        {
            new ChartData { Date = new DateTime(2024, 8, 15), Count = 5 },
            new ChartData { Date = new DateTime(2024, 8, 16), Count = 12 },
            new ChartData { Date = new DateTime(2024, 8, 17), Count = 7 },
            new ChartData { Date = new DateTime(2024, 8, 18), Count = 9 },
            new ChartData { Date = new DateTime(2024, 8, 19), Count = 14 }
        };
            

     


            // Group by date and sum the registrations
            var groupedData = registrations
                .GroupBy(r => r.Date)
                .Select(g => new
                {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    Count = g.Sum(r => r.Count)
                })
                .ToList();

            ViewBag.Dates = groupedData.Select(g => g.Date).ToArray();
            ViewBag.Counts = groupedData.Select(g => g.Count).ToArray();

            return View();
        }


        public IActionResult PieChart()
        {
            var chartData = new ChartData
            {
                Label = "Sales Data",
                Data = new List<int> { 30, 50, 100, 40, 70 }
            };

            return View(chartData);
        }
    }
}
