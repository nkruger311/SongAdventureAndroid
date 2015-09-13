using System;
using System.Collections.Generic;

namespace SongAdventureAndroid
{
	public class TutorialMapRow
	{
		public List<TutorialMapCell> Columns = new List<TutorialMapCell>();
	}

	public class TutorialTileMap
	{
		public List<TutorialMapRow> Rows = new List<TutorialMapRow>();
		public int MapWidth = 50;
		public int MapHeight = 50;

		public TutorialTileMap ()
		{
			for (int y = 0; y < MapHeight; y++)
			{
				TutorialMapRow thisRow = new TutorialMapRow();
				for (int x = 0; x < MapWidth; x++)
				{
					thisRow.Columns.Add(new TutorialMapCell(0));
				}
				Rows.Add(thisRow);
			}

			// Create Sample Map Data
			Rows[0].Columns[3].TileID = 3;
			Rows[0].Columns[4].TileID = 3;
			Rows[0].Columns[5].TileID = 1;
			Rows[0].Columns[6].TileID = 1;
			Rows[0].Columns[7].TileID = 1;

			Rows[1].Columns[3].TileID = 3;
			Rows[1].Columns[4].TileID = 1;
			Rows[1].Columns[5].TileID = 1;
			Rows[1].Columns[6].TileID = 1;
			Rows[1].Columns[7].TileID = 1;

			Rows[2].Columns[2].TileID = 3;
			Rows[2].Columns[3].TileID = 1;
			Rows[2].Columns[4].TileID = 1;
			Rows[2].Columns[5].TileID = 1;
			Rows[2].Columns[6].TileID = 1;
			Rows[2].Columns[7].TileID = 1;

			Rows[3].Columns[2].TileID = 3;
			Rows[3].Columns[3].TileID = 1;
			Rows[3].Columns[4].TileID = 1;
			Rows[3].Columns[5].TileID = 2;
			Rows[3].Columns[6].TileID = 2;
			Rows[3].Columns[7].TileID = 2;

			Rows[4].Columns[2].TileID = 3;
			Rows[4].Columns[3].TileID = 1;
			Rows[4].Columns[4].TileID = 1;
			Rows[4].Columns[5].TileID = 2;
			Rows[4].Columns[6].TileID = 2;
			Rows[4].Columns[7].TileID = 2;

			Rows[5].Columns[2].TileID = 3;
			Rows[5].Columns[3].TileID = 1;
			Rows[5].Columns[4].TileID = 1;
			Rows[5].Columns[5].TileID = 2;
			Rows[5].Columns[6].TileID = 2;
			Rows[5].Columns[7].TileID = 2;

			Rows[16].Columns[4].AddHeightTile(54);

			Rows[17].Columns[3].AddHeightTile(54);

			Rows[15].Columns[3].AddHeightTile(54);
			Rows[16].Columns[3].AddHeightTile(53);

			Rows[15].Columns[4].AddHeightTile(54);
			Rows[15].Columns[4].AddHeightTile(54);
			Rows[15].Columns[4].AddHeightTile(51);

			Rows[18].Columns[3].AddHeightTile(51);
			Rows[19].Columns[3].AddHeightTile(50);
			Rows[18].Columns[4].AddHeightTile(55);

			Rows[14].Columns[4].AddHeightTile(54);

			Rows[14].Columns[5].AddHeightTile(62);
			Rows[14].Columns[5].AddHeightTile(61);
			Rows[14].Columns[5].AddHeightTile(63);

			Rows[17].Columns[4].AddTopperTile(114);
			Rows[16].Columns[5].AddTopperTile(115);
			Rows[14].Columns[4].AddTopperTile(125);
			Rows[15].Columns[5].AddTopperTile(91);
			Rows[16].Columns[6].AddTopperTile(94);

			// End Create Sample Map Data
		}
	}
}

