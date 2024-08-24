using HotelReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Mail;
namespace HotelReservation.Controllers
{
    public class RegisterAndLoginController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _environment;


        public RegisterAndLoginController(ModelContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("CustomernameID,Customername,Email,Profileinfo,ImageFile,Password")] Customer customer, string Email, string Password)
        {
            if (ModelState.IsValid)
            {
                if (customer.ImageFile != null)
                {
                    string wwwRootPath = _environment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + customer.ImageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await customer.ImageFile.CopyToAsync(fileStream);
                    }

                    customer.Profileimage = fileName;
                }

                var verificationCode = Guid.NewGuid().ToString().Substring(0, 6);
                HttpContext.Session.SetString("VerificationCode", verificationCode);
                HttpContext.Session.SetString("Email", Email);

                _context.Add(customer);
                await _context.SaveChangesAsync();

                Userlogin login = new Userlogin
                {
                    Email = Email,
                    Password = Password,
                    Roleid = 2,
                    Customerid = customer.Customerid
                };
                _context.Add(login);
                await _context.SaveChangesAsync();

                SendVerificationEmail(customer.Email, verificationCode);

                return RedirectToAction("VerifyEmail", new { email = customer.Email });
            }
            return View(customer);
        }



        private void SendVerificationEmail(string email, string verificationCode)
        {
            var fromAddress = new MailAddress("sajedaalquraan1@gmail.com", "Hotel");
            var toAddress = new MailAddress(email);
            const string fromPassword = "izdw sras niqv jnnh";
            const string subject = "Email Verification";
            string body = $"Your verification code is {verificationCode}";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }



        [HttpGet]
        public IActionResult VerifyEmail(string email)
        {
            var model = new VerifyEmailViewModel { Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyEmail(VerifyEmailViewModel model)
        {
            var storedCode = HttpContext.Session.GetString("VerificationCode");

            if (storedCode == null || storedCode != model.VerificationCode)
            {
                ModelState.AddModelError(string.Empty, "Invalid verification code.");
                return View(model);
            }

            HttpContext.Session.Remove("VerificationCode");
            HttpContext.Session.Remove("Email");

            TempData["SuccessMessage"] = "Successfully registered! You can now log in to your profile.";

            return RedirectToAction("Login");
        }





        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login([Bind("Email,Password")] Userlogin userlogin)
        {
            var auth = _context.Userlogins.Where
                (x => x.Email == userlogin.Email && x.Password == userlogin.Password).SingleOrDefault();
            if (auth != null)
            {
                switch (auth.Roleid)
                {
                    case 1:
                        HttpContext.Session.SetInt32("AdminID", (int)auth.Customerid);
                        HttpContext.Session.SetString("AdminName", auth.Email);
                        return RedirectToAction("Index", "Admin");

                    case 2:
                        HttpContext.Session.SetInt32("CustomerID", (int)auth.Customerid);
                        return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            var user = _context.Userlogins.SingleOrDefault(x => x.Email == Email);
            if (user != null)
            {
                // Generate a random code
                var resetCode = new Random().Next(100000, 999999).ToString();

                // Store the code in session
                HttpContext.Session.SetString("ResetCode", resetCode);
                HttpContext.Session.SetString("ResetEmail", Email);

                // Send the code via email
                await SendResetCodeEmail(Email, resetCode);

                return RedirectToAction("VerifyCode");
            }
            ModelState.AddModelError("", "Email not found.");
            return View();
        }

        // Method to send email (implement your email sending logic here)
        private async Task SendResetCodeEmail(string email, string resetCode)
        {
            // Example using SmtpClient (replace with your email provider's details)
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("sajedaalquraan1@gmail.com", "izdw sras niqv jnnh"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("Hotel@gmail.com"),
                Subject = "Password Reset Code",
                Body = $"Your password reset code is: {resetCode}",
                IsBodyHtml = false,
            };

            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }

        [HttpGet]
        public IActionResult VerifyCode()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyCode(string ResetCode)
        {
            var sessionCode = HttpContext.Session.GetString("ResetCode");

            if (sessionCode == ResetCode)
            {
                return RedirectToAction("ResetPassword");
            }

            ModelState.AddModelError("", "Invalid code. Please try again.");
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(string NewPassword, string ConfirmPassword)
        {
            if (NewPassword == ConfirmPassword)
            {
                var email = HttpContext.Session.GetString("ResetEmail");

                if (!string.IsNullOrEmpty(email))
                {
                    var user = _context.Userlogins.SingleOrDefault(x => x.Email == email);
                    var customer=_context.Customers.SingleOrDefault(x=>x.Email == email);
                    if (user != null)
                    {
                        // Update the password
                        user.Password = NewPassword;
                        customer.Password= NewPassword;
                        _context.SaveChanges();

                        // Clear session
                        HttpContext.Session.Remove("ResetCode");
                        HttpContext.Session.Remove("ResetEmail");

                        return RedirectToAction("Login");
                    }
                }
            }

            ModelState.AddModelError("", "Passwords do not match or there was an issue.");
            return View();
        }



    }
}