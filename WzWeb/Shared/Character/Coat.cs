using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WzWeb.Shared.Character
{
    public class Coat : BodyComponentBase
    {
        public override string BaseNodePath => @"Character\Coat";

        public override string BaseStringNodePath => @"String\Eqp.img\Eqp\Coat";

        public override int DefaultID => 1040036;

        public override string DefaultMotionName => "walk1";

        public override ConfigType ConfigType => ConfigType.Coat;

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
