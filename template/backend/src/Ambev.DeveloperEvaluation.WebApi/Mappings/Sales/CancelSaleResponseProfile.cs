using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.Sales
{
    public class CancelSaleResponseProfile : Profile
    {
        public CancelSaleResponseProfile()
        {
            CreateMap <CancelSaleRequest, CancelSaleCommand>();
        }
    }
}
