using DatabaseLayer.Entities;
using DTOs.ReqestDTOs.Meeting;
using DTOs.RequestDTOs.Meeting;
using DTOs.ResultDTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.PharamacyDTOs;
using DTOs.ReqestDTOs.Refund;
using static Services.OrderServices;
using static Services.TokenServices;
using DTOs.ProductDTOs;

namespace Services
{
    public class RequestServices
    {
        public enum eRequestStatus { Successfullsending=1,Pending=2, Accepted=3, NotAccepted=4, Cancled=5, Finished=6 };
        public enum eRefundType { Meeting=1,Order=2 };
        public enum eRefundStatus { Successfullsending=1, Pending=2, Accepted=3, Rejected=4 };
        public enum eRequestType { Meeting=1,Refund=2}

        private readonly AppDbContext _context;
        private PaymentServices _paymentServices;
        public RequestServices(AppDbContext context)
        {
            _context = context; 
            _paymentServices = new PaymentServices(context);
        }

        public async Task<RequestMeeting?> AddNewMeetingRequest(NewMeetingReqDTO meetingReqDTO)
        {
            var NewRequest = new RequestMeeting()
            {
                CustomerId = meetingReqDTO.CustomerId,
                PharmacyId = meetingReqDTO.PharmacyId,
                RequestStatusId = (int)eRequestStatus.Pending
            };

            _context.RequestMeetings.Add(NewRequest);

            if(await _context.SaveChangesAsync() <=0)
            {
                return null;
            }

            return NewRequest;
        }
        
        public async Task<AddingMeetingResultDTO> HandelNewMeetingRequest(NewMeetingReqDTO newMeetingReqDTO,decimal PaidAmount)
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                RequestMeeting? requestMeeting = await AddNewMeetingRequest(newMeetingReqDTO);

                if(requestMeeting is null)
                {
                    return new AddingMeetingResultDTO() { IsSuccess = false };
                }

                MeetingPayDTO meetingPayDTO = new MeetingPayDTO()
                {
                    RequestId = requestMeeting.RequestId,
                    CustomerId = requestMeeting.CustomerId,
                    PaidAmount = PaidAmount,
                    PharmacyId = requestMeeting.PharmacyId
                };

                if (!await _paymentServices.AddMeetingReqPayRecord(meetingPayDTO))
                {
                    return new AddingMeetingResultDTO() { IsSuccess = false };
                }
                
                await transaction.CommitAsync();

