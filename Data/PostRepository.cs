using PostService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PostService.Data.BlockMockRepository;
using PostService.Data.FollowingMockRepository;

namespace PostService.Data
{
    public class PostRepository : IPostRepository
    {

        private readonly DatabaseContext dbContext;
        private readonly IBlockMockRepository blockMockRepository;
        private readonly IFollowingMockRepository followingMockRepository;
        public PostRepository(DatabaseContext context, IBlockMockRepository blockMockRepository, IFollowingMockRepository followingMockRepository)
        {
            dbContext = context;
            this.blockMockRepository = blockMockRepository;
            this.followingMockRepository = followingMockRepository;
        }

        public Post CreatePost(Post post)
        {
            var postEntity = dbContext.Post.Add(post);
            dbContext.SaveChangesAsync();
            return postEntity.Entity;
        }

        public void DeletePost(Guid PostId)
        {

            Post post = dbContext.Post.FirstOrDefault(post => post.PostId == PostId);
            dbContext.Remove(post);
            dbContext.SaveChanges();
        }

        public void DeletePostsByUserId(int UserId)
        {
            var query = from post in dbContext.Post
                        where post.UserId == UserId
                        select post;

            var toBeDeleted = query.ToList();
            foreach(Post post in toBeDeleted)
            {
                dbContext.Remove(post);
                dbContext.SaveChanges();
            }
        }

        public Post GetPostById(Guid PostId)
        {
            return dbContext.Post.FirstOrDefault(c => c.PostId == PostId);
        }

        public List<Post> GetPostsByUserId(int UserId)
        {
            var query = from post in dbContext.Post
                        where post.UserId == UserId
                        select post;
            return query.ToList();
        }


        public List<Post> GetPosts()
        {
            return dbContext.Post.ToList();
        }

        /*
         Potreban nam je id jer prilikom provere blokiranih 
         korisnika moramo da znamo za kojeg konkretnog korisnika
         proveravamo blokiranja
         */
        public List<Post> GetPostsByCity(int UserId, string City)
        {
            var query = from post in dbContext.Post
                        where post.City == City && !(from block in blockMockRepository.GetBlockedUsersForUser(UserId) select block).Contains(post.UserId)
                        select post;
            return query.ToList();
        }

        public List<Post> GetPostsByTitle(int UserId, string PostTitle)
        {
            var query = from post in dbContext.Post
                        where post.PostTitle == PostTitle && !(from block in blockMockRepository.GetBlockedUsersForUser(UserId) select block).Contains(post.UserId)
                        select post;
            return query.ToList();
        }

        public List<Post> GetPostsFromWall(int UserId, int SubjectId)
        {
                var query = from post in dbContext.Post
                            where post.UserId == SubjectId
                            select post;
                return query.ToList();
        }

        public void UpdatePost()
        {
            dbContext.SaveChanges();
        }

        public bool ContainsType(Guid TypeId)
        {
            if(dbContext.Post.FirstOrDefault(c => c.PostTypeId == TypeId)!=null)
            {
                return true;
            }
            return false;
        }
    }
}
