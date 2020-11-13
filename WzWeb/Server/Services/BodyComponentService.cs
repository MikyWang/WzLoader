using System;
using System.Linq;
using WzLib;
using WzWeb.Shared;
using WzWeb.Server.Extentions;
using WzWeb.Shared.Character;
using System.Collections.Generic;

namespace WzWeb.Server.Services
{
    public class BodyComponentService : IBodyComponetService
    {
        public Wz_Node CharacterNode { get; private set; }
        public Wz_Node StringNode { get; private set; }

        private readonly IWzLoader wzLoader;

        private static readonly object _lock = new object();

        public BodyComponentService(IWzLoader _wzLoader)
        {
            wzLoader = _wzLoader;
            CharacterNode = wzLoader.CharacterNode;
            StringNode = wzLoader.StringNode;
        }

        public BodyComponent GetBodyComponent(BodyComponent bodyComponent, bool isDefault = true)
        {

            Wz_Node compNode;
            Wz_Node compStringNode;

            var actComponent = BodyComponentBase.GetActuallyComponent(bodyComponent);


            compNode = CharacterNode.SearchNode(actComponent.BaseNodePath);
            compStringNode = StringNode.SearchNode(actComponent.BaseStringNodePath);

            if (isDefault)
            {
                bodyComponent.ID = actComponent.DefaultID;
            }

            var node = compNode.Nodes.First(nd => actComponent.FormatID(nd.Text) == bodyComponent.ID).GetImageNode();
            var stringNode = compStringNode.SearchNode(bodyComponent.ID.ToString());
            string name;
            if (stringNode == null)
            {
                name = null;
            }
            else if (stringNode.Nodes["name"] == null)
            {
                name = $"{stringNode.Text}:{stringNode.Value}";
            }
            else
            {
                name = stringNode.Nodes["name"].Value.ToString();
            }
            bodyComponent.Name = name;
            bodyComponent.Info = node.GetCharacterInfo(CharacterNode);
            var motionNode = node.SearchNode(actComponent.DefaultMotionName);
            bodyComponent.Motion = motionNode.GetCharacterMotion(CharacterNode, bodyComponent.ConfigType);
            return bodyComponent;

        }

        public ListResponse<BodyComponent> GetBodyComponentList(ListRequest<BodyComponent> request)
        {
            lock (_lock)
            {
                var actComponent = BodyComponentBase.GetActuallyComponent(request.Parameter);
                var compNode = CharacterNode.SearchNode(actComponent.BaseNodePath);
                compNode.Nodes.SortByImgID();

                var resp = new ListResponse<BodyComponent>
                {
                    Results = compNode.Nodes.Skip(request.Start).Take(request.Num).Select(nd =>
                    {
                        var comp = new BodyComponent
                        {
                            ID = actComponent.FormatID(nd.Text),
                            ConfigType = actComponent.ConfigType
                        };
                        return GetBodyComponent(comp, false);
                    }).ToList()
                };
                resp.HasNext = resp.Results.Count == request.Num;
                return resp;
            }

        }
    }
}
