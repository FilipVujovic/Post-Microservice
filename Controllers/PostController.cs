using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PostService.AuthorizationMock;
using PostService.Logger;
using PostService.Models;
using PostService.Services;
using System;
using System.Collections.Generic;

namespace PostService.Controllers
{
    [Route("api")]
    [Produces("application/json", "application/xml")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService postService;
        private readonly IAuthorizationMockService authorizationMockService;
        private readonly ILoggerRepository<PostController> loggerRepository;

        /// <summary>
        /// Kontroler koji upravlja Post tabelom.
        /// </summary>
        /// <param name="postService">Post servis.</param>
        /// <param name="authorizationMockService">Mock autorizacionog servisa.</param>
        /// <param name="loggerRepository">Logger</param>
        public PostController(IPostService postService, IAuthorizationMockService authorizationMockService, ILoggerRepository<PostController> loggerRepository)
        {
            this.postService = postService;
            this.authorizationMockService = authorizationMockService;
            this.loggerRepository = loggerRepository;
        }

        /// <summary>
        /// Kreira novi post.
        /// </summary>
        /// <param name="secretToken">Token za autorizaciju.</param>
        /// <param name="newPost">Novi post tipa PostCreationDto.</param>
        /// <returns>Potvrdu o kreiranju posta.</returns>
        /// <remarks>
        /// 
        /// POST 'https://localhost:44200/api/post'
        /// 
        /// Authorization header: 
        /// secretToken : Bearer Secret
        /// 
        /// Primer zahteva za kreiranje novog posta.
        /// {
        /// "postTitle" : "Asus VivoBook",
        /// "postDescription" : "Asus VivoBook Max nov, jednom koriscen za majnovanje bitkoina ali nista strasno. Radi kao sat."
        /// "City" : "Novi Sad",
        /// "UserId" : 2,
        /// "Type": "Oglas"
        /// }
        /// 
        /// Da bi ovaj zahtev bio uspešan potrebno je da Type obeležje postoji u bazi s obzirom da je to strani ključ u tabeli Post.
        /// </remarks>
        /// <response code="201">Post je uspešno kreiran.</response>
        /// <response code="401">Neuspešna autorizacija.</response>
        /// <response code="404">Korisnik ili tip ne postoji.</response>
        /// <response code="500">Serverska greška.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPost("post")]
        public ActionResult<PostDto> CreateNewPost([FromHeader] string secretToken, [FromBody] PostCreationDto newPost)
        {
            if (authorizationMockService.AuthorizeToken(secretToken))
            {
                try
                {
                    var toBeCreated = postService.CreatePost(newPost);
                    loggerRepository.LogInformation("New post successfuly created!");
                    return StatusCode(201, toBeCreated);
                }
                catch (Exception ex)
                {
                    if(ex.Message == "User not found!" || ex.Message == "Type not found!")
                    {
                        return StatusCode(404, ex.Message);
                    } 
                    return StatusCode(500, ex.Message);
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "Authorization failed!");
        }

        /// <summary>
        /// Briše post iz baze.
        /// </summary>
        /// <param name="secretToken">Token za autorizaciju.</param>
        /// <param name="PostId">Id posta koji želimo da obrišemo.</param>
        /// <returns>Potvrdu o brisanju.</returns>
        /// <remarks>
        /// 
        /// DELETE 'https://localhost:44200/api/?PostId=PostId'
        /// 
        /// Authorization header: 
        /// secretToken : Bearer Secret
        /// 
        /// Primer zahteva za brisanje posta.
        /// Id posta se čita iz query parametra.
        /// DELETE localhost:44200/api/PostId=51db2a5e-5b2c-4724-1e99-08d96662d1cd
        /// </remarks>
        /// <response code="204">Post je uspešno obrisan.</response>
        /// <response code="401">Neuspešna autorizacija.</response>
        /// <response code="404">Post sa prosledjenim id-jem ne postoji.</response>
        /// <response code="500">Serverska greška.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete]
        public IActionResult DeletePost([FromHeader] string secretToken, [FromQuery] Guid PostId)
        {
            if (authorizationMockService.AuthorizeToken(secretToken))
            {
                try
                {
                    postService.DeletePost(PostId);
                    loggerRepository.LogInformation("Post deleted successfully!");
                    return StatusCode(StatusCodes.Status204NoContent, "Post successfully deleted!");
                }
                catch (Exception ex)
                {
                    if(ex.Message == "Post not found!")
                    {
                        return StatusCode(StatusCodes.Status404NotFound, "An error occured while deleting post : " + ex.Message);
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while deleting post : " + ex.Message);
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "Authorization failed!");
        }


        /// <summary>
        /// Vraća post po Id-ju posta.
        /// </summary>
        /// <param name="secretToken">Token za autorizaciju.</param>
        /// <param name="PostId">Id posta koji želimo da vratimo.</param>
        /// <returns>PostDto reprezentaciju posta.</returns>
        /// <remarks>
        /// 
        /// GET 'https://localhost:44200/api/post/?PostId=PostId'
        /// 
        /// Authorization header: 
        /// secretToken : Bearer Secret
        /// 
        /// Primer zahteva za vraćanje posta po Id-ju.
        /// Id posta se čita iz query parametra.
        /// GET localhost:44200/api/post/?PostId=a39e1850-1ca6-4805-270b-08d96663b948
        /// </remarks>
        /// <response code="401">Neuspešna autorizacija.</response>
        /// <response code="200">Postovi uspešno vraćeni.</response>
        /// <response code="404">Post nije pronađen.</response>
        /// <response code="500">Serverska greška.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("post")]
        public ActionResult<PostDto> GetPostById([FromHeader] string secretToken, [FromQuery] Guid PostId)
        {
            if (authorizationMockService.AuthorizeToken(secretToken))
            {
                try
                {
                    var fetchedPost = postService.GetPostById(PostId);
                    loggerRepository.LogInformation("Post successfully fetched!");
                    return StatusCode(StatusCodes.Status200OK, fetchedPost);
                }
                catch (Exception ex)
                {
                    if(ex.Message == "Post not found!")
                    {
                        return StatusCode(StatusCodes.Status404NotFound, ex.Message);
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "Authorization failed!");
        }


        /// <summary>
        /// Vraća postove po Id-ju korisnika.
        /// </summary>
        /// <param name="secretToken">Token za autorizaciju.</param>
        /// <param name="UserId">Id korisnika čije postove želimo da vidimo.</param>
        /// <returns>Listu postova određenog korisnika.</returns>
        /// <remarks>
        /// 
        /// GET 'https://localhost:44200/api/postsByUserId/?UserId=UserId'
        /// 
        /// Authorization header: 
        /// secretToken : Bearer Secret
        /// 
        /// Primer zahteva za vraćanje posta po Id-ju korisnika.
        /// Id se čita iz query parametra.
        /// GET localhost:44200/api/postsByUserId/?UserId=3
        /// 
        /// Primer neuspešnog zahteva : 
        /// GET localhost:44200/api/postsByUserId/?UserId=1&SubjectId=2
        /// 
        /// Korisnik sa id-jem 2 je blokirao korisnika sa id-jem 1, u ovom slučaju 
        /// korisniku će se prikazati poruka "User blocked!"
        /// 
        /// GET localhost:44200/api/postsByUserId/?UserId=1&SubjectId=22
        /// 
        /// Korisnik sa id-jem 22 ne postoji, korisniku se prikazuje poruka
        /// "User not found!"
        /// 
        /// </remarks>
        /// <response code="401">Neuspešna autorizacija.</response>
        /// <response code="200">Postovi uspešno vraćeni.</response>
        /// <response code="404">Korisnik nije pronađen.</response>
        /// <response code="400">Prosleđen je id korisnika koji je blokiran ili je blokirao korisnika sa id-jem 1.</response>
        /// <response code="500">Serverska greška.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("postsByUserId")]
        public ActionResult<List<PostDto>> GetPostsByUserId([FromHeader] string secretToken, [FromQuery] int UserId, [FromQuery] int SubjectId)
        {
            if (authorizationMockService.AuthorizeToken(secretToken))
            {
                try
                {
                    var fetchedPosts = postService.GetPostsByUserId(UserId,SubjectId);
                    loggerRepository.LogInformation("Posts successfully fetched!");
                    return StatusCode(StatusCodes.Status200OK, fetchedPosts);
                }
                catch (Exception ex)
                {
                    if(ex.Message == "User not found!")
                    {
                        return StatusCode(StatusCodes.Status404NotFound, ex.Message);
                    } else if (ex.Message == "User blocked!")
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "Authorization failed!");
        }

        /// <summary>
        /// Vraća sve postove koji se trenutno nalaze u bazi. Ne uzima u obzir bilo kakve parametre
        /// osim tokena za autorizaciju. Token je tu pod pretpostavkom da ovoj ruti može da
        /// pristupi samo ulogovani korisnik.
        /// </summary>
        /// 
        /// GET 'https://localhost:44200/api/posts'
        /// 
        /// Authorization header: 
        /// secretToken : Bearer Secret
        /// 
        /// <param name="secretToken">Token za autorizaciju. U realnoj situaciji JWT Token generisan na osnovu 
        /// korisničkih podataka.</param>
        /// <returns>Listu objekata tipa PostDto, odnosno listu svih postova.</returns>
        /// <remarks>
        /// Primer zahteva za vraćanje svih postova.
        /// GET localhost:44200/api/posts
        /// </remarks>
        /// <response code="200">Postovi uspešno vraćeni.</response>
        /// <response code="401">Neuspešna autorizacija.</response>
        /// <response code="500">Serverska greška.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("posts")]
        public ActionResult<List<PostDto>> GetAllPosts([FromHeader] string secretToken)
        {
            if (authorizationMockService.AuthorizeToken(secretToken))
            {
                try
                {
                    var fetchedPosts = postService.GetPosts();
                    loggerRepository.LogInformation("Posts successfully fetched!");
                    return StatusCode(StatusCodes.Status200OK, fetchedPosts);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "Authorization failed!");
        }



        /// <summary>
        /// Vraća postove po gradovima.
        /// </summary>
        /// <param name="secretToken">Token za autorizaciju. U realnoj situaciji JWT Token generisan na osnovu 
        /// korisničkih podataka.</param>
        /// <param name="UserId">Id korisnika koji šalje zahtev.</param>
        /// <param name="City">Grad po kojem vršimo filtriranje.</param>
        /// <returns>Listu postova u određenom gradu.</returns>
        /// <remarks>
        /// 
        /// GET 'https://localhost:44200/api/postsByCity?City=City&UserId=UserId'
        /// 
        /// Authorization header: 
        /// secretToken : Bearer Secret
        /// 
        /// Primer zahteva za vraćanje postova po gradu.
        /// Grad se čita iz query parametra.
        /// GET localhost:44200/api/postsByCity?City=Beograd&UserId=3
        /// 
        /// Primer blokiranog zahteva: 
        /// 
        /// localhost:44200/api/postsByCity?City=Novi Sad&UserId=1
        /// 
        /// Sistem će filtrirati postove koje je postavio korisnik sa id-jem 2 i koje za grad imaju postavljeno Novi Sad
        /// s obzirom da je korisnik sa id-jem 1 blokiran od strane korisnika sa id-jem 2.
        /// 
        /// Vratiće se poruka "No posts found in Novi Sad!"
        /// 
        /// </remarks>
        /// <response code="200">Postovi uspešno vraćeni.</response>
        /// <response code="401">Neuspešna autorizacija.</response>
        /// <response code="500">Serverska greška.</response>
        /// <response code="404">Nisu pronađeni postovi u gradu.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("postsByCity")]
        public ActionResult<List<PostDto>> GetPostsByCity([FromHeader] string secretToken, [FromQuery] int UserId, [FromQuery] string City)
        {
            if (authorizationMockService.AuthorizeToken(secretToken))
            {
                try
                {
                    var fetchedPosts = postService.GetPostsByCity(UserId,City);
                    loggerRepository.LogInformation("Posts successfully fetched!");
                    return StatusCode(StatusCodes.Status200OK, fetchedPosts);
                }
                catch (Exception ex)
                {
                    if(ex.Message == "No posts found in city : " + City)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, ex.Message);
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "Authorization failed!");
        }

        /// <summary>
        /// Vraća postove po naslovu.
        /// </summary>
        /// <param name="secretToken">Token za autorizaciju. U realnoj situaciji JWT Token generisan na osnovu 
        /// korisničkih podataka.</param>
        /// <param name="UserId">Id korisnika koji šalje zahtev.</param>
        /// <param name="Title">Naslov posta.</param>
        /// <returns>Listu postova sa zajedničkim naslovom.</returns>
        /// <remarks>
        /// 
        /// GET 'https://localhost:44200/api/postsByTitle?Title=Naslov&UserId=UserId'
        /// 
        /// Authorization header: 
        /// secretToken : Bearer Secret
        /// 
        /// Primer zahteva za vraćanje postova po naslovu.
        /// Naslov se čita iz query parametra.
        /// GET localhost:44200/api/postsByTitle?Title=Asus Vivobook&Userid=1
        /// </remarks>
        /// <response code="200">Postovi uspešno vraćeni.</response>
        /// <response code="401">Neuspešna autorizacija.</response>
        /// <response code="500">Serverska greška.</response>
        /// <response code="404">Nisu pronađeni postovi sa prosleđenim naslovom.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("postsByTitle")]
        public ActionResult<List<PostDto>> GetPostsByTitle([FromHeader] string secretToken, [FromQuery] int UserId, [FromQuery] string Title)
        {
            if (authorizationMockService.AuthorizeToken(secretToken))
            {
                try
                {
                    var fetchedPosts = postService.GetPostsByTitle(UserId,Title);
                    loggerRepository.LogInformation("Posts successfully fetched!");
                    return StatusCode(StatusCodes.Status200OK, fetchedPosts);
                }
                catch (Exception ex)
                {
                    if(ex.Message == "No posts found with title : " + Title)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, ex.Message);
                    } else if (ex.Message == "User not found!")
                    {
                        return StatusCode(StatusCodes.Status404NotFound, ex.Message);
                    } 
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "Authorization failed!");
        }

        /// <summary>
        /// Briše postove po Id-ju korisnika.
        /// </summary>
        /// <param name="secretToken">Token za autorizaciju. U realnoj situaciji JWT Token generisan na osnovu 
        /// korisničkih podataka.</param>
        /// <param name="UserId">Id korisnika čije postove želimo da obrišemo.</param>
        /// <returns>Potvrdu o uspešnom brisanju postova.</returns>
        /// <remarks>
        /// 
        /// DELETE 'https://localhost:44200/api/deleteByUserId?UserId=UserId'
        /// 
        /// Authorization header: 
        /// secretToken : Bearer Secret
        /// 
        /// Ovaj zahtev služi u slučaju da želimo da obrišemo korisnika. Ako ne postoji korisnik u bazi ne smeju da postoje
        /// postovi koje je taj korisnik namestio. 
        /// 
        /// Primer zahteva za brisanje postova po Id-ju.
        /// Id korisnika se čita iz query parametra.
        /// DELETE localhost:44200/api/deleteByUserId?UserId=2
        /// </remarks>
        /// <response code="204">Postovi uspešno obrisani.</response>
        /// <response code="401">Neuspešna autorizacija.</response>
        /// <response code="500">Serverska greška.</response>
        /// <response code="404">Korisnik nije pronađen.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("deleteByUserId")]
        public IActionResult DeletePostsByUserId([FromHeader] string secretToken, [FromQuery] int UserId)
        {
            if (authorizationMockService.AuthorizeToken(secretToken))
            {
                try
                {
                    postService.DeletePostsByUserId(UserId);
                    loggerRepository.LogInformation("Post successfully deleted!");
                    return StatusCode(StatusCodes.Status204NoContent, "Posts deleted successfully!");
                }
                catch (Exception ex)
                {
                    if(ex.Message == "User not found!")
                    {
                        return StatusCode(StatusCodes.Status404NotFound, "Error deleting posts : " + ex.Message);
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting posts : " + ex.Message);
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "Authorization failed!");
        }


        /// <summary>
        /// Vraća postove za korisnika kojeg pratimo.
        /// </summary>
        /// <param name="secretToken">Token za autorizaciju.</param>
        /// <param name="UserId">Id korisnika koji šalje zahtev.</param>
        /// <param name="SubjectId">Id korisnika čije postove želimo da vratimo.</param>
        /// <returns>Listu postova.</returns>
        /// <remarks>
        /// 
        /// GET 'https://localhost:44200/api/getPostsFromWall?UserId=UserId&SubjectId=SubjectId'
        /// 
        /// Authorization header: 
        /// secretToken : Bearer Secret
        /// 
        /// Da bi ovaj zahtev bio uspešan prvo moramo da pratimo korisnika čije postove želimo da vratimo. 
        /// User Id i Subject Id nam služe za proveru praćenja i blokiranja pre vraćanja podataka.
        /// 
        /// Primer uspešnog zahteva :
        /// 
        /// GET localhost:44200/api/getPostsFromWall?UserId=3&SubjectId=2
        /// 
        /// Primer neuspešnih zahteva :
        /// 
        /// GET localhost:44200/api/getPostsFromWall?UserId=2&SubjectId=3
        /// 
        /// Korisnik sa id-jem 2 ne prati korisnika sa id-jem 3 i njemu se prikazuje poruka :
        /// "Error fetching posts : You are not following that user!"
        /// 
        /// GET localhost:44200/api/getPostsFromWall?UserId=2&SubjectId=1
        /// Korisnik sa id-jem 1 je blokirao korisnika sa id-jem 2. U ovom slučaju korisniku sa id-jem 2 se prikazuje poruka :
        /// 
        /// "Error fetching posts : User blocked!"
        /// 
        /// </remarks>
        /// <response code="200">Postovi uspešno vraćeni.</response>
        /// <response code="401">Neuspešna autorizacija.</response>
        /// <response code="500">Serverska greška.</response>
        /// <response code="404">Korisnik nije pronađen ili korisnik nema postove.</response>
        /// <response code="400">Korisnik je blokiran ili ne prati korisnika čije postove želi da vrati.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("getPostsFromWall")]
        public IActionResult GetPostsFromWall([FromHeader] string secretToken, [FromQuery] int UserId, [FromQuery] int SubjectId)
        {
            if (authorizationMockService.AuthorizeToken(secretToken))
            {
                try
                {
                    var fetchedPosts = postService.GetPostsFromWall(UserId, SubjectId);
                    loggerRepository.LogInformation("Posts successfully fetched!");
                    return StatusCode(StatusCodes.Status200OK, fetchedPosts);
                }
                catch (Exception ex)
                {
                    if(ex.Message == "User not found!")
                    {
                        return StatusCode(StatusCodes.Status404NotFound, "Error fetching posts : " + ex.Message);
                    } else if (ex.Message == "User blocked!" || ex.Message == "You are not following that user!")
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "Error fetching posts : " + ex.Message);
                    } else if (ex.Message == "This user does not have any posts yet!")
                    {
                        return StatusCode(StatusCodes.Status404NotFound, "Error fetching posts : " + ex.Message);
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error fetching posts : " + ex.Message);
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "Authorization failed!");
        }

        /// <summary>
        /// Ažurira post.
        /// </summary>
        /// <param name="secretToken">Token za autorizaciju.</param>
        /// <param name="UserId"></param>
        /// <param name="post">Nova verzija posta.</param>
        /// <returns>Potvrdu o ažuriranju posta.</returns>
        /// <remarks>
        /// 
        /// PUT 'https://localhost:44200/api/?UserId=UserId'
        /// 
        /// Authorization header: 
        /// secretToken : Bearer Secret
        /// 
        /// Primer zahteva za ažuriranje posta.
        /// 
        /// PUT localhost:44200/api/?UserId=2
        /// {
        /// "postId": "3ce2f994-ec2f-4488-a977-08d96a18253c",
        /// "postDescription": "Laptop",
        /// "postTitle": "Asus",
        /// "city": "Novi Sad"
        ///}
        ///
        /// Zahtev će biti neuspešsan ukoliko korisnik pokuša da ažurira post koji nije on postavio.
        /// 
        /// </remarks>
        /// <response code="200">Post je ažuriran.</response>
        /// <response code="404">Post sa prosleđenim id-jem ne postoji.</response>
        /// <response code="401">Neuspešna autorizacija.</response>
        /// <response code="500">Serverska greška</response>
        /// <response code="400">Korisnik je pokušao da promeni id posta.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        [HttpPut]
        public IActionResult UpdatePost([FromHeader] string secretToken, [FromQuery] int UserId, [FromBody] PostModificationDto post)
        {
            if(authorizationMockService.AuthorizeToken(secretToken))
            {
                try
                {
                    var updatedPost = postService.UpdatePost(UserId,post);
                    loggerRepository.LogInformation("Post successfully updated!");
                    return StatusCode(StatusCodes.Status200OK, updatedPost);

                } catch (Exception ex)
                {
                    if(ex.Message == "Post with that Id does not exist!")
                    {
                        return StatusCode(StatusCodes.Status404NotFound, "Error updating posts : " + ex.Message);
                    } else if (ex.Message == "Changing of the post Id is not permitted!")
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "Error updating posts : " + ex.Message);
                    } else if (ex.Message == "You are not the author of this post!")
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "Authorization failed : Unknown author!");
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error updating posts : " + ex.Message);
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "Authorization failed!");
        }
    }
}
