using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WzLib;
using WzWeb.Shared.Character;

namespace WzWeb.Server.Services
{
    public interface ICharacterService
    {
        public int DefaultID { get; }
        public Wz_Node CharacterNode { get; }
        public IEnumerable<int> CharacterIDList { get; }
        public Character GetCharacter(int id);
    }
}
