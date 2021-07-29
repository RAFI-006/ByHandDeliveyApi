using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ByHandDeliveryApi.Models;

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
        public virtual DbSet<TblDdvalues> TblDdvalues { get; set; }
        public virtual DbSet<TblDeliveryCity> TblDeliveryCity { get; set; }
        public virtual DbSet<TblDeliveryPerson> TblDeliveryPerson { get; set; }
        public virtual DbSet<TblDeliveryPersonDetails> TblDeliveryPersonDetails  { get; set; }
        public virtual DbSet<TblDeliveryPersonCancelOrderDetails> TblDeliveryPersonCancelOrderDetails { get; set; }

        public virtual DbSet<TblDeliveryPersonWallet> TblDeliveryPersonWallet { get; set; }
        public virtual DbSet<TblDropDown> TblDropDown { get; set; }
        public virtual DbSet<TblOrderDeliveryAddress> TblOrderDeliveryAddress { get; set; }
        public virtual DbSet<TblOrders> TblOrders { get; set; }
        public virtual DbSet<TblUsers> TblUsers { get; set; }
        public virtual DbSet<TblDeliveryPersonPaymentTransactionDetails> TblDeliveryPersonPaymentTransactionDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Server=tcp:Sqlplesk7.securehostdns.com,1234;Initial Catalog=db_byhanddelivery;Persist Security Info=False;User ID=hand;Password=Delivery1234!@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;",builder => {

                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);

                });
                

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:DefaultSchema", "hand");

            modelBuilder.Entity<TblCustomers>(entity =>
            {
                entity.HasKey(e => e.CustomerID);

                entity.ToTable("tbl_Customers");

                entity.Property(e => e.CustomerID).HasColumnName("CustomerID");

                entity.Property(e => e.Address)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

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

                entity.Property(e => e.EmailID)
                    .HasColumnName("EmailID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FcmToken).IsUnicode(false);

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

            modelBuilder.Entity<TblDdvalues>(entity =>
            {
                entity.HasKey(e => e.DdvalueID);

                entity.ToTable("tbl_DDValues");

                entity.Property(e => e.DdvalueID).HasColumnName("DDValueID");

                entity.Property(e => e.Ddkey)
                    .HasColumnName("DDKey")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ddvalue)
                    .HasColumnName("DDValue")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DropDownID).HasColumnName("DropDownID");

                entity.HasOne(d => d.DropDown)
                    .WithMany(p => p.TblDdvalues)
                    .HasForeignKey(d => d.DropDownID)
                    .HasConstraintName("FK_tbl_DDValues_tbl_DropDown");
            });



            modelBuilder.Entity<TblDeliveryPersonWallet>(entity =>
            {
                entity.HasKey(e => e.DeliveryPersonWalletID );

                entity.ToTable("tbl_DeliveryPersonWallet");

                entity.Property(e => e.DeliveryPersonWalletID).HasColumnName("DeliveryPersonWalletID");


                entity.Property(e => e.DeliveryPersonID).HasColumnName("DeliveryPersonID");

                entity.Property(e => e.Wallet).HasColumnName("Wallet");

                entity.HasOne(d => d.DeliveryPerson )
                    .WithMany(p => p.TblDeliveryPersonWallet )
                    .HasForeignKey(d => d.DeliveryPersonID)
                    .HasConstraintName("FK_tbl_DeliveryPersonWallet_tbl_DeliveryPerson");
            });



            modelBuilder.Entity<TblDeliveryCity>(entity =>
            {
                entity.HasKey(e => e.DeliveryCityID);

                entity.ToTable("tbl_DeliveryCity");
                entity.Property(e => e.Latitude).HasColumnType("decimal(20, 6)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(20, 6)");
                entity.Property(e => e.DeliveryCityID).HasColumnName("DeliveryCityID");

                entity.Property(e => e.SortOrderNo);

                entity.Property(e => e.DeliveryCity)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblDeliveryPerson>(entity =>
            {
                entity.HasKey(e => e.DeliveryPersonID);

                entity.ToTable("tbl_DeliveryPerson");

                entity.Property(e => e.DeliveryPersonID).HasColumnName("DeliveryPersonID");

                entity.Property(e => e.PersonName)
                  .HasMaxLength(100)
                  .IsUnicode(false);

                entity.Property(e => e.MobileNo)
                   .HasMaxLength(50)
                   .IsUnicode(false);

                entity.Property(e => e.AlternateNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Address)
                   .HasMaxLength(1000)
                   .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Pincode)
                  .HasMaxLength(10)
                  .IsUnicode(false);

                entity.Property(e => e.EmailID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                   .HasMaxLength(100)
                   .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsActive).HasColumnType("Boolean");

                entity.Property(e => e.IsVerified).HasColumnType("Boolean");

                entity.Property(e => e.Fcmtoken)
                    .HasColumnName("FCMToken")
                    .IsUnicode(false);

                entity.Property(e => e.MyPromocode)
                   .HasMaxLength(20)
                   .IsUnicode(false);

                entity.Property(e => e.ReferPromocode)
                   .HasMaxLength(20)
                   .IsUnicode(false);


                entity.Property(e => e.ProfileImage)
                    .HasMaxLength(500)
                    .IsUnicode(false);

              
            });

            modelBuilder.Entity <TblDeliveryPersonDetails>(entity =>
            {
                entity.HasKey(e => e.DeliveryPersonDetailID);

                entity.ToTable("tbl_DeliveryPersonDetails");

                entity.Property(e => e.DeliveryPersonID).HasColumnName("DeliveryPersonID");

                entity.Property(e => e.AadhaarNo)
                 .HasMaxLength(50)
                 .IsUnicode(false);

                entity.Property(e => e.AadhaarBackImage)
                 .HasMaxLength(500)
                 .IsUnicode(false);

                entity.Property(e => e.AadhaarFrontImage)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Pan)
                  .HasColumnName("PAN")
                  .HasMaxLength(50)
                  .IsUnicode(false);

                entity.Property(e => e.Panimage)
                    .HasColumnName("PANImage")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DrivingLicenceNo)
               .HasMaxLength(50)
               .IsUnicode(false);

                entity.Property(e => e.DrivingLicenceFrontImage)
                .HasMaxLength(500)
                .IsUnicode(false);

                entity.Property(e => e.DrivingLicenceBackImage)
                .HasMaxLength(500)
                .IsUnicode(false);

                entity.Property(e => e.VehicleNo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.VehicleDocumentImage)
               .HasMaxLength(500)
               .IsUnicode(false);

                entity.Property(e => e.VehicleFrontPhoto)
              .HasMaxLength(500)
              .IsUnicode(false);

                entity.Property(e => e.VehicleBackPhoto)
                .HasMaxLength(500)
                .IsUnicode(false);

                entity.Property(e => e.VehicleInsuranceNo)
                   .HasMaxLength(500)
                   .IsUnicode(false);

                entity.Property(e => e.VehicleInsuranceDocumentImage)
                .HasMaxLength(500)
                .IsUnicode(false);

                entity.Property(e => e.AccountName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);


            entity.Property(e => e.BankName)
                   .HasMaxLength(50)
                   .IsUnicode(false);

            entity.Property(e => e.Ifsc)
            .HasColumnName("IFSC")
            .HasMaxLength(50)
            .IsUnicode(false);

                entity.Property(e => e.CanceledChequeImage)
                .HasMaxLength(500)
                .IsUnicode(false);



                entity.HasOne(d => d.DeliveryPerson)
                   .WithMany(p => p.tblDeliveryPersonDetails)
                   .HasForeignKey(d => d.DeliveryPersonID)
                   .HasConstraintName("FK_tbl_DeliveryPersonDetails_tbl_DeliveryPerson");

            });


            modelBuilder.Entity<TblDeliveryPersonPaymentTransactionDetails>(entity =>
            {
                entity.HasKey(e => e.DeliveryPersonAccountDetailID);

                entity.ToTable("tbl_DeliveryPersonPaymentTransactionDetails");

                entity.Property(e => e.DeliveryPersonAccountDetailID).HasColumnName("DeliveryPersonAccountDetailID");

                entity.Property(e => e.CrDr)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryPersonID).HasColumnName("DeliveryPersonID");

                entity.Property(e => e.PaymentType)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Comment)
                .IsUnicode(false);


                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.HasOne(d => d.DeliveryPerson)
                    .WithMany(p => p.TblDeliveryPersonPaymentTransactionDetails)
                    .HasForeignKey(d => d.DeliveryPersonID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_DeliveryPersonAccountDetails_tbl_DeliveryPerson");
            });


            modelBuilder.Entity<TblDeliveryPersonCancelOrderDetails>(entity =>
            {
                entity.HasKey(e => e.DeliveryPersonCancelOrderDetailID);
                entity.ToTable("tbl_DeliveryPersonCancelOrderDetails");


                entity.Property(e => e.DeliveryPersonCancelOrderDetailID).HasColumnName("DeliveryPersonCancelOrderDetailID");
                entity.Property(e => e.DeliveryPersonID).HasColumnName("DeliveryPersonID");
                entity.Property(e => e.OrderID).HasColumnName("OrderID");
                entity.Property(e => e.CancellationFee).HasColumnName("CancellationFee");
                entity.Property(e => e.CancellationDate).HasColumnName("CancellationDate").HasColumnType("datetime");

                entity.HasOne(d => d.DeliveryPerson)
                  .WithMany(p => p.TblDeliveryPersonCancelOrderDetails)
                  .HasForeignKey(d => d.DeliveryPersonID)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_tbL_DeliveryPersonCancelOrderDetails_tbl_DeliveryPerson");

                entity.HasOne(d => d.Orders)
               .WithMany(p => p.TblDeliveryPersonCancelOrderDetails)
               .HasForeignKey(d => d.OrderID)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_tbL_DeliveryPersonCancelOrderDetails_tbl_Orders");




            }


                );

            modelBuilder.Entity<TblDropDown>(entity =>
            {
                entity.HasKey(e => e.DropDownID);

                entity.ToTable("tbl_DropDown");

                entity.Property(e => e.DropDownID).HasColumnName("DropDownID");

                entity.Property(e => e.Ddname)
                    .HasColumnName("DDName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DropDownKey)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblOrderDeliveryAddress>(entity =>
            {
                entity.HasKey(e => e.OrderDeliveryAddressID);

                entity.ToTable("tbl_OrderDeliveryAddress");

                entity.Property(e => e.OrderDeliveryAddressID).HasColumnName("OrderDeliveryAddressID");

                entity.Property(e => e.Action)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContactPerson)
                    .HasMaxLength(100)
                    .IsUnicode(false);

              
                entity.Property(e => e.DeliveryAddress)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryFromTime).HasColumnType("datetime");

                entity.Property(e => e.DeliveryToTime).HasColumnType("datetime");

                entity.Property(e => e.DropLocality).IsUnicode(false);

                entity.Property(e => e.InternalOrderNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Latitude).HasColumnType("decimal(20, 4)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(20, 4)");

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OrderID).HasColumnName("OrderID");

                entity.Property(e => e.ProductImage)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.TblOrderDeliveryAddress)
                    .HasForeignKey(d => d.OrderID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_OrderDeliveryAddress_tbl_Orders");
            });

            modelBuilder.Entity<TblOrders>(entity =>
            {
                entity.HasKey(e => e.OrderID);

                entity.ToTable("tbl_Orders");

                entity.Property(e => e.OrderID).HasColumnName("OrderID");

                entity.Property(e => e.Action)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ContactPerson)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ContactPersonMobile)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerID).HasColumnName("CustomerID");

                entity.Property(e => e.DeliveryPersonID).HasColumnName("DeliveryPersonID");

                entity.Property(e => e.FromLat).HasColumnType("decimal(20, 6)");

                entity.Property(e => e.FromLong).HasColumnType("decimal(20, 6)");
                entity.Property(e => e.CommissionFee).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.GoodsType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.InternalOrderNo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OrderStatusID).HasColumnName("OrderStatusID");

                entity.Property(e => e.PaymentFrom).IsUnicode(false);

                entity.Property(e => e.PaymentTypeID).HasColumnName("PaymentTypeID");
                entity.Property(e => e.PaymentStatusID).HasColumnName("PaymentStatusID");

                entity.Property(e => e.PickupAddress)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.PickupFromTime).HasColumnType("datetime");

                entity.Property(e => e.PickupLocality)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.PickupToTime).HasColumnType("datetime");

                entity.Property(e => e.ProductImage).IsUnicode(false);

                entity.Property(e => e.PromoCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Weight)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.CustomerID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_Orders_tbl_Customers");

                entity.HasOne(d => d.DeliveryPerson)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.DeliveryPersonID)
                    .HasConstraintName("FK_tbl_Orders_tbl_DeliveryPerson");
            });

            modelBuilder.Entity<TblUsers>(entity =>
            {
                entity.HasKey(e => e.UserID);

                entity.ToTable("tbl_Users");

                entity.Property(e => e.UserID).HasColumnName("UserID");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserFullName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }

       }
}
