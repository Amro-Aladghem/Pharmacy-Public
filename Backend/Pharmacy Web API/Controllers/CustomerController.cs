using DatabaseLayer.Entities;
using DTOs.CustomerDTOs;
using DTOs.PersonDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using DTOs.RequestDTOs;
using System.Runtime.CompilerServices;

namespace Pharmacy_Web_API.Controllers
{
    [Route("api/v1/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly TokenServices _tokenServices;
        private readonly CustomerServices _customerServices;
        private readonly EmailVerficationService _emailVerficationService;
        private readonly PersonServices _personServices;
        private readonly FileUploadService _fileUploadService;

        public CustomerController(TokenServices tokenServices, CustomerServices customerServices,
            EmailVerficationService emailVerficationService,PersonServices personServices, FileUploadService fileUploadService)
        {
            _tokenServices = tokenServices;
            _customerServices = customerServices;
            _emailVerficationService = emailVerficationService;
            _personServices = personServices;
            _fileUploadService = fileUploadService;
        }

        [HttpPost("Login",Name ="Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromForm]CustomerLoginDTO LoginDTO)
        {

            if (string.IsNullOrEmpty(LoginDTO.Email) || string.IsNullOrEmpty(LoginDTO.Password))
            {
                return BadRequest(new { message = "Missing Email Or Password!" });
            }

            
            try
            {
                CustomerDTO? customer = await _customerServices.CheckEmailAndPassword(LoginDTO.Email, LoginDTO.Password);

                if(customer == null)
                {
                    return Unauthorized(new { message = "Wrong Email/Password Try Again!" });
                }

                await _customerServices.HandelCustomerLoggedIn(customer.CustomerId);

                
                (string token,DateTime ExpiredTokenTime) = await _tokenServices.CreateNewToken(customer.CustomerId,TokenServices.eUserType.Customer);

                Response.Cookies.Append("AuthToken", token, new CookieOptions()
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddHours(4)
                });

                return Ok(new {customer,ExpiredTokenTime});

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }

        }

        [HttpPost("PreRegister",Name ="CustomerPreRegister")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PreRegister([FromForm]PersonPreRegisterDTO personPreRegisterDTO)
        {
            if(string.IsNullOrEmpty(personPreRegisterDTO.Password)||string.IsNullOrEmpty(personPreRegisterDTO.Email))
            {
                return BadRequest(new { message = "Email or Password is empty!" });
            }

            if(!_emailVerficationService.CheckIfEmailIsValied(personPreRegisterDTO.Email))
            {
                return BadRequest(new { message = "Invalied Email!" });
            }

            if(await _emailVerficationService.IsEmailExistInDb(personPreRegisterDTO.Email))
            {
                return BadRequest(new {message="this email has been taken"});
            }

            try
            {
                RegisterdPersonDTO? registerdPersonDTO= await _personServices.AddPreRegisterdInfo(personPreRegisterDTO);

                if(registerdPersonDTO is null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Faild to Pre Register!" });


                (string token, DateTime ExpiredTokenTime) = await _tokenServices.CreateNewToken(registerdPersonDTO.PersonId, TokenServices.eUserType.Customer);

                Response.Cookies.Append("AuthToken", token, new CookieOptions()
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddHours(4)
                });


                return Ok(new { registerdPersonDTO, ExpiredTokenTime });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Faild to Pre Register!" });
            }

        }

        [HttpPost("Register",Name ="CustomerRegister")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromForm]CustomerRegisterDTO customerRegisterDTO, IFormFile? Image)
        {
            if(!_customerServices.IsValiedValues(customerRegisterDTO))
            {
                return BadRequest(new { message = "Some values is empty!" });
            }

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is invalid or missing");
            }


            try
            {

                if (!await _tokenServices.CheckIfTokenCorrect(customerRegisterDTO.PersonRegister.PersonId, TokenServices.eUserType.Pre_Registered, token))
                    return Unauthorized("Token is invalid and not correct!");

                if (Image is not null)
                {
                    using (var stream = Image.OpenReadStream())
                    {
                        customerRegisterDTO.PersonRegister.ProfileImageLink = 
                            await _fileUploadService.UploadImageAsync(stream, Image.FileName);
                    }
                }

                CustomerDTO? customerDTO= await _customerServices.AddNewCustomer(customerRegisterDTO);

                if(customerDTO is null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Faild To Add Customer!" });
                }

                await _customerServices.HandelCustomerLoggedIn(customerDTO.CustomerId);


                (string newtoken, DateTime ExpiredTokenTime) = await _tokenServices.CreateNewToken(customerDTO.CustomerId, TokenServices.eUserType.Customer);


                Response.Cookies.Append("AuthToken", newtoken, new CookieOptions()
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                });

                return Created("", new {data=customerDTO, ExpiredTokenTime });

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }

        }

        [HttpPut("ChangeEmail",Name ="CustomerChangeEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeEmail(ChangeEmailDTO changeEmailDTO)
        {
            if(changeEmailDTO.UserId<=0 || string.IsNullOrEmpty(changeEmailDTO.NewEmail))
            {
                return BadRequest(new { message = "Invalied data!" });
            }

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is invalid or missing");
            }

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(changeEmailDTO.UserId, TokenServices.eUserType.Customer, token))
                    return Unauthorized("Token is invalid and not correct!");

                if (!await _customerServices.ChangeEmail(changeEmailDTO))
                    return StatusCode(StatusCodes.Status500InternalServerError, "Faild to Update Email!");

                return Ok(new { result = true });
            }
            catch(Exception ex )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut("Update",Name ="CustomerUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCustomerInfo([FromForm]UpdateCustomerDTO updateCustomerDTO)
        {

            if (string.IsNullOrEmpty(updateCustomerDTO.Person.FirstName) || string.IsNullOrEmpty(updateCustomerDTO.Person.LastName) ||
                string.IsNullOrEmpty(updateCustomerDTO.Person.Phone) )
            {
                return BadRequest(new { message = "Invalid or empty data!" });
            }

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is invalid or missing");
            }

            try
            {

                if (!await _tokenServices.CheckIfTokenCorrect(updateCustomerDTO.CustomerId, TokenServices.eUserType.Customer, token))
                    return Unauthorized("Token is invalid and not correct!");

                if (await _customerServices.IsCustomerHasActiveOrder(updateCustomerDTO.CustomerId))
                    return BadRequest("You have Active Order , you can't change Info!");
                

                CustomerDTO? customerDTO = await _customerServices.UpdateCustomerInfo(updateCustomerDTO);

                if (customerDTO is null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Faild to Update Info");

                return Ok(new { customerDTO });

            }
            catch(Exception ex )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("Show/{CustomerId}",Name ="CustomerShow")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ShowCustomerInfo(int CustomerId)
        {
            if(CustomerId<=0)
            {
                return BadRequest("Invalid Customer Id!");
            }

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is invalid or missing");
            }

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(CustomerId, TokenServices.eUserType.Customer, token))
                    return Unauthorized("Token is invalid and not correct!");

                ShowCustomerDTO? customer = await _customerServices.GetCustomerInfo(CustomerId);
                
                if(customer is null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Server Error!");

                return Ok(new { customer });

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error!");
            }

        }

        [HttpGet("logged/status/{CustomerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> CheckCustomerLoggedStatus(int CustomerId)
        {
            if (CustomerId <= 0)
                return BadRequest("Invalied or empty values!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is invalid or missing");
            }

            try
            {
                bool IsLogged = await _tokenServices.CheckIfTokenCorrect(CustomerId, TokenServices.eUserType.Customer, token);

                return Ok(new { IsLogged });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status203NonAuthoritative, new { message = ex.Message });
            }
        }

        [HttpPut("{CustomerId}/location/edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> EditLoaction(int CustomerId,[FromBody]EditLocationDTO editLocationDTO)
        {
            if(CustomerId<0 || editLocationDTO.Latitude<=0 || editLocationDTO.Longitude<=0)
            {
                return BadRequest(new { message = "Invalied or empty values!" });
            }

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is invalid or missing");
            }

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(CustomerId, TokenServices.eUserType.Customer, token))
                    return Unauthorized("Token is invalid and not correct!");

                bool isDone = await _customerServices.ChnageLocation(CustomerId, editLocationDTO);

                return Ok(new { isDone });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "server error!" });
            }
        }


        // Change Password code ....



    }
}
