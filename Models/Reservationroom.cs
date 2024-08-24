using System;
using System.Collections.Generic;

namespace HotelReservation.Models;

public partial class Reservationroom
{
    public decimal Reservationid { get; set; }

    public decimal? Customerid { get; set; }

    public decimal? Roomid { get; set; }

    public DateTime Reservationdate { get; set; }

    public DateTime? Checkindate { get; set; }

    public DateTime? Checkoutdate { get; set; }

    public string Paymentstatus { get; set; } = null!;

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<Paymentroom> Paymentrooms { get; set; } = new List<Paymentroom>();

    public virtual Room? Room { get; set; }
}
