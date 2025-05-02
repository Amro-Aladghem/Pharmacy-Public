using DatabaseLayer.Entities;
using DTOs.MessageDTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class MessagesServices
    {
        public enum eUserTypes { Admin=1,Customer=2,Manager=3}

        private readonly AppDbContext _context;

        public MessagesServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddNewMessage(NewMessageDTO messageDTO)
        {
            int? AdminId = messageDTO.UserTypeId == (int)eUserTypes.Admin ? messageDTO.AdmingId : null;

            var message = new Message()
            {
                UserTypeId = messageDTO.UserTypeId,
                CustomerId=messageDTO.CustomerId,
                AdminId=AdminId,
                DateOfMessage = messageDTO.DateOfMessage,
                Time = messageDTO.Time,
                message = messageDTO.Message,
                PharmacyId = messageDTO.PharmacyId
            };

            _context.Messages.Add(message);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<MessageDTO>?> GetMessagesHistoryForCustomer(int CustomerId)
        {
            var messages =await _context.Messages.Where(M => M.CustomerId == CustomerId)
                                                .Select(M => new MessageDTO()
                                                {
                                                    UserTypeId = M.UserTypeId,
                                                    MessageId = M.MessageId,
                                                    Message = M.message
                                                })
                                                .ToListAsync();

            return messages;
        }

        public async Task<bool> EndMessagesSession(int CustomerId)
        {
            _context.Messages.Where(M => M.CustomerId == CustomerId).ExecuteDelete();

            return await _context.SaveChangesAsync() > 0;
        }


    }
}
