namespace WzWeb.Client
{
    public class CommonStrings
    {
        public static readonly string EMPTY_LINK = @"javascript:void(0)";

        #region MapleControllerAPI
        public static string MAPLE => @"api/maple";
        public static string MAPLE_GET_NODE => $"{MAPLE}/GetNode";
        public static string MAPLE_POST_NODELIST => $"{MAPLE}/GetNodeList";
        public static string MAPLE_POST_FILEINFO => $"{MAPLE}/GetFileInfo";
        public static string MAPLE_POST_PNG => $"{MAPLE}/GetPng";
        public static string MAPLE_POST_UOL => $"{MAPLE}GetUol";
        #endregion

        #region CharacterControllerAPI
        public static string CHARACTER => @"api/Character";
        public static string CHARACTER_GET_DEFAULT_FACE => $"{CHARACTER}/GetDefaultFace";
        public static string CHARACTER_POST_CHARACTER => $"{CHARACTER}/GetCharacter";
        public static string CHARACTER_GET_SKIN_LIST => $"{CHARACTER}/GetSkins";
        public static string CHARACTER_GET_ACTION_LIST => $"{CHARACTER}/GetActions";
        public static string CHARACTER_POST_FACES => $"{CHARACTER}/GetFaces";
        #endregion

        #region BodyComponentControllerAPI
        public static string BODY_POST_COMPONENT = @"api/BodyComponent/GetBodyComponent";
        public static string BODY_POST_COMPONENT_LIST = @"api/BodyComponent/GetBodyComponentList";
        #endregion
    }
}
