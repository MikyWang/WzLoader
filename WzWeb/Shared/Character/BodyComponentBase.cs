using System;
namespace WzWeb.Shared.Character
{
    public abstract class BodyComponentBase
    {
        public abstract string BaseNodePath { get; }
        public abstract string BaseStringNodePath { get; }
        public abstract int DefaultID { get; }
        public abstract string DefaultMotionName { get; }

        public abstract int FormatID(string text);

        public BodyComponent Component { get; set; }

        public static BodyComponentBase GetActuallyComponent(BodyComponent component)
        {
            switch (component.ConfigType)
            {
                case ConfigType.Hair:
                    return new Hair { Component = component };
                default:
                    return new Hair { Component = component };
            }
        }

    }

    public class BodyComponent
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public CharacterInfo Info { get; set; }
        public CharacterMotion Motion { get; set; }
        public ConfigType ConfigType { get; set; }
    }

}
