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
        public int DefaultFaceID { get; }
        public string DefaultFaceMotionName { get; }
        public Wz_Node CharacterNode { get; }
        public Wz_Node FaceNode { get; }
        public Wz_Node StringNode { get; }
        public Wz_Node FaceStringNode { get; }
        public IEnumerable<int> CharacterIDList { get; }
        public IEnumerable<int> FaceIDList { get; }
        public CharacterCollection GetCharacter(int id, string motionName);
        public IEnumerable<string> GetActions(int id);
        public Face GetFace(int faceId, string faceMotionName);
        public ListResponse<Face> GetFaces(ListRequest<Face> request);

    }
}
