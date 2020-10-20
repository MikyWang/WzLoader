using System;
using System.Xml;
using WzLib;

namespace WzLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            var structure = WzLoader.Instance.Structure;

            foreach (var node in structure.WzNode.Nodes)
            {
                WzLoader.Instance.OutputNode(node);

            }

            structure.Clear();
        }
    }
}
