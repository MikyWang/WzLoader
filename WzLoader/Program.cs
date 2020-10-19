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
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            settings.ConformanceLevel = ConformanceLevel.Auto;
            var xmlWriter = XmlWriter.Create("test.xml", settings);

            foreach (var node in structure.WzNode.Nodes)
            {
                WzLoader.Instance.OutputNode(node);
            }

        }
    }
}
