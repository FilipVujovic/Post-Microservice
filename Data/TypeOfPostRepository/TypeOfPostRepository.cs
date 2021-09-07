using AutoMapper;
using PostService.Entities;
using PostService.Entities.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Data.TypeOfPostRepository
{
    public class TypeOfPostRepository : ITypeOfPostRepository
    {
        private readonly DatabaseContext dbContext;
        private readonly IMapper autoMapper;
        public TypeOfPostRepository(DatabaseContext dbContext, IMapper autoMapper)
        {
            this.dbContext = dbContext;
            this.autoMapper = autoMapper;
        }

        public TypeOfPostCreatedConfirmation CreateType(TypeOfPost Type)
        {
            dbContext.TypeOfPost.Add(Type);
            dbContext.SaveChanges();
            return autoMapper.Map<TypeOfPostCreatedConfirmation>(Type);
        }

        public void DeleteType(Guid TypeId)
        {
            var type = GetTypeById(TypeId);
            dbContext.Remove(type);
            dbContext.SaveChanges();
        }

        public TypeOfPost GetTypeById(Guid TypeId)
        {
            return dbContext.TypeOfPost.FirstOrDefault(t => t.TypeOfPostId == TypeId);
        }

        public List<TypeOfPost> GetTypes()
        {
            return dbContext.TypeOfPost.ToList();
        }

        public void UpdateType(Guid TypeId)
        {
            throw new NotImplementedException();
        }

        public Guid GetIdByType(string Type)
        {
            var typeOfPost = dbContext.TypeOfPost.FirstOrDefault(t => t.Type == Type);
            return typeOfPost.TypeOfPostId;
        }

        public bool ContainsType(string Type)
        {
            if(dbContext.TypeOfPost.FirstOrDefault(t => t.Type == Type) != null)
            {
                return true;
            }
            return false;
        }
    }
}
