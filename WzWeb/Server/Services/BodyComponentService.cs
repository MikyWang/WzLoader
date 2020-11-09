using System;
using System.Linq;
using WzLib;
using WzWeb.Shared;
using WzWeb.Server.Extentions;
using WzWeb.Shared.Character;

namespace WzWeb.Server.Services
{
    public class BodyComponentService : IBodyComponetService
    {
        public Wz_Node CharacterNode { get; private set; }
        public Wz_Node StringNode { get; private set; }

        private readonly IWzLoader wzLoader;

        public BodyComponentService(IWzLoader _wzLoader)
        {
            wzLoader = _wzLoader;
            CharacterNode = wzLoader.CharacterNode.Clone() as Wz_Node;
            StringNode = wzLoader.StringNode.Clone() as Wz_Node;
        }

        public BodyComponent GetBodyComponent(BodyComponent bodyComponent, bool isDefault = true)
        {
            var actComponent = BodyComponentBase.GetActuallyComponent(bodyComponent);
            var compNode = CharacterNode.SearchNode(actComponent.BaseNodePath);
            var compStringNode = StringNode.SearchNode(actComponent.BaseStringNodePath);

            if (isDefault)
            {
                bodyComponent.ID = actComponent.DefaultID;
            }

            var node = compNode.Nodes.First(nd => actComponent.FormatID(nd.Text) == bodyComponent.ID).GetImageNode();
            var motionNode = node.SearchNode(actComponent.DefaultMotionName);
            var stringNode = compStringNode.SearchNode(bodyComponent.ID.ToString());
            bodyComponent.Info = node.GetCharacterInfo();
            bodyComponent.Motion = motionNode.GetCharacterMotion(CharacterNode, ConfigType.Face);
            return bodyComponent;
        }

        public ListResponse<BodyComponent> GetBodyComponentList(ListRequest<BodyComponent> request)
        {
            var actComponent = BodyComponentBase.GetActuallyComponent(request.Parameter);
            var compNode = CharacterNode.SearchNode(actComponent.BaseNodePath);
            var compStringNode = StringNode.SearchNode(actComponent.BaseStringNodePath);
            var resp = new ListResponse<BodyComponent>
            {
                Results = compNode.Nodes.Skip(request.Start).Take(request.Num).Select(nd =>
                {
                    var comp = request.Parameter;
                    return GetBodyComponent(comp);
                }).ToList()
            };
            resp.HasNext = resp.Results.Count == request.Num;
            return resp;
        }
    }
}
