using PostService.Entities.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Services.TypeService
{
    public interface ITypeOfPostService 
    {
        List<TypeOfPost> GetTypes();

        TypeOfPost GetTypeById(Guid TypeId);

        TypeOfPostCreatedConfirmation CreateType(TypeOfPost Type);

        void DeleteType(Guid TypeId);
    }
}
