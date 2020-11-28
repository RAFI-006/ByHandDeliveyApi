using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ByHandDeliveryApi.Models
{
    public partial class db_byhanddeliveryContext : DbContext
    {
        public db_byhanddeliveryContext()
        {
        }

        public db_byhanddeliveryContext(DbContextOptions<db_byhanddeliveryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblCustomers> TblCustomers { get; set; }
        public virtual DbSet<TblDeliveryCity> TblDeliveryCity { get; set; }
        public virtual DbSet<TblDeliveryPerson> TblDeliveryPerson { get; set; }
        public virtual DbSet<TblOrderDeliveryAddress> TblOrderDeliveryAddress { get; set; }
        public virtual DbSet<TblOrders> TblOrders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:Bindiadb.securehostdns.com,1533;Initial Catalog=db_byhanddelivery;Persist Security Info=False;User ID=hand;Password=Delivery1234!@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:DefaultSchema", "hand");

            modelBuilder.Entity<TblCustomers>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.ToTable("tbl_Customers");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Address)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreditCardCvv)
                    .HasColumnName("CreditCardCVV")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CreditCardExpiry).HasColumnType("date");

                entity.Property(e => e.CreditCardHolderName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreditCardNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmailId)
                    .HasColumnName("EmailID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Pincode)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblDeliveryCity>(entity =>
            {
                entity.HasKey(e => e.DeliveryCityId);

                entity.ToTable("tbl_DeliveryCity");

                entity.Property(e => e.DeliveryCityId).HasColumnName("DeliveryCityID");

                entity.Property(e => e.DeliveryCity)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblDeliveryPerson>(entity =>
            {
                entity.HasKey(e => e.DeliveryPersonId);

                entity.ToTable("tbl_DeliveryPerson");

                entity.Property(e => e.DeliveryPersonId).HasColumnName("DeliveryPersonID");

                entity.Property(e => e.AadhaarImage)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AadhaarNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AccountName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AccountNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Address)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.AlternateNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BankName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CanceledChequeImage)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DrivingLicenceImage)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DrivingLicenceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ifsc)
                    .HasColumnName("IFSC")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Pan)
                    .HasColumnName("PAN")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Panimage)
                    .HasColumnName("PANImage")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PersonName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Pincode)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblOrderDeliveryAddress>(entity =>
            {
                entity.HasKey(e => e.OrderDeliveryAddressId);

                entity.ToTable("tbl_OrderDeliveryAddress");

                entity.Property(e => e.OrderDeliveryAddressId).HasColumnName("OrderDeliveryAddressID");

                entity.Property(e => e.Action)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContactPerson)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryAddress)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryDate).HasColumnType("date");

                entity.Property(e => e.DeliveryFromTime)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryToTime)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InternalOrderNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.TblOrderDeliveryAddress)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_OrderDeliveryAddress_tbl_Orders");
            });

            modelBuilder.Entity<TblOrders>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.ToTable("tbl_Orders");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");
                entity.Property(e => e.Status);
                entity.Property(e => e.Action)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Cod).HasColumnName("COD");

                entity.Property(e => e.ContactPerson)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ContactPersonMobile)
                   .HasMaxLength(50)
                   .IsUnicode(false);

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.DeliveryPersonId).HasColumnName("DeliveryPersonID");

                entity.Property(e => e.GoodsType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.InternalOrderNo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PickupAddress)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.PickupDate).HasColumnType("date");

                entity.Property(e => e.PickupFromTime)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PickupLocality)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.PickupToTime)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Weight)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_Orders_tbl_Customers");

                entity.HasOne(d => d.DeliveryPerson)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.DeliveryPersonId)
                    .HasConstraintName("FK_tbl_Orders_tbl_DeliveryPerson");
            });
        }
    }
}
