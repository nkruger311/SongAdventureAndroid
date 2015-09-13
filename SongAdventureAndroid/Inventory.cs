using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SongAdventureAndroid
{
    public class Inventory
    {
        [XmlIgnore]
        public List<Songism> SongBook;

		[XmlIgnore]
		public List<InventoryItem> Items;

        private static Inventory instance;

		public static Inventory Instance
        {
            get
            {
                if (instance == null)
                    instance = new Inventory();

                return instance;
            }
        }

        private Inventory()
        {
            XmlManager<List<Songism>> songBookLoader = new XmlManager<List<Songism>>();
            SongBook = songBookLoader.Load(System.IO.Path.Combine(Globals.LoadGameplayDirectory, "SongBook.xml"));

			XmlManager<List<InventoryItem>> xmItemsLoader = new XmlManager<List<InventoryItem>> ();
			Items = xmItemsLoader.Load (System.IO.Path.Combine (Globals.LoadGameplayDirectory, "InventoryItems.xml"));
        }

        public void Unload()
        {
            Save();
        }

        public void Save()
        {
            XmlManager<List<Songism>> songBookSaver = new XmlManager<List<Songism>>();
            songBookSaver.Save(System.IO.Path.Combine(Globals.LoadGameplayDirectory, "SongBook.xml"), SongBook);

			XmlManager<List<InventoryItem>> xmItemsSaver = new XmlManager<List<InventoryItem>> ();
			xmItemsSaver.Save (System.IO.Path.Combine (Globals.LoadGameplayDirectory, "InventoryItems.xml"), Items);
        }

		public int GetNumberOfDiscoveredSongismsPerLevel(int levelNumber)
		{
			int iDiscoveredSongisms = 0;
			foreach (Songism songism in SongBook) {
				//if (songism.Discovered && songism.LevelNumber == levelNumber.ToString()) {
				//	iDiscoveredSongisms++;
				//}
				iDiscoveredSongisms++;
			}

			return iDiscoveredSongisms;
		}

		public bool PrerequisiteSongismHasBeenDiscovered(string songName)
		{
			foreach (Songism song in SongBook) {
				if (string.Equals (song.Name, songName)) {
					return true;
				}
			}

			return false;
		}

		public bool PrerequisiteInventoryItemHasBeenDiscovered(string itemName)
		{
			foreach (InventoryItem item in Items) {
				if (string.Equals (item.Name, itemName)) {
					return true;
				}
			}

			return false;
		}
    }
}
