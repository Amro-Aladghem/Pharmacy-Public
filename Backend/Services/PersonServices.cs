using DatabaseLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Security;
using DTOs.PersonDTOs;
using DTOs.AdminDTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Collections.Frozen;

namespace Services
{
    public  class PersonServices
    {
        private readonly AppDbContext _context;
        private readonly SecurityMethode _securityMethode;
        private readonly IConfiguration _configuration;

        public PersonServices(AppDbContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _securityMethode = new SecurityMethode(configuration);
        }

        private readonly string DefaultImageURL = "https://res.cloudinary.com/dlu3aolnh/image/upload/v1735059932/udjkg3nflplvydkrngf0.png";

        public async Task<string?> GetAdminPassword(int AdminId)
        {
            string? Password = await _context.Admins.Where(A => A.AdminId == AdminId)
                                                 .Select(A => A.Person.Password)
                                                 .FirstOrDefaultAsync();

            return Password;
        }

        public async Task<string?> GetManagerPassword(int ManagerId)
        {
            string? Password = await _context.Managers.Where(M => M.ManagerId == ManagerId)
                                                .Select(M => M.Person.Password)
                                                .FirstOrDefaultAsync();

            return Password;
        }

        public async Task<RegisterdPersonDTO?> AddPreRegisterdInfo(PersonPreRegisterDTO preRegisterDTO)
        {

            string EncryptedPassword = _securityMethode.Encrypt(preRegisterDTO.Password);

            var NewPerson = new Person()
            {
                Email = preRegisterDTO.Email,
                Password = EncryptedPassword,
            };

            _context.Persons.Add(NewPerson);

            if (await _context.SaveChangesAsync() <= 0)
            {
                return null;
            }

            return new RegisterdPersonDTO()
            {
                PersonId = NewPerson.PersonId,
                Email = NewPerson.Email
            };
        }

        public async Task<Person?> AddFullRegisterInfo(PersonRegisterDTO registerDTO)
        {
            if (registerDTO.Phone.StartsWith("0"))
                return null;

            var registeredPerson = await _context.Persons.FirstOrDefaultAsync(P => P.PersonId == registerDTO.PersonId);

            if (registeredPerson is null || registeredPerson.IsVerified)
            {
                return null;
            }


            string? ImageURL = registerDTO.ProfileImageLink;

            registeredPerson.FirstName = registerDTO.FirstName;
            registeredPerson.LastName = registerDTO.LastName;
            registeredPerson.Phone = "0"+registerDTO.Phone;

            registeredPerson.ProfileImageURL = ImageURL is null ? DefaultImageURL : ImageURL;
            registeredPerson.IsVerified = true;

            return registeredPerson;
        }

        public async Task<bool> IsPasswordCorrect(int UserId,TokenServices.eUserType userType,string Password)
        {
            string? passwordFromDb = userType switch
            {
                TokenServices.eUserType.Admin => await GetAdminPassword(UserId),
                TokenServices.eUserType.Manager => await GetManagerPassword(UserId),
                _ => null

            };

            if (string.IsNullOrEmpty(passwordFromDb))
                return false;

            if (!_securityMethode.VerifyEncryptPassword(passwordFromDb, Password))
                return false;

            return true;
        }

        public async Task<int> FindPersonIdByEmail(string email)
        {
            int PersonId = await _context.Persons.Where(M => M.Email == email)
                                          .Select(M=>M.PersonId)
                                          .FirstOrDefaultAsync();

            return PersonId;
        }

        public async Task<bool> MakePersonVerfied(int PersonId)
        {
            var person = new Person()
            {
                PersonId = PersonId,
                IsVerified = true,
            };

            _context.Attach(person);
            _context.Entry(person).Property(P => P.IsVerified).IsModified = true;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsPersonVerfied(int PersonId)
        {
            bool IsVerfied = await _context.Persons.Where(P => P.PersonId == PersonId && P.IsVerified == true).AnyAsync();

            return IsVerfied;
        }
    }
}
