using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WzLib;
using WzWeb.Shared;
using WzWeb.Shared.Character;

namespace WzWeb.Server.Services
{
    public interface ICharacterService
    {
        public int DefaultID { get; }
        public string DefaultMotionName { get; }
        public Wz_Node CharacterNode { get; }
        public Wz_Node StringNode { get; }
        public IEnumerable<int> CharacterIDList { get; }
        public CharacterCollection GetCharacter(int id, string motionName);
        public IEnumerable<string> GetActions(int id);
    }
}
