using CloudinaryDotNet.Actions;
using DatabaseLayer.Entities;
using DTOs.ProductDTOs;
using DTOs.ReqestDTOs.Meeting;
using DTOs.ReqestDTOs.Refund;
using DTOs.RequestDTOs.Meeting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Services;
using static Services.TokenServices;
using static Services.RequestServices;
using DTOs.ResultDTOs;

namespace Pharmacy_Web_API.Controllers
{
    [Route("api/v1/request")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly TokenServices _tokenServices;
        private readonly PharmacyServices _pharmacyServices;
        private readonly RequestServices _requestServices;
        private readonly SystemDefaultServices _systemDefaultServices;
        public RequestController(TokenServices tokenServices,PharmacyServices pharmacyServices,RequestServices requestServices,
            SystemDefaultServices systemDefaultServices)
        {
            _tokenServices=tokenServices;
            _pharmacyServices=pharmacyServices;
            _requestServices=requestServices;
            _systemDefaultServices = systemDefaultServices;
        }

        [HttpGet("meet/{RequestId}/info/{CustomerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMeetingRequestInfo(int RequestId,int CustomerId)
        {
            if (RequestId <= 0 || CustomerId <= 0)
                return BadRequest(new { message = "Invalied or Empty values!" });

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { message = "Invalied Token!" });

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(CustomerId, TokenServices.eUserType.Customer, token))
                    return Unauthorized(new { message = "Token is invalid and not correct!" });


                var requestInfo = await _requestServices.GetMeetingRequestInfo(RequestId,CustomerId);

                if (requestInfo is null)
                    return NotFound(new { message = "No Request found with this Id!" });

                if (requestInfo.MeetingReqStatus.Id == (int)eRequestStatus.Finished)
                    return BadRequest(new { message="this meeting is finished!" });

                return Ok(new { requestInfo });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Falied to get data!" });
            }
        }


