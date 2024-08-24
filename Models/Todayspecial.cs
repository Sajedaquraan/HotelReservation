using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelReservation.Models;

public partial class Todayspecial
{
    public decimal Todayspecialid { get; set; }

    public string? Todayspecialname { get; set; }

    public string? Todayspecialimage { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFile { get; set; }
    public decimal? Todayspecialprice { get; set; }

    public string? Todayspecialcontant { get; set; }
}
