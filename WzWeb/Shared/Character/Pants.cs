using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WzWeb.Shared.Character
{
    public class Pants : BodyComponentBase
    {
        public override string BaseNodePath => @"Character\Pants";

        public override string BaseStringNodePath => @"String\Eqp.img\Eqp\Pants";

        public override int DefaultID => 1060026;

        public override string DefaultMotionName => "walk1";

        public override ConfigType ConfigType => ConfigType.Pants;

        public override PngInfo DefaultPngInfo => Component.Info.IconRaw;

        public override IList<string> ExceptMotionName => new List<string> { "info" };

        public override int FormatID(string text)
        {
            var regex = new Regex(@"(?<=0)[\d]+(?=(\.)img)");
            var match = regex.Match(text);
            return match.Success ? int.Parse(match.Value) : 0;
        }
    }
}