        [HttpGet("meet/recent/{CustomerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMostRecentMeetingReqDetails(int CustomerId)
        {
            if (CustomerId <= 0)
                return BadRequest(new { message = "Invalied or empty values!" });

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { message = "Invalied or empty token!" });

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(CustomerId, TokenServices.eUserType.Customer, token))
                    return Unauthorized(new { message = "Token is invalid and not correct!" });

                var request = await _requestServices.GetMostRecentReqDetailsForCustomer(CustomerId);

                return Ok(new { request });

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Faild to get data!" });
            }
        }

        [HttpGet("meet/active/{CustomerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMostActiveReq(int CustomerId)
        {
            if (CustomerId <= 0)
                return BadRequest(new { message = "Invalied or empty values!" });

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { message = "Invalied or empty token!" });

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(CustomerId, TokenServices.eUserType.Customer, token))
                    return Unauthorized(new { message = "Token is invalid and not correct!" });

                var Request = await _requestServices.GetMostActiveReq(CustomerId);

                if (Request == null)
                    return NotFound(new { message = "No Active Request was found for this customer!" });

                return Ok(new { Request });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to get data!" });
            }
        }


        [HttpGet("meet/customer/{CustomerId}/history")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMeetingReqHistory(int CustomerId)
        {
            if (CustomerId <= 0)
                return BadRequest("Invalied or empty values!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied or empty token!");

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(CustomerId, TokenServices.eUserType.Customer, token))
                    return Unauthorized("Token is invalid and not correct!");

                var requests = await _requestServices.GetCustomerReqHistory(CustomerId);

                if (requests is null)
                    return NotFound("You don't have any Request!");

                return Ok(new { requests });
            }
            catch ( Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("meet/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelMeetingRequest([FromForm]CancelMeetinReqDTO meetinReqDTO)
        {
            if (meetinReqDTO.Id <= 0 || meetinReqDTO.CustomerId <= 0)
                return BadRequest("Invalied or empty values!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied or empty Token!");

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(meetinReqDTO.CustomerId, TokenServices.eUserType.Customer, token))
                    return Unauthorized("Token is invalid and not correct!");

                bool IsDone =  await _requestServices.CancelRequest(meetinReqDTO);

                if (!IsDone)
                    return BadRequest("You can't cancel this request!");

                return Ok(new { result = true });
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("meet/recent/status/{CustomerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMostActiveReqStatus(int CustomerId)
        {
            if (CustomerId <= 0)
                return BadRequest(new { message = "Invalied or empty values!" });

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { message = "Invalied or empty token!" });

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(CustomerId, TokenServices.eUserType.Customer, token))
                    return Unauthorized(new { message = "Token is invalid and not correct!" });

                var Status = await _requestServices.GetMeetingReqStatusForCustomer(CustomerId);

                if (Status is null)
                    return NotFound(new { message = "no meeting request was found for this customer" });

                if (Status.Id == (int)eRequestStatus.Finished)
                    return BadRequest(new { message = "This meeting was finished" });

                return Ok(new { Status });
            }
            catch(Exception ex )
            {
                return NotFound(new { message = "No Active Request was found!" });
            }
        }

        [HttpGet("Meet/Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        public async Task<IActionResult> ChangeMeetingRequestStatus([FromForm]UpdateMeetingStatus updateMeetingStatus,[FromQuery] int UserId,
            [FromQuery] string ?role="admin")
        {
            if (UserId <= 0 || updateMeetingStatus.RequestId <= 0 || updateMeetingStatus.StatusId <= 0)
                return BadRequest("Invalied or Empty values!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied or empty token!");

            try
            {
                eUserType userType = role.ToLower() == "admin" ? eUserType.Admin : eUserType.Manager;

                if (!await _tokenServices.CheckIfTokenCorrect(UserId, userType, token))
                    return Unauthorized("Token is invalid and not correct!");

                if (!await _pharmacyServices.IsPharmacyHasThisUser(UserId, userType, updateMeetingStatus.PharmacyId))
                    return Unauthorized("You are not Admin or Manager for this Pharmacy!");

                bool IsDone =await _requestServices.ChangeRequestStatus(updateMeetingStatus);

                return Ok(new { IsDone });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Faild to Update Status!");
            }

        }


        [HttpPost("Meet/Start")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> StartMeeting([FromForm]StartMeetingDTO startMeetingDTO, [FromQuery]int UserId, [FromQuery] string? role="admin")
        {
            if (startMeetingDTO.RequestId <= 0 || startMeetingDTO.PharmacyId <= 0 || string.IsNullOrEmpty(startMeetingDTO.MeetingURL)
                || UserId <= 0)
                return BadRequest("Invalied or empty values!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied or empty token!");

            try
            {
                eUserType userType = role.ToLower() == "admin" ? eUserType.Admin : eUserType.Manager;

                if (!await _tokenServices.CheckIfTokenCorrect(UserId, userType, token))
                    return Unauthorized("Token is invalid and not correct!");

                if (!await _pharmacyServices.IsPharmacyHasThisUser(UserId, userType, startMeetingDTO.PharmacyId))
                    return Unauthorized("You are not Admin or Manager for this Pharmacy!");

                bool IsDone = await _requestServices.StartMeeting(startMeetingDTO);

                return Ok(new { IsDone });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost("Meet/End")]
        public async Task<IActionResult> EndMeeting([FromForm] EndMeetingDTO endMeetingDTO)
        {
            if (!_requestServices.IsEndMeetingValuesCorrect(endMeetingDTO))
                return BadRequest("Invalied or Empty values!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied or empty value!");

            try
            {
                eUserType userType = endMeetingDTO.role.ToLower() == "admin" ? eUserType.Admin : eUserType.Manager;

                if (!await _tokenServices.CheckIfTokenCorrect(endMeetingDTO.UserId, userType, token))
                    return Unauthorized("Token is invalid and not correct!");

                if (!await _pharmacyServices.IsPharmacyHasThisUser(endMeetingDTO.UserId, userType, endMeetingDTO.PharmacyId))
                    return Unauthorized("You are not Admin or Manager for this Pharmacy!");

               bool IsDone = await _requestServices.EndMeeting(endMeetingDTO);

                return Ok(new { IsDone });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Faild to save end meeting record!");
            }
        }

        [HttpGet("Meet/PhUser/History")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> GetPharmacyMeetingHistory( [FromQuery] int UserId, [FromForm] PagenaitedReqHistoryDTO reqHistoryDTO,
             [FromQuery] string ?role="admin")
        {
            if (reqHistoryDTO.PharmacyId <= 0 || UserId <= 0)
                return BadRequest("Invalied values or Empty!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied or Empty Token!");

            try
            {
                eUserType userType = role.ToLower() == "admin" ? eUserType.Admin : eUserType.Manager;

                if (!await _tokenServices.CheckIfTokenCorrect(UserId, userType, token))
                    return Unauthorized("Token is invalid and not correct!");

                if (!await _pharmacyServices.IsPharmacyHasThisUser(UserId, userType, reqHistoryDTO.PharmacyId))
                    return Unauthorized("You are not Admin or Manager for this Pharmacy!");

                var RequestsHistory =await _requestServices.GetRequestHistoryForAdmin(reqHistoryDTO);

                if (RequestsHistory is null)
                    return NotFound("There is no data!");

                return Ok(new { RequestsHistory });

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get data!");
            }


        }

        [HttpGet("Meet/PhUser/History/Active")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetActiveMeeting( [FromQuery] int UserId, [FromForm] PagenaitedReqHistoryDTO reqHistoryDTO,
             [FromQuery] string? role = "admin")
        {
            if ( reqHistoryDTO.PharmacyId <= 0 || UserId <= 0 || reqHistoryDTO.Limit<=0)
                return BadRequest("Invalied or empty data!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied or empty token!");

            try
            {
                eUserType userType = role.ToLower() == "admin" ? eUserType.Admin : eUserType.Manager;

                if (!await _tokenServices.CheckIfTokenCorrect(UserId, userType, token))
                    return Unauthorized("Token is invalid and not correct!");

                if (!await _pharmacyServices.IsPharmacyHasThisUser(UserId, userType, reqHistoryDTO.PharmacyId))
                    return Unauthorized("You are not Admin or Manager for this Pharmacy!");

                var Requests = await _requestServices.GetActiveMeetingRequest(reqHistoryDTO);

                if (Requests is null)
                    return NotFound("No data was found!");

                return Ok(new { Requests });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Faild to load data!");
            }
        }

        [HttpPost("refund/meeting/add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddRefundRequestForMeeting ([FromForm]NewRefundReqDTO refundReqDTO)
        {
            if (refundReqDTO.CustomerId < 0 || refundReqDTO.RefferenceId <= 0 || string.IsNullOrEmpty("TypeName"))
            {
                return BadRequest("Invalied or emtpy data!");
            }

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied or emtpy token!");

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(refundReqDTO.CustomerId, eUserType.Customer, token))
                    return Unauthorized("Token is invalid and not correct!");

                eRefundType? refundType =  _requestServices.GetRefundType(refundReqDTO.TypeName);

                if (refundType is null || refundType==eRefundType.Order)
                    return BadRequest("Inavlied type name!");

                var result = await _requestServices.AddNewRefundReqForOrder(refundReqDTO, (eRefundType)refundType);

                if (result.IsError)
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Faild to add Refund Request!" });

                return Ok(new { result });

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("refund/order/add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddRefundRequestForOrder([FromForm]NewRefundReqDTO refundReqDTO)
        {
            if (refundReqDTO.CustomerId < 0 || refundReqDTO.RefferenceId <= 0 || string.IsNullOrEmpty("TypeName"))
            {
                return BadRequest(new { message = "Invalied or emtpy data!" });
            }

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { message = "Invalied or emtpy token!" });

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(refundReqDTO.CustomerId, eUserType.Customer, token))
                    return Unauthorized(new { message = "Token is invalid and not correct!" });

                eRefundType? refundType = _requestServices.GetRefundType(refundReqDTO.TypeName);

                if (refundType is null || refundType == eRefundType.Meeting)
                    return BadRequest(new { message = "Inavlied type name!" });

                var result = await _requestServices.AddNewRefundReqForOrder(refundReqDTO, (eRefundType)refundType);

                if (result.IsError)
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Faild to add Refund Request!" });

                return Ok(new { result});

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("refund/history/{CustomerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRefundHistory(int CustomerId)
        {
            if (CustomerId <= 0)
                return BadRequest(new { message = "Invalied or empty values!" });

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { message = "Invalied or empty token!" });

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(CustomerId, eUserType.Customer, token))
                    return Unauthorized(new { message = "Token is invalid and not correct!" });

                var Requests = await _requestServices.RefundReqHistoryForCustomer(CustomerId);

                return Ok(new { Requests });

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("System/values")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSysDefaultValues([FromQuery] bool withMedicalType=false)
        {
            try
            {
                var Countries = _systemDefaultServices.GetCountries();
                var Governates = _systemDefaultServices.GetGovernates();
                List<MedicalTypeDTO>? MedicalTypes = null;

                if(withMedicalType)
                {
                   MedicalTypes = await _systemDefaultServices.GetMedicalTypes();
                }

                return Ok(new { Countries, Governates, MedicalTypes });

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get data!");
            }
        }

        [HttpGet("System/values/DeliveryTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDeliveryTypes()
        {
            try
            {
                var DeliveryTypes = await _systemDefaultServices.GetDeliveryTypes();

                return Ok(new { DeliveryTypes });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get data!");
            }
        }

        [HttpGet("System/values/PaymentMethode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPaymentMethodes()
        {
            try
            {
                var PaymentMethodes = await _systemDefaultServices.GetPaymentMethodes();

                return Ok(new { PaymentMethodes });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get data!");
            }
        }

        [HttpGet("system/customer/{CustomerId}/pharmacy/{PharmacyId}/whatsapp/link")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public async Task<IActionResult> GetWhatsAppLink(int CustomerId,int PharmacyId)
        {
            if(CustomerId<=0 || PharmacyId<=0)
            {
                return BadRequest(new { message = "Invalied data!" });
            }

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { message = "Invalied or empty token!" });

            try
            {
                if(!await _tokenServices.CheckIfTokenCorrect(CustomerId, eUserType.Customer, token))
                    return Unauthorized(new { message = "Invalied or empty token!" });

                if (await _requestServices.IsCustomerHasAnyActiveMeetingRequest(CustomerId))
                    return Conflict(new { message = "you have an active or not finished meeting!" });

                var result = _systemDefaultServices.GetWhatsAppMessageLink(CustomerId, PharmacyId);

                if(!await _requestServices.AddNewTempRequest(CustomerId,PharmacyId))
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Server Error!" });

                return Ok(new { result });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Server Error!" });
            }
        }

        [HttpGet("system/values/regions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public async Task<IActionResult> GetRegions([FromQuery] int GovernorateId)
        {
            if (GovernorateId <= 0)
                return BadRequest(new { message = "invalied gov id!" });

            try
            {
                var regions = await _systemDefaultServices.GetGovernorateRegions(GovernorateId);

                return Ok(new { regions });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to get!" });
            }
        }




    }
}
