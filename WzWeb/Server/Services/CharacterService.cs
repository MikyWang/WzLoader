﻿using System;
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
        public string DefaultMotionName => "walk1";
        public Wz_Node CharacterNode => wzLoader.CharacterNode;
        public IEnumerable<int> CharacterIDList { get; private set; }

        private readonly IWzLoader wzLoader;

        public CharacterService(IWzLoader wzLoader)
        {
            this.wzLoader = wzLoader;
            CharacterIDList = CharacterNode.Nodes.Select(node => FormatID(node.Text)).Where(id => id != 0);
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
                HeadMotion = headNode.Nodes[motionName]?.GetCharacterMotion(CharacterNode),
                BodyInfo = bodyNode.GetCharacterInfo(),
                BodyMotion = bodyNode.Nodes[motionName]?.GetCharacterMotion(CharacterNode)
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
