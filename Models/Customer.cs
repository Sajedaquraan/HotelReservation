using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelReservation.Models;

public partial class Customer
{
    public decimal Customerid { get; set; }

    public string Customername { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Profileinfo { get; set; }

    public string? Profileimage { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFile { get; set; }
    public virtual ICollection<Reservationevent> Reservationevents { get; set; } = new List<Reservationevent>();

    public virtual ICollection<Reservationroom> Reservationrooms { get; set; } = new List<Reservationroom>();

    public virtual ICollection<Userlogin> Userlogins { get; set; } = new List<Userlogin>();
}
