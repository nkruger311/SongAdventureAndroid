using System;
using System.Collections.Generic;

namespace SongAdventureAndroid
{
	public class TutorialMapCell
	{
		public int TileID
		{
			get { return BaseTiles.Count > 0 ? BaseTiles[0] : 0; }
			set
			{
				if (BaseTiles.Count > 0)
					BaseTiles[0] = value;
				else
					AddBaseTile(value);
			}
		}

		public List<int> BaseTiles = new List<int>();
		public List<int> HeightTiles = new List<int>();
		public List<int> TopperTiles = new List<int>();

		public TutorialMapCell(int tileID)
		{
			TileID = tileID;
		}

		public TutorialMapCell ()
		{
		}

		public void AddBaseTile(int tileID)
		{
			BaseTiles.Add(tileID);
		}

		public void AddHeightTile(int tileID)
		{
			HeightTiles.Add(tileID);
		}

		public void AddTopperTile(int tileID)
		{
			TopperTiles.Add(tileID);
		}

	}
}

