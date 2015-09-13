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
    [XmlType("MapEntrance")]
    public class MapEntrance
    {
        public event EventHandler OnClick;
		public event EventHandler OnPlayerEnter;

        public string Name;
		public bool HasImage;
		public Image Image {get;set;}
		public Globals.MapEntranceInteractionType InteractionType;
        public bool Locked;
        public Vector2 MapPosition;
        [XmlIgnore]
        public Rectangle BoundingBox;
        public Vector2 BoundingBoxDimensions;
        public Vector2 BoundingBoxPosition;
        public Tile.TileState State = Tile.TileState.Solid;
		public string LevelNumber;
		public bool HasPrerequisiteSongism;
		public String PrerequisiteSongism;

		public bool HasPrerequisiteInventoryItem;
		public String PrerequisiteInventoryItem;

		public string NextMapName;

        public MapEntrance()
        {
            
        }

        public void LoadContent()
        {
			if (HasImage) {
				Image.LoadContent();
			}
            BoundingBox = new Rectangle((int)BoundingBoxPosition.X, (int)BoundingBoxPosition.Y, (int)BoundingBoxDimensions.X, (int)BoundingBoxDimensions.Y);
        }

        public void UnloadContent()
        {
            if (HasImage) {
				Image.UnloadContent();
			}
        }

        public void Update(GameTime gameTime, ref Player player)
        {
			if (InteractionType == Globals.MapEntranceInteractionType.Click) {
				if (InputManager.Instance.TouchPanelPressed () && BoundingBox.Contains (InputManager.Instance.TransformedTouchPosition)) {
					if (Locked) {
						if (!HasPrerequisiteSongism || (HasPrerequisiteSongism && Inventory.Instance.PrerequisiteSongismHasBeenDiscovered (PrerequisiteSongism))) {
							Locked = false;
						}
					}

					if (!Locked) {
						OnClick (this, null);	
					}
				}
			}

            if (HasImage) {
				Image.Update(gameTime);
			}

            if ((State & Tile.TileState.Solid) == Tile.TileState.Solid)
            {
                //Rectangle playerRect = new Rectangle((int)player.Image.Position.X, (int)player.Image.Position.Y, player.Image.SourceRect.Width, player.Image.SourceRect.Height);
				Rectangle playerRect = new Rectangle ((int)player.Image.Position.X, (int)(player.Image.Position.Y + (player.Image.SourceRect.Height / 2)), player.Image.SourceRect.Width, (int)(player.Image.SourceRect.Height / 2));

                if (playerRect.Intersects(BoundingBox))
                {
					if (InteractionType == Globals.MapEntranceInteractionType.PlayerEnter) {
						OnPlayerEnter (this, null);
					}

					Rectangle rectIntersection = Rectangle.Intersect (playerRect, BoundingBox);

					if (rectIntersection.Height > rectIntersection.Width) {
						if (player.Velocity.X < 0) {
							player.UpdateImagePositionX (BoundingBox.Right);
							player.Velocity = new Vector2 (0, player.Velocity.Y);
						}

						if (player.Velocity.X > 0) {
							player.UpdateImagePositionX (BoundingBox.Left - player.Image.SourceRect.Width);
							player.Velocity = new Vector2 (0, player.Velocity.Y);
						}
					}

					if (rectIntersection.Width > rectIntersection.Height) {
						if (player.Velocity.Y < 0) {
							//player.Image.Position.Y = BoundingBox.Bottom;
							player.UpdateImagePositionY (BoundingBox.Bottom - (player.Image.SourceRect.Height / 2));
							player.Velocity = new Vector2 (player.Velocity.X, 0);
						}

						if (player.Velocity.Y > 0) {
							player.UpdateImagePositionY (BoundingBox.Top - player.Image.SourceRect.Height);
							player.Velocity = new Vector2 (player.Velocity.X, 0);
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
			if (HasImage) {
				if (Camera2D.Instance.IsInView (Image.Position, Image.Texture)) {
					Image.Draw (spriteBatch);
				}
			}
        }
    }
}
