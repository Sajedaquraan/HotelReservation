using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelReservation.Models;

public partial class Room
{
    public decimal Roomid { get; set; }

    public decimal? Hotelid { get; set; }

    public string Roomtype { get; set; } = null!;

    public decimal Price { get; set; }

    public string Availabilitystatus { get; set; } = null!;

    public string? Imagepath { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFile { get; set; }
    public DateTime? Datefrom { get; set; }

    public DateTime? Dateto { get; set; }

    public virtual Hotel? Hotel { get; set; }

    public virtual ICollection<Reservationroom> Reservationrooms { get; set; } = new List<Reservationroom>();
}
