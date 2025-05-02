using DatabaseLayer.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Security;
using DTOs.ManagerDTOs;
using DTOs.AdminDTOs;
using DTOs.PharamacyDTOs;
using Microsoft.Identity.Client;
using DTOs.PersonDTOs;
using Microsoft.EntityFrameworkCore;
using DTOs.RequestDTOs;
using Microsoft.Extensions.Logging.Abstractions;

namespace Services
{
    public class ManagerServices
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly SecurityMethode _securityMethode;
        private readonly PersonServices _personServices;

        public ManagerServices(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _securityMethode = new SecurityMethode(configuration);
            _personServices = new PersonServices(context, configuration);
        }

        public string DefaultImageURL = "";
        
        private async Task<PharmacyDTO?> ReturnManagerPharmacyInfo(int PharmacyId)
        {
           PharmacyDTO? pharmacy= await _context.Pharmacies.Where(p => p.PharmacyId == PharmacyId)
                                                           .Select(p => new PharmacyDTO()
                                                           {
                                                               PharmacyId = p.PharmacyId,
                                                               Name = p.Name,
                                                               ArabicName = p.ArabicName,
                                                               ImageURL = p.ImageURL
                                                           })
                                                           .FirstOrDefaultAsync();

            return pharmacy;
        }

        private ManagerDTO TransferToManagerDTO(Manager manager,Person person)
        {
            return new ManagerDTO()
            {
                ManagerId = manager.ManagerId,
                Person = new PersonDTO()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    ProfileImageLink = person.ProfileImageURL,
                },
                Pharmacy = null
            };
        }
        
        public async Task<ManagerDTO?> AddNewManager(MangerRegisterDTO mangerRegisterDTO)
        {
            Person? registeredPerson = await _personServices.AddFullRegisterInfo(mangerRegisterDTO.PersonRegisterDTO);

            if(registeredPerson is null)
            {
                return null;
            }


            var NewManager = new Manager()
            {
                PersonId= registeredPerson.PersonId,
                PharmacyId=mangerRegisterDTO.PharmacyId,
            };

            _context.Managers.Add(NewManager);

            if (await _context.SaveChangesAsync() <= 0)
            {
                return null;
            }
           
            return TransferToManagerDTO(NewManager, registeredPerson);
        }

        private async Task<ManagerDTO?> GetManagerDTOWithoutPharmacy(int ManagerId)
        {
            var managerDTO = await _context.Managers.Where(m => m.ManagerId == ManagerId)
                                                      .Select(m => new ManagerDTO()
                                                      {
                                                          ManagerId = m.ManagerId,
                                                          Person = new PersonDTO()
                                                          {
                                                              FirstName = m.Person.FirstName,
                                                              LastName = m.Person.LastName,
                                                              ProfileImageLink = m.Person.ProfileImageURL
                                                          },
                                                      }).FirstOrDefaultAsync();

            if (managerDTO is not null)
                managerDTO.IsHasPharmacy = false;

            return managerDTO;
        }

        private async Task<ManagerDTO?> GetManagerDTOWithPharmacy(int ManagerId)
        {
            var managerDTO =await _context.Managers.Where(m => m.ManagerId == ManagerId)
                                                     .Select(m => new ManagerDTO()
                                                     {
                                                         ManagerId = m.ManagerId,
                                                         Person = new PersonDTO()
                                                         {
                                                             FirstName = m.Person.FirstName,
                                                             LastName = m.Person.LastName,
                                                             ProfileImageLink = m.Person.ProfileImageURL
                                                         },
                                                         Pharmacy = new PharmacyDTO()
                                                         {
                                                             Name = m.Pharmacy.Name,
                                                             ArabicName = m.Pharmacy.ArabicName,
                                                             ImageURL = m.Pharmacy.ImageURL,
                                                             PharmacyId = (int)m.PharmacyId
                                                         },

                                                     }).FirstOrDefaultAsync();

            return managerDTO;
        }

        public async Task<ManagerDTO?> GetManagerDTO(int ManagerId,int?PharmacyId)
        {
            if (PharmacyId is null || PharmacyId == 0)
            {
                return await GetManagerDTOWithoutPharmacy(ManagerId);
            }
            else
            {
               return await  GetManagerDTOWithPharmacy(ManagerId);
            }
        }

        public async Task<ManagerDTO?> CheckManagerLoginInfo(ManagerLoginDTO managerLoginDTO)
        {

            ManagerCheckInfo? managerInfo =await _context.Managers.Where(m => m.Person.Email == managerLoginDTO.Email)
                                                                     .Select(m => new ManagerCheckInfo()
                                                                     {
                                                                         ManagerId=m.ManagerId,
                                                                         Password=m.Person.Password,
                                                                         IsActive=m.IsActive,
                                                                         PharamcyId=m.PharmacyId,
                                                                         PINCode=m.Pharmacy.PINCode
                                                                     })
                                                                     .FirstOrDefaultAsync();

            if (managerInfo == null)
            {
                return null;
            }

            if (!managerInfo.IsActive)
            {
                throw new Exception("Your Account is DeActivate!");
            }

            if(managerInfo.PharamcyId is not null || managerInfo.PharamcyId !=0)
            {
                if (managerInfo.PINCode != managerInfo.PINCode)
                {
                    throw new Exception("The PIN Is not correct!");
                }
            }
           

            if (!_securityMethode.VerifyEncryptPassword(managerLoginDTO.Password, managerLoginDTO.Password))
            {
                throw new Exception("The Passowrd is not correct!");
            }

            var ManagerDTO = await GetManagerDTO(managerInfo.ManagerId,managerInfo.PharamcyId);

            return ManagerDTO;
        }

        public async Task<Person?> UpdatePersonInfoForAdmin(UpdateManagerDTO updateManagerDTO)
        {
            int ? personId =await  _context.Managers.Where(m=>m.ManagerId==updateManagerDTO.ManagerId)
                                                     .Select(m => (int?)m.PersonId)
                                                     .FirstOrDefaultAsync();

            if(personId is null)
            {
                return null;
            }

            var person =await _context.Persons.FirstOrDefaultAsync(p => p.PersonId == personId);

            if(person is null)
            {
                return null;
            }


            person.FirstName = updateManagerDTO.Person.FirstName;
            person.LastName = updateManagerDTO.Person.LastName;
            person.Phone = updateManagerDTO.Person.Phone;

            person.ProfileImageURL = string.IsNullOrEmpty(person.ProfileImageURL) ? DefaultImageURL : person.ProfileImageURL;

            if(await _context.SaveChangesAsync()<=0)
            {
                return null;
            }

            return person;
        }

        public async Task<ManagerDTO?> UpdateManagerDTO(UpdateManagerDTO updateManagerDTO)
        {

            var Pharmacy =await ReturnManagerPharmacyInfo(updateManagerDTO.PharmacyId);

            var person = await UpdatePersonInfoForAdmin(updateManagerDTO);

            if(person == null)
            {
                throw new Exception("The Updated Person Info was faild!");
            }

            return new ManagerDTO()
            {
                ManagerId=updateManagerDTO.ManagerId,
                Person= new PersonDTO()
                {
                    FirstName=person.FirstName,
                    LastName=person.LastName,
                    ProfileImageLink= person.ProfileImageURL
                },
                Pharmacy=Pharmacy
            };

        }

        public async Task<bool> ChangeEmail(int ManagerId, string NewEmail)
        {
            var manager =await _context.Managers.Include(a => a.Person)
                                                .Where(a => a.ManagerId == ManagerId)
                                                .FirstOrDefaultAsync();

            if (manager is null || manager.Person is null)
            {
                throw new Exception("No admin with this Id!");
            }

            manager.Person.Email = NewEmail;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ChangePassword(int ManagerId, string NewPassword)
        {
            var manager =await _context.Managers.Include(a => a.Person)
                                                  .Where(a => a.ManagerId == ManagerId)
                                                  .FirstOrDefaultAsync();

            if (manager is null || manager.Person is null)
            {
                throw new Exception("No admin with this Id!");
            }

            string EncryptPassword = _securityMethode.Encrypt(NewPassword);

            manager.Person.Password = EncryptPassword;

            return await _context.SaveChangesAsync() > 0;
        }

        public AdminDTO TransferToAdminDTO(Admin admin, Person person, PharmacyDTO pharmacyDTO)
        {
            AdminDTO adminDTO = new AdminDTO()
            {
                AdminId = admin.AdminId,
                Person = new PersonDTO
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    ProfileImageLink = person.ProfileImageURL
                },
                Pharmacy = pharmacyDTO
            };

            return adminDTO;
        }

        public async Task<AdminDTO?> AddNewAdmin(AdminRegisterDTO adminRegisterDTO)
        {
            Person? registeredPerson = await _personServices.AddFullRegisterInfo(adminRegisterDTO.PersonRegisterDTO);

            if (registeredPerson is null)
            {
                return null;
            }

            var NewAdmin = new Admin()
            {
                PersonId = registeredPerson.PersonId,
                PharamcyId = adminRegisterDTO.PharmacyId
            };

            _context.Admins.Add(NewAdmin);

            if(await _context.SaveChangesAsync()<=0)
            {
                return null;
            }

            var pharmacyDTO = _context.Pharmacies.Where(p => p.PharmacyId == adminRegisterDTO.PharmacyId)
                                                .Select(p => new PharmacyDTO()
                                                {
                                                    PharmacyId = p.PharmacyId,
                                                    Name = p.Name,
                                                    ArabicName = p.ArabicName,
                                                    ImageURL = p.ImageURL
                                                })
                                                .FirstOrDefault();

            return TransferToAdminDTO(NewAdmin, registeredPerson, pharmacyDTO);
        }

        public async Task HandelManagerLoggedIn(int ManagerId)
        {
            var Manager = new Manager()
            {
                ManagerId = ManagerId,
                LastLoggedInTime = DateTime.Now
            };

            _context.Attach(Manager);
            _context.Entry(Manager).Property(P => P.LastLoggedInTime).IsModified = true;

            _context.SaveChanges();
        }

        public async Task<int> GetManagerIdByPersonId(int PersonId)
        {
            int ManagerId =await _context.Managers.Where(M => M.PersonId == PersonId)
                                                 .Select(M => M.ManagerId)
                                                 .FirstOrDefaultAsync();

            return ManagerId;
        }

        public async Task<bool> IsTheManagerOfPharmacy(int ManagerId,int PharmacyId)
        {
            bool IsTheManager =await _context.Managers.Where(M => M.ManagerId == ManagerId && M.PharmacyId == PharmacyId).AnyAsync();

            return IsTheManager;
        }

        public async Task<bool> DeActivateAdmin(int AdminId)
        {
            var Admin = new Admin()
            {
                AdminId = AdminId,
                IsActive = false
            };

            _context.Attach(Admin);
            _context.Entry(Admin).Property(P=>P.IsActive).IsModified = true;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActivateAdmin(int AdminId)
        {
            var Admin = new Admin()
            {
                AdminId = AdminId,
                IsActive = true
            };

            _context.Attach(Admin);
            _context.Entry(Admin).Property(P => P.IsActive).IsModified = true;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<AdminPreifInfoDTO>?> GetAdminList(int PharmacyId)
        {
            var admins =await _context.Admins.Where(A => A.PharamcyId == PharmacyId)
                                            .Select(A => new AdminPreifInfoDTO()
                                            {
                                                AdminId = A.AdminId,
                                                IsActive = A.IsActive,
                                                IsOnline = A.IsOnline,
                                                LastLoggedInTime = A.LastLoggedInTime,
                                                AdminName = A.Person.FirstName + "" + A.Person.LastName
                                            })
                                            .ToListAsync();

            return admins;
        }

        public async Task<List<AdminLoggsDTO>?> GetAdminsLoggsHistoryForSepcificDay(ShowAdminsLoggsDTO reqDTO)
        {
            var adminsLoggs =await _context.AdminLogs.Where(A => A.Admin.PharamcyId == reqDTO.PharmacyId
                                                     && A.DateOfLoggedIn >= reqDTO.DateOfLoggs.Date
                                                     &&A.DateOfLoggedIn<reqDTO.DateOfLoggs.AddDays(1))
                                                     .Select(A => new AdminLoggsDTO()
                                                     {
                                                         AdminId = A.AdminId,
                                                         DateOfLogged = A.DateOfLoggedIn,
                                                         IsLogut = A.IsLogout,
                                                         DateOfLogOut = A.DateOfLoggout
                                                     })
                                                     .ToListAsync();

            return adminsLoggs;
        }


        private class ManagerCheckInfo
        {
            public int ManagerId { get; set; }
            public string Password { get; set; }
            public bool IsActive { get; set; }
            public int? PharamcyId { get; set; }
            public string? PINCode { get; set; }
        }

    }
}
