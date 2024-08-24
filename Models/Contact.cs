using System;
using System.Collections.Generic;

namespace HotelReservation.Models;

public partial class Contact
{
    public decimal Contactusid { get; set; }

    public string? Contactusname { get; set; }

    public string? Contactusemail { get; set; }

    public string? Contactusdescription { get; set; }

    public DateTime? Logintime { get; set; }
}
