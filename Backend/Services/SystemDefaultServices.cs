using DatabaseLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using DTOs.RequestDTOs;
using Microsoft.Extensions.Configuration;
using DTOs.DefaultValuesDTOs;
using DTOs.ProductDTOs;
using DTOs.ResultDTOs;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class SystemDefaultServices
    {
        private AppDbContext _context;
        private IConfiguration _configuration;

        enum eMessageType { ForOrder = 1 , ForMeeting=2};

        public SystemDefaultServices(AppDbContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<decimal> GetDistance(DirectionDTO CustomerDir,DirectionDTO PharmacyDir)
        {
            string mapboxApiKey = _configuration.GetSection("MapboxKey").Value;

            string origin = CustomerDir.Longitude+","+CustomerDir.Latitude;

            string destination = PharmacyDir.Longitude + "," + PharmacyDir.Latitude;

            string url = $"https://api.mapbox.com/directions/v5/mapbox/driving/{origin};{destination}?access_token={mapboxApiKey}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var json = JsonDocument.Parse(jsonResponse);
                    var distance = json
                        .RootElement
                        .GetProperty("routes")[0]
                        .GetProperty("distance")
                        .GetDecimal();

                    return distance / 1000;
                }
                else
                {
                    return 0.0M;
                }
            }

        }

        public async Task<List<CountryDTO>> GetCountries()
        {
            var Countries = await _context.Countrys.Select(c => new CountryDTO()
            {
                CountryId = c.CountryId,
                CountryName = c.Name,

            })
            .ToListAsync();

            return Countries;
        }

        public async Task<List<GovernateDTO>> GetGovernates()
        {
            var Governates = await _context.Governorates.Select(g => new GovernateDTO()
            {
                GovernotId = g.GovernorId,
                Name = g.Name,
                NameArabic = g.NameArabic
            })
            .ToListAsync();

            return Governates;
        }

        public async Task<List<PaymentMethodeDTO>> GetPaymentMethodes()
        {
            // MethodeId>=4 because this is only pay methodes available in sys for now .

            var Methods = await _context.PaymentMethodes.Where(P => P.MethodeId >= 4)
                                                      .Select(P => new PaymentMethodeDTO()
                                                      {
                                                          Id = P.MethodeId,
                                                          Name = P.MethodeName,
                                                      })
                                                      .ToListAsync();

            return Methods;
        }

        public async Task<List<MedicalTypeDTO>> GetMedicalTypes()
        {
            var Types = await _context.MedicalTypes.Select(m => new MedicalTypeDTO()
            {
                TypeId = m.TypeId,
                MedicalTypeName = m.MedicalTypeName,
                MedicalTypeNameArabic= m.MedicalTypeNameArabic
            })
            .ToListAsync();

            return Types;
        }

        public async Task<List<DeliveryTypeDTO>> GetDeliveryTypes()
        {
            var Types = await _context.DeliveryFees.Select(D => new DeliveryTypeDTO()
            {
                Id = D.Id,
                MaxDistanceKm = D.MaxDistanceKm,
                MinDistanceKm = D.MinDistanceKm,
                Fees = D.Fees
            })
            .ToListAsync();

            return Types;
        }

        public WhatsAppRequestLinkResultDTO GetWhatsAppMessageLink(int CustomerId,int PharmacyId,string message="")
        {
            string defaultMessage = $"""
                مرحبا , أريد حجز استشارة مكالمة صوتية 
                رقم العميل : {CustomerId}
                رقم الصيدلية : {PharmacyId}
                الرجاء عدم تغيير الرسالة أو الأضافة عليها!
                """; 

            string encodeMessage = Uri.EscapeDataString(defaultMessage);

            string phoneNumber = _configuration.GetSection("PhoneNumber").Value;

            string WhatsAppLink = $"https://wa.me/{phoneNumber}?text={encodeMessage}";

            return new WhatsAppRequestLinkResultDTO()
            {
                IsDone = true,
                url = WhatsAppLink,
            };
        }

        public async Task<List<RegionDTO>> GetGovernorateRegions(int GovernorateId)
        {
            var regions = await _context.Regions.Where(R => R.GovernorateId == GovernorateId)
                                                  .Select(R => new RegionDTO()
                                                  {
                                                      RegionID = R.RegionId,
                                                      RegionName = R.RegionName,
                                                  })
                                                  .ToListAsync();

            return regions;
        }


    }
}
