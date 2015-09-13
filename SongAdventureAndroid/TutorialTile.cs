using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SongAdventureAndroid
{
	static class TutorialTile
	{
		static public Texture2D TileSetTexture;
		//static public int TileWidth = 33;
		//static public int TileHeight = 27;

		//static public int TileStepX = 52;
		//static public int TileStepY = 14;

		//static public int OddRowXOffset = 26;



		static public int TileWidth = 64;
		static public int TileHeight = 64;
		static public int TileStepX = 64;
		static public int TileStepY = 16;
		static public int OddRowXOffset = 32;
		static public int HeightTileOffset = 32;

		static public Rectangle GetSourceRectangle(int tileIndex)
		{
			int iTileY = tileIndex / (TileSetTexture.Width / TileWidth);
			int iTileX = tileIndex % (TileSetTexture.Width / TileWidth);

			return new Rectangle (iTileX * TileWidth, iTileY * TileHeight, TileWidth, TileHeight);
		}
	}
}

