using DatabaseLayer.Entities;
using DTOs;
using DTOs.CustomerDTOs;
using Microsoft.Extensions.Configuration;
using Security;
using DTOs.PersonDTOs;
using Microsoft.EntityFrameworkCore;
using System.Net;
using DTOs.RequestDTOs;
using static Services.OrderServices;
using System;



namespace Services
{
    public class CustomerServices
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly SecurityMethode _securityMethode;
        private readonly PersonServices _personServices;

        public CustomerServices(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _securityMethode = new SecurityMethode(configuration);
            _personServices=new PersonServices(context,configuration);
        }

        private string DefaultImageURL = "https://res.cloudinary.com/dlu3aolnh/image/upload/v1735059932/udjkg3nflplvydkrngf0.png";

        public bool IsValiedValues(CustomerRegisterDTO customerRegisterDTO)
        {
            if (customerRegisterDTO.Longitude == 0 || customerRegisterDTO.Latitude == 0||customerRegisterDTO.PersonRegister.PersonId<=0)
                return false;

            PersonRegisterDTO personRegisterDTO = customerRegisterDTO.PersonRegister;

            if(string.IsNullOrEmpty(personRegisterDTO.FirstName)||string.IsNullOrEmpty(personRegisterDTO.LastName)||
                string.IsNullOrEmpty(personRegisterDTO.Phone))
            {
                return false;
            }

            return true;
        }

        private CustomerDTO TransferToCustomerDTO(Customer NewCustomer, Person NewPerson)
        {
            return new CustomerDTO()
            {
                CustomerId = NewCustomer.CutomerId,
                Person = new PersonDTO()
                {
                    FirstName = NewPerson.FirstName,
                    LastName = NewPerson.LastName,
                    ProfileImageLink = NewPerson.ProfileImageURL,
                },
                Longitude = NewCustomer.Longitude,
                Latitude = NewCustomer.Latitude
            };
        }

        public async Task<CustomerDTO?> AddNewCustomer(CustomerRegisterDTO customerRegisterDTO)
        {
            Person? registeredPerson =await _personServices.AddFullRegisterInfo(customerRegisterDTO.PersonRegister);

            if(registeredPerson is null)
            {
                return null;
            }

            var NewCustomer = new Customer()
            {
                PersonId = registeredPerson.PersonId,
                Longitude = customerRegisterDTO.Longitude,
                Latitude = customerRegisterDTO.Latitude,
            };

            _context.Customers.Add(NewCustomer);

            if(await _context.SaveChangesAsync() <=0)
            {
                return null;
            }

            return TransferToCustomerDTO(NewCustomer, registeredPerson);
        }

        private async Task<CustomerDTO?>GetCustomerInfoByPersonId(int PersonId)
        {
            var customerDTO =await _context.Customers
                                          .Where(c => c.PersonId == PersonId)
                                          .Select(c => new CustomerDTO()
                                          {
                                              CustomerId = c.CutomerId,
                                              Person = new PersonDTO
                                              {
                                                  FirstName = c.Person.FirstName,
                                                  LastName = c.Person.LastName,
                                                  ProfileImageLink = c.Person.ProfileImageURL
                                              }
                                          })
                                          .FirstOrDefaultAsync();
            return customerDTO;
        }

        public async Task<CustomerDTO?> CheckEmailAndPassword(string Email,string Password)
        {

            PersonWithPassword? person = _context.Persons
                                        .Where(P =>P.Email == Email)
                                        .Select(P => new PersonWithPassword()
                                        {
                                            PersonId = P.PersonId,
                                            Password = P.Password
                                        })
                                        .FirstOrDefault();

            if (person == null)
            {
                return null;
            }


            if(person.Password == null || !_securityMethode.VerifyEncryptPassword(person.Password,Password))
            {
                return null;
            }

            var customerDTO = await GetCustomerInfoByPersonId(person.PersonId.Value);

            return customerDTO;
        }

        public async Task HandelCustomerLoggedIn(int CustomerId)
        {
            var customer = await _context.Customers.Where(c=>c.CutomerId==CustomerId).FirstOrDefaultAsync();

            if(customer==null)
            {
                throw new NullReferenceException("No Customer with this Id");
            }

            customer.IsOnline = true;
            customer.LastLoggedInDateTime=DateTime.Now;


            var NewCustomerLog = new CustomerLog()
            {
                CustomerId=CustomerId
            };

            await _context.SaveChangesAsync();
        }

