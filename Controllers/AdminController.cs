using HotelReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Controllers
{
    public class AdminController : Controller
    {

        private readonly ModelContext _context;
        private readonly ReportService _reportService;

        public AdminController(ModelContext context, ReportService reportService)
        {
            _context = context;
            _reportService = reportService;
        }

        public IActionResult Index()
        {
            //ViewBag.name = HttpContext.Session.GetString("AdminName");
            var id = HttpContext.Session.GetInt32("AdminID");
            var users = _context.Customers.Where(x => x.Customerid == id).SingleOrDefault();
            ViewBag.name = users.Customername;
            ViewBag.image = users.Profileimage;
            ViewBag.email = users.Email;

            ViewBag.numberOfCustomer = _context.Customers.Count();

            ViewBag.numberofAvailableRoom = _context.Rooms
                .Where(x => x.Availabilitystatus == "available")
                .Count();

            ViewBag.numberofBookedRoom = _context.Rooms
                .Where(x => x.Availabilitystatus == "Unavailable")
                .Count();

            ViewBag.numberofTestimonial = _context.Testimonials.Count();


            var customers = _context.Customers.ToList();
            ViewBag.Customers = customers;


            ViewBag.TotalPrice = _context.Paymentrooms.Sum(x => x.Amountpaid);

            var Payment = _context.Paymentrooms.ToList();
            ViewBag.Payment = Payment;

            var result=Tuple.Create<IEnumerable<Customer>, IEnumerable<Paymentroom>>(customers,Payment);
            return View(result);
        }


        public IActionResult Profile()
        {
            var id = HttpContext.Session.GetInt32("AdminID");
            var user = _context.Customers.SingleOrDefault(x => x.Customerid == id);

            if (user == null)
            {
                // Handle the case where the user is not found
                return RedirectToAction("Login", "RegisterAndLogin");
            }

            ViewBag.UserId = id;
            ViewBag.Name = user.Customername;
            ViewBag.Image = user.Profileimage;
            ViewBag.Email = user.Email;

            return View(user);
        }



        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _context.Customers.SingleOrDefault(x => x.Customerid == id);
            var id1 = HttpContext.Session.GetInt32("AdminID");
            var users = _context.Customers.Where(x => x.Customerid == id1).SingleOrDefault();
            ViewBag.name = users.Customername;
            ViewBag.image = users.Profileimage;
            ViewBag.email = users.Email;
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [HttpPost]
        public IActionResult Edit(Customer editedCustomer)
        {

            var sessionId = HttpContext.Session.GetInt32("AdminID");

            if (sessionId == null || sessionId != editedCustomer.Customerid)
            {
                return RedirectToAction("Forbidden", "Home"); // Or any error page you prefer
            }

            // Fetch the existing customer from the database
            var user = _context.Customers.SingleOrDefault(x => x.Customerid == editedCustomer.Customerid);
            var login = _context.Userlogins.Where(x => x.Customerid == user.Customerid).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            // Update the customer details
            user.Customername = editedCustomer.Customername;
            user.Email = editedCustomer.Email;
            user.Password= editedCustomer.Password;
            user.Profileinfo = editedCustomer.Profileinfo;
            user.ImageFile = editedCustomer.ImageFile; // Assuming the image upload is handled
            ViewBag.Image=user.ImageFile;

            // Update other fields if needed

            login.Email = user.Email;
            login.Password = user.Password;

            // Save the changes to the database
            _context.SaveChanges();

            // Redirect back to the profile page
            return RedirectToAction("Profile", "Admin");
        }

        [HttpGet, HttpPost]
        public IActionResult Report(DateTime? startDate, DateTime? endDate)
        {
            
            var Hotel = _context.Hotels.ToList();
            var Room = _context.Rooms.ToList();
            var ReservationRoom = _context.Reservationrooms.ToList();
            var PaymentRoom = _context.Paymentrooms.ToList();

            var result = from h in Hotel
                         join r in Room on h.Hotelid equals r.Hotelid
                         join rr in ReservationRoom on r.Roomid equals rr.Roomid
                         join pr in PaymentRoom on rr.Reservationid equals pr.Reservationid
                         select new JoinTables
                         {
                             Hotel = h,
                             Room = r,
                             ReservationRoom = rr,
                             PaymentRoom = pr
                         };

            if (startDate == null && endDate == null) 
            {
                result = result.ToList();
            }

            else if (startDate != null && endDate == null) //1/4/2024 
            {
                result = result.Where(x => x.ReservationRoom.Reservationdate.Date >= startDate).ToList();
            }

            else if (startDate == null && endDate != null) //1/5/2024
            {
                result = result.Where(x => x.ReservationRoom.Reservationdate.Date <= endDate).ToList();
            }
            else if (startDate != null && endDate != null)
            {
                result=result.Where(x => x.ReservationRoom.Reservationdate.Date >= startDate && x.ReservationRoom.Reservationdate.Date <= endDate).ToList();
            }

            
                        // Group testimonials by state
                        var testimonialData = _context.Testimonials
                            .GroupBy(t => t.State)
                            .Select(g => new
                            {
                                State = g.Key,
                                Count = g.Count()
                            })
                            .ToList();

                        // Prepare the ViewModel for the chart
                        var testimonialChart = new TestimonialChartViewModel
                        {
                            Labels = testimonialData.Select(d => d.State).ToList(),
                            Counts = testimonialData.Select(d => d.Count).ToList()
                        };

                        // Prepare the combined ViewModel
                        var viewModel = new ReportViewModel
                        {
                            ReportData = result.ToList(),
                            TestimonialChart = testimonialChart,
                            StartDate = startDate,
                            EndDate = endDate
                        };
            return View(viewModel);
            
        }

        public IActionResult Monthly()
        {
            var model = _reportService.GetMonthlyReport();
            return View(model);
        }

        public IActionResult Annual()
        {
            var model = _reportService.GetAnnualReport();
            return View(model);
        }
    }
}