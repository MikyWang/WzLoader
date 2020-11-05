using System;
namespace WzWeb.Shared.Character
{
    public class Face
    {
        public int FaceId { get; set; }
        public string FaceName { get; set; }

        public CharacterInfo FaceInfo { get; set; }
        public CharacterMotion FaceMotion { get; set; }

    }
}
