using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelReservation.Models;

public partial class Hotel
{
    public decimal Hotelid { get; set; }

    public string Hotelname { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string? Description { get; set; }

    public string? Imagepath { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFile { get; set; }
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
