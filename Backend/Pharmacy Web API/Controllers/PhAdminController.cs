using DatabaseLayer.Entities;
using DTOs.AdminDTOs;
using DTOs.CustomerDTOs;
using DTOs.ReqestDTOs;
using DTOs.RequestDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Pharmacy_Web_API.Controllers
{
    [Route("api/v1/PhAdmin")]
    [ApiController]
    public class PhAdminController : ControllerBase
    {
        private readonly TokenServices _tokenServices;
        private readonly AdminService _adminServices;
        private readonly EmailVerficationService _emailVerficationService;
        private readonly PersonServices _personServices;
        private readonly FileUploadService _fileUploadService;

        public PhAdminController(TokenServices tokenServices, AdminService adminServices,
            EmailVerficationService emailVerficationService, PersonServices personServices, FileUploadService fileUploadService)
        {
            _tokenServices = tokenServices;
            _adminServices= adminServices;
            _emailVerficationService = emailVerficationService;
            _personServices = personServices;
            _fileUploadService = fileUploadService;
        }


        [HttpPost("Login",Name ="AdminLogin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(AdminLoginDTO adminLoginDTO)
        {
            if(string.IsNullOrEmpty(adminLoginDTO.PINCode)||string.IsNullOrEmpty(adminLoginDTO.Email)
                ||string.IsNullOrEmpty(adminLoginDTO.Email))
            {
                return BadRequest("Invalied Or empty value!");
            }

            try
            {
                AdminDTO? adminDTO =await _adminServices.CheckAdminLoginInfo(adminLoginDTO);

                if (adminDTO is null)
                    return Unauthorized("Wrong Email/Password try again!");

                await _adminServices.HandelAdminLoggedIn(adminDTO.AdminId);

                //string token = _tokenServices.CreateNewToken((int)TokenServices.eUserType.Admin, adminDTO.AdminId);

                //Response.Cookies.Append("AuthToken", token, new CookieOptions()
                //{
                //    HttpOnly = true,
                //    Secure = true,
                //    SameSite = SameSiteMode.Strict,
                //});

                return Ok(new {adminDTO});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }

        }

        [HttpPost("Update", Name = "Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateInfo(UpdateAdminDTO updateAdminDTO, IFormFile? Image)
        {
            if (string.IsNullOrEmpty(updateAdminDTO.Person.FirstName) || string.IsNullOrEmpty(updateAdminDTO.Person.LastName)
                || string.IsNullOrEmpty(updateAdminDTO.Person.Phone))
            {
                return BadRequest("Invalied or empty values!");
            }

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is invalid or missing");
            }

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(updateAdminDTO.AdminId, TokenServices.eUserType.Admin, token))
                    return Unauthorized("Token is invalid and not correct!");


                if (Image is not null)
                {
                    using (var stream = Image.OpenReadStream())
                    {
                        updateAdminDTO.Person.ProfileImageLink =
                            await _fileUploadService.UploadImageAsync(stream, Image.FileName);
                    }
                }

                AdminDTO? adminDTO=await _adminServices.UpdateAdminInfo(updateAdminDTO);

                if (adminDTO is null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Faild To Update Info!");

                return Ok(new { adminDTO });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost("sendcode",Name ="sendCodeForChangeEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendVerficationCodeForChangingEmail(int AdminId)
        {
            if (AdminId <= 0)
                return BadRequest("Invalid Ph Admin Id!");


            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is invalid or missing");
            }

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(AdminId, TokenServices.eUserType.Admin, token))
                    return Unauthorized("Token is invalid and not correct!");

                if ( _emailVerficationService.IsUserHasAnActiveCode(AdminId, TokenServices.eUserType.Admin))
                    return BadRequest("You have already an active code!");

                if (!_emailVerficationService.SendVerificationCodeToEmailProcess(AdminId, TokenServices.eUserType.Admin))
                    return StatusCode(StatusCodes.Status500InternalServerError, "Faild to send code!");

                return Ok(new { result = true });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Faild to send code!");
            }
        }


        [HttpPut("changemail",Name ="AdminChangeEmail")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CheckVerficationCodeAndUpdateEmail(ChangeEmailRequestDTO changeEmailRequestDTO)
        {
            if(string.IsNullOrEmpty(changeEmailRequestDTO.NewEmail)||string.IsNullOrEmpty(changeEmailRequestDTO.VerficationCode)||
                    changeEmailRequestDTO.UserId<=0)
            {
                return BadRequest("Invalid values!");
            }

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is invalid or missing");
            }
            
            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(changeEmailRequestDTO.UserId, TokenServices.eUserType.Admin, token))
                    return Unauthorized("Token is invalid and not correct!");

                EmailVerficationResultDTO result = _emailVerficationService.CheckVeficationCodeFromUser(changeEmailRequestDTO.UserId, token);

                if(result.IsRightCode)
                {
                    if(!await _adminServices.ChangeEmail(changeEmailRequestDTO.UserId, changeEmailRequestDTO.NewEmail))
                    {
                        throw new Exception("The code is correct , but faild to update please try again!");
                    }
                }

                return Ok(new { result });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut("changepassword",Name ="ChangePassword")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangePasswordInLoggedIn(ChangePasswordReqDTO reqDTO)
        {
            if (reqDTO.UserId <= 0 || string.IsNullOrEmpty(reqDTO.currentPassowrd) || string.IsNullOrEmpty(reqDTO.NewPassword))
                return BadRequest("Invalid or empty value!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is invalid or missing");
            }

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(reqDTO.UserId, TokenServices.eUserType.Admin, token))
                    return Unauthorized("Token is invalid and not correct!");

                if (!await _personServices.IsPasswordCorrect(reqDTO.UserId, TokenServices.eUserType.Admin, reqDTO.currentPassowrd))
                    return NotFound("Password is not correct!");

                if (await _adminServices.ChangePassword(reqDTO.UserId, reqDTO.NewPassword))
                    return StatusCode(StatusCodes.Status500InternalServerError, "Faild to update password!");

                return Ok(new { message = true });

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("Info",Name ="GetInfo")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAdminInfo(int AdminId)
        {
            if (AdminId <= 0)
                return BadRequest("AdminId is Invalied!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is invalid or missing");
            }

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(AdminId, TokenServices.eUserType.Admin, token))
                    return Unauthorized("Token is invalid and not correct!");

                ShowAdminDTO? admin = await _adminServices.GetAdminInfo(AdminId);

                if (admin is null)
                    return NotFound("No Admin with this Id!");

                return Ok(new { admin });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Faild to get Info From Server!");
            }
        }






    }
}
