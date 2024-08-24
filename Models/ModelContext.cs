using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bank> Banks { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Expenseamount> Expenseamounts { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<Page> Pages { get; set; }

    public virtual DbSet<Paymentevent> Paymentevents { get; set; }

    public virtual DbSet<Paymentroom> Paymentrooms { get; set; }

    public virtual DbSet<Reservationevent> Reservationevents { get; set; }

    public virtual DbSet<Reservationroom> Reservationrooms { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<Userlogin> Userlogins { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521) (CONNECT_DATA=(SID=xe))));User Id=C##Hotel; Password=Test321;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##HOTEL")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasKey(e => e.Bankid).HasName("SYS_C009038");

            entity.ToTable("BANK");

            entity.HasIndex(e => e.Cardnumber, "SYS_C009039").IsUnique();

            entity.Property(e => e.Bankid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("BANKID");
            entity.Property(e => e.Balance)
                .HasColumnType("NUMBER")
                .HasColumnName("BALANCE");
            entity.Property(e => e.Bankname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("BANKNAME");
            entity.Property(e => e.Cardholdername)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CARDHOLDERNAME");
            entity.Property(e => e.Cardnumber)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("CARDNUMBER");
            entity.Property(e => e.Cvv)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("CVV");
            entity.Property(e => e.Expirydate)
                .HasColumnType("DATE")
                .HasColumnName("EXPIRYDATE");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Contactusid).HasName("SYS_C009008");

            entity.ToTable("CONTACT");

            entity.Property(e => e.Contactusid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CONTACTUSID");
            entity.Property(e => e.Contactusdescription)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("CONTACTUSDESCRIPTION");
            entity.Property(e => e.Contactusemail)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("CONTACTUSEMAIL");
            entity.Property(e => e.Contactusname)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("CONTACTUSNAME");
            entity.Property(e => e.Logintime)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP\n")
                .HasColumnName("LOGINTIME");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Customerid).HasName("SYS_C008936");

            entity.ToTable("CUSTOMER");

            entity.Property(e => e.Customerid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CUSTOMERID");
            entity.Property(e => e.Customername)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CUSTOMERNAME");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Profileimage)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("PROFILEIMAGE");
            entity.Property(e => e.Profileinfo)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("PROFILEINFO");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Eventid).HasName("SYS_C008951");

            entity.ToTable("EVENT");

            entity.Property(e => e.Eventid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("EVENTID");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Eventdate)
                .HasColumnType("DATE")
                .HasColumnName("EVENTDATE");
            entity.Property(e => e.Eventname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EVENTNAME");
            entity.Property(e => e.Hotelid)
                .HasColumnType("NUMBER")
                .HasColumnName("HOTELID");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Events)
                .HasForeignKey(d => d.Hotelid)
                .HasConstraintName("FK_EVENT_HOTEL");
        });

        modelBuilder.Entity<Expenseamount>(entity =>
        {
            entity.HasKey(e => e.Expenseid).HasName("SYS_C009055");

            entity.ToTable("EXPENSEAMOUNT");

            entity.Property(e => e.Expenseid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("EXPENSEID");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.Expensedate)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP\n")
                .HasColumnName("EXPENSEDATE");
            entity.Property(e => e.Expensetype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EXPENSETYPE");
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.Hotelid).HasName("SYS_C008941");

            entity.ToTable("HOTEL");

            entity.Property(e => e.Hotelid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("HOTELID");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Hotelname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("HOTELNAME");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("LOCATION");
        });

        modelBuilder.Entity<Page>(entity =>
        {
            entity.HasKey(e => e.Pageid).HasName("SYS_C009006");

            entity.ToTable("PAGES");

            entity.Property(e => e.Pageid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("PAGEID");
            entity.Property(e => e.Pagecontent)
                .HasColumnType("CLOB")
                .HasColumnName("PAGECONTENT");
            entity.Property(e => e.Pageimage)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PAGEIMAGE");
            entity.Property(e => e.Pagename)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PAGENAME");
        });

        modelBuilder.Entity<Paymentevent>(entity =>
        {
            entity.HasKey(e => e.Paymentid).HasName("SYS_C009047");

            entity.ToTable("PAYMENTEVENT");

            entity.Property(e => e.Paymentid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("PAYMENTID");
            entity.Property(e => e.Amountpaid)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNTPAID");
            entity.Property(e => e.Bankid)
                .HasColumnType("NUMBER")
                .HasColumnName("BANKID");
            entity.Property(e => e.Paymentdate)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("PAYMENTDATE");
            entity.Property(e => e.Reservationid)
                .HasColumnType("NUMBER")
                .HasColumnName("RESERVATIONID");

            entity.HasOne(d => d.Bank).WithMany(p => p.Paymentevents)
                .HasForeignKey(d => d.Bankid)
                .HasConstraintName("FK_PAYMENT_BANK");

            entity.HasOne(d => d.Reservation).WithMany(p => p.Paymentevents)
                .HasForeignKey(d => d.Reservationid)
                .HasConstraintName("FK_PAYMENT_RESERVATION2");
        });

        modelBuilder.Entity<Paymentroom>(entity =>
        {
            entity.HasKey(e => e.Paymentid).HasName("SYS_C009042");

            entity.ToTable("PAYMENTROOM");

            entity.Property(e => e.Paymentid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("PAYMENTID");
            entity.Property(e => e.Amountpaid)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNTPAID");
            entity.Property(e => e.Bankid)
                .HasColumnType("NUMBER")
                .HasColumnName("BANKID");
            entity.Property(e => e.Paymentdate)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("PAYMENTDATE");
            entity.Property(e => e.Reservationid)
                .HasColumnType("NUMBER")
                .HasColumnName("RESERVATIONID");

            entity.HasOne(d => d.Bank).WithMany(p => p.Paymentrooms)
                .HasForeignKey(d => d.Bankid)
                .HasConstraintName("FK_PAYMENT_BANK2");

            entity.HasOne(d => d.Reservation).WithMany(p => p.Paymentrooms)
                .HasForeignKey(d => d.Reservationid)
                .HasConstraintName("FK_PAYMENT_RESERVATION");
        });

        modelBuilder.Entity<Reservationevent>(entity =>
        {
            entity.HasKey(e => e.Reservationid).HasName("SYS_C008966");

            entity.ToTable("RESERVATIONEVENT");

            entity.Property(e => e.Reservationid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("RESERVATIONID");
            entity.Property(e => e.Checkindate)
                .HasColumnType("DATE")
                .HasColumnName("CHECKINDATE");
            entity.Property(e => e.Checkoutdate)
                .HasColumnType("DATE")
                .HasColumnName("CHECKOUTDATE");
            entity.Property(e => e.Customerid)
                .HasColumnType("NUMBER")
                .HasColumnName("CUSTOMERID");
            entity.Property(e => e.Eventid)
                .HasColumnType("NUMBER")
                .HasColumnName("EVENTID");
            entity.Property(e => e.Paymentstatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PAYMENTSTATUS");
            entity.Property(e => e.Reservationdate)
                .HasColumnType("DATE")
                .HasColumnName("RESERVATIONDATE");

            entity.HasOne(d => d.Customer).WithMany(p => p.Reservationevents)
                .HasForeignKey(d => d.Customerid)
                .HasConstraintName("FK_RESERVATION_CUSTOMER2");

            entity.HasOne(d => d.Event).WithMany(p => p.Reservationevents)
                .HasForeignKey(d => d.Eventid)
                .HasConstraintName("FK_RESERVATION_EVENT2");
        });

        modelBuilder.Entity<Reservationroom>(entity =>
        {
            entity.HasKey(e => e.Reservationid).HasName("SYS_C008956");

            entity.ToTable("RESERVATIONROOM");

            entity.Property(e => e.Reservationid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("RESERVATIONID");
            entity.Property(e => e.Checkindate)
                .HasColumnType("DATE")
                .HasColumnName("CHECKINDATE");
            entity.Property(e => e.Checkoutdate)
                .HasColumnType("DATE")
                .HasColumnName("CHECKOUTDATE");
            entity.Property(e => e.Customerid)
                .HasColumnType("NUMBER")
                .HasColumnName("CUSTOMERID");
            entity.Property(e => e.Paymentstatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PAYMENTSTATUS");
            entity.Property(e => e.Reservationdate)
                .HasColumnType("DATE")
                .HasColumnName("RESERVATIONDATE");
            entity.Property(e => e.Roomid)
                .HasColumnType("NUMBER")
                .HasColumnName("ROOMID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Reservationrooms)
                .HasForeignKey(d => d.Customerid)
                .HasConstraintName("FK_RESERVATION_CUSTOMER");

            entity.HasOne(d => d.Room).WithMany(p => p.Reservationrooms)
                .HasForeignKey(d => d.Roomid)
                .HasConstraintName("FK_RESERVATION_ROOM");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("SYS_C008930");

            entity.ToTable("ROLE");

            entity.Property(e => e.Roleid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Rolename)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ROLENAME");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Roomid).HasName("SYS_C008946");

            entity.ToTable("ROOM");

            entity.Property(e => e.Roomid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ROOMID");
            entity.Property(e => e.Availabilitystatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("AVAILABILITYSTATUS");
            entity.Property(e => e.Datefrom)
                .HasColumnType("DATE")
                .HasColumnName("DATEFROM");
            entity.Property(e => e.Dateto)
                .HasColumnType("DATE")
                .HasColumnName("DATETO");
            entity.Property(e => e.Hotelid)
                .HasColumnType("NUMBER")
                .HasColumnName("HOTELID");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Price)
                .HasColumnType("NUMBER")
                .HasColumnName("PRICE");
            entity.Property(e => e.Roomtype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ROOMTYPE");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.Hotelid)
                .HasConstraintName("FK_ROOM_HOTEL");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.Testimonialid).HasName("SYS_C009030");

            entity.ToTable("TESTIMONIAL");

            entity.Property(e => e.Testimonialid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("TESTIMONIALID");
            entity.Property(e => e.Comments)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("COMMENTS");
            entity.Property(e => e.State)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("STATE");
            entity.Property(e => e.Userloginid2)
                .HasColumnType("NUMBER")
                .HasColumnName("USERLOGINID2");

            entity.HasOne(d => d.Userloginid2Navigation).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.Userloginid2)
                .HasConstraintName("FK_TESTIMONIAL_CUSTOMER2");
        });

        modelBuilder.Entity<Userlogin>(entity =>
        {
            entity.HasKey(e => e.Userloginid).HasName("SYS_C008998");

            entity.ToTable("USERLOGIN");

            entity.Property(e => e.Userloginid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("USERLOGINID");
            entity.Property(e => e.Customerid)
                .HasColumnType("NUMBER")
                .HasColumnName("CUSTOMERID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Roleid)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Userlogins)
                .HasForeignKey(d => d.Customerid)
                .HasConstraintName("FK_TESTIMONIAL_USERLOGIN");

            entity.HasOne(d => d.Role).WithMany(p => p.Userlogins)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("FK_ROLE_USERLOGIN");
        });
        modelBuilder.HasSequence("ISEQ$$_82189");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
