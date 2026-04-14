using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace RESTMusicRecord.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicRecordController : ControllerBase
    {
        private IREPOMusicRecords _repo;

        public MusicRecordController(IREPOMusicRecords repo)
        {
            _repo = repo;
        }

        // GET api/MusicRecord
        // Henter alle music records
        // Kan også bruges til filtrering og sortering
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MusicRecord>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<MusicRecord>> Get(
            string? title = null,
            string? artist = null,
            int? duration = null,
            int? publicationYear = null,
            string? sortBy = null)
        {
            IEnumerable<MusicRecord> musicRecords = _repo.GetAll(title, artist, duration, publicationYear, sortBy);

            // Hvis listen er tom → 204 NoContent
            if (!musicRecords.Any())
            {
                return NoContent();
            }

            // Ellers returneres data
            return Ok(musicRecords);
        }

        // GET api/MusicRecord/5
        // Henter en enkelt music record ud fra id
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MusicRecord), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<MusicRecord> Get(int id)
        {
            // Tjekker om id er gyldigt
            if (id <= 0)
            {
                return BadRequest("Id skal være større end 0.");
            }

            // Finder record
            MusicRecord? musicRecord = _repo.GetById(id);

            // Hvis ikke fundet - 404
            if (musicRecord == null)
            {
                return NotFound();
            }

            // Returnerer record
            return Ok(musicRecord);
        }

        // POST api/MusicRecord
        // Tilføjer en ny music record
        // KRÆVER LOGIN
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(MusicRecord), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<MusicRecord> Post([FromBody] MusicRecord newMusicRecord)
        {
            // Tjekker om objektet er null
            if (newMusicRecord == null)
            {
                return BadRequest("Music record må ikke være null.");
            }

            // Simpel validering (meget vigtigt til eksamen)
            if (newMusicRecord.Title == null || newMusicRecord.Title.Trim().Length == 0)
            {
                return BadRequest("Title skal udfyldes.");
            }

            if (newMusicRecord.Artist == null || newMusicRecord.Artist.Trim().Length == 0)
            {
                return BadRequest("Artist skal udfyldes.");
            }

            if (newMusicRecord.Duration <= 0)
            {
                return BadRequest("Duration skal være større end 0.");
            }

            if (newMusicRecord.PublicationYear <= 0)
            {
                return BadRequest("PublicationYear skal være større end 0.");
            }

            // Opretter ny record
            MusicRecord createdMusicRecord = _repo.Add(newMusicRecord);

            // Returnerer 201 Created + location header
            return CreatedAtAction(nameof(Get), new { id = createdMusicRecord.Id }, createdMusicRecord);
        }

        // PUT api/MusicRecord/5
        // Opdaterer en eksisterende music record
        // KRÆVER LOGIN
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MusicRecord), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<MusicRecord> Put(int id, [FromBody] MusicRecord value)
        {
            // Tjekker id
            if (id <= 0)
            {
                return BadRequest("Id skal være større end 0.");
            }

            // Tjekker input
            if (value == null)
            {
                return BadRequest("Music record må ikke være null.");
            }

            // Validering
            if (value.Title == null || value.Title.Trim().Length == 0)
            {
                return BadRequest("Title skal udfyldes.");
            }

            if (value.Artist == null || value.Artist.Trim().Length == 0)
            {
                return BadRequest("Artist skal udfyldes.");
            }

            if (value.Duration <= 0)
            {
                return BadRequest("Duration skal være større end 0.");
            }

            if (value.PublicationYear <= 0)
            {
                return BadRequest("PublicationYear skal være større end 0.");
            }

            // Opdaterer record
            MusicRecord? updatedMusicRecord = _repo.Update(id, value);

            // Hvis ikke fundet → 404
            if (updatedMusicRecord == null)
            {
                return NotFound();
            }

            // Returnerer opdateret record
            return Ok(updatedMusicRecord);
        }

        // DELETE api/MusicRecord/5
        // Sletter en music record ud fra id
        // KRÆVER LOGIN
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MusicRecord), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<MusicRecord> Delete(int id)
        {
            // Tjekker id
            if (id <= 0)
            {
                return BadRequest("Id skal være større end 0.");
            }

            // Sletter record
            MusicRecord? deletedMusicRecord = _repo.Remove(id);

            // Hvis ikke fundet → 404
            if (deletedMusicRecord == null)
            {
                return NotFound();
            }

            // Returnerer slettet record
            return Ok(deletedMusicRecord);
        }

        // OPTIONS api/MusicRecord
        // Bruges til at vise hvilke requests der er tilladt
        [HttpOptions]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Options()
        {
            Response.Headers.Append("Allow", "GET, POST, PUT, DELETE, OPTIONS");
            return Ok();
        }
    }
}