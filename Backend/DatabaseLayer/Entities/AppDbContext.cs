using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class AppDbContext : DbContext
    {

        public AppDbContext() { }
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } 



        public DbSet<Admin>Admins { get; set; }
        public DbSet<AdminLog> AdminLogs { get; set; }
        public DbSet<Country> Countrys { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerLog> CustomersLogs { get; set; }
        public DbSet<DeliveryFees>DeliveryFees { get; set; }
        public DbSet<Governorate>Governorates  { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<MedicalType> MedicalTypes { get; set; }
        public DbSet<Meeting>Meetings { get; set; }
        public DbSet<MeetingPayment>MeetingPayments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderPayment> OrderPayments { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<PaymentMethode> PaymentMethodes { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Pharmacy>Pharmacies { get; set; }
        public DbSet<PharmacyCurrency> PharmacyCurrencies { get; set; }
        public DbSet<PhPramacyProduct> PhPramacyProducts { get; set; }
        public DbSet<RefundRequest> RefundRequests { get; set; }
        public DbSet<RefundStatus> RefundStatuses { get; set; }
        public DbSet<RefundType> RefundTypes { get; set; }
        public DbSet<SysProduct> SysProducts { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<PharmacyDeliveryLocation> PharmacyDeliveryLocations { get; set; }
        public DbSet<Manager>Managers { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }
        public DbSet<Cart>Carts { get; set; }
        public DbSet<CartItem>CartItems { get; set; }
        public DbSet<RequestMeeting> RequestMeetings { get; set; }
        public DbSet<CustomerPharmacyDistance> customerPharmacyDistances { get; set; }
        public DbSet<SystemAdmin> SystemAdmins { get; set; }
        public DbSet<MedicalCategory> MedicalCategories { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }
        public DbSet<TempMeetingRequest>TempMeetingRequests { get; set; }
        public DbSet<TempOrderRequest> TempOrderRequests  { get; set; }
        public DbSet<Region> Regions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(clsConnection.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Pharmacy>()
                .HasOne(p => p.country)                       
                .WithMany(c => c.Pharmacies)                  
                .HasForeignKey(p => p.CountryId)             
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Pharmacy>()
               .HasOne(p => p.Governorate)                 
               .WithMany(g => g.Pharmacies)                  
               .HasForeignKey(p => p.GovernorateId)          
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Token>()
            .Property(t => t.ExpiredDate)
            .HasColumnType("datetime2(0)");


            modelBuilder.Entity<UserType>().HasData(

                new UserType { TypeId = 1, UserTypeName = "Admin" },
                new UserType { TypeId = 2, UserTypeName = "Customer" },
                new UserType { TypeId = 3, UserTypeName = "Manager" },
                new UserType { TypeId = 4, UserTypeName = "SysAmdin" },
                new UserType { TypeId=5,   UserTypeName = "Pre_Registered" }
            );

            modelBuilder.Entity<Token>().Property(p => p.GeneratedDateTime).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Token>().Property(p => p.IsActive).HasDefaultValue(true);

            modelBuilder.Entity<RequestMeeting>().Property(p => p.RequestDateTime).HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<RequestStatus>().HasData(

                new RequestStatus { ReqStatusId = 1, ReqStatusName = "Successfull sending",ReqStatusArabic="تم الأرسال بنجاح" },
                new RequestStatus { ReqStatusId = 2, ReqStatusName = "Pending",ReqStatusArabic="انتظار" },
                new RequestStatus { ReqStatusId = 3, ReqStatusName = "Accepted",ReqStatusArabic="مقبولة" },
                new RequestStatus { ReqStatusId = 4, ReqStatusName = "Not Accepted",ReqStatusArabic="مرفوضة" },
                new RequestStatus { ReqStatusId = 5, ReqStatusName = "Cancled",ReqStatusArabic="ملغاة" },
                new RequestStatus { ReqStatusId = 6, ReqStatusName = "Finished",ReqStatusArabic="منتهية" }
            );

            modelBuilder.Entity<RefundType>().HasData(

                new RefundType { TypeId = 1, Name = "Refund Order" },
                new RefundType { TypeId = 2, Name = "Refund Request" }
            );

            modelBuilder.Entity<RefundStatus>().HasData(

                new RefundStatus { Id = 1, Name = "Successfull sending",NameArabic= "تم الأرسال بنجاح" },
                new RefundStatus { Id = 2, Name = "Pending",NameArabic= "انتظار" },
                new RefundStatus { Id = 3, Name = "Accepted",NameArabic= "مقبولة" },
                new RefundStatus { Id = 4, Name = "Rejected",NameArabic= "مرفوضة" }
            );

            modelBuilder.Entity<RefundRequest>().Property(p => p.DateAndTimeOfRequest)
                                                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Pharmacy>().Property(p => p.DateOfRegister)
                                           .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<PaymentMethode>().HasData(

                new PaymentMethode { MethodeId = 1, MethodeName = "Paypal" },
                new PaymentMethode { MethodeId = 2, MethodeName = "MasterCard" },
                new PaymentMethode { MethodeId = 3, MethodeName = "Visa" },
                new PaymentMethode { MethodeId = 4, MethodeName = "On Delivery" },
                new PaymentMethode { MethodeId = 5, MethodeName = "Click" }
            );

            modelBuilder.Entity<OrderStatus>().HasData(

                new OrderStatus { StatusId = 1, StatusName = "Successfull sending",StatusNameArabic= "تم الأرسال بنجاح" },
                new OrderStatus { StatusId = 2, StatusName = "Pending" ,StatusNameArabic= "انتظار" },
                new OrderStatus { StatusId = 3, StatusName = "Preparing", StatusNameArabic="يتم التحضير" },
                new OrderStatus { StatusId = 4, StatusName = "Delivering", StatusNameArabic="توصيل" },
                new OrderStatus { StatusId = 5, StatusName = "Canceled" , StatusNameArabic="ملغاة"},
                new OrderStatus { StatusId = 6, StatusName = "Not Accepted" ,StatusNameArabic="مرفوض"},
                new OrderStatus { StatusId = 7, StatusName = "Finished", StatusNameArabic="منتهي" }
            );

            modelBuilder.Entity<OrderPayment>().Property(p => p.DateTimeOfPaid)
                                               .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Order>().Property(p => p.OrderDateAndTime)
                                        .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Message>().Property(p => p.DateOfMessage)
                                           .HasDefaultValueSql("CONVERT(DATE, GETDATE())");

            modelBuilder.Entity<Message>().Property(p => p.Time)
                                          .HasDefaultValueSql("CONVERT(TIME, GETDATE())");

            modelBuilder.Entity<MeetingPayment>().Property(p => p.DateTimeOfPaid)
                                                 .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<MedicalType>().HasData(


                new MedicalType { TypeId = 1, MedicalTypeName = "Tablet", MedicalTypeNameArabic = "أقراص" },
                new MedicalType { TypeId = 2, MedicalTypeName = "Capsule", MedicalTypeNameArabic = "كبسولات" },
                new MedicalType { TypeId = 3, MedicalTypeName = "Syrup", MedicalTypeNameArabic = "شراب" },
                new MedicalType { TypeId = 4, MedicalTypeName = "Injection", MedicalTypeNameArabic = "حقن" },
                new MedicalType { TypeId = 5, MedicalTypeName = "Ointment", MedicalTypeNameArabic = "مرهم" },
                new MedicalType { TypeId = 6, MedicalTypeName = "Cream", MedicalTypeNameArabic = "كريم" },
                new MedicalType { TypeId = 7, MedicalTypeName = "Gel", MedicalTypeNameArabic = "جل" },
                new MedicalType { TypeId = 8, MedicalTypeName = "Drop", MedicalTypeNameArabic = "قطرات" },
                new MedicalType { TypeId = 9, MedicalTypeName = "Inhaler", MedicalTypeNameArabic = "بخاخ" },
                new MedicalType { TypeId = 10, MedicalTypeName = "Spray", MedicalTypeNameArabic = "رذاذ" },
                new MedicalType { TypeId = 11, MedicalTypeName = "Cosmetic", MedicalTypeNameArabic = "تجميل" },
                new MedicalType { TypeId = 12, MedicalTypeName = "Patch", MedicalTypeNameArabic = "لاصق" }
            );

            modelBuilder.Entity<Country>().HasData(

                new Country { CountryId = 1, Name = "Jordan" }
            );

            modelBuilder.Entity<Governorate>().HasData(

                new Governorate { GovernorId = 1, CountryId=1, Name = "Amman", NameArabic = "عمان" },
                new Governorate { GovernorId = 2, CountryId=1, Name = "Zarqa", NameArabic = "الزرقاء" },
                new Governorate { GovernorId = 3, CountryId=1, Name = "Irbid", NameArabic = "إربد" },
                new Governorate { GovernorId = 4, CountryId=1, Name = "Ajloun", NameArabic = "عجلون" },
                new Governorate { GovernorId = 5, CountryId=1, Name = "Jerash", NameArabic = "جرش" },
                new Governorate { GovernorId = 6, CountryId=1, Name = "Mafraq", NameArabic = "المفرق" },
                new Governorate { GovernorId = 7, CountryId=1, Name = "Balqa", NameArabic = "البلقاء" },
                new Governorate { GovernorId = 8, CountryId=1, Name = "Madaba", NameArabic = "مادبا" },
                new Governorate { GovernorId = 9, CountryId=1, Name = "Karak", NameArabic = "الكرك" },
                new Governorate { GovernorId = 10,CountryId=1, Name = "Tafilah", NameArabic = "الطفيلة" },
                new Governorate { GovernorId = 11,CountryId=1, Name = "Ma'an", NameArabic = "معان" },
                new Governorate { GovernorId = 12,CountryId=1, Name = "Aqaba", NameArabic = "العقبة" }
            );


            modelBuilder.Entity<CustomerLog>().Property(p => p.DateOfLoggedIn)
                                              .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<CustomerLog>().Property(p => p.IsLogout)
                                              .HasDefaultValue(false);

            modelBuilder.Entity<Customer>().Property(p => p.DateOfRegister)
                                           .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<AdminLog>().Property(p => p.DateOfLoggedIn)
                                           .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Customer>().Property(p => p.IsOnline)
                                           .HasDefaultValue(true);

            modelBuilder.Entity<Admin>().Property(p => p.IsOnline)
                                        .HasDefaultValue(true);

            modelBuilder.Entity<Admin>().Property(p=>p.IsActive)
                                        .HasDefaultValue(true);
            
            modelBuilder.Entity<Manager>().Property(p=>p.IsActive)
                                          .HasDefaultValue(true);

            modelBuilder.Entity<PharmacyCurrency>().Property(p => p.CurrentCurrency)
                                                   .HasDefaultValue(0.00M);

            modelBuilder.Entity<EmailVerification>().Property(p => p.TimeOfCreated)
                                                     .HasColumnType("TIME(0)")
                                                     .HasDefaultValueSql("CONVERT(TIME,GETDATE())");

            modelBuilder.Entity<EmailVerification>().Property(p => p.TimeOfExpired)
                                                     .HasColumnType("TIME(0)")
                                                     .HasComputedColumnSql("DATEADD(MINUTE,3,[TimeOfCreated])",stored:true);

            modelBuilder.Entity<EmailVerification>().Property(p => p.AttemptCount)
                                                    .HasDefaultValue(0);

            modelBuilder.Entity<EmailVerification>().Property(p => p.DateOfCreated)
                                                    .HasDefaultValueSql("CONVERT(DATE,GETDATE())");

            modelBuilder.Entity<EmailVerification>().Property(p => p.IsActive)
                                                     .HasDefaultValue(true);

            modelBuilder.Entity<DeliveryFees>().HasData(

                new DeliveryFees { Id = 4, MinDistanceKm = 00.0M, MaxDistanceKm = 3.00M, Fees = 1.00M },
                new DeliveryFees { Id = 5, MinDistanceKm = 3.10M, MaxDistanceKm = 20.00M, Fees = 2.00M },
                new DeliveryFees { Id = 6, MinDistanceKm = 20.10M, MaxDistanceKm = 35.00M, Fees = 3.00M },
                new DeliveryFees { Id = 7, MinDistanceKm = 35.10M, MaxDistanceKm = 50.00M, Fees = 4.00M }

            );

            modelBuilder.Entity<Cart>().Property(p => p.LastUpdatedTime)
                                       .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Person>().Property(p => p.IsVerified)
                                         .HasDefaultValue(false);

            modelBuilder.Entity<SystemAdmin>()
                        .Property(a => a.AdminId)
                        .ValueGeneratedNever();

            modelBuilder.Entity<Customer>()
                        .Property(a => a.LastLoggedInDateTime)
                        .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Admin>()
                        .Property(a => a.LastLoggedInTime)
                        .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<MedicalCategory>().HasData(

                new MedicalCategory { CategoryId = 1, Name = "مسكنات الألم" },
                new MedicalCategory { CategoryId = 2, Name = "مضادات حيوية" },
                new MedicalCategory { CategoryId = 3, Name = "مضادات الفيروسات" },
                new MedicalCategory { CategoryId = 4, Name = "مضادات الفطريات" },
                new MedicalCategory { CategoryId = 5, Name = "مضادات الحساسية" },
                new MedicalCategory { CategoryId = 6, Name = "مضادات الالتهابات" },
                new MedicalCategory { CategoryId = 7, Name = "الفيتامينات والمكملات الغذائية" },
                new MedicalCategory { CategoryId = 8, Name = "أدوية الجهاز الهضمي" },
                new MedicalCategory { CategoryId = 9, Name = "أدوية الجهاز التنفسي" },
                new MedicalCategory { CategoryId = 10, Name = "أدوية القلب والأوعية الدموية" },
                new MedicalCategory { CategoryId = 11, Name = "أدوية السكري" },
                new MedicalCategory { CategoryId = 12, Name = "أدوية الأعصاب" },
                new MedicalCategory { CategoryId = 13, Name = "أدوية الأمراض الجلدية" },
                new MedicalCategory { CategoryId = 14, Name = "مستحضرات التجميل والعناية بالبشرة" },
                new MedicalCategory { CategoryId = 15, Name = "أدوية العيون" },
                new MedicalCategory { CategoryId = 16, Name = "أدوية المسالك البولية" },
                new MedicalCategory { CategoryId = 17, Name = "أدوية الهرمونات والغدد الصماء" },
                new MedicalCategory { CategoryId = 18, Name = "أدوية الصحة الجنسية" }

            );

            modelBuilder.Entity<TempMeetingRequest>().Property(P => P.DateAndTime)
                                                     .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<TempOrderRequest>().Property(P => P.DateOfSet).HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Order>().Property(P => P.PaymentMethodeId)
                                        .HasDefaultValue(1);


            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))

            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

        }

    }
}
