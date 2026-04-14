using Microsoft.AspNetCore.Mvc;

namespace RESTMusicRecord.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicRecordController : Controller
    {
        private IREPOMusicRecords _repoMusicRecords;

        public MusicRecordController(IREPOMusicRecords musicRecordRepository)
        {
            _repoMusicRecords = musicRecordRepository;
        }
        
     
    }
}
