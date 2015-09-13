using System;

namespace SongAdventureAndroid
{
    public static class Globals
    {
        public static string ApplicationDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static string LoadDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Load");
        public static string LoadGameplayDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Load/Gameplay");
        public static string LoadGameplayMapsDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Load/Gameplay/Maps");
		public static string LoadGameplayMapEntrancesDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Load/Gameplay/MapEntrances");
        public static string LoadGameplaySongismsDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Load/Gameplay/Songisms");
		public static string LoadGameplayNPCDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Load/Gameplay/NPCs");
        public static string LoadGameplayScreensDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Load/Gameplay/Screens");
        public static string LoadMenusDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Load/Menus");

        public enum TextAlignment { Left, Center, Right }

		[Flags]
		public enum MapEntranceInteractionType
		{ 
			Click = 1, 
			PlayerEnter = 2 
		}
    }
}