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
    [Route("[controller]")]
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
        public Character Get()
        {
            return GetCharacter(characterService.DefaultID);
        }

        [HttpGet("{id}")]
        public Character GetCharacter(int id)
        {
            return characterService.GetCharacter(id);
        }
    }
}
