using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class Pharmacy
    {
        [Key]
        public int PharmacyId { get; set; }

        [Required]
        [MaxLength(150,ErrorMessage ="The Max length is 150 char!")]
        public string Name { get; set; }

        
        [MaxLength(150, ErrorMessage = "The Max length is 150 char!")]
        public string ? ArabicName { get; set; }


        public string ? About { get;set; }

        
        public int CountryId { get; set; } 
        public Country country { get; set; }

        
        public int GovernorateId { get; set; }
        public Governorate Governorate { get; set; }

        [Required]
        [Column("Latitude", TypeName = "decimal(10,5)")]
        public decimal Latitude { get; set; }

        [Required]
        [Column("Longitude", TypeName = "decimal(10,5)")]
        public decimal Longitude { get; set; }


        [MaxLength(200, ErrorMessage = "The Max length is 150 char!")]
        public string ? StreetName { get; set; }

        [Required]
        [MaxLength(15, ErrorMessage = "The Max length is 15 char!")]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(120, ErrorMessage = "The Max length is 120 char!")]
        public string PhEmail { get; set; }

        [MaxLength(200, ErrorMessage = "The Max length is 200 char!")]
        public string ? PINCode { get; set; }

        [Column("VedioCallPrice", TypeName = "decimal(18,2)")]
        public decimal ? VedioCallPrice { get; set; }
        public DateOnly? DateOfRegister { get; set; }
        public bool IsHasDelivery { get; set; }

        [MaxLength(1000)]
        public string ? ImageURL { get; set; }
        public bool IsHasMeetingService { get; set; }

        [ForeignKey("Region")]
        public int? RegionId { get; set; }
        public Region Region { get; set; }

        public ICollection<Admin> Admins { get; set; }
        public ICollection<PhPramacyProduct> phPramacyProducts { get; set; } 
        public  ICollection<Order> Orders { get; set; }
        public ICollection<Message> Messages { get; set;}
        public  ICollection<RequestMeeting> RequestMeetings { get; set; }
        public ICollection<PharmacyDeliveryLocation> PharmacyDeliveryLocations { get; set; }

    }
}
