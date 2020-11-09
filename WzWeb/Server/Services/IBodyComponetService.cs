using System;
using WzLib;
using WzWeb.Shared;
using WzWeb.Shared.Character;
namespace WzWeb.Server.Services
{
    public interface IBodyComponetService
    {
        public Wz_Node CharacterNode { get; }
        public Wz_Node StringNode { get; }

        public BodyComponent GetBodyComponent(BodyComponent bodyComponent, bool isDefault = true);
        public ListResponse<BodyComponent> GetBodyComponentList(ListRequest<BodyComponent> request);
    }
}
