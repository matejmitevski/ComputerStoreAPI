using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ComputerStore.BLL.DTOs;
using ComputerStore.DAL.Entities;

namespace ComputerStore.BLL.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryResponse>();

            CreateMap<CategoryRequest, Category>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()));

            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src =>
                    src.ProductCategories.Select(pc => pc.Category.Name).ToList()));
        }
    }
}