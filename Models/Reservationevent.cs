using System;
using System.Collections.Generic;

namespace HotelReservation.Models;

public partial class Reservationevent
{
    public decimal Reservationid { get; set; }

    public decimal? Customerid { get; set; }

    public decimal? Eventid { get; set; }

    public DateTime Reservationdate { get; set; }

    public DateTime? Checkindate { get; set; }

    public DateTime? Checkoutdate { get; set; }

    public string Paymentstatus { get; set; } = null!;

    public virtual Customer? Customer { get; set; }

    public virtual Event? Event { get; set; }

    public virtual ICollection<Paymentevent> Paymentevents { get; set; } = new List<Paymentevent>();
}
