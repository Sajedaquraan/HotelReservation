using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelReservation.Models;

public partial class Event
{
    public decimal Eventid { get; set; }

    public decimal? Hotelid { get; set; }

    public string Eventname { get; set; } = null!;

    public DateTime Eventdate { get; set; }

    public string? Imagepath { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFile { get; set; }
    public string? Description { get; set; }

    public decimal Price { get; set; }
    public virtual Hotel? Hotel { get; set; }

    public virtual ICollection<Reservationevent> Reservationevents { get; set; } = new List<Reservationevent>();
}
