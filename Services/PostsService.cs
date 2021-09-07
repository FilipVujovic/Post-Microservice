using AutoMapper;
using PostService.Data;
using PostService.Data.BlockMockRepository;
using PostService.Data.UserMockRepository;
using PostService.Entities;
using PostService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PostService.Exceptions;
using PostService.Data.TypeOfPostRepository;
using PostService.Data.FollowingMockRepository;

namespace PostService.Services
{
    public class PostsService : IPostService
    {
        private readonly IPostRepository postRepository;
        private readonly IUserMockRepository userMockRepository;
        private readonly IMapper autoMapper;
        private readonly IBlockMockRepository blockMockRepository;
        private readonly ITypeOfPostRepository typeRepository;
        private readonly IFollowingMockRepository followingMockRepository;
        public PostsService(IFollowingMockRepository followingMockRepository, IPostRepository postRepository, IUserMockRepository userMockRepository, IMapper autoMapper, IBlockMockRepository blockMockRepository, ITypeOfPostRepository typeRepository)
        {
            this.postRepository = postRepository;
            this.userMockRepository = userMockRepository;
            this.autoMapper = autoMapper;
            this.blockMockRepository = blockMockRepository;
            this.typeRepository = typeRepository;
            this.followingMockRepository = followingMockRepository;
        }

        public PostDto CreatePost(PostCreationDto post)
        {
            if(userMockRepository.GetUserByID(post.UserId) == null)
            {
                throw new _404Exception("User not found!");
            }
            if(!typeRepository.ContainsType(post.Type))
            {
                throw new _404Exception("Type not found!");
            }

            var newPost = autoMapper.Map<Post>(post);
            newPost.PostPublishingDateTime = DateTime.Now;
            newPost.LastModified = DateTime.Now;
            newPost.PostTypeId = typeRepository.GetIdByType(post.Type);
            try
            {
                var addedPost = postRepository.CreatePost(newPost);
                return autoMapper.Map<PostDto>(addedPost);
            } catch (Exception ex)
            {
                throw new GeneralException(ex.Message);
            }
        }

        public void DeletePost(Guid PostId)
        {
            Console.WriteLine(PostId);
            var toBeDeleted = postRepository.GetPostById(PostId);
            if(toBeDeleted == null)
            {
               throw new _404Exception("Post not found!");
           }
            try
            {
                postRepository.DeletePost(PostId);
            } catch (Exception ex)
            {
                throw new GeneralException(ex.Message);
            }
        }

        public void DeletePostsByUserId(int UserId)
        {
            if(userMockRepository.GetUserByID(UserId) == null)
            {
                throw new _404Exception("User not found!");
            }
            postRepository.DeletePostsByUserId(UserId);
        }

        public PostDto GetPostById(Guid PostId)
        {
            if(postRepository.GetPostById(PostId) == null)
            {
                throw new _404Exception("Post not found!");
            }
            var fetchedPost = postRepository.GetPostById(PostId);
            return autoMapper.Map<PostDto>(fetchedPost);
        }

        public List<PostDto> GetPostsByUserId(int UserId, int SubjectId)
        {
            if(userMockRepository.GetUserByID(SubjectId) == null)
            {
                throw new _404Exception("User not found!");
            }
            if(blockMockRepository.CheckIfBlocked(UserId, SubjectId))
            {
                throw new BlockException("User blocked!");
            }

            var fetchedPosts = postRepository.GetPostsByUserId(UserId);
            return autoMapper.Map<List<PostDto>>(fetchedPosts);
        }

        public List<PostDto> GetPosts()
        {
            var fetchedPosts = postRepository.GetPosts();
            return autoMapper.Map<List<PostDto>>(fetchedPosts);
        }


        public List<PostDto> GetPostsByCity(int UserId, string City)
        {
            var fetchedPosts = postRepository.GetPostsByCity(UserId, City);
            if(fetchedPosts.Count == 0)
            {
                throw new _404Exception("No posts found in city : " + City);
            }
           
            return autoMapper.Map<List<PostDto>>(fetchedPosts);
        }

        public List<PostDto> GetPostsByTitle(int UserId,string PostTitle)
        {

            if(userMockRepository.GetUserByID(UserId) == null)
            {
                throw new _404Exception("User not found!");
            }
            try
            {
                var fetchedPosts = postRepository.GetPostsByTitle(UserId, PostTitle);
                if (fetchedPosts.Count == 0)
                {
                    throw new _404Exception("No posts found with title : " + PostTitle);
                }
                return autoMapper.Map<List<PostDto>>(fetchedPosts);
            } catch (Exception ex)
            {
                throw new GeneralException(ex.Message);
            }
        }
        /*Prvo proveravamo blokiranje kako ne bismo slali zahtev za dzabe.*/
        public List<PostDto> GetPostsFromWall(int UserId, int SubjectId)
        {
            if(userMockRepository.GetUserByID(SubjectId) == null)
            {
                throw new _404Exception("User not found!");
            }

            if (blockMockRepository.CheckIfBlocked(UserId, SubjectId))      
            {
                throw new BlockException("User blocked!");
            }
           
            if (followingMockRepository.CheckFollowing(UserId, SubjectId) == false)
            {
                throw new FollowingException("You are not following that user!");
            }
            var fetchedPosts = postRepository.GetPostsFromWall(UserId, SubjectId);
            if(fetchedPosts.Count == 0)
            {
                throw new GeneralException("This user does not have any posts yet!");
            }
            return autoMapper.Map<List<PostDto>>(fetchedPosts);
        }

        public PostDto UpdatePost(int UserId, PostModificationDto post)
        {
            if(UserId != postRepository.GetPostById(post.PostId).UserId)
            {
                throw new GeneralException("You are not the author of this post!");
            }
            if(postRepository.GetPostById(post.PostId) == null)
            {
                throw new _404Exception("Post with that Id does not exist!");
            }
            var oldPost = postRepository.GetPostById(post.PostId);
            var newPost = autoMapper.Map<Post>(post);

            if(oldPost.PostId != newPost.PostId)
            {
                throw new GeneralException("Changing of the post Id is not permitted!");
            }

            try
            {
                newPost.LastModified = DateTime.Now;
                newPost.UserId = oldPost.UserId;
                newPost.PostPublishingDateTime = oldPost.PostPublishingDateTime;
                newPost.PostTypeId = oldPost.PostTypeId;
                autoMapper.Map(newPost, oldPost);
                postRepository.UpdatePost();
                return autoMapper.Map<PostDto>(newPost);
            } catch (Exception ex)
            {
                throw new GeneralException(ex.Message);
            }
        }
    }
}
