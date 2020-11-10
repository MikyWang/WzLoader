using System;
using System.Text.RegularExpressions;
namespace WzWeb.Shared.Character
{
    public class Hair : BodyComponentBase
    {
        public override string BaseNodePath => @"Character\Hair";
        public override string BaseStringNodePath => @"String\Eqp.img\Eqp\Hair";
        public override int DefaultID => 30000;
        public override string DefaultMotionName => "walk1";
        public override ConfigType ConfigType => ConfigType.Hair;

        public override int FormatID(string text)
        {
            var regex = new Regex(@"(?<=000)[\d]+(?=(\.)img)");
            var match = regex.Match(text);
            return match.Success ? int.Parse(match.Value) : 0;
        }
    }
}
