using System;
using System.Collections.Generic;

namespace HotelReservation.Models;

public partial class Testimonial
{
    public decimal Testimonialid { get; set; }

    public decimal? Userloginid2 { get; set; }

    public string? Comments { get; set; }

    public string? State { get; set; }

    public virtual Userlogin? Userloginid2Navigation { get; set; }
}
