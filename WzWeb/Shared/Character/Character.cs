using System;
using System.Drawing;
using System.Numerics;

namespace WzWeb.Shared.Character
{
    public class Character
    {
        public int Id { get; set; }
        public CharacterMotion CurrentHeadMotion { get; set; }
        public CharacterMotion CurrentBodyMotion { get; set; }
        public CharacterMotion CurrentFaceMotion { get; set; }

        public string CurrentFrame { get; set; }
        public string CurrentFaceFrame { get; set; }
        public EarType EarType { get; set; } = EarType.Normal;
        public Point BodyPosition { get; set; } = new Point(70, 80);


        public bool HasFace => CurrentBodyMotion[CurrentFrame].HasFace == "1";
        public int Delay => int.Parse(CurrentBodyMotion[CurrentFrame].Delay);

        public CharacterConfig Body => CurrentBodyMotion[CurrentFrame]["body"];
        public CharacterConfig Face => CurrentFaceMotion[CurrentFaceFrame]["face"];
        public CharacterConfig Arm => CurrentBodyMotion[CurrentFrame]["arm"];
        public CharacterConfig Head => CurrentHeadMotion[CurrentFrame]["head"];
        public CharacterConfig Ear => CurrentHeadMotion[CurrentFrame]["ear"];
        public CharacterConfig LefEar => CurrentHeadMotion[CurrentFrame]["lefEar"];
        public CharacterConfig HighLefEar => CurrentHeadMotion[CurrentFrame]["highlefEar"];
        public CharacterConfig Hand => CurrentBodyMotion[CurrentFrame]["hand"];
        public CharacterConfig LHand => CurrentBodyMotion[CurrentFrame]["lHand"];
        public CharacterConfig RHand => CurrentBodyMotion[CurrentFrame]["rHand"];
        public CharacterConfig ArmOverHair => CurrentBodyMotion[CurrentFrame]["armOverHair"];

        public Point HeadPosition => new Point
        {
            X = NeckPosition.X - Head["neck"].X,
            Y = NeckPosition.Y - Head["neck"].Y
        };
        public Point EarPosition => new Point
        {
            X = NeckPosition.X - Ear["neck"].X,
            Y = NeckPosition.Y - Ear["neck"].Y
        };
        public Point NeckPosition => new Point
        {
            X = BodyPosition.X + Body["neck"].X,
            Y = BodyPosition.Y + Body["neck"].Y
        };
        public Point ArmPosition => new Point
        {
            X = BodyPosition.X + Body["navel"].X - Arm["navel"].X,
            Y = BodyPosition.Y + Body["navel"].Y - Arm["navel"].Y
        };
        public Point HandPosition => new Point
        {
            X = BodyPosition.X + Body["navel"].X - Hand["navel"].X,
            Y = BodyPosition.Y + Body["navel"].Y - Hand["navel"].Y
        };
        public Point FacePosition => new Point
        {
            X = HeadPosition.X + Head["brow"].X - Face["brow"].X,
            Y = HeadPosition.Y + Head["brow"].Y - Face["brow"].Y
        };
    }
}
