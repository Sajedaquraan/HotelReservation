using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelReservation.Models;

public partial class Page
{
    public decimal Pageid { get; set; }

    public string Pagename { get; set; } = null!;

    public string? Pageimage { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFile { get; set; }
    public string? Pagecontent { get; set; }
}
