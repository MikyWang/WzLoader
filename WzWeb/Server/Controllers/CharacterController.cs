using WzWeb.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WzWeb.Server.Services;
using WzLib;
using WzWeb.Server.Extentions;
using WzWeb.Shared.Character;

namespace WzWeb.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : Controller
    {

        private readonly ILogger<CharacterController> logger;
        private readonly ICharacterService characterService;

        public CharacterController(ICharacterService characterService, ILogger<CharacterController> logger)
        {
            this.logger = logger;
            this.characterService = characterService;
        }

        [HttpGet]
        public CharacterResponse Get()
        {
            var request = new CharacterRequest { CharacterId = characterService.DefaultID, MotionName = characterService.DefaultMotionName };
            return GetCharacter(request);
        }

        [HttpPost("GetCharacter")]
        public CharacterResponse GetCharacter(CharacterRequest request)
        {
            var collection = characterService.GetCharacter(request.CharacterId, request.MotionName);
            return new CharacterResponse
            {
                CharacterCollection = collection
            };
        }
    }
}
