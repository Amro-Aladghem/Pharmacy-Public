using DatabaseLayer.Entities;
using DTOs.CartDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Tokens;
using Services;
using static Services.TokenServices;
using static Services.PaymentServices;
using DTOs.OrderDTOs;
using Azure.Core;
using DTOs.RequestDTOs;

namespace Pharmacy_Web_API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly TokenServices _tokenServices;
        private readonly PharmacyServices _pharmacyServices;
        private readonly OrderServices _ordererServices;

        public OrderController(TokenServices tokenServices, PharmacyServices pharmacyServices, OrderServices ordererServices)
        {
            _tokenServices = tokenServices;
            _pharmacyServices = pharmacyServices;
            _ordererServices = ordererServices;
        }

        [HttpGet("cart/loggedIn/{CustomerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomerCarAtLoggedIn(int CustomerId)
        {
            if (CustomerId <= 0)
                return BadRequest("Invalied or empty value!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied or empty values!");

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(CustomerId, eUserType.Customer, token))
                    return Unauthorized("Token is invalid and not correct!");

                var Cart = await _ordererServices.GetCustomerCartAtLoggedIn(CustomerId);

                return Ok(new { Cart });

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Faild to get Customer Cart,Server Error!");
            }
        }

        [HttpPost("cart/add/{CustomerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddCartToCustomerForFirstTime(int CustomerId, [FromForm] CartItemDTO cartItemDTO)
        {
            if(CustomerId<=0 ||cartItemDTO.PhProductId<=0 ||cartItemDTO.PharmacyId<=0||cartItemDTO.Quantity<=0)
                return BadRequest("Invalied or empty value!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied or empty values!");

            if(!await _pharmacyServices.IsPharmacyHasDelivery(cartItemDTO.PharmacyId))
                return BadRequest("This Pharmacy doesn't have Delivery!");
            

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(CustomerId, eUserType.Customer, token))
                    return Unauthorized("Token is invalid and not correct!");

                var Cart = await _ordererServices.GetCustomerCartFirstTime(CustomerId, cartItemDTO);

                if (Cart is null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Faild to add cart, Try again!");

                return Ok(new { Cart });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Faild to add cart, Try again, Server Error!");
            }
        }

        [HttpPost("cart/cart-items/add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddNewItemToCustomerCart([FromForm] NewCartItemDTO newCartItemDTO)
        {
            if (newCartItemDTO.CustomerId <= 0 || newCartItemDTO.CartId <= 0 || newCartItemDTO.CartItem is null)
                return BadRequest("Invalied or empty values!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new {message="Invalied or empty values!" });


            try
            {
                if (!await _pharmacyServices.IsPharmacyHasDelivery(newCartItemDTO.CartItem.PharmacyId))
                    return BadRequest(new { message = "This Pharmacy doesn't have Delivery!" });

                if (!await _tokenServices.CheckIfTokenCorrect(newCartItemDTO.CustomerId, eUserType.Customer, token))
                    return Unauthorized(new { message = "Token is invalid and not correct!" });

                var Cart = await _ordererServices.HandelAddNewItemToCart(newCartItemDTO);

                if (Cart is null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Faild To Add Item,Please Try again!" });

                return Ok(new { Cart });

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Faild To Add Item,Please Try again,Server Error!" });
            }
        }

        [HttpDelete("Cart/CartItems/Clear/{CartId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ClearCartItems(int CartId, [FromQuery] int CustomerId)
        {
            if (CartId <= 0 || CustomerId <= 0)
                return BadRequest("Invalied or empty values!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied or empty values!");

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(CustomerId, eUserType.Customer, token))
                    return Unauthorized("Token is invalid and not correct!");

                bool result = await _ordererServices.ClearCartFromItems(CartId);

                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Falied to clear cart,Server Error!");
            }
        }

        [HttpGet("cart/show/{CartId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ShowCartItemsList(int CartId, [FromQuery] int CustomerId)
        {
            if (CartId <= 0 || CustomerId <= 0)
                return BadRequest("Invalied or empty values!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied or empty values!");

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(CustomerId, eUserType.Customer, token))
                    return Unauthorized("Token is invalid and not correct!");

                var CartList = await _ordererServices.ShowCartList(CartId,CustomerId);

                return Ok(new { CartList });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Faild to get Items,Server Error!");
            }
        }

        [HttpPost("order/checkout/verify-location")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> VerfiyCustomerLocationAtCheckOut([FromForm] LocationCheckRequest checkRequest)
        {
            if (checkRequest.CustomerId == 0 || checkRequest.Latitude == 0 || checkRequest.Longitude == 0)
                return BadRequest(new { message = "Invalied or empty values!" });

            try
            {
                var result = await _ordererServices.IsCustomerLocationChangedAtCheckOut(checkRequest);

                return Ok(new { result });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Server Error!" });
            }
        }

        [HttpGet("cart/{CartId}/check-out/info/{CustomerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCartInfoForCheckOut(int CartId,int CustomerId)
        {
            if (CartId <= 0 || CustomerId <= 0)
                return BadRequest(new { message = "Invalied sent values!" });

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { message = "Invalied or incorrect token!" });
            
            try
            {
                if(!await _tokenServices.CheckIfTokenCorrect(CustomerId,eUserType.Customer,token))
                    return Unauthorized(new { message = "Invalied or incorrect token!" });

                var CartInfo = await _ordererServices.GetCartInfoForCheckOut(CartId, CustomerId);

                if(CartInfo is null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Server Error,Somthing happend in functions!" });

                return Ok(new { CartInfo });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Server Error,Somthing happend in functions!" });
            }
        }


        [HttpPut("cart/item/update/{CustomerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCartItemQuantity([FromForm]UpdateCartItemDTO updateCartItemDTO, int CustomerId)
        {
            if (CustomerId <= 0 || updateCartItemDTO.ItemId <= 0 ||updateCartItemDTO.Quantity<0)
                return BadRequest("Invalied or empty values!");


            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied or empty values!");

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(CustomerId, eUserType.Customer, token))
                    return Unauthorized("Token is invalid and not correct!");

                var CartList = await _ordererServices.UpdateCartItemInfo(updateCartItemDTO,CustomerId);

                if (CartList is null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Falied to update Info!");

                return Ok(new { CartList });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Falied to update Info, Server Error!");
            }
        }

        [HttpDelete("cart/item/delete/{CustomerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteItemFromCart([FromForm] UpdateCartItemDTO deleteCartItemDTO, int CustomerId)
        {
            if (CustomerId <= 0 || deleteCartItemDTO.ItemId <= 0 || deleteCartItemDTO.Quantity < 0)
                return BadRequest("Invalied or empty values!");


            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied or empty values!");

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(CustomerId, eUserType.Customer, token))
                    return Unauthorized("Token is invalid and not correct!");

                var result = await _ordererServices.DeleteItemFromCart(deleteCartItemDTO, CustomerId);

                if (result is null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Falied to update Info!");

                return Ok(new { result });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Falied to update Info, Server Error!");
            }
        }

        [HttpPost("order/checkout/via-whatsapp/link")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CheckOutWithWhatsApp([FromForm] CheckOutDTO checkOutDTO)
        {
            if (checkOutDTO.CustomerId <= 0 || checkOutDTO.CartId <= 0)
                return BadRequest(new { message = "Invalied or Empty value!" });

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { Message = "Invalied or empty token!" });

            try
            {
                if(!await _tokenServices.CheckIfTokenCorrect(checkOutDTO.CustomerId,eUserType.Customer,token))
                     return Unauthorized(new { Message = "Invalied or empty token!" });

                var result = await _ordererServices.CreateTemporaryOrderAndSendWhatsAppLink(checkOutDTO.CartId, checkOutDTO.CustomerId);

                return Ok(new { result });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Server Error,Falied to checkout this order!" });
            }
        }


        [HttpPost("order/checkout/on-delviery")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CheckOutOnDelivery([FromForm]CheckOutDTO checkOutDTO)
        {
            if (checkOutDTO.CustomerId <= 0 || checkOutDTO.CartId <= 0 ||checkOutDTO.PaymentMethodId<=0)
                return BadRequest("Invalied or empty data!");

            var token = Request.Cookies["AuthToken"];

            if (token is null)
                return Unauthorized(new { message = "Inavlied or emtpy token!" });
            
            try
            {
                if(checkOutDTO.PaymentMethodId!=(int)ePaymentMethode.OnDelivery)
                {
                    return BadRequest(new { message = "You Can only pay OnDelivery!" });
                }

                if (!await _tokenServices.CheckIfTokenCorrect(checkOutDTO.CustomerId, eUserType.Customer, token))
                    return Unauthorized(new { message = "Token is invalid and not correct!" });

                var result = await _ordererServices.HandelAddingNewOrderByOnDelivreyPayment(checkOutDTO);

                return Ok(new { result });

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Falied to place order,please try again!" });
            }
        }

        [HttpGet("Order/history/{CustomerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomerOrdersHistory(int CustomerId)
        {
            if (CustomerId <= 0)
                return BadRequest("Invalied or empty values!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied or empty values!");

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(CustomerId, eUserType.Customer, token))
                    return Unauthorized("Token is invalid and not correct!");

                var OrdersList = await _ordererServices.ShowOrdersHistoryForCustomer(CustomerId);

                return Ok(new { OrdersList });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get data!");
            }
        }

        [HttpGet("order/Get/Active/{CustomerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMostActiveOrderForTheCustomer(int CustomerId)
        {
            if (CustomerId <= 0)
                return BadRequest("Invalied or empty values!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied or empty values!");

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(CustomerId, eUserType.Customer, token))
                    return Unauthorized("Token is invalid and not correct!");

                var Order = await _ordererServices.ShowMostActiveOrderForCustomer(CustomerId);

                return Ok(new { Order });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Faild to get order,Server Error!");
            }
        }

        [HttpGet("order/{OrderId}/details")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrderDetails(int OrderId, [FromQuery] int CustomerId)
        {
            if (OrderId <= 0 || CustomerId <= 0)
                return BadRequest(new { message = "Invalied or empty values!" });

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { message = "Invalied or empty values!" });

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(CustomerId, eUserType.Customer, token))
                    return Unauthorized(new { message = "Token is invalid and not correct!" });

                var OrderDetails = await _ordererServices.ShowOrderDetailsForCustomer(OrderId,CustomerId);

                if (OrderDetails is null)
                    return NotFound(new { message = "No Details was found for this order!" });

                return Ok(new { OrderDetails });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to get order details,Server Error!" });
            }
        }


        [HttpGet("order/status/{OrderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrderStatus(int OrderId)
        {
            if (OrderId <= 0)
                return BadRequest("Inavlied or empty values!");

            try
            {
                var OrderStatus = await _ordererServices.GetOrderStatus(OrderId);

                if (OrderStatus is null)
                    return NotFound("No Order with this Id!");


                return Ok(new { OrderStatus });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Faild to get status,Server Error!");
            }
        }

        [HttpPut("order/cancel/{OrderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CancelOrder(int OrderId, [FromQuery] int CustomerId)
        {
            if (OrderId <= 0 || CustomerId <= 0)
                return BadRequest("Invalied or empty values!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied or empty token!");

            try
            {
                if (!await _tokenServices.CheckIfTokenCorrect(CustomerId, eUserType.Customer, token))
                    return Unauthorized("Token is invalid and not correct!");

                bool result =await _ordererServices.HandelCancelOrder(OrderId);

                return Ok(new { result });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to cancel order,Server Error!");
            }
        }

        [HttpGet("Order/Pharmacy/Active")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetActiveOrderForThePharmacy(PaginatedOrderDTO paginatedOrderDTO, [FromQuery] int UserId , 
            [FromQuery] string? role = "Admin")
        {
            if (UserId <= 0 || paginatedOrderDTO.PharmacyId <= 0 || paginatedOrderDTO.PageNumber <= 0 || paginatedOrderDTO.Limit <= 0)
                return BadRequest("Invalied or empty values!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied or empty token!");

            try
            {
                eUserType userType = role.ToLower() == "admin" ? eUserType.Admin : eUserType.Manager;

                if (!await _tokenServices.CheckIfTokenCorrect(UserId, userType, token))
                    return Unauthorized("Token is invalid and not correct!");

                if (!await _pharmacyServices.IsPharmacyHasThisUser(UserId, userType, paginatedOrderDTO.PharmacyId))
                    return Unauthorized("You are not Admin or Manager for this Pharmacy!");

                var data = await _ordererServices.GetActiveOrderForPharmacy(paginatedOrderDTO);

                return Ok(new {data});
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get data,Server Error!");
            }
        }

        [HttpGet("Order/Pharmacy/Active/Count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTotalActiveOrderCountForPharmacy(int PharmacyId, [FromQuery] int UserId,
            [FromQuery] string? role = "Admin")
        {
            if (PharmacyId <= 0 || UserId <= 0)
                return BadRequest("Invalied or empty data!");

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalied or empty token!");

            try
            {
                eUserType userType = role.ToLower() == "admin" ? eUserType.Admin : eUserType.Manager;

                if (!await _tokenServices.CheckIfTokenCorrect(UserId, userType, token))
                    return Unauthorized("Token is invalid and not correct!");

                if (!await _pharmacyServices.IsPharmacyHasThisUser(UserId, userType, PharmacyId))
                    return Unauthorized("You are not Admin or Manager for this Pharmacy!");

                int Count = await _ordererServices.GetNumberOfActiveOrder(PharmacyId);

                return Ok(new { Count });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get data,Server Error!");
            }
        }















    }
}
