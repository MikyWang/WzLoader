using System;
using System.Collections.Generic;

namespace WzWeb.Shared.Character
{
    public abstract class BodyComponentBase
    {
        public abstract string BaseNodePath { get; }
        public abstract string BaseStringNodePath { get; }
        public abstract int DefaultID { get; }
        public abstract string DefaultMotionName { get; }
        public abstract ConfigType ConfigType { get; }
        public abstract int FormatID(string text);
        public abstract PngInfo DefaultPngInfo { get; }
        public abstract IList<string> ExceptMotionName { get; }

        public BodyComponent Component { get; set; }

        public static BodyComponentBase GetActuallyComponent(BodyComponent component)
        {
            switch (component.ConfigType)
            {
                case ConfigType.Hair:
                    return new Hair { Component = component };
                case ConfigType.Face:
                    return new Face { Component = component };
                case ConfigType.Coat:
                    return new Coat { Component = component };
                case ConfigType.Pants:
                    return new Pants { Component = component };
                default:
                    throw new DataMisalignedException("未添加身体部位类型判断!");
            }
        }

    }

    public class BodyComponent
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string MotionName { get; set; }
        public CharacterInfo Info { get; set; }
        public CharacterMotion Motion { get; set; }
        public ConfigType ConfigType { get; set; }
    }

}
