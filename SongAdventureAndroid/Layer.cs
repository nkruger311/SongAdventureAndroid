using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SongAdventureAndroid
{
    public class Layer
    {
        public class TileMap
        {
            [XmlElement("Row")]
			public List<string> Row { get; set; }

            public TileMap()
            {
                Row = new List<string>();
            }
        }

        [XmlElement("TileMap")]
		public TileMap Tile { get; set; }
		public Image Image {get;set;}
		public string SolidTiles { get; set; }
		public string OverlayTiles { get; set; }
		public string EntranceTiles { get; set; }
        List<Tile> underlayTiles, overlayTiles;
        Tile.TileState state;
		public Vector2 TileDimensions{ get; set; }

        public Layer()
        {
            Image = new Image();
            underlayTiles = new List<Tile>();
            overlayTiles = new List<Tile>();
			SolidTiles = OverlayTiles = EntranceTiles = String.Empty;
        }

        public void LoadContent(Vector2 tileDimensions)
        {
            Image.LoadContent();
            Vector2 position = -tileDimensions;

            foreach (string row in Tile.Row)
            {
                string[] split = row.Split(']');
                position.X = -tileDimensions.X;
                position.Y += tileDimensions.Y;
                foreach (string s in split)
                {
                    if (s != String.Empty)
                    {
                        position.X += tileDimensions.X;
                        if (!s.Contains('x'))
                        {
                            state = SongAdventureAndroid.Tile.TileState.Passive;
                            Tile tile = new Tile();
                            
                            string str = s.Replace("[", String.Empty);
                            int value1 = int.Parse(str.Substring(0, str.IndexOf(':')));
                            int value2 = int.Parse(str.Substring(str.IndexOf(':') + 1));

							if (SolidTiles.Contains ("[" + value1.ToString () + ":" + value2.ToString () + "]"))
								state = SongAdventureAndroid.Tile.TileState.Solid;
							else if (EntranceTiles.Contains (String.Format ("[{0}:{1}]", value1.ToString (), value2.ToString ())))
								state = SongAdventureAndroid.Tile.TileState.Entrance;

                            tile.LoadContent(position, new Rectangle(value1 * (int)tileDimensions.X, value2 * (int)tileDimensions.Y, (int)tileDimensions.X, (int)tileDimensions.Y), state);

                            if (OverlayTiles.Contains("[" + value1.ToString() + ":" + value2.ToString() + "]"))
                                overlayTiles.Add(tile);
                            else
                                underlayTiles.Add(tile);
                        }
                    }
                }
            }
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime, ref Player player)
        {
            foreach (Tile tile in underlayTiles)
                tile.Update(gameTime, ref player);

            foreach (Tile tile in overlayTiles)
                tile.Update(gameTime, ref player);
        }

        public void Draw(SpriteBatch spriteBatch, string drawType)
        {
            List<Tile> tiles;
            if (drawType == "Underlay")
                tiles = underlayTiles;
            else
                tiles = overlayTiles;

            foreach (Tile tile in tiles)
            {
				if (Camera2D.Instance.IsInView (tile.Position, Image.Texture)) {
					Image.Position = tile.Position;
					Image.SourceRect = tile.SourceRect;
					Image.Draw (spriteBatch);
				}
            }
        }
    }
}
