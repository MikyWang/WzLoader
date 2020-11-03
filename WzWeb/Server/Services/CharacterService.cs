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
        private readonly IWzLoader wzLoader;
        public int DefaultID => 2000;
        public Wz_Node CharacterNode => wzLoader.CharacterNode;
        public IEnumerable<int> CharacterIDList { get; private set; }

        public CharacterService(IWzLoader wzLoader)
        {
            this.wzLoader = wzLoader;
            CharacterIDList = CharacterNode.Nodes.Select(node => FormatID(node.Text)).Where(id => id != 0);
        }
        public Character GetCharacter(int id)
        {
            if (!CharacterIDList.Contains(id)) return null;

            var wz_nodes = CharacterNode.Nodes.ToList().FindAll(node => FormatID(node.Text) == id);
            if (wz_nodes.Count == 0) return null;

            var headNode = wz_nodes.Find(node => node.Text.StartsWith("0001")).GetImageNode();
            var bodyNode = wz_nodes.Find(node => node.Text.StartsWith("0000")).GetImageNode();

            #region 获取所有动作
            Func<Wz_Node, Wz_Node, Dictionary<string, CharacterMotion>> getMotions = (node, baseNode) =>
              {
                  var actionNodes = node.Nodes.Where(node => (node.Text != "info") && (node.Text != "front") && (node.Text != "back")).Take(1);
                  var actions = new Dictionary<string, CharacterMotion>();
                  foreach (var acNode in actionNodes)
                  {
                      actions.Add(acNode.Text, acNode.GetCharacterMotion(baseNode));
                  }
                  return actions;
              };
            #endregion

            Character character = new Character
            {
                Id = id,
                HeadInfo = headNode.GetCharacterInfo(),
                HeadActions = getMotions(headNode, CharacterNode),
                BodyInfo = bodyNode.GetCharacterInfo(),
                BodyActions = getMotions(bodyNode, CharacterNode)
            };

            return character;

        }
        private int FormatID(string id)
        {
            var regex = new Regex(@"(?<=000(0|1))[\d]+(?=(\.)img)");
            var match = regex.Match(id);

            return match.Success ? int.Parse(match.Value) : 0;
        }
    }
}
