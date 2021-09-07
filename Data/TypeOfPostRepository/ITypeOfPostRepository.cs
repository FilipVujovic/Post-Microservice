using PostService.Entities.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Data.TypeOfPostRepository
{
    public interface ITypeOfPostRepository
    {
        List<TypeOfPost> GetTypes();

        TypeOfPost GetTypeById(Guid TypeId);

        TypeOfPostCreatedConfirmation CreateType(TypeOfPost Type);

        void UpdateType(Guid TypeId);

        void DeleteType(Guid TypeId);

        Guid GetIdByType(string Type);
        bool ContainsType(string Type);
    }
}