                return new AddingMeetingResultDTO()
                {
                    IsSuccess = true,
                    Status=new MeetingReqStatusDTO()
                    {
                        Id=(int)eRequestStatus.Pending,
                        Name="pending",
                        ArabicName="انتظار"
                    }
                };
            }
        }

        public async Task<RequestInfoDTO?> GetMeetingRequestInfo(int RequestId,int CustomerId)
        {
            var RequestInfo = await _context.RequestMeetings.Where(R => R.RequestId == RequestId && R.CustomerId==CustomerId)
                                                              .Select(R => new RequestInfoDTO()
                                                              {
                                                                  RequestId = R.RequestId,
                                                                  Pharmacy = new PharmacyDTO()
                                                                  {
                                                                      PharmacyId = R.PharmacyId,
                                                                      Name = R.Pharmacy.Name,
                                                                      ArabicName = R.Pharmacy.ArabicName,
                                                                      ImageURL = R.Pharmacy.ImageURL
                                                                  },
                                                                  MeetingReqStatus = new MeetingReqStatusDTO()
                                                                  {
                                                                      Id = R.RequestStatusId,
                                                                      RequestId = R.RequestId,
                                                                      ArabicName = R.RequestStatus.ReqStatusArabic,
                                                                      Name = R.RequestStatus.ReqStatusName,
                                                                      MeetingURL = R.MeetingURL
                                                                  },
                                                                  DateOfRequest=R.RequestDateTime,
                                                                  Price=(decimal)R.Pharmacy.VedioCallPrice
                                                              })
                                                              .FirstOrDefaultAsync();
            return RequestInfo;
        }

        public async Task<bool> IsRequestForthisCustomer(int CustomerId,int RequestId,eRequestType requestType)
        {
            bool IsForCustomer = false;

            if(requestType==eRequestType.Meeting)
            {
                IsForCustomer = await _context.RequestMeetings.AnyAsync(R => R.CustomerId == CustomerId && R.RequestId == RequestId);
            }
            else
            {
                IsForCustomer = await _context.RefundRequests.AnyAsync(R=>R.RefundId==RequestId&& R.CustomerId==CustomerId);
            }

            return IsForCustomer;
        }

        public async Task<RequestInfoDTO?> GetMostRecentReqDetailsForCustomer(int CustomerId)
        {
            var RequestInfo = await _context.RequestMeetings.Where(R=>R.CustomerId==CustomerId&&R.RequestStatusId>(int)eRequestStatus.NotAccepted)
                                                              .OrderByDescending(R=>R.RequestId)
                                                              .Select(R => new RequestInfoDTO()
                                                              {
                                                                  RequestId = R.RequestId,
                                                                  Pharmacy = new PharmacyDTO()
                                                                  {
                                                                      PharmacyId = R.PharmacyId,
                                                                      Name = R.Pharmacy.Name,
                                                                      ArabicName = R.Pharmacy.ArabicName,
                                                                      ImageURL = R.Pharmacy.ImageURL
                                                                  },
                                                                  MeetingReqStatus = new MeetingReqStatusDTO()
                                                                  {
                                                                      Id = R.RequestStatusId,
                                                                      RequestId = R.RequestId,
                                                                      ArabicName = R.RequestStatus.ReqStatusArabic,
                                                                      Name = R.RequestStatus.ReqStatusName,
                                                                      MeetingURL = R.MeetingURL
                                                                  },
                                                                  DateOfRequest=R.RequestDateTime,
                                                                  Price=(decimal)R.Pharmacy.VedioCallPrice
                                                              })
                                                              .FirstOrDefaultAsync();
            return RequestInfo;
        }

        public async Task<CustomerActiveRequestDTO?> GetMostActiveReq(int CustomerId)
        {
            var Request = await _context.RequestMeetings.Where(R => R.CustomerId == CustomerId)
                                                          .OrderByDescending(R => R.RequestId)
                                                          .Select(R => new CustomerActiveRequestDTO()
                                                          {
                                                              Id = R.RequestId,
                                                              PharmacyId=R.PharmacyId,
                                                              PharmacyName=R.Pharmacy.ArabicName,
                                                              Status = new MeetingReqStatusDTO()
                                                              {
                                                                  Id = R.RequestStatusId,
                                                                  RequestId = R.RequestId,
                                                                  ArabicName = R.RequestStatus.ReqStatusArabic,
                                                                  Name = R.RequestStatus.ReqStatusName,
                                                                  MeetingURL = R.MeetingURL
                                                              },
                                                              Date=R.RequestDateTime
                                                          })
                                                          .FirstOrDefaultAsync();

            if (Request is null)
                return null;

            bool isNotAccepted = Request.Status.Id == (int)eRequestStatus.NotAccepted;
            bool isNotAcceptedOrBeyond = Request.Status.Id > (int)eOrderStatus.NotAccepted;

            if (!isNotAccepted)
            {
                if (isNotAcceptedOrBeyond)
                    return null;

                Request.IsNotAccepted = false;
            }
            else
            {
                Request.IsNotAccepted = true;
            }

            Request.IsTheSameDay = Request.Date.Date == DateTime.Now.Date;

            return Request;
        }

        public async Task<List<MeetingReqSummeryInfoDTO>?> GetCustomerReqHistory(int CustomerId)
        {
            var MeetingReqRecords = await _context.RequestMeetings.Where(R => R.CustomerId == CustomerId)
                                                                    .OrderByDescending(R=>R.RequestId)
                                                                    .Select(R => new MeetingReqSummeryInfoDTO()
                                                                    {
                                                                        RequsetId = R.RequestId,
                                                                        PharmacyName = R.Pharmacy.Name,
                                                                        PharmacyImageURL = R.Pharmacy.ImageURL,
                                                                        StatusName = R.RequestStatus.ReqStatusName,
                                                                        StatusArabicName = R.RequestStatus.ReqStatusArabic,
                                                                        Date = R.RequestDateTime,
                                                                        StatusId=R.RequestStatusId
                                                                    })
                                                                    .ToListAsync();

            return MeetingReqRecords;
        }

        public async Task<bool> CancelRequest(CancelMeetinReqDTO cancelMeetinReq)
        {
            var Request = await _context.RequestMeetings.FirstOrDefaultAsync(R => R.RequestId == cancelMeetinReq.Id);
            
            if (Request is null)
            {
                throw new Exception("No Requst with this Id!");
            }

            if(Request.RequestStatusId >(int)eRequestStatus.Pending)
            {
                return false;
            }

            Request.RequestStatusId = (int)eRequestStatus.Cancled;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<MeetingReqStatusDTO?> GetMeetingReqStatusForCustomer(int CustomerId)
        {
            var RequestStatus = await _context.RequestMeetings.Where(R => R.CustomerId == CustomerId 
                                                                 && R.RequestStatusId<=(int)eRequestStatus.Accepted)
                                                                .OrderByDescending(R => R.RequestId)
                                                                .Select(R => new MeetingReqStatusDTO()
                                                                {
                                                                    Id = R.RequestStatusId,
                                                                    RequestId = R.RequestId,
                                                                    Name = R.RequestStatus.ReqStatusName,
                                                                    ArabicName = R.RequestStatus.ReqStatusArabic,
                                                                    MeetingURL = R.MeetingURL
                                                                })
                                                                .FirstOrDefaultAsync();
            return RequestStatus;
        }

        public async Task<bool> ChangeRequestStatus(UpdateMeetingStatus updateMeeting)
        {
            var Request = await _context.RequestMeetings.FirstOrDefaultAsync(R => R.RequestId == updateMeeting.RequestId);

            if(Request is null)
            {
                throw new Exception("Server Error, not data!");
            }

            bool condition = Request.RequestStatusId == (int)eRequestStatus.NotAccepted || Request.RequestStatusId == (int)eRequestStatus.Cancled
                             || Request.RequestStatusId == (int)eRequestStatus.Finished;

            if (condition)
            {
                return false;
            }

            Request.RequestStatusId = updateMeeting.StatusId;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> StartMeeting(StartMeetingDTO startMeetingDTO)
        {
            //Meeting logic in Frontend , I used custome library api for it !!!

            var Request = _context.RequestMeetings.FirstOrDefault(R => R.RequestId == startMeetingDTO.RequestId);

            if(Request is null)
            {
                throw new Exception("Server Error, not data!");
            }

            Request.RequestStatusId = (int)eRequestStatus.Accepted;
            Request.MeetingURL=startMeetingDTO.MeetingURL;

            return await _context.SaveChangesAsync() > 0;
        }

        public bool IsEndMeetingValuesCorrect(EndMeetingDTO endMeetingDTO)
        {
            if (endMeetingDTO.UserId <= 0 || endMeetingDTO.PharmacyId <= 0 || endMeetingDTO.RequestId <= 0)
                return false;

            return true;
        }

        public async Task<bool> EndMeeting(EndMeetingDTO endMeetingDTO)
        {
            var Request = await _context.RequestMeetings.FirstOrDefaultAsync(R => R.RequestId == endMeetingDTO.RequestId);

            if(Request is null)
            {
                throw new Exception("Server Network Error!");
            }

            if (Request.RequestStatusId != (int)eRequestStatus.Accepted)
            {
                return false;
            }

            Request.RequestStatusId = (int)eRequestStatus.Finished;

            var NewMeeting = new Meeting()
            {
                RequestId = Request.RequestId,
                RefferenceId = endMeetingDTO.UserId,
                UserTypeId = endMeetingDTO.role.ToLower() == "admin" ? (int)eUserType.Admin : (int)eUserType.Manager,
                StartDateTime = endMeetingDTO.StartDate,
                EndDateTime = endMeetingDTO.EndDate,
                Duration = endMeetingDTO.Duration,
            };

            _context.Meetings.Add(NewMeeting);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<MeetingsListHistoryResultDTO?> GetRequestHistoryForAdmin(PagenaitedReqHistoryDTO pagenaitedReqHistoryDTO)
        {
            var RequestRecords = await _context.RequestMeetings.Where(R => R.PharmacyId == pagenaitedReqHistoryDTO.PharmacyId)
                                                                 .OrderByDescending(R => R.RequestId)
                                                                 .Select(R => new MeetingReqInfoForAdminDTO()
                                                                 {
                                                                     RequestId = R.RequestId,
                                                                     CustomerId = R.CustomerId,
                                                                     Status = new MeetingReqStatusDTO()
                                                                     {
                                                                         RequestId = R.RequestId,
                                                                         Id = R.RequestStatusId,
                                                                         ArabicName = R.RequestStatus.ReqStatusArabic,
                                                                         Name = R.RequestStatus.ReqStatusName,
                                                                     }
                                                                 })
                                                                 .ToListAsync();

            if (RequestRecords is null)
            {
                return null;
            }

            int? PrevPage = pagenaitedReqHistoryDTO.PageNumber == 1 ? null : pagenaitedReqHistoryDTO.PageNumber - 1;

            int RowsCount = _context.PhPramacyProducts.Where(ph => ph.PharamcyId == pagenaitedReqHistoryDTO.PharmacyId).Count();


            return new MeetingsListHistoryResultDTO()
            {
                Meetings = RequestRecords,
                PrevPage = PrevPage.ToString(),
                NextPage = "",
                Total=RowsCount
            };
        }

        public async Task<ActiveMeetingListDTO?> GetActiveMeetingRequest(PagenaitedReqHistoryDTO pagenaitedReqHistoryDTO)
        {
            var baseQuery = _context.RequestMeetings.Where(R => R.PharmacyId == pagenaitedReqHistoryDTO.PharmacyId
                                                        && R.RequestStatusId >= (int)eRequestStatus.Successfullsending
                                                        && R.RequestStatusId <= (int)eRequestStatus.Accepted
                                                        && R.RequestId > pagenaitedReqHistoryDTO.LastReqMeetingId);


            var Requests = await baseQuery.Take(pagenaitedReqHistoryDTO.Limit)
                                                        .Select(R => new MeetingReqInfoForAdminDTO()
                                                        {
                                                            RequestId = R.RequestId,
                                                            CustomerId = R.CustomerId,
                                                            Status = new MeetingReqStatusDTO()
                                                            {
                                                                Id = R.RequestStatusId,
                                                                RequestId = R.RequestId,
                                                                Name = R.RequestStatus.ReqStatusName,
                                                                ArabicName = R.RequestStatus.ReqStatusArabic
                                                            }
                                                        })
                                                        .ToListAsync();


            if (Requests is null)
            {
                return null;
            }

            int? PrevPage = pagenaitedReqHistoryDTO.PageNumber == 1 ? null : pagenaitedReqHistoryDTO.PageNumber - 1;

            int Total = await baseQuery.CountAsync();


            return new ActiveMeetingListDTO()
            {
                Meetings = Requests,
                PrevPage = PrevPage.ToString(),
                NextPage = "",
                Total = Total
            };

        }

        public async Task<bool> IsCustomerHasAnyActiveMeetingRequest(int CustomerId)
        {
            bool IsExsits = await _context.RequestMeetings.AnyAsync(R => R.CustomerId == CustomerId
                                                             && R.RequestStatusId <= (int)eRequestStatus.Accepted);

            return IsExsits;
        }

        public eRefundType ? GetRefundType(string Name)
        {
            if (Name.ToLower() == "meeting")
                return eRefundType.Meeting;

            if (Name.ToLower() == "order")
                return eRefundType.Order;

            return null;
        }

        public async Task<AddingRefundReqDTO> AddNewRefundReqForMeeting(NewRefundReqDTO refundReqDTO, eRefundType refundType)
        {
            AddingRefundReqDTO addingRefundReqDTO = new AddingRefundReqDTO() { IsAccepted = false, message = "Failed To Add Request!", IsError = true };

            if (refundType != eRefundType.Meeting)
            {
                return addingRefundReqDTO;
            }

            var RequestStatusId =await  _context.RequestMeetings.Where(R => R.RequestId == refundReqDTO.RefferenceId)
                                                                  .Select(R => R.RequestStatusId)
                                                                  .FirstOrDefaultAsync();

            if(RequestStatusId == 0)
            {
                return addingRefundReqDTO;
            }

            bool condition = RequestStatusId == (int)eRequestStatus.Cancled || RequestStatusId == (int)eRequestStatus.NotAccepted;

            if (!condition)
            {
                addingRefundReqDTO.IsError = false;
                addingRefundReqDTO.message = "You can't make refund request for this meeting booking!";

                return addingRefundReqDTO;
            }

            var NewRefundReq = new RefundRequest()
            {
                CustomerId = refundReqDTO.CustomerId,
                RefferenceId = refundReqDTO.RefferenceId,
                RefundTypeId = (int)refundType,
                AdditionalInformation = refundReqDTO.AdditionalInformation,
                RefundStatusId = (int)eRefundStatus.Pending
            };

            _context.RefundRequests.Add(NewRefundReq);

            if (await _context.SaveChangesAsync() <= 0)
                return addingRefundReqDTO;

            addingRefundReqDTO.IsError = false;
            addingRefundReqDTO.message = "Is Done!";
            addingRefundReqDTO.IsAccepted = true;

            return addingRefundReqDTO;
        }

        public async Task<AddingRefundReqDTO> AddNewRefundReqForOrder(NewRefundReqDTO refundReqDTO, eRefundType refundType)
        {
            AddingRefundReqDTO addingRefundReqDTO = new AddingRefundReqDTO() { IsAccepted = false, message = "Failed To Add Request!",IsError=true };

            if (refundType != eRefundType.Order)
            {
                return addingRefundReqDTO;
            }

            var OrderStatusId = await _context.Orders.Where(O => O.OrderId == refundReqDTO.RefferenceId && O.CustomerId==refundReqDTO.CustomerId)
                                                       .Select(O => O.OrderStatusId)
                                                       .FirstOrDefaultAsync();

            if(OrderStatusId==0)
            {
                return addingRefundReqDTO;
            }

            bool condition = OrderStatusId == (int)eOrderStatus.Canceled
                             || OrderStatusId == (int)eOrderStatus.NotAccepted;

            if(!condition)
            {
                addingRefundReqDTO.message = "You can't make refund for this order!";
                addingRefundReqDTO.IsError = false;

                return addingRefundReqDTO;
            }

            var NewRefundReq = new RefundRequest()
            {
                CustomerId = refundReqDTO.CustomerId,
                RefferenceId = refundReqDTO.RefferenceId,
                RefundTypeId = (int)refundType,
                AdditionalInformation = refundReqDTO.AdditionalInformation,
                RefundStatusId = (int)eRefundStatus.Pending
            };

            _context.RefundRequests.Add(NewRefundReq);

            if(await _context.SaveChangesAsync()<=0)
            {
                return addingRefundReqDTO;
            }

            addingRefundReqDTO.IsAccepted = true;
            addingRefundReqDTO.message = "Is Done!";
            addingRefundReqDTO.IsError = false;

            return addingRefundReqDTO;
        }

        public async Task<List<RefundReqDetailDTO>?> RefundReqHistoryForCustomer(int customerId)
        {
            var Requests = await _context.RefundRequests.Where(R => R.CustomerId == customerId)
                                                  .OrderByDescending(R => R.RefundId)
                                                  .Select(R => new RefundReqDetailDTO()
                                                  {
                                                      Id = R.RefundId,
                                                      RefferenceId = R.RefferenceId,
                                                      DateAndTimeOfRequest = R.DateAndTimeOfRequest,
                                                      TypeName=R.RefundTypeId==(int)eRefundType.Order?"order":"meeting",
                                                      Status = new RefundStatusDTO()
                                                      {
                                                          Id = R.RefundStatusId,
                                                          Name = R.RefundStatus.Name,
                                                          ArabicName = R.RefundStatus.NameArabic
                                                      }
                                                  })
                                                  .ToListAsync();

            return Requests;
        }

        private bool AcceptRefundReq(RefundRequest refundRequest)
        {
            if(refundRequest.RefundTypeId==(int)eRefundType.Order)
            {
                var updatePayment = new OrderPayment()
                {
                    OrderId = refundRequest.RefferenceId,
                    IsRefund = true
                };

                _context.Attach(updatePayment);
                _context.Entry(updatePayment).Property(P => P.IsRefund).IsModified = true;

                return true;
            }
            else
            {
                var updatePayment = new MeetingPayment()
                {
                    RequestId = refundRequest.RefferenceId,
                    IsRefund = true
                };

                _context.Attach(updatePayment);
                _context.Entry(updatePayment).Property(P => P.IsRefund).IsModified = true;

                return true;

            }
        }
        
        public async Task<bool> ChangeRefundReqStatus(UpdateRefundReqStatusDTO statusDTO)
        {
            var refundRequest = await _context.RefundRequests.FirstOrDefaultAsync(R => R.RefundId == statusDTO.RefundReqId);

            if(refundRequest is null)
            {
                throw new Exception("Server Network Error, No Req with this Id!");
            }

            if(statusDTO.StatusId == (int)eRefundStatus.Accepted)
            {
                AcceptRefundReq(refundRequest);
            }

            refundRequest.RefundStatusId = statusDTO.StatusId;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddNewTempRequest(int CustomerId,int PharmacyId)
        {
            var TempReqeustEntity = new TempMeetingRequest()
            {
                PharmacyId = PharmacyId,
                CustomerId = CustomerId
            };

            _context.TempMeetingRequests.Add(TempReqeustEntity);

            return await _context.SaveChangesAsync() > 0;
        }


    }
}
