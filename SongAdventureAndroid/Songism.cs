using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SongAdventureAndroid
{
    [XmlType("Songism")]
    public class Songism
    {
        public event EventHandler OnClick;

        public string Name;
        public Image Image;
        public bool Discovered;
        public Vector2 MapPosition;
        [XmlIgnore]
        public Rectangle BoundingBox;
        public Vector2 BoundingBoxDimensions;
        public Vector2 BoundingBoxPosition;
        public Tile.TileState State = Tile.TileState.Solid;
        public string AlbumName;
        public string SongInfo;
		public string LevelNumber;
		public bool HasPrerequisiteSongism;
		public String PrerequisiteSongism;

		public bool HasPrerequisiteInventoryItem;
		public String PrerequisiteInventoryItem;
		public InventoryItem InventoryReward;

        public Songism()
        {
            
        }

        public void LoadContent()
        {
            Image.LoadContent();
            BoundingBox = new Rectangle((int)BoundingBoxPosition.X, (int)BoundingBoxPosition.Y, (int)BoundingBoxDimensions.X, (int)BoundingBoxDimensions.Y);
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime, ref Player player)
        {
            if (InputManager.Instance.TouchPanelPressed() && BoundingBox.Contains(InputManager.Instance.TransformedTouchPosition))
                OnClick(this, null);

            Image.Update(gameTime);

            if ((State & Tile.TileState.Solid) == Tile.TileState.Solid)
            {
                //Rectangle playerRect = new Rectangle((int)player.Image.Position.X, (int)player.Image.Position.Y, player.Image.SourceRect.Width, player.Image.SourceRect.Height);
				Rectangle playerRect = new Rectangle ((int)player.Image.Position.X, (int)(player.Image.Position.Y + (player.Image.SourceRect.Height / 2)), player.Image.SourceRect.Width, (int)(player.Image.SourceRect.Height / 2));

                if (playerRect.Intersects(BoundingBox))
                {
					Rectangle rectIntersection = Rectangle.Intersect (playerRect, BoundingBox);

					if (rectIntersection.Height > rectIntersection.Width) {
						if (player.Velocity.X < 0) {
							player.Image.Position.X = BoundingBox.Right;
							player.Velocity.X = 0;
						}

						if (player.Velocity.X > 0) {
							player.Image.Position.X = BoundingBox.Left - player.Image.SourceRect.Width;
							player.Velocity.X = 0;
						}
					}

					if (rectIntersection.Width > rectIntersection.Height) {
						if (player.Velocity.Y < 0) {
							//player.Image.Position.Y = BoundingBox.Bottom;
							player.Image.Position.Y = BoundingBox.Bottom - (player.Image.SourceRect.Height / 2);
							player.Velocity.Y = 0;
						}

						if (player.Velocity.Y > 0) {
							player.Image.Position.Y = BoundingBox.Top - player.Image.SourceRect.Height;
							player.Velocity.Y = 0;
						}
					}

                    //if (player.Velocity.X < 0)
                        //player.Image.Position.X = BoundingBox.Right;
                    //else if (player.Velocity.X > 0)
                        //player.Image.Position.X = BoundingBox.Left - player.Image.SourceRect.Width;
                    //else if (player.Velocity.Y < 0)
                        //player.Image.Position.Y = BoundingBox.Bottom;
                    //else
                        //player.Image.Position.Y = BoundingBox.Top - player.Image.SourceRect.Height;

                    //player.Velocity = Vector2.Zero;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Camera2D.Instance.IsInView(Image.Position, Image.Texture))
                Image.Draw(spriteBatch);
        }
    }
}
