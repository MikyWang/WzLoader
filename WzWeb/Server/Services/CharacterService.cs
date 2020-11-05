using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WzLib;
using WzWeb.Server.Extentions;
using WzWeb.Shared.Character;

namespace WzWeb.Server.Services
{
    public class CharacterService : ICharacterService
    {
        public int DefaultID => 2000;
        public int DefaultFaceID => 20000;
        public string DefaultMotionName => "walk1";
        public string DefaultFaceMotionName => "blink";
        public Wz_Node CharacterNode => wzLoader.CharacterNode;
        public Wz_Node StringNode => wzLoader.StringNode;
        public Wz_Node FaceNode => CharacterNode.SearchNode("Face");
        public Wz_Node FaceStringNode => StringNode.SearchNode(@"Eqp.img\Eqp\Face");
        public IEnumerable<int> CharacterIDList { get; private set; }
        public IEnumerable<int> FaceIDList { get; private set; }

        private readonly IWzLoader wzLoader;

        public CharacterService(IWzLoader wzLoader)
        {
            this.wzLoader = wzLoader;
            CharacterIDList = CharacterNode.Nodes.Select(node => FormatID(node.Text)).Where(id => id != 0);
            FaceIDList = FaceNode.Nodes.Select(nd => FormatFaceId(nd.Text)).Where(id => id != 0);
        }
        public CharacterCollection GetCharacter(int id, string motionName)
        {
            if (!CharacterIDList.Contains(id)) return null;

            var wz_nodes = CharacterNode.Nodes.ToList().FindAll(node => FormatID(node.Text) == id);
            if (wz_nodes.Count == 0) return null;

            var headNode = wz_nodes.Find(node => node.Text.StartsWith("0001")).GetImageNode();
            var bodyNode = wz_nodes.Find(node => node.Text.StartsWith("0000")).GetImageNode();

            if (headNode == null || bodyNode == null) return null;

            CharacterCollection character = new CharacterCollection
            {
                Id = id,
                HeadInfo = headNode.GetCharacterInfo(),
                HeadMotion = headNode.Nodes[motionName]?.GetCharacterMotion(CharacterNode, ConfigType.Head),
                BodyInfo = bodyNode.GetCharacterInfo(),
                BodyMotion = bodyNode.Nodes[motionName]?.GetCharacterMotion(CharacterNode, ConfigType.Body),
            };

            return character;

        }

        public IEnumerable<string> GetActions(int id)
        {
            var node = CharacterNode.Nodes.First(node => FormatID(node.Text) == id)?.GetImageNode();
            if (node == null) return null;
            return node.Nodes.Where(node => (node.Text != "info")).Select(node => node.Text);
        }
        public Face GetFace(int faceId, string faceMotionName)
        {
            if (!FaceIDList.Contains(faceId)) return null;

            var node = FaceNode.Nodes.First(nd => FormatFaceId(nd.Text) == faceId).GetImageNode();
            var motionNode = node.SearchNode(faceMotionName);
            var faceString = FaceStringNode.SearchNode(faceId.ToString()).Nodes["name"].Value.ToString();
            return new Face
            {
                FaceId = faceId,
                FaceInfo = node.GetCharacterInfo(),
                FaceMotion = motionNode.GetCharacterMotion(CharacterNode, ConfigType.Face),
                FaceName = faceString
            };

        }

        private int FormatID(string id)
        {
            var regex = new Regex(@"(?<=000(0|1))[\d]+(?=(\.)img)");
            var match = regex.Match(id);

            return match.Success ? int.Parse(match.Value) : 0;
        }
        private int FormatFaceId(string faceId)
        {
            var regex = new Regex(@"(?<=000)[\d]+(?=(\.)img)");
            var match = regex.Match(faceId);
            return match.Success ? int.Parse(match.Value) : 0;
        }
    }
}
