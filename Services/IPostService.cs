using PostService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Services
{
    public interface IPostService
    {
        List<PostDto> GetPosts();

        List<PostDto> GetPostsFromWall(int UserId, int SubjectId);

        PostDto GetPostById(Guid PostId);

        List<PostDto> GetPostsByUserId(int UserId, int SubjectId);

        List<PostDto> GetPostsByTitle(int UserId, string PostTitle);

        List<PostDto> GetPostsByCity(int UserId,string City);

        public PostDto CreatePost(PostCreationDto post);

        public PostDto UpdatePost(int UserId, PostModificationDto post);

        public void DeletePost(Guid PostId);

        public void DeletePostsByUserId(int UserId);
    }
}
