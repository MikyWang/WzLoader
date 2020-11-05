﻿using System;
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
        public Point NeckPosition { get; set; } = new Point(50, 50);

        public CharacterConfig Body => CurrentBodyMotion.Actions[CurrentFrame].Configs["body"];
        public CharacterConfig Face => CurrentFaceMotion?.Actions[CurrentFaceFrame].Configs["face"];
        public CharacterConfig Arm => CurrentBodyMotion.Actions[CurrentFrame].Configs["arm"];
        public bool HasFace => CurrentBodyMotion.Actions[CurrentFrame].HasFace == "1";
        public int Delay => int.Parse(CurrentBodyMotion.Actions[CurrentFrame].Delay);
        public CharacterConfig Head => CurrentHeadMotion.Actions[CurrentFrame].Configs["head"];
        public CharacterConfig Ear => CurrentHeadMotion.Actions[CurrentFrame].Configs["ear"];
        public CharacterConfig LefEar => CurrentHeadMotion.Actions[CurrentFrame].Configs["lefEar"];
        public CharacterConfig HighLefEar => CurrentHeadMotion.Actions[CurrentFrame].Configs["highlefEar"];
        public CharacterConfig Hand => CurrentBodyMotion.Actions[CurrentFrame].Configs["hand"];
        public CharacterConfig LHand => CurrentBodyMotion.Actions[CurrentFrame].Configs["lHand"];
        public CharacterConfig RHand => CurrentBodyMotion.Actions[CurrentFrame].Configs["rHand"];
        public CharacterConfig ArmOverHair => CurrentBodyMotion.Actions[CurrentFrame].Configs["armOverHair"];

        public Point HeadPosition => new Point(NeckPosition.X - Head.Map["neck"].X, NeckPosition.Y - Head.Map["neck"].Y);
        public Point BodyPosition => new Point(NeckPosition.X - Body.Map["neck"].X, NeckPosition.Y - Body.Map["neck"].Y);
        public Point ArmPosition => new Point(BodyPosition.X + Body.Map["navel"].X - Arm.Map["navel"].X, BodyPosition.Y + Body.Map["navel"].Y - Arm.Map["navel"].Y);
        public Point FacePosition => new Point
        {
            X = HeadPosition.X + Head.Map["brow"].X - Face.Map["brow"].X,
            Y = HeadPosition.Y + Head.Map["brow"].Y - Face.Map["brow"].Y
        };
    }
}
