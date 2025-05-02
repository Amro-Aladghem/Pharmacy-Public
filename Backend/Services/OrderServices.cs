using DatabaseLayer.Entities;
using DTOs.CartDTOs;
using DTOs.OrderDTOs;
using DTOs.ProductDTOs;
using DTOs.ResultDTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.PharamacyDTOs;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using DTOs.RequestDTOs;
using Microsoft.Extensions.Configuration;
using System.Collections.Immutable;

namespace Services
{
    public class OrderServices
    {
        public enum eOrderStatus { Successfullsending=1, Pending=2, Preparing=3, Delivering=4, Canceled=5, NotAccepted=6, Finished=7};

        private readonly AppDbContext _context;
        private readonly PaymentServices _paymentServices;
        private IConfiguration _configuration;

        private const decimal AcceptableLocationDiff = 0.03m;

        public OrderServices(AppDbContext context,IConfiguration configuration)
        {
            _context = context; 
            _paymentServices= new PaymentServices(context);
            _configuration = configuration;
        }

        private class CancelOrderInfo()
        {
            public int OrderId { get; set; }
            public int PaymentMethodeId { get; set; }
            public int OrderStatusId { get; set; }
        }

        private async Task<decimal> GetPhProductPrice(int PhProductId)
        {
            decimal  Price =await _context.PhPramacyProducts.Where(ph => ph.PhProductId == PhProductId).Select(ph => ph.Price).FirstOrDefaultAsync();

            return Price;
        }

        private async Task<bool> UpdateItemQuantity(decimal ItemPrice,CartItem Item,CartItemDTO cartItemDTO)
        {
            decimal TotalPrice = ItemPrice * cartItemDTO.Quantity;

            Item.Quantity = cartItemDTO.Quantity;
            Item.Price = TotalPrice;


            return  await _context.SaveChangesAsync() > 0;
        }

        private async Task<bool> CreateCartItem(int CartId,CartItemDTO cartItemDTO,ICollection<CartItem>?CartItems=null)
        {
            decimal Price =await GetPhProductPrice(cartItemDTO.PhProductId);

            if (Price == 0.0M)
            {
                return false;
            }

            if(CartItems is not null)
            {
                CartItem? Item = CartItems.FirstOrDefault(c => c.PhProductId == cartItemDTO.PhProductId);

                if(Item is not null)
                {
                    await UpdateItemQuantity(Price, Item, cartItemDTO);
                    return true;
                }
            }


            decimal TotalPrice = Price * cartItemDTO.Quantity;


            var NewCartItem = new CartItem()
            {
                CartId = CartId,
                PhProductId = cartItemDTO.PhProductId,
                Quantity = cartItemDTO.Quantity,
                Price = TotalPrice
            };

            _context.CartItems.Add(NewCartItem);

            if (await _context.SaveChangesAsync() <= 0)
            {
                return false;
            }

            return true;
        }

        private async Task<CartDTO?> AddNewCartWithFirstItem(int CustomerId,CartItemDTO cartItemDTO)
        {
            using ( var transaction = _context.Database.BeginTransaction())
            {
                var cart = new Cart()
                {
                    CustomerId = CustomerId,
                    PharmacyId = cartItemDTO.PharmacyId,
                };

                _context.Carts.Add(cart);

                if (await _context.SaveChangesAsync() <= 0)
                {
                    return null;
                }

                if (!await CreateCartItem(cart.CartId, cartItemDTO))
                {
                    return null;
                }

                await transaction.CommitAsync();

                return new CartDTO
                {
                    CartId = cart.CartId,
                    NumberOfItems = 1,
                    PharmacyId = cartItemDTO.PharmacyId,
                };
            }
            
        }

        public async Task<CartDTO?> GetCustomerCartFirstTime(int CustomerId, CartItemDTO cartItemDTO)
        {
            var Cart = await _context.Carts.Where(C => C.CustomerId == CustomerId).FirstOrDefaultAsync();

            if(Cart != null)
            {
                var NewCartItem = new NewCartItemDTO()
                {
                    CartId = Cart.CartId,
                    CustomerId = CustomerId,
                    CartItem = new CartItemDTO()
                    {
                        PharmacyId = cartItemDTO.PharmacyId,
                        PhProductId = cartItemDTO.PhProductId,
                        Quantity = cartItemDTO.Quantity,
                    }
                };

                return await HandelAddNewItemToCart(NewCartItem);
            }


            CartDTO? CustomerCart =await AddNewCartWithFirstItem(CustomerId, cartItemDTO);

            return CustomerCart;
        }

