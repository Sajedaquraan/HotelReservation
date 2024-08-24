using System;
using System.Collections.Generic;

namespace HotelReservation.Models;

public partial class Paymentroom
{
    public decimal Paymentid { get; set; }

    public decimal? Reservationid { get; set; }

    public decimal Amountpaid { get; set; }

    public DateTime? Paymentdate { get; set; }

    public decimal? Bankid { get; set; }

    public virtual Bank? Bank { get; set; }

    public virtual Reservationroom? Reservation { get; set; }
}