        public async Task<CustomerDTO?> UpdateCustomerInfo(UpdateCustomerDTO updateCustomerDTO)
        {
            var customer = await _context.Customers.Include(C=>C.Person).FirstOrDefaultAsync(c => c.CutomerId == updateCustomerDTO.CustomerId); 

            if(customer is null||customer.Person is null)
            {
                throw new ArgumentNullException("No customer with this Id!");
            }



            customer.Person.FirstName = updateCustomerDTO.Person.FirstName;
            customer.Person.LastName = updateCustomerDTO.Person.LastName;
            customer.Person.Phone= "0"+updateCustomerDTO.Person.Phone;


            if( await _context.SaveChangesAsync()<=0)
            {
                return null;
            }


            return new CustomerDTO()
            {
                CustomerId=customer.CutomerId,
                Person=new PersonDTO()
                {
                    FirstName= customer.Person.FirstName,
                    LastName= customer.Person.LastName,
                    ProfileImageLink= customer.Person.ProfileImageURL
                },
                Longitude=customer.Latitude,
                Latitude=customer.Longitude
            };
        }

        public async Task<bool> ChangeEmail(ChangeEmailDTO changeEmailDTO)
        {

            Customer? Customer = await _context.Customers.Include(c => c.Person)
                                                         .FirstOrDefaultAsync(c => c.CutomerId == changeEmailDTO.UserId);
                                 

            if (Customer==null || Customer.Person==null)
            {
                return false;
            }

            Customer.Person.Email = changeEmailDTO.NewEmail;
            
            return await _context.SaveChangesAsync() >0; 
        }

        public async Task<bool> ChangePassword(int CustomerId,string NewPassword)
        {
            Customer? Customer = await _context.Customers.Include(c => c.Person)
                                                   .FirstOrDefaultAsync(c => c.CutomerId == CustomerId);


            if (Customer == null || Customer.Person == null)
            {
                return false;
            }

            string EncryptPassword = _securityMethode.Encrypt(NewPassword);

            Customer.Person.Password = EncryptPassword;

            return await _context.SaveChangesAsync() > 0;
        }
    
        public async Task<bool> IsCustomerHasActiveOrder(int CustomerId)
        {
            bool IsExists = await _context.Orders.Where(O => O.CustomerId == CustomerId && O.OrderStatusId < (int)eOrderStatus.Canceled).AnyAsync();

            return IsExists;
        }

        public async Task<ShowCustomerDTO?> GetCustomerInfo(int CustomerId)
        {
            var customerDTO =await _context.Customers.Where(C => C.CutomerId == CustomerId)
                                                     .Select(C => new ShowCustomerDTO()
                                                     {
                                                         CustomerId = CustomerId,
                                                         Person = new ShowPersonDTO()
                                                         {
                                                             FirstName = C.Person.FirstName,
                                                             LastName = C.Person.LastName,
                                                             Email = C.Person.Email,
                                                             Phone = C.Person.Phone
                                                         },
                                                         Latitude = C.Latitude,
                                                         Longitude = C.Longitude
                                                     })
                                                     .FirstOrDefaultAsync();

            if (customerDTO is null)
                return null;

            customerDTO.Person.Phone = customerDTO.Person.Phone.TrimStart('0');

            return customerDTO;
        }

        public async Task<bool> ChnageLocation(int CustomerId,EditLocationDTO editLocationDTO)
        {
            var customerEntity = new Customer()
            {
                CutomerId = CustomerId,
                Longitude = editLocationDTO.Longitude,
                Latitude = editLocationDTO.Latitude
            };

            _context.Attach(customerEntity);
            _context.Entry(customerEntity).Property(P => P.Longitude).IsModified = true;
            _context.Entry(customerEntity).Property(P => P.Latitude).IsModified = true;

            return await _context.SaveChangesAsync() > 0;
        }

        private class PersonWithPassword
        {
              public int? PersonId { get; set; }
              public string? Password { get; set; }
        }


    }

}
