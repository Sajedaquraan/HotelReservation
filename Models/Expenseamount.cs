using System;
using System.Collections.Generic;

namespace HotelReservation.Models;

public partial class Expenseamount
{
    public decimal Expenseid { get; set; }

    public string Expensetype { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime? Expensedate { get; set; }
}
