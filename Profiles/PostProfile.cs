using AutoMapper;
using PostService.Entities;
using PostService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<PostCreationDto, Post>();

            CreateMap<Post, PostDto>();

            CreateMap<Post, Post>();

            CreateMap<PostModificationDto, Post>();
        }
    }
}
