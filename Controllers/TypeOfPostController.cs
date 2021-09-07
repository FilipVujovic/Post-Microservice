using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostService.Entities.Type;
using PostService.Models;
using PostService.Services.TypeService;
using System;

namespace PostService.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    [Route("api/typeOfPost")]
    
    public class TypeOfPostController : ControllerBase
    {
        private readonly ITypeOfPostService typeService;
        private readonly IMapper autoMapper;
        /// <summary>
        /// Kontroler koji upravlja TypeOfPost tabelom i vrši ubacivanje podataka, brisanje podataka i vraćanje podataka.
        /// </summary>
        /// <param name="autoMapper">Maper za mapiranje različitih oblika TypeOfPost podataka.</param>
        /// <param name="typeService">TypeOfPost servis.</param>
        public TypeOfPostController(IMapper autoMapper, ITypeOfPostService typeService)
        {
            this.autoMapper = autoMapper;
            this.typeService = typeService;
        }

        /// <summary>
        /// Dodaje novi tip u bazu.
        /// </summary>
        /// <param name="type">Tip koji želimo da dodamo.</param>
        /// <returns>Potvrdu o uspešnom kreiranju tipa.</returns>
        /// <remarks>
        /// DELETE 'https://localhost:44200/api/typeOfPost/PostId'
        /// </remarks>
        /// <response code="201">Tip je uspešno kreiran.</response>
        /// <response code="500">Serverska greška.</response>
        [HttpPost("type")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<TypeOfPostCreationConfirmationDto> CreateType([FromBody] TypeOfPostCreationDto type)
        {
            try
            {
                var typeOfPost = autoMapper.Map<TypeOfPost>(type);
                TypeOfPostCreatedConfirmation createdType = typeService.CreateType(typeOfPost);
                return StatusCode(201, createdType);
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Briše tip posta iz baze.
        /// </summary>
        /// <param name="TypeId">Id tipa koji želimo da obrišemo.</param>
        /// <returns>Potvrdu o uspešnom brisanju.</returns>
        /// <remarks>
        /// DELETE 'https://localhost:44200/api/typeOfPost/PostId'
        /// </remarks>
        /// <response code="409">Konflikt stranog ključa. Pokušali smo da obrišemo tip koji je referenciran u tabeli Post.</response>
        /// <response code="404">Tip posta ne postoji.</response>
        /// <response code="500">Serverska greška.</response>
        /// <response code="204">Post uspešno obrisan.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpDelete("{TypeId}")]
        public IActionResult DeleteType(Guid TypeId)
        {
            try
            {
                typeService.DeleteType(TypeId);
                return NoContent();
            } catch (Exception ex)
            {
                if(ex.Message == "Type not found!")
                {
                    return StatusCode(StatusCodes.Status404NotFound, ex.Message);
                } else if(ex.Message == "Foreign key constraint violated!")
                {
                    return StatusCode(StatusCodes.Status409Conflict, ex.Message);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Vraća tip posta po id-ju.
        /// </summary>
        /// <param name="TypeId">Id tipa koji želimo da vratimo.</param>
        /// <returns>Potvrdu o uspešnom brisanju.</returns>
        /// <remarks>
        /// DELETE 'https://localhost:44200/api/typeOfPost/PostId'
        /// </remarks>
        /// <response code="200">Tip posta uspešno vraćen.</response>
        /// <response code="404">Tip posta ne postoji.</response>
        /// <response code="500">Serverska greška.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{TypeId}")]
        public ActionResult<TypeOfPostDto> GetTypeById(Guid TypeId)
        {
            try
            {
                var type = typeService.GetTypeById(TypeId);
                return StatusCode(StatusCodes.Status200OK, type);
            } catch (Exception ex)
            {
                if(ex.Message == "Type not found!")
                {
                    return StatusCode(StatusCodes.Status404NotFound, ex.Message);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
