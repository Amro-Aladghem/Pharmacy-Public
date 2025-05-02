using DatabaseLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Security;
using DTOs.SysAdminDTO;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class SystemAdminServices
    {
        private readonly AppDbContext _context;
        private readonly SecurityMethode _securityMethode;
        private readonly IConfiguration _configuration;
        public SystemAdminServices(AppDbContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _securityMethode = new SecurityMethode(configuration);
        }
        public async Task<int> CheckLoginInfo(SysAdminLoginDTO loginDTO)
        {
            var SysAmdin = await _context.SystemAdmins.Where(SA => SA.Email == loginDTO.Email).FirstOrDefaultAsync();

            if (SysAmdin is null)
                throw new Exception("No Admin with this Email");

            if (SysAmdin.PINcode != loginDTO.PINcode)
                throw new Exception("PIN Code is not correct!");

            if (!_securityMethode.VerifyEncryptPassword(SysAmdin.PINcode, loginDTO.Password))
                throw new Exception("Password/Email is not correct");

            return SysAmdin.AdminId;
        }



        



    }
}
