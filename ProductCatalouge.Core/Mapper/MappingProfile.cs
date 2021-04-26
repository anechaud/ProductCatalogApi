using AutoMapper;
using ProductCatalouge.Core.DataTransferObject;
using ProductCatalouge.Core.Models;
using ProductCatalouge.EntityFramework.Models.DB;

namespace ProductCatalouge.Core.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductRequestModel, TblProductDetail>();
            CreateMap<TblProductDetail, ProductDTO>();
            CreateMap<ProductDTO, TblProductDetail>();
        }
    }
}
