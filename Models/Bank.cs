using System;
using System.Collections.Generic;

namespace HotelReservation.Models;

public partial class Bank
{
    public decimal Bankid { get; set; }

    public string? Bankname { get; set; }

    public string Cardnumber { get; set; } = null!;

    public DateTime Expirydate { get; set; }

    public string Cardholdername { get; set; } = null!;

    public string Cvv { get; set; } = null!;

    public decimal? Balance { get; set; }

    public virtual ICollection<Paymentevent> Paymentevents { get; set; } = new List<Paymentevent>();

    public virtual ICollection<Paymentroom> Paymentrooms { get; set; } = new List<Paymentroom>();
}
