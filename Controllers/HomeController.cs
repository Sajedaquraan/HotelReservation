using HotelReservation.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
//using System.Web.Mvc;
//using System.Web.Security; // For FormsAuthentication

namespace HotelReservation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _environment;
        public HomeController(ILogger<HomeController> logger, ModelContext context, IWebHostEnvironment environment)
        {
            _logger = logger;
            _context = context;
            _environment = environment;

        }

        public IActionResult Index()
        {
            var id = HttpContext.Session.GetInt32("CustomerID");
            var user = _context.Customers.Where(x => x.Customerid == id).SingleOrDefault();

            // Set default profile image
            var defaultProfileImage = "default-profile-image.jpg"; // Adjust the path as necessary
            ViewBag.Name = user?.Customername;
            ViewBag.Image = user?.Profileimage ?? defaultProfileImage; // Use default image if profile image is null
            ViewBag.Email = user?.Email;

            var hotels = _context.Hotels.ToList();

            var pageImage = _context.Pages
                .Where(p => p.Pagename == "Logo")
                .Select(p => p.Pageimage).FirstOrDefault();
            ViewBag.PageImage = pageImage;

            var ContactEmail = _context.Pages
                .Where(p => p.Pagename == "ContactEmail")
                .Select(p => p.Pagecontent).FirstOrDefault();
            ViewBag.ContactEmail = ContactEmail;

            var ContactPhone = _context.Pages
                .Where(p => p.Pagename == "ContactPhone")
                .Select(p => p.Pagecontent).FirstOrDefault();
            ViewBag.ContactPhone = ContactPhone;

            var ContactLocation = _context.Pages
                .Where(p => p.Pagename == "ContactLocation")
                .Select(p => p.Pagecontent).FirstOrDefault();
            ViewBag.ContactLocation = ContactLocation;

            var ReservationPhone = _context.Pages
                .Where(p => p.Pagename == "ReservationPhone")
                .Select(p => p.Pagecontent).FirstOrDefault();
            ViewBag.ReservationPhone = ReservationPhone;


            var ReservationEmail = _context.Pages
                .Where(p => p.Pagename == "ReservationEmail")
                .Select(p => p.Pagecontent).FirstOrDefault();
            ViewBag.ReservationEmail = ReservationEmail;

            var FooterAbout = _context.Pages
                .Where(p => p.Pagename == "FooterAbout")
                .Select(p => p.Pagecontent).FirstOrDefault();
            ViewBag.FooterAbout = FooterAbout;


            var AboutImage = _context.Pages
                .Where(p => p.Pagename == "About")
                .Select(p => p.Pageimage).FirstOrDefault();
            ViewBag.AboutImage = AboutImage;

            var AboutContent = _context.Pages
                .Where(p => p.Pagename == "About")
                .Select(p => p.Pagecontent).FirstOrDefault();
            ViewBag.AboutContent = AboutContent;

            var Testimonial = _context.Testimonials.ToList();

            var contact = new Contact();
            var model = Tuple.Create<Customer, IEnumerable<Hotel>,Contact, IEnumerable<Testimonial>>(user, hotels, contact, Testimonial);

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Contactusid,Contactusname,Contactusemail,Contactusdescription")] Contact contact)
        {
            if (ModelState.IsValid)
            {                

                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }


            //var model =  Tuple.Create<IEnumerable<Contact>>;
            return View(contact);
        }

        public IActionResult Contact() 
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact([Bind("Contactusid,Contactusname,Contactusemail,Contactusdescription")] Contact contact)
        {
            if (ModelState.IsValid)
            {

                _context.Add(contact);
                await _context.SaveChangesAsync();

                SendEmailcontact(contact.Contactusemail);

                return RedirectToAction("Index");
            }
            return View(contact);
        }
        public void SendEmailcontact(string recipientEmail)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Hotel", "hotel@example.com"));
            message.To.Add(new MailboxAddress("", recipientEmail));
            message.Subject = "Payment Confirmation";

            var bodyBuilder = new BodyBuilder
            {
                TextBody = "Thank you for contacting us. We will get back to you as soon as possible.\n\nBest regards,\nYour Website Team"
            };

            // Attach the PDF
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("sajedaAlquraan1@gmail.com", "izdw sras niqv jnnh");
                client.Send(message);
                client.Disconnect(true);
            }
        }


        [HttpGet]
        public IActionResult GetRoomAndEventById(int id)
        {
            var rooms = _context.Rooms.Where(x => x.Hotelid == id && x.Availabilitystatus.Trim().ToLower() == "available").ToList();                  


            var id1 = HttpContext.Session.GetInt32("CustomerID");
            var user = _context.Customers.Where(x => x.Customerid == id1).SingleOrDefault();

            // Set default profile image
            var defaultProfileImage = "default-profile-image.jpg"; // Adjust the path as necessary
            ViewBag.Name = user?.Customername;
            ViewBag.Image = user?.Profileimage ?? defaultProfileImage; // Use default image if profile image is null
            ViewBag.Email = user?.Email;




            var model = Tuple.Create<IEnumerable<Room>, Customer>(rooms, user);
            return View(model);  // Ensure this matches the view's model type
        }

        [HttpPost]
        public IActionResult GetRoomAndEventById(int id, DateTime? startDate, DateTime? endDate)
        {
            var rooms = _context.Rooms.Where(x => x.Hotelid == id && x.Availabilitystatus.Trim().ToLower() == "available").ToList();
            var customers = _context.Customers.Where(c => c.Customerid == id).ToList(); // Adjust this as needed

            if (startDate == null && endDate == null)
            {
                var model = Tuple.Create<IEnumerable<Room>, IEnumerable<Customer>>(rooms, customers);
                return View(model);
            }
            else if (startDate != null && endDate == null)
            {
                rooms = rooms.Where(x => x.Dateto.Value.Date >= startDate && x.Availabilitystatus.Trim().ToLower() == "available").ToList();
                var model = Tuple.Create<IEnumerable<Room>, IEnumerable<Customer>>(rooms, customers);
                return View(model);
            }
            else if (startDate == null && endDate != null)
            {
                rooms = rooms.Where(x => x.Datefrom.Value.Date <= endDate && x.Availabilitystatus.Trim().ToLower() == "available").ToList();
                var model = Tuple.Create<IEnumerable<Room>, IEnumerable<Customer>>(rooms, customers);
                return View(model);
            }
            else
            {
                rooms = rooms.Where(x => x.Dateto.Value.Date >= startDate && x.Datefrom.Value.Date <= endDate && x.Availabilitystatus.Trim().ToLower() == "available").ToList();
                var model = Tuple.Create<IEnumerable<Room>, IEnumerable<Customer>>(rooms, customers);
                return View(model);
            }
        }





        [HttpGet]
        public IActionResult BookRoom(int roomId)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Roomid == roomId);

            if (room == null)
            {
                return NotFound();
            }

            var id1 = HttpContext.Session.GetInt32("CustomerID");
            var user = _context.Customers.Where(x => x.Customerid == id1).SingleOrDefault();

            // Set default profile image
            var defaultProfileImage = "default-profile-image.jpg"; // Adjust the path as necessary
            ViewBag.Name = user?.Customername;
            ViewBag.Image = user?.Profileimage ?? defaultProfileImage; // Use default image if profile image is null
            ViewBag.Email = user?.Email;

            ViewBag.RoomId = roomId;
            ViewBag.RoomPrice = room.Price;

            return View("Payment");
        }

        [HttpPost]
        public IActionResult ProcessPayment(int roomId, int Bankid, string Cardnumber, string Cardholdername, string Cvv)
        {
            // Step 1: Check if the card details exist in the bank
            var bank = _context.Banks
                .FirstOrDefault(b => b.Bankid == Bankid
                                     && b.Cardnumber == Cardnumber
                                     && b.Cardholdername == Cardholdername
                                     && b.Cvv == Cvv);

            if (bank == null)
            {
                // If the card information is not found, return an error message
                TempData["ErrorMessage"] = "The information of the card is not true. Please check your details and try again.";
                return RedirectToAction("BookRoom", new { roomId = roomId });
            }

            // Step 2: Retrieve the logged-in user's CustomerID, name, and email from the session

            var id = HttpContext.Session.GetInt32("CustomerID");
            var user = _context.Customers.Where(x => x.Customerid == id).SingleOrDefault();
            ViewBag.Name = user?.Customername;
            ViewBag.Email = user?.Email;



            if (id == null || ViewBag.Name == null || ViewBag.Email == null)
            {
                // If the customer is not logged in, redirect to the login page
                TempData["ErrorMessage"] = "Please log in to complete the booking.";
                return RedirectToAction("Login");
            }

            // Step 3: Check if the room exists
            var room = _context.Rooms.FirstOrDefault(r => r.Roomid == roomId);
            if (room == null)
            {
                return NotFound();
            }

            // Step 4: Check if the bank balance is sufficient to pay for the room
            if (bank.Balance < room.Price)
            {
                // If the balance is less than the room price, show an error message
                TempData["ErrorMessage"] = "You do not have enough money to pay for this room.";
                return RedirectToAction("BookRoom", new { roomId = roomId });
            }

            // Step 5: Deduct the room price from the bank balance
            bank.Balance -= room.Price;
            _context.Banks.Update(bank);

            // Step 6: Create and save a reservation record
            var reservation = new Reservationroom
            {
                Roomid = roomId,
                Reservationdate = DateTime.Now,
                Paymentstatus = "Paid",
                Customerid = id.Value // Assign the logged-in customer's ID
            };
            _context.Reservationrooms.Add(reservation);

            // Save the reservation first to ensure the Reservationid is generated
            _context.SaveChanges();

            // Step 7: Create a payment record linked to the saved reservation
            var payment = new Paymentroom
            {
                Reservationid = reservation.Reservationid, // The generated Reservationid is used here
                Amountpaid = room.Price,
                Paymentdate = DateTime.Now,
                Bankid = bank.Bankid // Link the payment to the valid bank record
            };
            _context.Paymentrooms.Add(payment);

            // Step 8: Generate the payment PDF using the customer's dynamic data
            string roomDetails = $"Room ID: {room.Roomid}, Price: ${room.Price}";
            var pdfBytes = GeneratePaymentPdf(ViewBag.Name, room.Price, DateTime.Now, roomDetails);

            // Step 9: Send the email with the PDF
            SendEmailWithPdf(ViewBag.Email, pdfBytes, "PaymentReceipt.pdf");

            // Step 10: Update room availability status
            room.Availabilitystatus = "Unavailable";
            _context.Rooms.Update(room);

            // Save all changes to the database
            _context.SaveChanges();

            // Redirect to a confirmation page or the homepage
            return RedirectToAction("GetRoomAndEventById");
        }


        public void SendEmailWithPdf(string recipientEmail, byte[] pdfBytes, string fileName)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Hotel", "hotel@example.com"));
        message.To.Add(new MailboxAddress("", recipientEmail));
        message.Subject = "Payment Confirmation";

        var bodyBuilder = new BodyBuilder
        {
            TextBody = "Thank you for your payment. Please find your payment receipt attached."
        };

        // Attach the PDF
        bodyBuilder.Attachments.Add(fileName, pdfBytes, new ContentType("application", "pdf"));
        message.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            client.Connect("smtp.gmail.com", 587, false);
            client.Authenticate("sajedaAlquraan1@gmail.com", "izdw sras niqv jnnh");
            client.Send(message);
            client.Disconnect(true);
        }
    }



    public byte[] GeneratePaymentPdf(string customerName, decimal amountPaid, DateTime paymentDate, string roomDetails)
    {
        using (var memoryStream = new MemoryStream())
        {
            Document document = new Document();
            PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            document.Add(new Paragraph("Payment Receipt"));
            document.Add(new Paragraph($"Customer Name: {customerName}"));
            document.Add(new Paragraph($"Amount Paid: ${amountPaid}"));
            document.Add(new Paragraph($"Payment Date: {paymentDate.ToString("MM/dd/yyyy")}"));
            document.Add(new Paragraph($"Room Details: {roomDetails}"));
            document.Add(new Paragraph("Thank you for your payment!"));

            document.Close();
            return memoryStream.ToArray();
        }
    }

        public IActionResult Testimonials()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Testimonials([Bind("Id,TestimonialId,UserLoginId,Comments,State")] Testimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                var id = HttpContext.Session.GetInt32("CustomerID");
                var user = await _context.Customers
                                          .Where(x => x.Customerid == id)
                                          .SingleOrDefaultAsync();

                //testimonial.Testimonialid = (int)id; // Casting the nullable int to int
                testimonial.State = "building";
                testimonial.Userloginid2 = (int)id;
                _context.Add(testimonial);
                await _context.SaveChangesAsync();

                // Redirect to a confirmation page or back to the form
                return RedirectToAction("Index");
            }

            // If we got this far, something failed; redisplay form
            return View(testimonial);
        }

        public IActionResult Profile()
        {
            var id = HttpContext.Session.GetInt32("CustomerID");
            var user = _context.Customers.Where(x => x.Customerid == id).SingleOrDefault();

            // Set default profile image
            var defaultProfileImage = "default-profile-image.jpg"; // Adjust the path as necessary
            ViewBag.Name = user?.Customername;
            ViewBag.Image = user?.Profileimage ?? defaultProfileImage; // Use default image if profile image is null
            ViewBag.Email = user?.Email;

            ViewBag.UserId = id;

            return View(user);
        }




        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _context.Customers.SingleOrDefault(x => x.Customerid == id);
            var defaultProfileImage = "default-profile-image.jpg"; // Adjust the path as necessary
            ViewBag.Name = user?.Customername;
            ViewBag.Image = user?.Profileimage ?? defaultProfileImage; // Use default image if profile image is null
            ViewBag.Email = user?.Email;

            ViewBag.UserId = id;
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
            user.Password = editedCustomer.Password;
            user.Profileinfo = editedCustomer.Profileinfo;
            user.ImageFile = editedCustomer.ImageFile; // Assuming the image upload is handled
            ViewBag.Image = user.ImageFile;

            // Update other fields if needed

            login.Email = user.Email;
            login.Password = user.Password;

            // Save the changes to the database
            _context.SaveChanges();

            // Redirect back to the profile page
            return RedirectToAction("Profile", "Home");
        }


        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Optionally, you can clear the session
            HttpContext.Session.Clear();

            // Redirect to the login page or home page
            return RedirectToAction("Login", "RegisterAndLogin");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
