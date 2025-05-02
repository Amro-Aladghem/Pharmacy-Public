using DatabaseLayer.Entities;
using DTOs.ProductDTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.ResultDTOs;
using DTOs.PharamacyDTOs;
using DTOs.CustomerDTOs;
using DTOs.DefaultValuesDTOs;
using Microsoft.Data.SqlClient;

namespace Services
{
    public class ProductServices
    {
        private readonly AppDbContext _context;
        
        public ProductServices(AppDbContext context)
        {
            _context = context;
        }

        public bool AreValuesCorrect(NewProductDTO newProductDTO)
        {

            if(newProductDTO is null)
            {
                return false;
            }

            if(newProductDTO.SysProduct.Id == 0 || newProductDTO.SysProduct.Id is null)
            {
                if (string.IsNullOrEmpty(newProductDTO.SysProduct.Name) || string.IsNullOrEmpty(newProductDTO.SysProduct.description)
                     || string.IsNullOrEmpty(newProductDTO.SysProduct.ImageURL))
                {
                    return false;
                }
            }

            var phProduct = newProductDTO.PhProduct;

            if(phProduct.Stoke==0 || phProduct.Price==0 || phProduct.MedicalQuantity==0 || phProduct.MedicalTypeId<=0 
                || phProduct.CategoryId<=0)
            {
                return false;
            }

            if (phProduct.ProducedDate >= phProduct.EndDate)
            {
                return false;
            }

            return true;
        }

        public bool CheckPhProductInfo(PhProductDTO phProduct)
        {
            if (phProduct.Stoke == 0 || phProduct.Price == 0 || phProduct.MedicalQuantity == 0 || phProduct.MedicalTypeId <= 0
                || phProduct.CategoryId <= 0)
            {
                return false;
            }

            if (phProduct.ProducedDate >= phProduct.EndDate)
            {
                return false;
            }

            return true;
        }

        private async Task<int?> AddNewSysProduct(SysProductDTO sysProductDTO)
        {
            var NewSysProduct = new SysProduct()
            {
                ProductName=sysProductDTO.Name,
                Description=sysProductDTO.description,
                ImageURL=sysProductDTO.ImageURL
            };

            _context.SysProducts.Add(NewSysProduct);

            if (await _context.SaveChangesAsync() <= 0)
            {
                return null;
            }

            return NewSysProduct.SysProductId;
        }

