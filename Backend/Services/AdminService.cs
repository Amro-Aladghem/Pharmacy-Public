using DatabaseLayer.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Security;
using DTOs.AdminDTOs;
using DatabaseLayer.Migrations;
using DTOs.PersonDTOs;
using DTOs.PharamacyDTOs;
using Microsoft.EntityFrameworkCore;
using DTOs.CustomerDTOs;

namespace Services
{
    public class AdminService
    {

        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly SecurityMethode _securityMethode;
        private readonly PersonServices _personServices;

        public AdminService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _securityMethode = new SecurityMethode(configuration);
            _personServices = new PersonServices(context,configuration);
        }

        string DefaultImageURL = "";

        private async Task<PharmacyDTO?> ReturnPharmacyInfoByAdminId(int AdminId)
        {
            PharmacyDTO? pharmacy = await _context.Admins.Where(a => a.AdminId == AdminId)
                                                           .Select(a => new PharmacyDTO()
                                                           {
                                                               PharmacyId = a.PharamcyId,
                                                               Name = a.pharmacy.Name,
                                                               ArabicName = a.pharmacy.ArabicName,
                                                               ImageURL = a.pharmacy.ImageURL

                                                           }).FirstOrDefaultAsync();

            return pharmacy;
        }

        private async Task<AdminDTO?> GetAdminInfoByAdminId(int AdminId)
        {
            var adminDTO = await _context.Admins.Where(a => a.AdminId == AdminId)
                                                  .Select(a => new AdminDTO()
                                                  {
                                                      AdminId = a.AdminId,
                                                      Person = new PersonDTO()
                                                      {
                                                          FirstName = a.Person.FirstName,
                                                          LastName = a.Person.LastName,
                                                          ProfileImageLink = a.Person.ProfileImageURL
                                                      },
                                                      Pharmacy = new PharmacyDTO()
                                                      {
                                                          Name = a.pharmacy.Name,
                                                          ArabicName = a.pharmacy.ArabicName,
                                                          ImageURL = a.pharmacy.ImageURL,
                                                          PharmacyId = a.PharamcyId
                                                      },
                                                      IsOnline = true

                                                  }).FirstOrDefaultAsync();
            return adminDTO;
        }

        public async  Task<AdminDTO?> CheckAdminLoginInfo(AdminLoginDTO adminLoginDTO)
        {
            var adminAndPassword = await _context.Admins.Where(a => a.Person.Email == adminLoginDTO.Email)
                                                       .Select(a => new AdminAndPassowrd()
                                                       {
                                                           AdminId = a.AdminId,
                                                           IsActive = a.IsActive,
                                                           Password = a.Person.Password,
                                                           PINCode = a.pharmacy.PINCode
                                                       })
                                                       .FirstOrDefaultAsync();

            if(adminAndPassword==null)
            {
                return null;
            }

            if(!adminAndPassword.IsActive)
            {
                throw new Exception("Your Account is DeActivate!");
            }

            if(adminAndPassword.PINCode!=adminLoginDTO.PINCode)
            {
                throw new Exception("The PIN Is not correct!");
            }

            if(!_securityMethode.VerifyEncryptPassword(adminAndPassword.Password,adminLoginDTO.Password))
            {
                throw new Exception("The Passowrd is not correct!");
            }

            var adminDTO = await GetAdminInfoByAdminId(adminAndPassword.AdminId);

            return adminDTO;
        }

        public async Task HandelAdminLoggedIn(int AdminId)
        {
            var admin = _context.Admins.Where(a => a.AdminId == AdminId).FirstOrDefault();

            if (admin == null)
            {
                throw new Exception("No admin with this Id !");
            }

            admin.IsOnline = true;
            admin.LastLoggedInTime = DateTime.Now;

            var NewAdminLog = new AdminLog()
            {
                AdminId = admin.AdminId
            };

            _context.AdminLogs.Add(NewAdminLog);

            await _context.SaveChangesAsync();
        }

        private async Task<Person?> UpdatePersonInfo(UpdateAdminDTO updateAdminDTO)
        {

            var admin = await _context.Admins.Include(A => A.Person).FirstOrDefaultAsync(A => A.AdminId == updateAdminDTO.AdminId);

            if (admin is null || admin.Person is null)
                return null;


            admin.Person.FirstName = updateAdminDTO.Person.FirstName;
            admin.Person.LastName = updateAdminDTO.Person.LastName;
            admin.Person.Phone = updateAdminDTO.Person.Phone;

            admin.Person.ProfileImageURL = admin.Person.ProfileImageURL is null? DefaultImageURL : admin.Person.ProfileImageURL;
            

            if(await _context.SaveChangesAsync()<=0)
            {
                return null;
            }

            return admin.Person;
        }

        public async Task<AdminDTO?> UpdateAdminInfo(UpdateAdminDTO updateAdminDTO)
        {

            var pharamacyDTO =await ReturnPharmacyInfoByAdminId(updateAdminDTO.AdminId);


            if(pharamacyDTO is null)
            {
                throw new ArgumentNullException("No Admin with this Id!");
            }

            var person =await UpdatePersonInfo(updateAdminDTO);

            if(person is null)
            {
                throw new NullReferenceException("The Updated Person Info was faild!");
            }

            return new AdminDTO()
            {
                AdminId = updateAdminDTO.AdminId,
                Person = new PersonDTO()
                {
                    FirstName = updateAdminDTO.Person.FirstName,
                    LastName = updateAdminDTO.Person.LastName,
                    ProfileImageLink = updateAdminDTO.Person.ProfileImageLink,
                },
                Pharmacy=pharamacyDTO
            };
        }

        public async Task<bool> ChangeEmail(int AdminId , string NewEmail)
        {
            var admin = await _context.Admins.Include(a=>a.Person)
                                           .Where(a=>a.AdminId==AdminId)
                                           .FirstOrDefaultAsync();

            if(admin is null || admin.Person is null)
            {
                throw new Exception("No admin with this Id!");
            }

            admin.Person.Email = NewEmail;

            return await _context.SaveChangesAsync() > 0; ;
        }

        public async Task<bool> ChangePassword(int AdminId,string NewPassword)
        {
            var admin = await _context.Admins.Include(a => a.Person)
                                              .Where(a => a.AdminId == AdminId)
                                              .FirstOrDefaultAsync();

            if (admin is null || admin.Person is null)
            {
                throw new Exception("No admin with this Id!");
            }

            string EncryptPassword = _securityMethode.Encrypt(NewPassword);

            admin.Person.Password = EncryptPassword;
            
            return await _context.SaveChangesAsync() > 0; 
        }

        public async Task<ShowAdminDTO?> GetAdminInfo(int AdminId)
        {
            var admin =await _context.Admins.Where(A => A.AdminId == AdminId)
                                           .Select(A => new ShowAdminDTO()
                                           {
                                               AdminId = A.AdminId,
                                               Person = new ShowPersonDTO()
                                               {
                                                   FirstName = A.Person.FirstName,
                                                   LastName = A.Person.LastName,
                                                   Email = A.Person.Email,
                                                   Phone = A.Person.Phone
                                               },
                                               PharmacyImage = A.pharmacy.ImageURL,
                                               PharmacyName = A.pharmacy.Name
                                           })
                                           .FirstOrDefaultAsync();

            return admin;
        }


        private class AdminAndPassowrd
        {
            public int AdminId { get; set; }
            public string Password { get; set; }
            public bool IsActive { get; set; }
            public int PharamcyId { get; set; }
            public string ? PINCode {get;set;}
        }
    }
}
