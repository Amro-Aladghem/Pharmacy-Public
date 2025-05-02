using Azure.Core;
using DatabaseLayer.Entities;
using DTOs.CustomerDTOs;
using DTOs.DefaultValuesDTOs;
using DTOs.PharamacyDTOs;
using DTOs.PharmacyDTOs;
using DTOs.RequestDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.IO;
using System.Reflection.Metadata.Ecma335;

namespace Pharmacy_Web_API.Controllers
{
    [Route("api/v1/pharmacy")]
    [ApiController]
    public class PharmacyController : ControllerBase
    {
        private readonly TokenServices _tokenServices;
        private readonly ManagerServices _managerServices;
        private readonly EmailVerficationService _emailVerficationService;
        private readonly FileUploadService _fileUploadService;
        private readonly PharmacyServices _pharmacyServices;

        public PharmacyController(TokenServices tokenServices, ManagerServices managerServices,
            EmailVerficationService emailVerficationService, FileUploadService fileUploadService, PharmacyServices pharmacyServices)
        {
            _tokenServices = tokenServices;
            _managerServices = managerServices;
            _emailVerficationService = emailVerficationService;
            _fileUploadService = fileUploadService;
            _pharmacyServices = pharmacyServices;
        }

        [HttpPost("addNew",Name="AddNew")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddNewPharmacy(int ManagerId, [FromForm] PharmacyInfoDTO InfoDTO, IFormFile? Image)
        {
            if (!_pharmacyServices.AreValuesCorrect(InfoDTO))
                return BadRequest("Some values are Invalied or empty!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Token is expired or Invalied!");

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(ManagerId, TokenServices.eUserType.Manager, token))
                    return Unauthorized("Token is invalid and not correct!");

                if (Image is not null)
                {
                    using (var stream = Image.OpenReadStream())
                    {
                        InfoDTO.ImageURL = await _fileUploadService.UploadImageAsync(stream, Image.FileName);
                    }
                }

                string? PINCode =await _pharmacyServices.AddNewPharamcy(InfoDTO, ManagerId);

                if(PINCode is null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Faild to add Pharmacy try again");

                return Ok(new { PINCode });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Faild to add Pharmacy try again");
            }
        }

