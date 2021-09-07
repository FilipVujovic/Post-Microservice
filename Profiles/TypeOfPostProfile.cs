using AutoMapper;
using PostService.Entities.Type;
using PostService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Profiles
{
    public class TypeOfPostProfile : Profile
    {

        public TypeOfPostProfile()
        {
            CreateMap<TypeOfPost, TypeOfPostCreatedConfirmation>();
            CreateMap<TypeOfPost, TypeOfPostDto>();
            CreateMap<TypeOfPost, TypeOfPostCreationConfirmationDto>();
            CreateMap<TypeOfPost, TypeOfPost>();
            CreateMap<TypeOfPostCreationDto, TypeOfPost>();
        }

    }
}
