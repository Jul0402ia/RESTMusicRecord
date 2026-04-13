using Microsoft.AspNetCore.Mvc;
using RESTMusicRecord.Models;

namespace RESTMusicRecord.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]

    public class MusicRecordController : Controller
    {
       private IMusicRecordRepository _musicRecordRepository;

        public MusicRecordController(IMusicRecordRepository musicRecordRepository)
        {
            _musicRecordRepository = musicRecordRepository;
        }
    }
}
