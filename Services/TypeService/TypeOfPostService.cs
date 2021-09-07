using PostService.Data;
using PostService.Data.TypeOfPostRepository;
using PostService.Entities.Type;
using PostService.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Services.TypeService
{
    public class TypeOfPostService : ITypeOfPostService
    {

        private readonly ITypeOfPostRepository typeRepository;
        private readonly IPostRepository postRepository;

        public TypeOfPostService(ITypeOfPostRepository typeRepository, IPostRepository postRepository)
        {
            this.typeRepository = typeRepository;
            this.postRepository = postRepository;
        }
        public TypeOfPostCreatedConfirmation CreateType(TypeOfPost Type)
        {
            TypeOfPostCreatedConfirmation type = typeRepository.CreateType(Type);
            return type;
        }

        public void DeleteType(Guid TypeId)
        {
            if(typeRepository.GetTypeById(TypeId)==null)
            {
                throw new _404Exception("Type not found!");
            }
            if(postRepository.ContainsType(TypeId))
            {
                throw new GeneralException("Foreign key constraint violated!");
            }
            try
            {
                typeRepository.DeleteType(TypeId);
            } catch (Exception ex)
            {
                throw new GeneralException(ex.Message);
            }
        }

        public TypeOfPost GetTypeById(Guid TypeId)
        {
            if(typeRepository.GetTypeById(TypeId) == null)
            {
                throw new _404Exception("Type not found!");
            }
            try
            {
                return typeRepository.GetTypeById(TypeId);
            } catch (Exception ex)
            {
                throw new GeneralException(ex.Message);
            }
        }

        public List<TypeOfPost> GetTypes()
        {
            return typeRepository.GetTypes();
        }
    }
}
