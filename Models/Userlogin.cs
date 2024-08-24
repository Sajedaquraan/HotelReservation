using System;
using System.Collections.Generic;

namespace HotelReservation.Models;

public partial class Userlogin
{
    public decimal Userloginid { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public decimal? Customerid { get; set; }

    public decimal? Roleid { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Role? Role { get; set; }

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
}