        public async Task<CartDTO?> GetCustomerCartAtLoggedIn(int CustomerId)
        {
            var Cart =await _context.Carts.Include(c => c.cartItems).Where(c => c.CustomerId == CustomerId).FirstOrDefaultAsync();

            if(Cart is null)
            {
                return null;
            }

            int ItemsCount = Cart.cartItems.Count;

            return new CartDTO()
            {
                CartId = Cart.CartId,
                NumberOfItems = ItemsCount,
                PharmacyId = Cart.PharmacyId is null ? null : Cart.PharmacyId
            };
        }

        private async Task ClearCartFromItemsWithoutSave (Cart cart)
        {
            await _context.CartItems.Where(C => C.CartId == cart.CartId).ExecuteDeleteAsync();
            
            cart.PharmacyId = null;

            _context.SaveChanges();
        }

        public async Task<bool> ClearCartFromItems(int CartId)
        {
            int DeleRowNum = await _context.CartItems.Where(C => C.CartId == CartId).ExecuteDeleteAsync();

            if (DeleRowNum <= 0)
                return false;

            var updatedCart = new Cart()
            {
                CartId = CartId,
                PharmacyId = null
            };

            _context.Attach(updatedCart);
            _context.Entry(updatedCart).Property(p => p.PharmacyId).IsModified = true;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<CartDTO?> HandelAddNewItemToCart(NewCartItemDTO cartItemDTO)
        {
            bool WasPharmacyChanged = true;

            var Cart =await _context.Carts.Include(c=>c.cartItems).Where(c => c.CartId == cartItemDTO.CartId && c.CustomerId==cartItemDTO.CustomerId).FirstOrDefaultAsync();
            

            if(Cart is null)
            {
                return null;
            }
           
            WasPharmacyChanged = Cart.PharmacyId != cartItemDTO.CartItem.PharmacyId;
            
             
            if (WasPharmacyChanged)
            {
                 Cart.PharmacyId = cartItemDTO.CartItem.PharmacyId;
                _context.CartItems.RemoveRange(Cart.cartItems);

                await _context.SaveChangesAsync();
            }

            if(!await CreateCartItem(cartItemDTO.CartId, cartItemDTO.CartItem, Cart.cartItems))
            {
                return null;
            }

            int CartItemsCount = Cart.cartItems.Count;

            int NumbersOfProduct = WasPharmacyChanged ? 1 : CartItemsCount;
                                                      

            return new CartDTO()
            {
                CartId = cartItemDTO.CartId,
                PharmacyId = cartItemDTO.CartItem.PharmacyId,
                NumberOfItems = NumbersOfProduct
            };
        }

        private async Task<bool> IsCartForThisCustomer(int CartId,int CustomerId)
        {
            bool IsExsits = await _context.Carts.Where(c=>c.CartId==CartId && c.CustomerId==CustomerId).AnyAsync();

            return IsExsits;
        }

        private async Task<int?> GetPharmacyIdFromCustomerCart(int CartId,int CustomerId)
        {
            int? PharmacyId =await _context.Carts.Where(C => C.CartId == CartId && C.CustomerId == CustomerId)
                                                 .Select(C=>C.PharmacyId)
                                                 .FirstOrDefaultAsync();

            return PharmacyId;
        }

        private async Task<List<ListingCartItemDTO>?> GetCartItemsForShowing(int CartId)
        {
            var Items =await _context.CartItems.Where(c => c.CartId == CartId)
                                              .Select(c => new ListingCartItemDTO()
                                              {
                                                  ItemId = c.ItemId,
                                                  Product = new PhProductSummeryDTO()
                                                  {
                                                      PhProductId = c.PhProductId,
                                                      ImageURL = c.PhPramacyProduct.SysProduct.ImageURL,
                                                      PhProductName = c.PhPramacyProduct.SysProduct.ProductName,
                                                      Price = c.PhPramacyProduct.Price
                                                  },
                                                  Quantity = c.Quantity,
                                                  Price = c.Price
                                              })
                                              .ToListAsync();

            return Items;
        }

        public async Task<CartItemsListDTO?> ShowCartList(int CartId,int CustomerId)    //////////////////////////////////////////////////<------------------------
        {
            int? PharmacyId = await GetPharmacyIdFromCustomerCart(CartId, CustomerId);

            if(PharmacyId==null || PharmacyId==0)
                return null;

            (decimal, decimal) DeliveryAndServiceFees = await GetDeliveryAndServiceFees(CustomerId,(int)PharmacyId);

            if(DeliveryAndServiceFees.Item1==0)
            {
                return new()
                {
                    DeliveryFees = 0
                };
            }

            List<ListingCartItemDTO>? CartItems = await GetCartItemsForShowing(CartId);

            if (CartItems == null)
            {
                return null;
            }

            decimal SubPrice = CartItems.Sum(c => c.Price);

            decimal TotalPrice = SubPrice + DeliveryAndServiceFees.Item1 + DeliveryAndServiceFees.Item2;


            return new ()
            {
                CartId = CartId,
                CartItmes = CartItems,
                TotalPrice = TotalPrice,
                DeliveryFees = DeliveryAndServiceFees.Item1,
                SubPrice=SubPrice,
                ServiceFees=DeliveryAndServiceFees.Item2
            };
        }

        public async Task<DeleteCartItemDTO?> DeleteItemFromCart(UpdateCartItemDTO updateCartItemDTO , int CustomerId)
        {
            var CartItems = await _context.CartItems.Where(c => c.Cart.CustomerId == CustomerId).ToListAsync();

            if (CartItems is null)
            {
                return null;
            }

            bool IsCartLastItem = CartItems.Count == 1;

            if(IsCartLastItem)
            {
                if (!await ClearCartFromItems(CartItems[0].CartId))
                    return null;

                return new DeleteCartItemDTO()
                {
                    IsDone = true,
                    diff = - CartItems[0].Price,
                };
            }

            CartItem ItemEntity = CartItems.Where(i => i.ItemId == updateCartItemDTO.ItemId).First();


            decimal diff = - ItemEntity.Price;

            _context.CartItems.Remove(ItemEntity);

            if (await _context.SaveChangesAsync()<=0)
            {
                return null;
            }

            return new DeleteCartItemDTO() 
            {
               IsDone=true,
               diff = diff,
            };
        }

        public async Task<UpdateCartItemResutlDTO?> UpdateCartItemInfo(UpdateCartItemDTO updateCartItemDTO,int CustomerId)
        {
            var ItemEntity = await _context.CartItems.FirstOrDefaultAsync(c => c.ItemId == updateCartItemDTO.ItemId);

            if(ItemEntity is null)
            {
                return null;
            }

            if(! await IsCartForThisCustomer(ItemEntity.CartId,CustomerId))
            {
                return null;
            }
            

            decimal ProductPrice = await GetPhProductPrice(ItemEntity.PhProductId);

            if(ProductPrice == 0)
            {
                return null;
            }

            decimal diff = ProductPrice * updateCartItemDTO.Quantity - ItemEntity.Price;


            ItemEntity.Quantity = updateCartItemDTO.Quantity;
            ItemEntity.Price = ProductPrice * updateCartItemDTO.Quantity;

            if(await _context.SaveChangesAsync()<=0)
            {
                return null;
            }


            return new UpdateCartItemResutlDTO()
            {
                ItemId=ItemEntity.ItemId,
                Price=ItemEntity.Price,
                Quantity=ItemEntity.Quantity,
                SubPriceDiff=diff,
                TotalPriceDiff=diff
            };
        }

        private async Task<(decimal,decimal)> GetDeliveryAndServiceFees(int CustomerId,int PharmacyId)
        {
            decimal DeliveryFees = await _context.customerPharmacyDistances.Where(c=>c.CustomerId==CustomerId&&c.PharmacyId==PharmacyId)
                                                                           .Select(c=>c.Fees)
                                                                           .FirstOrDefaultAsync();

            if (DeliveryFees == 0)
                return (0, 0);

            decimal ServiceFees = await _context.SystemSettings.Where(c => c.ServiceId == 1)
                                                               .Select(c => c.Fees)
                                                                .FirstOrDefaultAsync();

            return (DeliveryFees, ServiceFees);
        }

        public async Task<CartCheckOutInfoDTO?> GetCartInfoForCheckOut(int CartId,int CustomerId)
        {
            var result =await _context.Carts.Include(C => C.cartItems)
                                            .Where(C => C.CartId == CartId && C.CustomerId == CustomerId)
                                            .FirstOrDefaultAsync();

            if (result is null)
                return null;

            (decimal, decimal) DeliveryAndServiceFees = await GetDeliveryAndServiceFees(CustomerId,(int)result.PharmacyId);

            if (DeliveryAndServiceFees.Item1 == 0 )
                return null;

            decimal SubFees = result.cartItems.Sum(c => c.Price);
            decimal TotalFees = SubFees + DeliveryAndServiceFees.Item2 + DeliveryAndServiceFees.Item1;


            return new CartCheckOutInfoDTO()
            {
                CartId = result.CartId,
                TotalPrice = TotalFees,
                SubPrice = SubFees,
                ServiceFees = DeliveryAndServiceFees.Item2,
                DeliveryFees = DeliveryAndServiceFees.Item1
            };
        }

        public async Task<bool> IsCustomerLocationChangedAtCheckOut(LocationCheckRequest checkRequest)
        {
            (decimal Longitude, decimal Latitude) longAndLat = await _context.Customers
                                                                            .Where(c => c.CutomerId == checkRequest.CustomerId)
                                                                            .Select(c => new ValueTuple<decimal, decimal>(c.Longitude, c.Latitude))
                                                                            .FirstOrDefaultAsync();

            if (longAndLat.Longitude == 0 || longAndLat.Latitude == 0)
                new Exception("Failed to get data!");

            bool IsLongitudeCorrect = checkRequest.Longitude - longAndLat.Longitude <= AcceptableLocationDiff;
            bool IsLatitudeCorrect = checkRequest.Latitude - longAndLat.Latitude <= AcceptableLocationDiff;

            return !(IsLongitudeCorrect && IsLatitudeCorrect);
        }

        private async Task AddItemsToOrder(int OrderId,ICollection<CartItem> Items)
        {

            var OrderItems = Items.Select(c => new OrderItem()
            {
                PhProductId = c.PhProductId,
                Price = c.Price,
                Quantity = c.Quantity,
                OrderId = OrderId
            });

            await _context.OrderItems.AddRangeAsync(OrderItems);
        }

        private async Task<Order?> AddNewOrder(Cart Cart,int PaymentMethodeId)
        {
            decimal SubPrice = Cart.cartItems.Sum(c => c.Price);
            

            (decimal,decimal) DeliveryAndServiceFees = await GetDeliveryAndServiceFees(Cart.CustomerId,(int)Cart.PharmacyId);


            if(DeliveryAndServiceFees.Item1==0 )
            {
                new Exception("Can't calculate some fees , try again!");
            }

            decimal TotalPrice = SubPrice + DeliveryAndServiceFees.Item1 + DeliveryAndServiceFees.Item2;


            var NewOrder = new Order()
            {
                CustomerId = Cart.CustomerId,
                PharmacyId = (int)Cart.PharmacyId,
                TotalPrice = TotalPrice,
                DeliveryFees = DeliveryAndServiceFees.Item1,
                SubPrice = SubPrice,
                ServiceFees = DeliveryAndServiceFees.Item2,
                OrderStatusId = (int)eOrderStatus.Pending,
                PaymentMethodeId=PaymentMethodeId
            };

            _context.Orders.Add(NewOrder);

            if(await _context.SaveChangesAsync() <=0) 
            {
                return null; 
            }

            return NewOrder;
        }

        private async Task<bool> SetPaymentRecordToDB(AddOrderPaymentDTO paymentDTO)
        {
            return await _paymentServices.AddPaymentRecord(paymentDTO);
        }

        private void SetInvoiceForCustomer(int OrderId,int PaymentMethodeId)
        {
            var invoice = new Invoice()
            {
                OrderId = OrderId,
                IsPaid = PaymentMethodeId != (int)PaymentServices.ePaymentMethode.OnDelivery
            };

            _context.Invoices.Add(invoice);
        }

        private string GetWhatsAppLink(int CartId,int CustomerId)
        {
            string message = $"""
                مرحبا أريد طلب أوردر مع توصيل 
                رقم العميل :{CustomerId}
                رقم السلة الخاص بي:{CartId}
                الرجاء عدم تغيير محتوى الرسالة!
                """;

            string phoneNumber = _configuration.GetSection("PhoneNumber").Value;

            string encodeMessage = Uri.EscapeDataString(message);

            string WhatsAppLink = $"https://wa.me/{phoneNumber}?text={encodeMessage}";

            return WhatsAppLink;
        }

        public async Task<WhatsAppRequestLinkResultDTO> CreateTemporaryOrderAndSendWhatsAppLink(int CartId,int CustomerId)
        {
            WhatsAppRequestLinkResultDTO requestLinkResultDTO = new WhatsAppRequestLinkResultDTO() { IsDone = false, ErrorMessage = "" };

            bool IsCustomerHasActiveCart = await _context.Carts.Where(C=>C.CartId==CartId && C.CustomerId==CustomerId && C.PharmacyId!=null).AnyAsync();

            if(!IsCustomerHasActiveCart)
            {
                requestLinkResultDTO.ErrorMessage = "You Cart is empty";
                return requestLinkResultDTO;
            }

            var TempOrder = new TempOrderRequest()
            {
                OrderCartId = CartId,
                CustomerId = CustomerId,
            };

            _context.TempOrderRequests.Add(TempOrder);

            if(await _context.SaveChangesAsync()<=0)
            {
                requestLinkResultDTO.ErrorMessage = "Faild to checkout this order!";
                return requestLinkResultDTO;    
            }


            requestLinkResultDTO.url = GetWhatsAppLink(CartId, CustomerId);
            requestLinkResultDTO.IsDone = true;

            return requestLinkResultDTO;
        }

        public async Task<AddingOrderResult> HandelAddingNewOrderByOnDelivreyPayment(CheckOutDTO checkOutDTO)
        {
            AddingOrderResult result = new AddingOrderResult() { OrderID = -1, IsDone = false };

            using (var transaction = _context.Database.BeginTransaction())
            {

                var Cart = await _context.Carts.Include(c => c.cartItems).FirstOrDefaultAsync(c => c.CartId == checkOutDTO.CartId && c.CustomerId==checkOutDTO.CustomerId);

                if (Cart is null || Cart.cartItems is null)
                {
                    return result;
                }

                Order? order = await AddNewOrder(Cart,checkOutDTO.PaymentMethodId);

                if (order is null)
                {
                    return result;
                }

                await AddItemsToOrder((int)order.OrderId, Cart.cartItems);

                await ClearCartFromItemsWithoutSave(Cart);

                SetInvoiceForCustomer(order.OrderId, checkOutDTO.PaymentMethodId);

                _context.SaveChanges();
               

                AddOrderPaymentDTO paymentDTO = new AddOrderPaymentDTO()
                {
                    OrderId = order.OrderId,
                    CustomerId = order.CustomerId,
                    PaymentMethodeId = checkOutDTO.PaymentMethodId,
                    TotalPrice = order.TotalPrice
                };

                if(!await SetPaymentRecordToDB(paymentDTO))
                {
                    return result;
                }

                await transaction.CommitAsync();

                result.OrderID = (int)order.OrderId;
                result.IsDone = true;

                return result; 
            }
        }

        public async Task<List<OrderHistoryForCustomer>?> ShowOrdersHistoryForCustomer(int CustomerId)
        {
            var Orders =await _context.Orders.Where(O => O.CustomerId == CustomerId)
                                            .OrderByDescending(O=>O.OrderId)
                                            .Select(O => new OrderHistoryForCustomer()
                                            {
                                                Id = O.OrderId,
                                                PharmacyId = O.PharmacyId,
                                                Date = O.OrderDateAndTime,

                                                PharmacyName = O.Pharmacy.ArabicName,
                                                PhImageURL = O.Pharmacy.ImageURL,

                                                Status = new OrderStatusDTO()
                                                {
                                                    Id = O.OrderStatusId,
                                                    Name = O.OrderStatus.StatusName,
                                                    ArabicName = O.OrderStatus.StatusNameArabic
                                                }
                                            })
                                            .ToListAsync();

            return Orders;
        }

        public async Task<CustomerActiveOrderDTO?> ShowMostActiveOrderForCustomer(int CustomerId)
        {
            var Order = await _context.Orders.Where(O => O.CustomerId==CustomerId)
                                               .OrderByDescending(O=>O.OrderId)
                                               .Select(O => new CustomerActiveOrderDTO()
                                               {
                                                   Id = O.OrderId,
                                                   PharmacyId = O.PharmacyId,
                                                   Date = O.OrderDateAndTime,

                                                   PharmacyName = O.Pharmacy.Name,
                                                   PhImageURL = O.Pharmacy.ImageURL,

                                                   Status = new OrderStatusDTO()
                                                   {
                                                       Id = O.OrderStatusId,
                                                       Name = O.OrderStatus.StatusName,
                                                       ArabicName = O.OrderStatus.StatusNameArabic
                                                   }
                                               })
                                               .FirstOrDefaultAsync();

            if (Order is null)
                return null;


            bool isNotAccepted = Order.Status.Id == (int)eOrderStatus.NotAccepted;
            bool isCanceledOrBeyond = Order.Status.Id >= (int)eOrderStatus.Canceled;

            if (!isNotAccepted)
            {
                if (isCanceledOrBeyond)
                    return null;

                Order.IsNotAccepted = false;
            }
            else
            {
                Order.IsNotAccepted = true;
            }

            Order.IsTheSameDay = Order.Date.Date == DateTime.Now.Date;

            return Order;
        }

        public async Task<OrderDetailsForCustomerDTO?> ShowOrderDetailsForCustomer(int OrderId,int CustomerId)
        {
            var OrderDetails = await _context.Orders.Where(O => O.OrderId == OrderId && O.CustomerId == CustomerId)
                                                      .Select(O => new OrderDetailsForCustomerDTO()
                                                      {
                                                          Id = O.OrderId,
                                                          Pharmacy = new PharmacyDTO()
                                                          {
                                                              PharmacyId = O.PharmacyId,
                                                              Name = O.Pharmacy.Name,
                                                              ArabicName = O.Pharmacy.ArabicName,
                                                              ImageURL = O.Pharmacy.ImageURL,
                                                              Phone=O.Pharmacy.PhoneNumber
                                                          },
                                                          TotalPrice = O.TotalPrice,
                                                          SubPrice=O.SubPrice,
                                                          DeliveryFees=O.DeliveryFees,
                                                          ServiceFees=O.ServiceFees,
                                                          PaymentMethodeId=O.PaymentMethodeId,
                                                          Status = new OrderStatusDTO()
                                                          {
                                                              Id = O.OrderStatusId,
                                                              Name = O.OrderStatus.StatusName,
                                                              ArabicName = O.OrderStatus.StatusNameArabic
                                                          },
                                                          DateOfOrder = O.OrderDateAndTime
                                                      })
                                                      .FirstOrDefaultAsync();

            if (OrderDetails is null)
                return null;

            List<OrderItemDTO>? OrderItems = await GetOrderItemsForOrder(OrderId);

            if (OrderItems is null)
                return null;


            OrderDetails.IsAbleToCanceled = OrderDetails.Status.Id <= (int)eOrderStatus.Preparing;
            OrderDetails.Products = OrderItems;

            return OrderDetails;
        }
        
        private async Task<List<OrderItemDTO>?> GetOrderItemsForOrder(int OrderId)
        {
            var Items =await _context.OrderItems.Where(O => O.OrderId == OrderId)
                                                   .Select(O => new OrderItemDTO()
                                                   {
                                                      Name= O.PhPramacyProduct.SysProduct.ProductName,
                                                      Quantity=O.Quantity,
                                                      Price=O.Price
                                                   })
                                                   .ToListAsync();
            return Items;
        }

        public async Task<OrderStatusDTO?> GetOrderStatus(int OrderId)
        {
            var Status = await _context.Orders.Where(O => O.OrderId == OrderId)
                                             .Select(O => new OrderStatusDTO()
                                             {
                                                 Id = O.OrderStatusId,
                                                 Name = O.OrderStatus.StatusName,
                                                 ArabicName = O.OrderStatus.StatusNameArabic
                                             })
                                             .FirstOrDefaultAsync();

            return Status;
        }

        private bool CancelOrder(CancelOrderInfo orderInfo)
        {
            if (orderInfo.OrderStatusId > (int)eOrderStatus.Preparing)
            {
                return false;
            }

            var Order = new Order()
            {
                OrderId = orderInfo.OrderId,
                OrderStatusId = (int)eOrderStatus.Canceled
            };

            _context.Attach(Order);
            _context.Entry(Order).Property(P => P.OrderStatusId).IsModified = true;

            return true;
        }

        private async Task<bool> DeleteCustomerInvoice(int OrderId)
        {
            await _context.Invoices.Where(I => I.OrderId == OrderId).ExecuteDeleteAsync();

            return true;
        }
        
        public async Task<bool> HandelCancelOrder(int OrderId)
        {
            var OrderInfo = await _context.OrderPayments.Include(P => P.Order)
                                                  .Where(P=>P.OrderId==OrderId)
                                                  .Select(P => new CancelOrderInfo()
                                                  {
                                                      OrderId = OrderId,
                                                      PaymentMethodeId = P.PaymentMethodeId,
                                                      OrderStatusId = P.Order.OrderStatusId
                                                  })
                                                  .FirstOrDefaultAsync();

            if (OrderInfo is null)
            {
                throw new Exception("No Order Find, Server Error!");
            }

            if (!CancelOrder(OrderInfo))
            {
                return false;
            }

            if(OrderInfo.PaymentMethodeId == (int)PaymentServices.ePaymentMethode.OnDelivery)
            {
                await DeleteCustomerInvoice(OrderId);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<OrderListResultDTO?> GetActiveOrderForPharmacy(PaginatedOrderDTO paginatedOrderDTO)
        {

            var baseQuery = _context.Orders.Where(O => O.PharmacyId == paginatedOrderDTO.PharmacyId
                                               && O.OrderStatusId > (int)eOrderStatus.Canceled
                                               && O.OrderStatusId <= (int)eOrderStatus.Successfullsending);


            var Orders =await baseQuery.AsNoTracking()
                                      .OrderBy(O => O.OrderId)
                                      .Skip((paginatedOrderDTO.PageNumber - 1) * paginatedOrderDTO.Limit)
                                      .Take(paginatedOrderDTO.Limit)
                                      .Select(O => new OrdersListDTO()
                                      {
                                          OrderId = O.OrderId,
                                          CustomerId = O.CustomerId,
                                          Status = new OrderStatusDTO()
                                          {
                                              Id = O.OrderStatusId,
                                              Name = O.OrderStatus.StatusName,
                                              ArabicName = O.OrderStatus.StatusNameArabic
                                          }
                                      })
                                      .ToListAsync();

            string? PrevPage = paginatedOrderDTO.PageNumber - 1 == 0 ? null : (paginatedOrderDTO.PageNumber - 1).ToString();

            int TotalRows = baseQuery.Count();


            decimal TotalPages = Math.Ceiling((decimal)TotalRows / paginatedOrderDTO.Limit);

            return new OrderListResultDTO()
            {
                Orders = Orders,
                PrevPage = PrevPage,
                CurrentPage = paginatedOrderDTO.PageNumber,
                NextPage = "",
                RowsCount = TotalRows,
                TotalPages = (int)TotalPages

            };
        }

        public async Task<int> GetNumberOfActiveOrder(int PharmacyId)
        {
            var baseQuery =  _context.Orders.Where(O => O.PharmacyId == PharmacyId
                                                   && O.OrderStatusId > (int)eOrderStatus.Canceled
                                                   && O.OrderStatusId <= (int)eOrderStatus.Successfullsending);

            int TotalRow = await baseQuery.CountAsync();

            return TotalRow;
        }

    }
}
