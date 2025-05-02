using DatabaseLayer.Entities;
using DTOs.OrderDTOs;
using DTOs.RequestDTOs.Meeting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PaymentServices
    {
        public enum ePaymentMethode { Paypal =1, MasterCard =2,Visa=3, OnDelivery =4};

        private readonly AppDbContext _context;

        public PaymentServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddPaymentRecord(AddOrderPaymentDTO paymentDTO)
        {
            decimal PaidAmount = paymentDTO.PaymentMethodeId == (int)ePaymentMethode.OnDelivery ? 0 : paymentDTO.TotalPrice;


            var PayRecord = new OrderPayment()
            {
                OrderId = paymentDTO.OrderId,
                CustomerId = paymentDTO.CustomerId,
                PaymentMethodeId = paymentDTO.PaymentMethodeId,
                PaidAmount = PaidAmount,
            };

            _context.OrderPayments.Add(PayRecord);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddInvoiceForCustomer(InvoiceDTO invoiceDTO)
        {
            var PaymentRecord= await _context.OrderPayments.FirstOrDefaultAsync(P => P.OrderId == invoiceDTO.OrderId);

            if(PaymentRecord is null)
            {
                throw new Exception("No Payment was found for this order id!");
            }

            bool IsPaid = PaymentRecord.PaidAmount != 0.0M;

            var Invoice = new Invoice()
            {
                OrderId = invoiceDTO.OrderId,
                IsPaid = IsPaid,
            };

            _context.Invoices.Add(Invoice);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddMeetingReqPayRecord(MeetingPayDTO payDTO)
        { 
            var NewPay = new MeetingPayment() 
            {
               RequestId=payDTO.RequestId,
               CustomerId=payDTO.CustomerId,
               PaidAmount=payDTO.PaidAmount,
            };

            _context.MeetingPayments.Add(NewPay);

            return await _context.SaveChangesAsync() > 0;
        }

    }
}