        public async Task<bool> AddNewPhProduct(NewProductDTO newProductDTO)
        {
            int ? SysProductId = newProductDTO.SysProduct.Id;

            if(SysProductId == 0 || SysProductId is null)
            {
                SysProductId = await AddNewSysProduct(newProductDTO.SysProduct);

                if (SysProductId is null)
                    throw new Exception("Faild to add Product!");
            }


            var NewPhPrdouct = new PhPramacyProduct()
            {
                PharamcyId = newProductDTO.PhProduct.PharamcyId,
                PhDescription = newProductDTO.PhProduct.PhDescription,
                SysProductId = (int)SysProductId,
                Price = newProductDTO.PhProduct.Price,
                Stoke = newProductDTO.PhProduct.Stoke,
                MedicalQuantity = newProductDTO.PhProduct.MedicalQuantity,
                ProducedDate = newProductDTO.PhProduct.ProducedDate,
                EndDate = newProductDTO.PhProduct.EndDate,
                MedicalTypeId = newProductDTO.PhProduct.MedicalTypeId,
                CategoryId = newProductDTO.PhProduct.CategoryId
            };

            _context.PhPramacyProducts.Add(NewPhPrdouct);

            if(await  _context.SaveChangesAsync()<=0)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UpdatedPhProduct(UpdatedPhProductDTO updatedPhProductDTO)
        {
            var product = await _context.PhPramacyProducts.FirstOrDefaultAsync(p=>p.PhProductId==updatedPhProductDTO.PhProductId); 

            if(product==null)
            {
                return false;
            }

            var phProduct = updatedPhProductDTO.PhProduct;

            product.Price= phProduct.Price;
            product.Stoke= phProduct.Stoke;
            product.MedicalTypeId=phProduct.MedicalTypeId;
            product.EndDate=phProduct.EndDate;
            product.ProducedDate=phProduct.ProducedDate;
            product.MedicalQuantity=phProduct.MedicalQuantity;
            product.PhDescription=phProduct.PhDescription;
            product.CategoryId=phProduct.CategoryId;

            if(await _context.SaveChangesAsync() <=0)
            {
                return false;   
            }

            return true;
        }

        public async Task<bool> DeletePhProduct(int ProductId)
        {
            await _context.PhPramacyProducts.Where(p => p.PhProductId == ProductId)
                                            .ExecuteDeleteAsync();

            if(await _context.SaveChangesAsync() <= 0)
            {
                return false;
            }

            return true;
        }

        private IQueryable<PhPramacyProduct> GetListProductQueryWithFilter(PaginatedPhProductDTO paginatedPhProductDTO,int CategoryId,int ?GovernorateId,int?RegionId)
        {
            var query = _context.PhPramacyProducts.Where(p => p.PhProductId > paginatedPhProductDTO.LastPhProductId && p.CategoryId == CategoryId);

            if(GovernorateId.HasValue)
            {
                query =query.Where(p=>p.Pharmacy.GovernorateId==GovernorateId.Value);
            }

            if(RegionId.HasValue)
            {
                query= query.Where(p=>p.Pharmacy.RegionId==RegionId.Value);
            }

            return query;
        }

        public async Task<ProdutctsListResultDTO?> GetPhProductList(PaginatedPhProductDTO paginatedPhProductDTO,int CategoryId,int ?GovernorateId, int? RegionId)
        {
            var query = GetListProductQueryWithFilter(paginatedPhProductDTO, CategoryId, GovernorateId, RegionId);

            var PhProductsList =await  query.Take(paginatedPhProductDTO.Limit)
                                              .Select(p => new ListingProductDTO()
                                              {
                                                  PhProductId = p.PhProductId,
                                                  Name = p.SysProduct.ProductName,
                                                  Price = p.Price,
                                                  ProductImageURL = p.SysProduct.ImageURL,
                                                  PhName = p.Pharmacy.ArabicName,
                                                  PharmacyId=p.PharamcyId
                                              })
                                              .ToListAsync();


            if (PhProductsList==null)
            {
                return null;
            }


            int RowsCount = 0;

            if(!paginatedPhProductDTO.IsRowsCountCalculated)
            {
                RowsCount = query.Count();
            }


            return new ProdutctsListResultDTO()
            {
                Products = PhProductsList,
                Total = RowsCount,
                LastPhProductId = PhProductsList.Last().PhProductId
            };                    
        }

        public async Task<ProdutctsListResultDTO?> GetPharmacyProductsListById(PaginatedPhProductDTO paginatedPhProductDTO,int PharmacyId)
        {

            var query = _context.PhPramacyProducts.Where(p => p.PharamcyId == PharmacyId
                                                            && p.PhProductId > paginatedPhProductDTO.LastPhProductId);


            var PhProductsList = await query.Take(paginatedPhProductDTO.Limit)
                                               .Select(p => new ListingProductDTO()
                                               {
                                                   PhProductId = p.PhProductId,
                                                   Name = p.SysProduct.ProductName,
                                                   Price = p.Price,
                                                   PhName = p.Pharmacy.ArabicName,
                                                   ProductImageURL = p.SysProduct.ImageURL,
                                                   PharmacyId=p.PharamcyId
                                               })
                                               .ToListAsync();


            if (PhProductsList is null)
            {
                return null;
            }

            int LastPhProductId = PhProductsList.Last().PhProductId;

            int RowsCount = query.Count();
            string? NextPage = "?LastPhProductId=" + LastPhProductId + "&Limit=" + paginatedPhProductDTO.Limit+
                "&PharmacyId="+PharmacyId;

            return new ProdutctsListResultDTO()
            {
                NextPage = NextPage,
                Products = PhProductsList,
                Total = RowsCount,
                LastPhProductId=LastPhProductId
            };
        }

        public async Task<ShowProductDTO?> GetProductInfoForShowing(int ProductId,int PharmacyId)
        {
            var PhProduct = await _context.PhPramacyProducts.Where(ph => ph.PhProductId == ProductId&&ph.PharamcyId==PharmacyId)
                                                              .Select(ph => new ShowProductDTO()
                                                              {
                                                                  PhProductId = ph.PhProductId,
                                                                  Name = ph.SysProduct.ProductName,
                                                                  EndDate = ph.EndDate,
                                                                  ProducedDate = ph.ProducedDate,
                                                                  ImageURL = ph.SysProduct.ImageURL,
                                                                  Price = ph.Price,
                                                                  MedicalQuantity = ph.MedicalQuantity,
                                                                  Stoke=ph.Stoke,
                                                                  MedicalType = new MedicalTypeDTO()
                                                                  {
                                                                      TypeId = ph.MedicalTypeId,
                                                                      MedicalTypeName = ph.MedicalType.MedicalTypeName,
                                                                      MedicalTypeNameArabic = ph.MedicalType.MedicalTypeNameArabic
                                                                  },
                                                                  Pharmacy = new PharmacySummaryDTO()
                                                                  {
                                                                      PharmacyId = ph.PharamcyId,
                                                                      ImageURL=ph.Pharmacy.ImageURL,
                                                                      IsHasDelivery=ph.Pharmacy.IsHasDelivery,
                                                                      Name=ph.Pharmacy.ArabicName
                                                                  }

                                                              })
                                                              .FirstOrDefaultAsync();

            return PhProduct;
        }

        public async Task<string> ReturnPhProductDescription(int ProductId)
        {
            string ? description = await _context.PhPramacyProducts.Where(p=>p.PhProductId==ProductId)
                                                                   .Select(p=>p.SysProduct.Description)
                                                                   .FirstOrDefaultAsync();

            if(description is null)
            {
                return "";
            }

            return description;
        }

        public async Task<ProductListSummeryDTO?> GetProductsForAdmins(PaginatedPhProductDTO paginatedPhProductDTO,int PharmacyId)
        {

            var query = _context.PhPramacyProducts.Where(p => p.PharamcyId == PharmacyId
                                                      && p.PhProductId > paginatedPhProductDTO.LastPhProductId);


            var Products = await query.Take(paginatedPhProductDTO.Limit)
                                         .Select(p => new PhProductSummeryDTO()
                                         {
                                             PhProductId = p.PhProductId,
                                             PhProductName = p.SysProduct.ProductName,
                                             ImageURL = p.SysProduct.ImageURL,
                                             Stoke = p.Stoke,
                                             Price = p.Price
                                         })
                                         .ToListAsync();

            if (Products is null)
            {
                return null;
            }

            int RowsCount = query.Count();

            int LastPhProductId = Products.Last().PhProductId;

            string? NextPage = "?LastPhProductId=" + LastPhProductId + "&Limit=" + paginatedPhProductDTO.Limit;

            return new ProductListSummeryDTO()
            {
                Products = Products,
                NextPage = NextPage,
                Total = RowsCount,
                LastPhProductId = LastPhProductId
            };
        }

        public async Task<PhProductDTO?> ShowProductInfoForAdmin(int PhProductId)
        {
            var phProductDTO = await _context.PhPramacyProducts.Where(p => p.PhProductId == PhProductId)
                                                         .Select(p => new PhProductDTO()
                                                         {
                                                             PharamcyId = p.PhProductId,
                                                             EndDate = p.EndDate,
                                                             ProducedDate = p.ProducedDate,
                                                             Price = p.Price,
                                                             Stoke = p.Stoke,
                                                             PhDescription = p.PhDescription,
                                                             MedicalQuantity = p.MedicalQuantity,
                                                             CategoryId = p.CategoryId,
                                                             MedicalTypeId = p.MedicalTypeId
                                                         })
                                                         .FirstOrDefaultAsync();

            return phProductDTO;
        }
        
        public async Task<ProdutctsListResultDTO?> GetMatchProductFromSearching(string SearchText,int LastPhProductId)
        {
            var Products = await _context.Database
                                       .SqlQueryRaw<ListingProductDTO>("EXEC GetMatchProductaWithSearch @p0,@p1",
                                                                       SearchText, LastPhProductId)
                                       .ToListAsync();

            if (Products is null || Products.Count==0)
                return null;

            string Next = "/search?searchtext=" + SearchText + "&LastPhProductId=" + LastPhProductId;

            return new ProdutctsListResultDTO()
            {
                Products = Products,
                NextPage = Next,
                LastPhProductId = Products.Last().PhProductId
            };
        }

    }
}
