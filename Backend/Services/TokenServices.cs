using DatabaseLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Services
{
    public class TokenServices
    {
        public enum eUserType {Customer=1,Admin=2,Manager=3,SysAmdin=4,Pre_Registered=5}

        private readonly Dictionary<eUserType, int> UsersValidateTokenHours = new  Dictionary<eUserType, int>()
        {
            {eUserType.Customer,4 },
            {eUserType.Pre_Registered,1}
        };

        private readonly AppDbContext _context;

        public TokenServices(AppDbContext context)
        {
           _context = context;
        }

        private  string GenerateToken()
        {
            return Guid.NewGuid().ToString();   
        }

        public async Task<(string,DateTime)> CreateNewToken(int UserId ,eUserType userType=eUserType.Customer)
        {
            string token = GenerateToken();

            DateTime ExpiredTime = DateTime.Now.AddHours(UsersValidateTokenHours[userType]);

            var NewToken = new Token()
            {
               UserTypeId= (int)userType,
               UserId= UserId,
               GeneratedToken=token,
               ExpiredDate=ExpiredTime,
            };

            _context.Tokens.Add(NewToken);
            await _context.SaveChangesAsync();

            return (token,ExpiredTime);
        }

        private async Task<bool> IsTokenCorrect(int UserId,eUserType UserType,string Token)
        {
            DateTime now = DateTime.Now;


            string? tokenFromDB =await  _context.Tokens.Where(t=>t.UserId==UserId && t.UserTypeId==(int)UserType && t.IsActive==true && t.ExpiredDate >=now)
                                                         .OrderByDescending(t=>t.TokenId)
                                                         .Select(t=>t.GeneratedToken)
                                                         .FirstOrDefaultAsync();
                                                  
            if(string.IsNullOrEmpty(tokenFromDB)) 
                return false;


            if(tokenFromDB !=Token)
                return false;


            return true;
        }

        public async Task<bool> CheckIfTokenCorrect(int UserId , eUserType UserType,string Token)
        {
            return await IsTokenCorrect(UserId, UserType, Token);
        }

    }
}