        [HttpGet("general/info",Name ="GeneralInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ShowGeneralInfo(int ManagerId,int PharmacyId)
        {
            if (ManagerId <= 0 || PharmacyId <= 0)
                return BadRequest("Invalied values!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied or empty token!");

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(ManagerId, TokenServices.eUserType.Manager, token))
                    return Unauthorized("Token is invalid and not correct!");

                if (!await _pharmacyServices.IsPharmacyHasThisUser(ManagerId,TokenServices.eUserType.Manager ,PharmacyId))
                    return Unauthorized("You are not the manager");

                PharmacyGeneralInfoDTO? pharmacyGeneralInfo= await _pharmacyServices.ShowGeneralInfo(PharmacyId);

                if (pharmacyGeneralInfo is null)
                    return NotFound("Faild to get data,please try again!");

                return Ok(new { pharmacyGeneralInfo });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("profile/info", Name = "ProfileInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ShowProfileInfo(int PharmacyId)
        {
            if (PharmacyId <= 0)
                return BadRequest("Invalied Id!");

            try
            {
                PharmacyDTO? pharmacyDTO = await _pharmacyServices.ShowProfileInfo(PharmacyId);

                if (pharmacyDTO is null)
                    return NotFound("No Pharmacy with this Id!");

                return Ok(new { pharmacyDTO });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        [HttpPut("update/generalInfo",Name ="UpdateGeneralInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateGeneralInfo(int ManagerId, [FromForm] PharmacyGeneralInfoDTO generalInfoDTO)
        {
            if(ManagerId<=0||generalInfoDTO.PharamcyId<=0||string.IsNullOrEmpty(generalInfoDTO.StreetName)
               ||string.IsNullOrEmpty(generalInfoDTO.Phone)||generalInfoDTO.GovernateId<=0||generalInfoDTO.CountryId<=0
               ||generalInfoDTO.Longitude<=0||generalInfoDTO.Latitude<=0)
            {
                return BadRequest("Invalied or empty values!");
            }

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied token!");

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(ManagerId, TokenServices.eUserType.Manager, token))
                    return Unauthorized("Token is invalid and not correct!");

                if (!await _pharmacyServices.IsPharmacyHasThisUser(ManagerId, TokenServices.eUserType.Manager, generalInfoDTO.PharamcyId))
                    return Unauthorized("You are not the manager");

                if (!await _pharmacyServices.UpdatePharmacyGeneralInfo(generalInfoDTO))
                    return StatusCode(StatusCodes.Status500InternalServerError, "Faild to update,please try again!");

                return Ok(new { result = true });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("update/profileInfo", Name = "UpdateProfileInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProfileInfo(int ManagerId,[FromForm]PharmacyDTO pharmacyDTO, IFormFile? Image)
        {

            if(ManagerId<=0 || pharmacyDTO.PharmacyId<=0 ||string.IsNullOrEmpty(pharmacyDTO.Name) ||
                string.IsNullOrEmpty(pharmacyDTO.ArabicName))
            {
                return BadRequest("Invalied or empty values!");
            }

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied token!");

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(ManagerId, TokenServices.eUserType.Manager, token))
                    return Unauthorized("Token is invalid and not correct!");

                if (!await _pharmacyServices.IsPharmacyHasThisUser(ManagerId, TokenServices.eUserType.Manager, pharmacyDTO.PharmacyId))
                    return Unauthorized("You are not the manager");

                if (Image is not null)
                {
                    using(var stream = Image.OpenReadStream())
                    {
                        pharmacyDTO.ImageURL=await _fileUploadService.UploadImageAsync(stream,Image.Name);
                    }
                }

                PharmacyDTO? pharmacy = await _pharmacyServices.UpdatePharmacyProfileInfo(pharmacyDTO);

                if (pharmacy is null)
                    return StatusCode(StatusCodes.Status500InternalServerError,"Falied to update !");

                return Ok(new { pharmacy });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Falied to update !");
            }
        }

        [HttpGet("vediocall/info/{PharmacyId}", Name ="VedioCallInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVedioCallInfo(int PharmacyId)
        {
            if(PharmacyId<=0)
            {
                return BadRequest("Invalied Values!");
            }

            try
            {
                PharmacyVedioCallDTO? pharmacyVedioCall = await _pharmacyServices.GetPharmacyVedioCallInfO(PharmacyId);

                if (pharmacyVedioCall is null)
                    return NotFound("No data was found ,Try again!");

                return Ok(new { pharmacyVedioCall });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("vedioCall/Info/edit",Name ="UpdateCallInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateVedioCallInfo(int ManagerId, [FromForm] PharmacyVedioCallDTO pharmacyDTO)
        {
            if (ManagerId <= 0 || pharmacyDTO.PharamcyId <= 0)
                return BadRequest("Invalied or empty values!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied token!");

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(ManagerId, TokenServices.eUserType.Manager, token))
                    return Unauthorized("Token is invalid and not correct!");

                if (!await _pharmacyServices.IsPharmacyHasThisUser(ManagerId, TokenServices.eUserType.Manager, pharmacyDTO.PharamcyId))
                    return Unauthorized("You are not the manager");

                if (!await _pharmacyServices.UpdateVedioCallInfo(pharmacyDTO))
                    return StatusCode(StatusCodes.Status500InternalServerError, "Faild to update information!");

                return Ok(new { result = true });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        [HttpGet("delivery/info",Name = "DeliveryInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult>  GetDeliveryInfo(int ManagerId,int PharmacyId)
        {
            if (ManagerId <= 0 || PharmacyId <= 0)
                return BadRequest("Invalied or empty values");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Token is Invalied!");

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(ManagerId, TokenServices.eUserType.Manager, token))
                    return Unauthorized("Token is invalid and not correct!");

                PhDeliveryLocationsDTO? phDeliveryLocations= await _pharmacyServices.ShowDeliveryInfo(PharmacyId);

                if (phDeliveryLocations is null)
                    return NotFound("No data was found!");

                return Ok(new { phDeliveryLocations });

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("deliveryInfo/edit", Name = "UpdateDeliveryInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDeliveryLocations(int ManagerId, [FromForm]PhDeliveryLocationsDTO deliveryLocationsDTO) 
        {
            if (ManagerId <= 0 || deliveryLocationsDTO.PharmacyId <= 0 || deliveryLocationsDTO.DeliveryLocations is null)
                return BadRequest("Invalied or empty values!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied Token");

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(ManagerId, TokenServices.eUserType.Manager, token))
                    return Unauthorized("Token is invalid and not correct!");

                if (!await _pharmacyServices.IsPharmacyHasThisUser(ManagerId, TokenServices.eUserType.Manager, deliveryLocationsDTO.PharmacyId))
                    return Unauthorized("You are not the manager");

                if (!await _pharmacyServices.UpdateDeliveryInfo(deliveryLocationsDTO))
                    return StatusCode(StatusCodes.Status500InternalServerError, "Faild to update!");

                return Ok(new { result = true });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("show/{PharmacyId}",Name ="PhamacyShow")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ShowPharmacy(int PharmacyId)
        {
            if (PharmacyId <= 0)
                return BadRequest("Invalied or empty value!");

            try
            {
              
                ShowPharmacyDTO? pharmacyDTO =await _pharmacyServices.ShowPharmacyProfile(PharmacyId);

                if (pharmacyDTO is null)
                    return NotFound("No Pharmacy found for this Id!");

                return Ok(new { pharmacyDTO });
            
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);                                                 
            }
        }

        [HttpPut("update/activatedelivery")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ActivateDelivery(int ManagerId,int PharmacyId)
        {
            if (ManagerId <= 0 || PharmacyId <= 0)
                return BadRequest("Invalied or empty values!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied Token !");

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(ManagerId, TokenServices.eUserType.Manager, token))
                    return Unauthorized("Token is invalid and not correct!");

                if (!await _pharmacyServices.IsPharmacyHasThisUser(ManagerId, TokenServices.eUserType.Manager, PharmacyId))
                    return Unauthorized("You are not the manager");

                if (await _pharmacyServices.IsPharmacyHasDelivery(PharmacyId))
                    return BadRequest("This Pharmacy already has active deliver service");

                if (!await _pharmacyServices.ActivateDelivery(PharmacyId))
                    return StatusCode(StatusCodes.Status500InternalServerError, "Faild to Activate ,please try again!");

                return Ok(new { result = true });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("update/deactivatedelivery")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeActivateDelivery(int ManagerId,int PharmacyId)
        {
            if (ManagerId <= 0 || PharmacyId <= 0)
                return BadRequest("Invalied or empty values!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied Token !");

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(ManagerId, TokenServices.eUserType.Manager, token))
                    return Unauthorized("Token is invalid and not correct!");

                if (!await _pharmacyServices.IsPharmacyHasThisUser(ManagerId, TokenServices.eUserType.Manager, PharmacyId))
                    return Unauthorized("You are not the manager");

                if (!await _pharmacyServices.IsPharmacyHasDelivery(PharmacyId))
                    return BadRequest("This Pharmacy already has active delivery service");

                if (!await _pharmacyServices.DeActivateDelivery(PharmacyId))
                    return StatusCode(StatusCodes.Status500InternalServerError, "Faild to DeActivate ,please try again!");

                return Ok(new { result = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("update/email/{PharmacyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeEmail(int PharmacyId,ChangeEmailDTO emailDTO)
        {
            if (PharmacyId <= 0 || emailDTO.UserId <= 0 || string.IsNullOrEmpty(emailDTO.NewEmail))
                return BadRequest("Invalied or empty values!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied Token!");

            if (_emailVerficationService.CheckIfEmailIsValied(emailDTO.NewEmail, false))
                return BadRequest("Email is not valied!");

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(emailDTO.UserId, TokenServices.eUserType.Manager, token))
                    return Unauthorized("Token is invalid and not correct!");

                if (await _pharmacyServices.IsPharmacyHasThisUser(emailDTO.UserId, TokenServices.eUserType.Manager, PharmacyId))
                    return Unauthorized("You are not the manager");

                if (!await _pharmacyServices.PharmacyChangingEmail(PharmacyId, emailDTO.NewEmail))
                    return StatusCode(StatusCodes.Status500InternalServerError, "Faild to update email!");

                return Ok(new { result = true });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("update/pincode",Name ="UpdatePINcode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ResetPINCode(int ManagerId, int PharmacyId)
        {
            if (ManagerId <= 0 || PharmacyId <= 0)
                return BadRequest("Invalied or empty values!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied Token!");

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(ManagerId, TokenServices.eUserType.Manager, token))
                    return Unauthorized("Token is invalid and not correct!");

                if (!await _pharmacyServices.IsPharmacyHasThisUser(ManagerId, TokenServices.eUserType.Manager, PharmacyId))
                    return Unauthorized("Faild");

                string? PINcode= await _pharmacyServices.ResetPINCode(PharmacyId);

                if (string.IsNullOrEmpty(PINcode))
                    return StatusCode(StatusCodes.Status500InternalServerError,"Faild to rest PIN!");

                return Ok(new { PINcode });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Faild to rest PIN!");
            }

        }

        [HttpGet("callprice",Name ="callprice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVideoCallPrice(int PharmacyId)
        {
            if (PharmacyId <= 0)
                return BadRequest("Invalied Id!");

            try
            {
                if (!await _pharmacyServices.IsPharmacyExists(PharmacyId))
                    return NotFound("No Pharmacy with this Id!");

                decimal? Price = await _pharmacyServices.GetPharamcyVedioCallPrice(PharmacyId);

                if (Price is null)
                    return BadRequest("This Pharmacy does not have Video call service!");

                return Ok(new { Price });

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Faild to get price!");
            }
        }

        [HttpGet("nearest/{CustomerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMostNearerPharmaciesToCustomer(int CustomerId)
        {
            if (CustomerId <= 0)
                return BadRequest(new { message = "Invalied or empty value!" });

            try
            {
                var Pharmacies = await _pharmacyServices.GetMostNearerPhsFromCustomer(CustomerId);

                if (Pharmacies is null || Pharmacies.Count==0)
                    return NotFound(new { message = "No pharmacy was found!" });

                return Ok(new { Pharmacies });
            }
            catch (Exception ex)
            {
                return StatusCode (StatusCodes.Status500InternalServerError,new { message = ex.Message });
            }
        }

        [HttpGet("deliveryfees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetDeliverFees([FromQuery]CheckDeliveryDTO checkDeliveryDTO)
        {
            if (checkDeliveryDTO.PharmacyId <= 0 || checkDeliveryDTO.CustomerId <= 0)
                return BadRequest("Invalied or empty values!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied token!");

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(checkDeliveryDTO.CustomerId, TokenServices.eUserType.Customer, token))
                    return Unauthorized("Token is invalid and not correct!");

                if (!await _pharmacyServices.IsPharmacyHasDelivery(checkDeliveryDTO.PharmacyId))
                    return BadRequest("This Pharmacy does not has Delivery Service!");

                var result = await _pharmacyServices.HandelDeliveryFeesCalculate(checkDeliveryDTO);

                return Ok(new { result });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,new { message = ex.Message });
            }
        }


        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPharmaciesListByGovernorate([FromQuery]PaginatedPharmacyListDTO paginatedPharmacyListDTO, [FromQuery]int? GovernorateId
        , [FromQuery]int? RegionId)
        {
            if (paginatedPharmacyListDTO.LastPharmacyId < 0 || paginatedPharmacyListDTO.Limit <= 0)
                return BadRequest(new { message = "Invalid data!" });

            try
            {
                var PharmaciesResult= await _pharmacyServices.GetPharmaciesList(paginatedPharmacyListDTO,GovernorateId,RegionId);

                return Ok(new { PharmaciesResult });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mesage = "Faild to get data!,Server Error!" });
            }
        }

        [HttpGet("list/have-meeting")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetPharmaciesThatHaveMeetingService([FromQuery] PaginatedPharmacyListDTO paginatedPharmacyListDTO)
        {
            if (paginatedPharmacyListDTO.LastPharmacyId < 0 || paginatedPharmacyListDTO.Limit <= 0)
                return BadRequest(new { message = "Invalid data!" });

            try
            {
                var PharmaciesResult = await _pharmacyServices.GetPharmaciesThatHaveMeetingService(paginatedPharmacyListDTO);

                return Ok(new { PharmaciesResult });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mesage = "Faild to get data!,Server Error!" });
            }
        }


    }
}
