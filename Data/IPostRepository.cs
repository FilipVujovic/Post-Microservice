using PostService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Data
{
    public interface IPostRepository
    {

        List<Post> GetPosts();
        /*
         UserId - Korisnik koji salje zahtev/koji je ulogovan u trenutnoj sesiji
                  U realnoj aplikaciji bi se ovaj podatak cuvao u nekom globalnom stanju

         SubjectId - Korisnik ciji zid hocemo da vidimo
                     Ovaj podatak nam je potreban zbog provere pracenja i blokiranja
         */
        List<Post> GetPostsFromWall(int UserId, int SubjectId);

        Post GetPostById(Guid PostId);

        List<Post> GetPostsByUserId(int UserId);

        List<Post> GetPostsByTitle(int UserId, string PostTitle);

        List<Post> GetPostsByCity(int UserId, string City);

        public Post CreatePost(Post post);

        public void UpdatePost();

        public void DeletePost(Guid PostId);

        public void DeletePostsByUserId(int UserId);

        public bool ContainsType(Guid TypeId);

    }
}
