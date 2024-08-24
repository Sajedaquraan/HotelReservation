using HotelReservation.Models;

namespace HotelReservation.Models
{
    public class JoinTables
    {
        public Hotel Hotel { get; set; }
        public Room Room { get; set; }
        public Reservationroom ReservationRoom { get; set; }
        public Paymentroom PaymentRoom { get; set; }
    }
}

                           