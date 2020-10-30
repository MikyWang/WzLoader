using System;
namespace WzWeb.Shared
{
    public class UolRequest
    {
        public Node Node { get; set; }
    }

    public class UolResponse
    {
        public string Uol;
        //Uol对应的Node
        public Node TargetNode;
    }
}
