using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WzWeb.Shared.Character
{
    public class Face : BodyComponentBase
    {
        public override string BaseNodePath => @"Character\Face";
        public override string BaseStringNodePath => @"String\Eqp.img\Eqp\Face";
        public override int DefaultID => 20000;
        public override string DefaultMotionName => "blink";

        public override ConfigType ConfigType => ConfigType.Face;

        public override PngInfo DefaultPngInfo => Component.Motion["0"]["face"].PngInfo;

        public override IList<string> ExceptMotionName => new List<string> { "info", "default" };

        public override int FormatID(string text)
        {
            var regex = new Regex(@"(?<=000)[\d]+(?=(\.)img)");
            var match = regex.Match(text);
            return match.Success ? int.Parse(match.Value) : 0;
        }
    }
}
