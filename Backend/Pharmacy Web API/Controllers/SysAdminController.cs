using DTOs.SysAdminDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Pharmacy_Web_API.Controllers
{
    [Route("api/v1/SysAdmin")]
    [ApiController]
    public class SysAdminController : ControllerBase
    {
        public readonly SystemAdminServices _systemAdminServices;
        public readonly TokenServices _tokenServices;

        public SysAdminController(SystemAdminServices systemAdminServices, TokenServices tokenServices)
        {
            _systemAdminServices = systemAdminServices;
            _tokenServices = tokenServices;
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(SysAdminLoginDTO loginDTO)
        {
            if (string.IsNullOrEmpty(loginDTO.Password) || string.IsNullOrEmpty(loginDTO.Email) || string.IsNullOrEmpty(loginDTO.PINcode))
                return BadRequest("Invalied or empty values!");

            try
            {
                int SysAmdinId = await _systemAdminServices.CheckLoginInfo(loginDTO);

                //var token  = _tokenServices.CreateNewToken((int)TokenServices.eUserType.SysAmdin,SysAmdinId);

                return Ok(new {SysAmdinId}); 
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }


    }
}
