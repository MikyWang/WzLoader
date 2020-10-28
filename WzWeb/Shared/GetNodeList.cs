using System;
using System.Collections.Generic;

namespace WzWeb.Shared
{
    /// <summary>
    /// 分页返回节点数组
    /// </summary>
    public class GetNodeListRequest
    {
        public Node Node { get; set; }
        public int Start { get; set; }
        public int Num { get; set; }
    }

    public class GetNodeListResponse
    {
        public List<Node> Nodes { get; set; }
        public bool HasNext { get; set; }
    }
}
