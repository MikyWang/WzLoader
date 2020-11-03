using System;
namespace WzWeb.Shared.Character
{
    public class Character
    {
        public int Id { get; set; }
        public CharacterMotion CurrentHeadMotion { get; set; }
        public CharacterMotion CurrentBodyMotion { get; set; }
        public int CurrentFrame { get; set; }

        public CharacterConfig Body => CurrentBodyMotion.Actions[CurrentFrame].Configs["body"];
        public CharacterConfig Arm => CurrentBodyMotion.Actions[CurrentFrame].Configs["arm"];
        public CharacterConfig Face => CurrentBodyMotion.Actions[CurrentFrame].Configs["face"];
        public CharacterConfig Delay => CurrentBodyMotion.Actions[CurrentFrame].Configs["delay"];
        public CharacterConfig Head => CurrentHeadMotion.Actions[CurrentFrame].Configs["head"];
        public CharacterConfig Ear => CurrentHeadMotion.Actions[CurrentFrame].Configs["ear"];
        public CharacterConfig LefEar => CurrentHeadMotion.Actions[CurrentFrame].Configs["lefEar"];
        public CharacterConfig HighLefEar => CurrentHeadMotion.Actions[CurrentFrame].Configs["highlefEar"];
        public CharacterConfig Hand => CurrentBodyMotion.Actions[CurrentFrame].Configs["hand"];
        public CharacterConfig LHand => CurrentBodyMotion.Actions[CurrentFrame].Configs["lHand"];
        public CharacterConfig RHand => CurrentBodyMotion.Actions[CurrentFrame].Configs["rHand"];
        public CharacterConfig ArmOverHair => CurrentBodyMotion.Actions[CurrentFrame].Configs["armOverHair"];

    }
}
