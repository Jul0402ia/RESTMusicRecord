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

            if (!musicRecords.Any())
            {
                return NoContent();
            }

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
            if (id <= 0)
            {
                return BadRequest("Id skal være større end 0.");
            }

            MusicRecord? musicRecord = _repo.GetById(id);

            if (musicRecord == null)
            {
                return NotFound();
            }

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
            if (newMusicRecord == null)
            {
                return BadRequest("Music record må ikke være null.");
            }

            MusicRecord createdMusicRecord = _repo.Add(newMusicRecord);

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
            if (id <= 0)
            {
                return BadRequest("Id skal være større end 0.");
            }

            if (value == null)
            {
                return BadRequest("Music record må ikke være null.");
            }

            MusicRecord? updatedMusicRecord = _repo.Update(id, value);

            if (updatedMusicRecord == null)
            {
                return NotFound();
            }

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
            if (id <= 0)
            {
                return BadRequest("Id skal være større end 0.");
            }

            MusicRecord? deletedMusicRecord = _repo.Remove(id);

            if (deletedMusicRecord == null)
            {
                return NotFound();
            }

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