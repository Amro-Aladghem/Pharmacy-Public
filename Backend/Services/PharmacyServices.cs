using DatabaseLayer.Entities;
using DTOs.CustomerDTOs;
using DTOs.PersonDTOs;
using DTOs.PharamacyDTOs;
using DTOs.PharmacyDTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.ReqestDTOs;
using DTOs.RequestDTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Storage;
using DTOs.ResultDTOs;
using DTOs.OrderDTOs;
using DTOs.ProductDTOs;
using System.Diagnostics.CodeAnalysis;


namespace Services
{
    public class PharmacyServices
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly SystemDefaultServices _systemDefaultServices;

        private readonly string DefaultPharmacyURL = "";

        public PharmacyServices(AppDbContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration= configuration;
            _systemDefaultServices = new SystemDefaultServices(context, configuration);
        }

        private class FeesDistanse
        {
            public decimal? Fees { get; set; }
        }

        private string GeneratePINCode()
        {
            Random random = new Random();

            return random.Next(1000, 9999).ToString();
        }

        public bool AreValuesCorrect(PharmacyInfoDTO pharmacyInfoDTO)
        {
            if(string.IsNullOrEmpty(pharmacyInfoDTO.Name) || string.IsNullOrEmpty(pharmacyInfoDTO.Phone)
                || string.IsNullOrEmpty(pharmacyInfoDTO.PhEmail))
            {
                return false;
            }

            if(pharmacyInfoDTO.CountryId<=0 || pharmacyInfoDTO.GovernateId<=0||pharmacyInfoDTO.Latitude==0||pharmacyInfoDTO.Longitude==0)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> AddDefaultDeliveryLocations(int PharmacyId)
        {
            var LocationsIdList = await _context.DeliveryFees.Select(d => d.Id)
                                                             .ToListAsync();

            if(LocationsIdList is null)
            {
                return false;
            }

            List<PharmacyDeliveryLocation> pharmacyDeliveryLocations = new List<PharmacyDeliveryLocation>();
            bool IsDeliveriedToHere = true;

            foreach(var locationId in LocationsIdList)
            {
                pharmacyDeliveryLocations.Append(new PharmacyDeliveryLocation
                { PharmacyId = PharmacyId, DeliveryId = locationId, IsDeliveriedToHere = IsDeliveriedToHere });

                IsDeliveriedToHere = false;
            }

            _context.PharmacyDeliveryLocations.AddRange(pharmacyDeliveryLocations);

            if(await _context.SaveChangesAsync()<=0)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> SetPharmacyToManager(int ManagerId,int PharmacyId)
        {
            var Manager = new Manager()
            {
                ManagerId = ManagerId,
                PharmacyId = PharmacyId
            };

            _context.Attach(Manager);
            _context.Entry(Manager).Property(P => P.PharmacyId).IsModified = true;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<string?> AddNewPharamcy(PharmacyInfoDTO pharmacyInfoDTO,int ManagerId)
        {
            string PINCode = GeneratePINCode();

            var NewPharmacy = new Pharmacy()
            {
                Name = pharmacyInfoDTO.Name,
                ArabicName = pharmacyInfoDTO.ArabicName,
                About = pharmacyInfoDTO.About,
                CountryId = pharmacyInfoDTO.CountryId,
                GovernorateId = pharmacyInfoDTO.GovernateId,
                PhoneNumber = pharmacyInfoDTO.Phone,
                PhEmail = pharmacyInfoDTO.PhEmail,
                Longitude=pharmacyInfoDTO.Longitude,
                Latitude=pharmacyInfoDTO.Latitude,
                StreetName = pharmacyInfoDTO.StreetName,
                IsHasDelivery = pharmacyInfoDTO.IsHasDelivery,
                PINCode = PINCode,

                VedioCallPrice = pharmacyInfoDTO.VedioCallPrice==0? pharmacyInfoDTO.VedioCallPrice : null,
                ImageURL = pharmacyInfoDTO.ImageURL == null ? DefaultPharmacyURL : pharmacyInfoDTO.ImageURL
            };

            using(var transaction = _context.Database.BeginTransaction())
            {
                _context.Pharmacies.Add(NewPharmacy);
                await _context.SaveChangesAsync();

                if (NewPharmacy.IsHasDelivery)
                {
                    if (! await AddDefaultDeliveryLocations(NewPharmacy.PharmacyId))
                    {
                        return null;
                    }
                }

                if(!await SetPharmacyToManager(ManagerId, NewPharmacy.PharmacyId))
                {
                    return null;
                }

                transaction.Commit();

                return PINCode;
            }
        }

        public async Task<PharmacyDTO?> UpdatePharmacyProfileInfo(PharmacyDTO pharmacyDTO)
        {
            var pharmacy = await _context.Pharmacies.FirstOrDefaultAsync(ph => ph.PharmacyId == pharmacyDTO.PharmacyId);

            if(pharmacy is null)
            {
                return null;
            }

            pharmacy.Name = pharmacyDTO.Name;
            pharmacy.ArabicName = pharmacyDTO.ArabicName;
            pharmacy.About = pharmacyDTO.About;

            pharmacy.ImageURL= string.IsNullOrEmpty(pharmacyDTO.ImageURL)?DefaultPharmacyURL:pharmacyDTO.ImageURL;

            if(await _context.SaveChangesAsync()<=0)
            {
                return null;
            }

            pharmacyDTO.ImageURL = pharmacy.ImageURL;

            return pharmacyDTO;
        }

        public async Task<bool> UpdatePharmacyGeneralInfo(PharmacyGeneralInfoDTO pharmacyGeneralInfo)
        {
            var pharmacy = await _context.Pharmacies.FirstOrDefaultAsync(ph => ph.PharmacyId == pharmacyGeneralInfo.PharamcyId);
            
            if(pharmacy is null)
            {
                return false;
            }

            pharmacy.CountryId= pharmacyGeneralInfo.CountryId;
            pharmacy.GovernorateId = pharmacyGeneralInfo.GovernateId;
            pharmacy.PhoneNumber = pharmacyGeneralInfo.Phone;
            pharmacy.StreetName=pharmacyGeneralInfo.StreetName;
            pharmacy.Longitude= pharmacyGeneralInfo.Longitude;
            pharmacy.Latitude = pharmacyGeneralInfo.Latitude;

            if (await _context.SaveChangesAsync() <= 0)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateVedioCallInfo(PharmacyVedioCallDTO pharmacyVedioCallDTO)
        {
            bool IsHasVedioCall = pharmacyVedioCallDTO.IsHasVedioCall;

            var pharmacy = new Pharmacy()
            {
                PharmacyId = pharmacyVedioCallDTO.PharamcyId,
                VedioCallPrice = IsHasVedioCall ? pharmacyVedioCallDTO.Price : null
            };

            _context.Attach(pharmacy);
            _context.Entry(pharmacy).Property(p => p.VedioCallPrice).IsModified = true;


            if(await _context.SaveChangesAsync()<=0)
            {
                return false;
            }

            return true;
        }

        public async Task<PharmacyVedioCallDTO?> GetPharmacyVedioCallInfO(int PharmacyId)
        {
            var pharmacyInfo =await _context.Pharmacies.Where(P => P.PharmacyId == PharmacyId)
                                                      .Select(P => new PharmacyVedioCallDTO()
                                                      {
                                                          PharamcyId = P.PharmacyId,
                                                          Price = P.VedioCallPrice,
                                                          IsHasVedioCall = P.IsHasMeetingService,
                                                          Name = P.ArabicName,
                                                          Image = P.ImageURL

                                                      })
                                                      .FirstOrDefaultAsync();
            return pharmacyInfo;
        }

        private async Task<bool> UpdateDeliveryLoactions(PhDeliveryLocationsDTO pharmacyDeliveryLocation)
        {
            var DeliveryLocations = await _context.PharmacyDeliveryLocations.Where(pDL => pDL.PharmacyId == pharmacyDeliveryLocation.PharmacyId)
                                                                            .ToDictionaryAsync(pDL => pDL.DeliveryId);

            if(DeliveryLocations is null)
            {
                return false;
            }

            if(DeliveryLocations.Count != pharmacyDeliveryLocation.DeliveryLocations.Count)
            {
                throw new Exception("The number of Delivery locations is not correct !");
            }

            foreach(var delivery in pharmacyDeliveryLocation.DeliveryLocations)
            {
                DeliveryLocations[delivery.id].IsDeliveriedToHere = delivery.IsDeliverdToHere;
            }

            if(await _context.SaveChangesAsync() <=0)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateDeliveryInfo(PhDeliveryLocationsDTO pharmacyDeliveryLocation)
        {
           return await UpdateDeliveryLoactions(pharmacyDeliveryLocation);
        }

        public async Task<bool> ActivateDelivery(int PhamacyId)
        {
            using(var transaction = _context.Database.BeginTransaction())
            {
                await AddDefaultDeliveryLocations(PhamacyId);

                var pharmacy = new Pharmacy() 
                {
                   PharmacyId=PhamacyId,
                   IsHasDelivery=true
                };

                _context.Attach(pharmacy);
                _context.Entry(pharmacy).Property(P => P.IsHasDelivery).IsModified = true;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return true;
            }
        }

        public async Task<bool> DeActivateDelivery(int PhamacyId)
        {
            using(var transaction = _context.Database.BeginTransaction())
            {
                await _context.PharmacyDeliveryLocations.Where(pDL => pDL.PharmacyId == PhamacyId).ExecuteDeleteAsync();

                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new Exception("The delete Process faild!");
                }

                var updatedPharmacy = new Pharmacy()
                {
                    PharmacyId = PhamacyId,
                    IsHasDelivery = false
                };

                _context.Attach(updatedPharmacy);
                _context.Entry(updatedPharmacy).Property(p => p.IsHasDelivery).IsModified = true;

                await _context.SaveChangesAsync(); 

                await transaction.CommitAsync();

                return true ;
            }
        }

        public async Task<PharmacyGeneralInfoDTO?> ShowGeneralInfo(int PharmacyId)
        {
            var PharmacyGeneralInfo = await _context.Pharmacies.Where(ph => ph.PharmacyId == PharmacyId)
                                                                 .Select(ph => new PharmacyGeneralInfoDTO()
                                                                 {
                                                                     PharamcyId = ph.PharmacyId,
                                                                     Longitude = ph.Longitude,
                                                                     Latitude= ph.Latitude,
                                                                     CountryId = ph.CountryId,
                                                                     GovernateId = ph.GovernorateId,
                                                                     PhEmail = ph.PhEmail,
                                                                     Phone = ph.PhoneNumber,
                                                                     StreetName = ph.StreetName
                                                                 })
                                                                 .FirstOrDefaultAsync();

            return PharmacyGeneralInfo;
        }

        public async Task<PharmacyDTO?> ShowProfileInfo(int PharmacyId)
        {
            var PharmacyProfileInfo = await _context.Pharmacies.Where(ph => ph.PharmacyId == PharmacyId)
                                                                 .Select(ph => new PharmacyDTO()
                                                                 {
                                                                     PharmacyId = ph.PharmacyId,
                                                                     About = ph.About,
                                                                     ArabicName = ph.ArabicName,
                                                                     Name = ph.Name,
                                                                     ImageURL = ph.ImageURL
                                                                 })
                                                                 .FirstOrDefaultAsync();

            return PharmacyProfileInfo;
        }

        private async Task<List<DeliveryFeesDTO>?> GetPharamacyDeliveryLoactions(int PharmacyId)
        {
            var Locations = await _context.PharmacyDeliveryLocations.Where(pDL => pDL.PharmacyId == PharmacyId)
                                                                      .Select(pDL => new DeliveryFeesDTO()
                                                                      {
                                                                          id=pDL.DeliveryId,
                                                                          IsDeliverdToHere=pDL.IsDeliveriedToHere,
                                                                          Fees=pDL.DeliveryFees.Fees,
                                                                          MinDistanceKm=pDL.DeliveryFees.MinDistanceKm,
                                                                          MaxDistanceKm=pDL.DeliveryFees.MaxDistanceKm
                                                                      })
                                                                      .ToListAsync();

            return Locations;
        }

        public async Task<PhDeliveryLocationsDTO?> ShowDeliveryInfo(int PharmacyId)
        {
            var PharamcyDeliveryInfo = await _context.Pharmacies.Where(ph => ph.PharmacyId == PharmacyId)
                                                                  .Select(ph => new PhDeliveryLocationsDTO()
                                                                  {
                                                                      PharmacyId = ph.PharmacyId,
                                                                      IsHasDelivery = ph.IsHasDelivery,
                                                                  })
                                                                  .FirstOrDefaultAsync();

            if(PharamcyDeliveryInfo is null)
            {
                return null;
            }

            if(!PharamcyDeliveryInfo.IsHasDelivery)
            {
                return PharamcyDeliveryInfo;
            }

            var Locations = await GetPharamacyDeliveryLoactions(PharmacyId);

            if(Locations is null)
            {
                return null;
            }

            PharamcyDeliveryInfo.DeliveryLocations = Locations;

            return PharamcyDeliveryInfo;
        }

        public async Task<bool> PharmacyChangingEmail(int PharmacyId , string NewEamil)
        {
            var pharmacy = new Pharmacy()
            {
                PharmacyId = PharmacyId,
                PhEmail = NewEamil,
            };

            _context.Attach(pharmacy);
            _context.Entry(pharmacy).Property(p => p.PhEmail).IsModified = true;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<string?> ResetPINCode(int PharmacyId)
        {
            string NewPINCode = GeneratePINCode();

            var pharmacy = new Pharmacy()
            {
                PharmacyId = PharmacyId,
                PINCode = NewPINCode
            };

            _context.Attach(pharmacy);
            _context.Entry(pharmacy).Property(p => p.PINCode).IsModified = true;

            if(await _context.SaveChangesAsync() <= 0)
                return null;

            return NewPINCode;
        }

        public async Task<decimal?> GetPharamcyVedioCallPrice(int PharmacyId)
        {
            decimal? VedioCallPrice = await _context.Pharmacies.Where(P => P.PharmacyId == PharmacyId)
                                                               .Select(P => P.VedioCallPrice)
                                                               .FirstOrDefaultAsync();

            if(VedioCallPrice is null)
            {
                throw new Exception("Error , when get pharmacy meeting Price !");
            }

            return VedioCallPrice;
        }

        public async Task<List<PharmacyListDTO>?> GetMostNearerPhsFromCustomer(int CustomerId)
        {
            var Customer = await _context.Customers.FirstOrDefaultAsync(C => C.CutomerId == CustomerId);

            if(Customer is null)
            {
                throw new Exception("Server Network Error!");
            }

            var Pharmacies = await _context.Database.SqlQueryRaw<PharmacyListDTO>("SELECT * FROM [dbo].[GetMostNearerPhFromCustomer]({0},{1})",
                                                                        Customer.Latitude,Customer.Longitude).ToListAsync();

            return Pharmacies;
        }

        private async Task<CheckDeliveryFeesResultDTO> GetDeliveryFeesFormDB(CheckDeliveryDTO checkDeliveryDTO)
        {
            decimal? Fees =await _context.customerPharmacyDistances.Where(c => c.PharmacyId == checkDeliveryDTO.PharmacyId
                                                                      && c.CustomerId == checkDeliveryDTO.CustomerId)
                                                                      .Select(C => C.Fees)
                                                                      .FirstOrDefaultAsync();


            bool IsDeliveredToCustomer = (Fees != 0.0M || Fees != null);

            return new CheckDeliveryFeesResultDTO()
            {
                IsDelivered = IsDeliveredToCustomer,
                Fees = Fees
            };
        }

        private async Task<DirectionDTO?> GetCustomerDir(int CustomerId)
        {
            DirectionDTO? CustomerDirection = await _context.Customers.Where(C => C.CutomerId == CustomerId)
                                                                        .Select(C => new DirectionDTO()
                                                                        {
                                                                            Longitude = C.Longitude,
                                                                            Latitude = C.Latitude
                                                                        })
                                                                        .FirstOrDefaultAsync();
            return CustomerDirection;
        }

        private async Task<DirectionDTO?> GetPharmacyDir(int PharmacyId)
        {
            DirectionDTO? PharmacyDirection = await _context.Pharmacies.Where(P => P.PharmacyId == PharmacyId)
                                                                       .Select(P => new DirectionDTO()
                                                                       {
                                                                           Longitude = P.Longitude,
                                                                           Latitude = P.Latitude
                                                                       })
                                                                       .FirstOrDefaultAsync();

            return PharmacyDirection;
        }

        private async Task<decimal?> GetDeliveryFees(decimal Distance,int PharmacyId,int CustomerId)
        {
            var fees = await _context.Database
                                     .SqlQueryRaw<FeesDistanse>("EXEC GetFeesAndSaveDistance {0}, {1}, {2}",
                                                                   Distance, PharmacyId, CustomerId)
                                     .ToListAsync();

            if (fees is null)
                return 0;

            return fees.FirstOrDefault().Fees;
        }

        private async Task<CheckDeliveryFeesResultDTO> IsPharmacyDeliverToCustomer(CheckDeliveryDTO checkDeliveryDTO)
        {
            DirectionDTO? CustomerDirection = await GetCustomerDir(checkDeliveryDTO.CustomerId);
            DirectionDTO? PharmacyDirection = await GetPharmacyDir(checkDeliveryDTO.PharmacyId);


            if (CustomerDirection is null || PharmacyDirection is null)
            {
                throw new Exception("Somting Error with server!");
            }

            decimal distance = await _systemDefaultServices.GetDistance(CustomerDirection, PharmacyDirection);

            if (distance == 0)
            {
                throw new Exception("Connection Error with Calculate distance!");
            }


            decimal? Fees = await GetDeliveryFees(distance, checkDeliveryDTO.PharmacyId,checkDeliveryDTO.CustomerId);

            bool IsDeliveredToCustomer = (Fees != 0.0M && Fees!=null);

            return new CheckDeliveryFeesResultDTO()
            {
                IsDelivered = IsDeliveredToCustomer,
                Fees = Fees
            };

        }

        public async Task<CheckDeliveryFeesResultDTO> HandelDeliveryFeesCalculate(CheckDeliveryDTO checkDeliveryDTO)
        {
            bool IsDeliveryFeesCalculated = _context.customerPharmacyDistances.Any(C => C.PharmacyId == checkDeliveryDTO.PharmacyId
                                                                                  && C.CustomerId == checkDeliveryDTO.CustomerId);

            if(IsDeliveryFeesCalculated)
            {
                return await GetDeliveryFeesFormDB(checkDeliveryDTO);
            }
            else
            {
                return await IsPharmacyDeliverToCustomer(checkDeliveryDTO);
            }
        }

        public async Task<ShowPharmacyDTO?> ShowPharmacyProfile(int PharmacyId)
        {
            var PharmacyDTO =await  _context.Pharmacies.Where(P => P.PharmacyId == PharmacyId)
                                                 .Select(P => new ShowPharmacyDTO()
                                                 {
                                                     PharmacyId = P.PharmacyId,
                                                     Name = P.Name,
                                                     ArabicName = P.ArabicName,
                                                     About = P.About,
                                                     ImageURL = P.ImageURL,
                                                     IsHasDelivery = P.IsHasDelivery,
                                                     VedioCallPrice = P.VedioCallPrice,
                                                     CountryName = P.country.Name,
                                                     GovernateName = P.Governorate.NameArabic,
                                                     IsHasMeetingService=P.IsHasMeetingService,
                                                     RegionName=P.Region.RegionName

                                                 })
                                                 .FirstOrDefaultAsync();
            return PharmacyDTO;
        }

        public async Task<bool> IsPharmacyExists(int PharmacyId)
        {
            bool IsExists = await  _context.Pharmacies.AnyAsync(P=>P.PharmacyId==PharmacyId);

            return IsExists;
        }

        public async Task<bool> IsPharmacyHasThisUser(int UserId,TokenServices.eUserType userType,int PharmacyId)
        {
            bool IsCorrectUser = false;

            if (userType == TokenServices.eUserType.Customer)
                return false;

            if(userType==TokenServices.eUserType.Manager)
            {
                IsCorrectUser = await _context.Managers.Where(M => M.ManagerId == UserId && M.PharmacyId == PharmacyId).AnyAsync();
            }
            else
            {
                IsCorrectUser = await _context.Admins.Where(M => M.AdminId == UserId && M.PharamcyId==PharmacyId).AnyAsync();
            }

            return IsCorrectUser;
        }

        public async Task<bool> IsPharmacyHasDelivery(int PharmacyId)
        {
            bool? IsHasDelivery = await _context.Pharmacies.Where(P => P.PharmacyId == PharmacyId)
                                                         .Select(P => P.IsHasDelivery)
                                                         .FirstAsync();

            if(IsHasDelivery is null)
                return false;

            return (bool)IsHasDelivery;
        }

        private IQueryable<Pharmacy> CreateGetPharmaciesQuery(PaginatedPharmacyListDTO paginatedPharmacyListDTO, int? GovernorateId
            , int? RegionId)
        {
            var query = _context.Pharmacies.Where(Ph => Ph.PharmacyId > paginatedPharmacyListDTO.LastPharmacyId).AsQueryable();

            if (GovernorateId.HasValue)
            {
                query = query.Where(Ph => Ph.GovernorateId == GovernorateId.Value);
            }

            if (RegionId.HasValue)
            {
                query = query.Where(Ph => Ph.RegionId == RegionId.Value);
            }

            return query;
        }

        public async Task<PharmacyListResultDTO?> GetPharmaciesList(PaginatedPharmacyListDTO paginatedPharmacyListDTO,int? GovernorateId,int? RegionId)
        {
            var query = CreateGetPharmaciesQuery(paginatedPharmacyListDTO, GovernorateId, RegionId);

            var Pharmacies = await query.Take(paginatedPharmacyListDTO.Limit)
                                          .Select(Ph => new PharmacyListDTO()
                                          {
                                              PharmacyId = Ph.PharmacyId,
                                              Name = Ph.Name,
                                              ArabicName = Ph.ArabicName,
                                              ImageURL = Ph.ImageURL,
                                              GovernateName = Ph.Governorate.NameArabic,
                                              RegionName=Ph.Region.RegionName
                                          })
                                          .ToListAsync();

            if (Pharmacies is null)
                return null;

            int RowsCount = 0;

            if (!paginatedPharmacyListDTO.IsRowsCountCalculated)
            {
                 RowsCount = await query.CountAsync();
            }

            int LastPharmacyId = Pharmacies.Last().PharmacyId;


            return new PharmacyListResultDTO()
            {
                Pharmacies = Pharmacies,
                RowsCount=RowsCount,
                LastPharmacyId=LastPharmacyId
            };

        }
       
        public async Task<PharmacyMeetingListResultDTO?> GetPharmaciesThatHaveMeetingService(PaginatedPharmacyListDTO paginatedPharmacyListDTO)
        {
            var query =  _context.Pharmacies.Where(Ph => Ph.PharmacyId > paginatedPharmacyListDTO.LastPharmacyId && Ph.IsHasMeetingService==true);

            var Pharmacies = await query.Take(paginatedPharmacyListDTO.Limit)
                                          .Select(Ph => new PharmacyMeetingListDTO()
                                          {
                                              PharmacyId = Ph.PharmacyId,
                                              PharmacyName = Ph.ArabicName,
                                              ImageURL = Ph.ImageURL,
                                              GovernateName = Ph.Governorate.NameArabic,
                                              Price=(decimal)Ph.VedioCallPrice
                                          })
                                          .ToListAsync();

            if (Pharmacies is null)
                return null;

            int RowsCount = 0;

            if(!paginatedPharmacyListDTO.IsRowsCountCalculated)
            {
                RowsCount = query.Count();
            }

            int LastPharmacyId = Pharmacies.Last().PharmacyId;

            string NextPage = "?LastPharmacyId=" + LastPharmacyId + "&Limit=" + paginatedPharmacyListDTO.Limit + "&IsRowsCountCalculated=true";

            return new PharmacyMeetingListResultDTO()
            {
                pharmacies = Pharmacies,
                NextPage = NextPage,
                RowsCount = RowsCount,
                LastPharmacyId = LastPharmacyId
            };

        }



    }
}
