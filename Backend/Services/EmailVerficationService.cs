using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using DatabaseLayer.Entities;
using Microsoft.Extensions.Configuration;
using static Services.TokenServices;
using System.Reflection.Metadata.Ecma335;
using DTOs.ReqestDTOs;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class EmailVerficationService
    {

        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        private readonly int MaxAttempt = 3;

        public EmailVerficationService(AppDbContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private UserNameAndEmail? GetCustomerInfo(int UserId)
        {
           UserNameAndEmail? userNameAndEmail = _context.Customers.Where(c => c.CutomerId == UserId)
                                                                    .Select(c => new UserNameAndEmail()
                                                                    {
                                                                        Email = c.Person.Email,
                                                                        FirstName = c.Person.FirstName,
                                                                        LastName = c.Person.LastName

                                                                    }).FirstOrDefault();

            return userNameAndEmail;
        }

        private UserNameAndEmail? GetAdminInfo(int UserId)
        {
            UserNameAndEmail? userNameAndEmail = _context.Admins.Where(c => c.AdminId == UserId)
                                                                    .Select(c => new UserNameAndEmail()
                                                                    {
                                                                        Email = c.Person.Email,
                                                                        FirstName = c.Person.FirstName,
                                                                        LastName = c.Person.LastName

                                                                    }).FirstOrDefault();

            return userNameAndEmail;
        }

        private UserNameAndEmail? GetManagerInfo(int UserId)
        {
            UserNameAndEmail? userNameAndEmail = _context.Managers.Where(c => c.ManagerId == UserId)
                                                                        .Select(c => new UserNameAndEmail()
                                                                        {
                                                                            Email = c.Person.Email,
                                                                            FirstName = c.Person.FirstName,
                                                                            LastName = c.Person.LastName

                                                                        }).FirstOrDefault();

            return userNameAndEmail;
        }

        private UserNameAndEmail? GetPersonInfo(int PersonId)
        {
            UserNameAndEmail? person = _context.Persons.Where(P => P.PersonId == PersonId)
                                                       .Select(P => new UserNameAndEmail()
                                                       {
                                                           Email = P.Email,
                                                           FirstName="",
                                                           LastName=""
                                                       })
                                                       .FirstOrDefault();

            return person;
        }

        private string GenerateVerificationcCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private bool SaveEmailVerficationRecordToDb(string code,int? UserTypeId,int UserId)
        {
            var EmailVerfication = new EmailVerification
            {
                Code = code,
                RefferenceId = UserId,
                UserTypeId = UserTypeId,
            };

            _context.EmailVerifications.Add(EmailVerfication);
            _context.SaveChanges();

            return true;
        }

        private bool SendEmailMessage(string Subject, string Body, string EmailTo)
        {
            try
            {

                string smtpAddress = "smtp.gmail.com";
                int portNumber = 587;
                bool enableSSL = true;

                string emailFrom = "amerhk681@gmail.com";
                string password = _configuration.GetSection("MailPassword").Value;
                string emailTo = EmailTo;
                string subject = Subject;
                string body = Body;

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFrom);
                    mail.To.Add(emailTo);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = false;
                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(emailFrom, password);
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SendVerificationCodeToEmailProcess(int UserId ,TokenServices.eUserType? userType)
        {

            UserNameAndEmail? userNameAndEmail = userType switch
            {
                TokenServices.eUserType.Customer => GetCustomerInfo(UserId),
                TokenServices.eUserType.Admin => GetAdminInfo(UserId),
                TokenServices.eUserType.Manager => GetManagerInfo(UserId),
                _ => GetPersonInfo(UserId)
            };

            if(userNameAndEmail==null)
            {
                return false;
            }

            string verficationCode = GenerateVerificationcCode();

            string subject = "Verification Code";
            string body = $"Dear,{userNameAndEmail.FullName}\n This is your code: {verficationCode}";
            string Email = userNameAndEmail.Email;


            if (!SendEmailMessage(subject, body,Email))
            {
                return false;
            }

            int? userTypeId = userType is null ? null : (int)userType;

            return SaveEmailVerficationRecordToDb(verficationCode, userTypeId, UserId);
        }

        public void IncrementAttemptValue(EmailVerification emailVerificationInstance)
        {
            emailVerificationInstance.AttemptCount++;

            _context.SaveChanges();
        }

        public EmailVerficationResultDTO  CheckVeficationCodeFromUser(int UserId,string UserCode)
        {
            var EmailVerfication = _context.EmailVerifications.Where(e => e.DateOfCreated == DateOnly.FromDateTime(DateTime.Now) 
                                                               && e.RefferenceId == UserId)
                                                              .OrderByDescending(e => e.VerificationId)
                                                              .FirstOrDefault();


            var result = new EmailVerficationResultDTO { IsRightCode = false, Isblocked = false, message="" };

            if (EmailVerfication==null)
            {
                result.message = "No Verfication Code for this User!";
                return result;
            }

            if (EmailVerfication.AttemptCount >= MaxAttempt)
            {

                result.message = "you have exeeds 3 times tried, please try again after 3 minuts";
                result.Isblocked = true;
                return result;
            }

            if (TimeOnly.FromDateTime(DateTime.Now)>EmailVerfication.TimeOfExpired)
            {
                result.message = "This code is expired!";
                return result;
            }

            if(EmailVerfication.Code!=UserCode)
            {
                IncrementAttemptValue(EmailVerfication);
                result.message = "Wrong Code!";
                return result;
            }

            result.message = "Write Code!";
            result.IsRightCode= true;

            return result;
        }

        public async Task<bool> IsEmailExistInDb(string email)
        {
            var emailChecker = await _context.Persons.AnyAsync(p => p.Email == email);

            return emailChecker;
        }

        private bool CheckEmailSyntax(string Email)
        {
            return MailAddress.TryCreate(Email, out _);
        }

        private bool IsValiedDomain(string Email)
        {
            string[] Domains = new string[] { "gmail.com", "yahoo.com", "outlook.com" };

            string emailDomain = Email.Split('@')[1];

            return Domains.Contains(emailDomain);
        }

        public bool CheckIfEmailIsValied(string Email,bool CheckDomain=true)
        {
            if (!CheckEmailSyntax(Email))
                return false;

            if(CheckDomain)
            {
                if (!IsValiedDomain(Email))
                    return false;
            }

            return true;
        }

        public bool IsUserHasAnActiveCode(int UserId,TokenServices.eUserType userType)
        {
            bool IsExists = _context.EmailVerifications.Where(E => E.RefferenceId == UserId && E.UserTypeId == (int)eUserType.Admin
                                                       && E.TimeOfExpired > TimeOnly.FromDateTime(DateTime.Now))
                                                       .Any();

            return IsExists;
        }

        private class UserNameAndEmail 
        {
           public string? FirstName { get; set; }
           public string? LastName { get; set; }    
           public string Email { get; set; }    
           public string? FullName =>FirstName+" " + LastName;
        }



    }
}
