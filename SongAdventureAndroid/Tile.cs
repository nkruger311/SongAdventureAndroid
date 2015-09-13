using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SongAdventureAndroid
{
    public class Tile
    {
        [Flags]
        public enum TileState
        { 
            Passive = 1, 
            Solid = 2,
			Entrance = 4
        }

        Vector2 position;
        Rectangle sourceRect;
        TileState state;

        public Rectangle SourceRect { get { return sourceRect; } }
        public Vector2 Position { get { return position; } }

        public void LoadContent(Vector2 position, Rectangle sourceRect, TileState state)
        {
            this.position = position;
            this.sourceRect = sourceRect;
            this.state = state;
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime, ref Player player)
        {
			if ((state & TileState.Solid) == TileState.Solid || (state & TileState.Entrance) == TileState.Entrance)
            {
                Rectangle tileRect = new Rectangle((int)position.X, (int)position.Y, sourceRect.Width, sourceRect.Height);
                //Rectangle playerRect = new Rectangle((int)player.Image.Position.X, (int)player.Image.Position.Y, player.Image.SourceRect.Width, player.Image.SourceRect.Height);
				Rectangle playerRect = new Rectangle ((int)player.Image.Position.X, (int)(player.Image.Position.Y + (player.Image.SourceRect.Height / 2)), player.Image.SourceRect.Width, (int)(player.Image.SourceRect.Height / 2));

                if (playerRect.Intersects(tileRect))
                {
					if ((state & TileState.Entrance) == TileState.Entrance) {
						
					}

					Rectangle rectIntersection = Rectangle.Intersect (playerRect, tileRect);

					if (rectIntersection.Height == rectIntersection.Width) {
						if (player.Velocity.X < 0) {
							player.UpdateImagePositionX (tileRect.Right);
							player.Velocity = new Vector2 (0, player.Velocity.Y);
						}

						if (player.Velocity.X > 0) {
							player.UpdateImagePositionX (tileRect.Left - player.Image.SourceRect.Width);
							player.Velocity = new Vector2 (0, player.Velocity.Y);
						}

						if (player.Velocity.Y < 0) {
							//player.Image.Position.Y = tileRect.Bottom;
							player.UpdateImagePositionY (tileRect.Bottom - (player.Image.SourceRect.Height / 2));
							player.Velocity = new Vector2 (player.Velocity.X, 0);
						}

						if (player.Velocity.Y > 0) {
							player.UpdateImagePositionY (tileRect.Top - player.Image.SourceRect.Height);
							player.Velocity = new Vector2 (player.Velocity.X, 0);
						}
					}

					if (rectIntersection.Height > rectIntersection.Width) {
						if (player.Velocity.X < 0) {
							player.UpdateImagePositionX (tileRect.Right);
							player.Velocity = new Vector2 (0, player.Velocity.Y);
						}

						if (player.Velocity.X > 0) {
							player.UpdateImagePositionX (tileRect.Left - player.Image.SourceRect.Width);
							player.Velocity = new Vector2 (0, player.Velocity.Y);
						}
					}

					if (rectIntersection.Width > rectIntersection.Height) {
						if (player.Velocity.Y < 0) {
							//player.Image.Position.Y = tileRect.Bottom;
							player.UpdateImagePositionY(tileRect.Bottom - (player.Image.SourceRect.Height / 2));
							player.Velocity = new Vector2 (player.Velocity.X, 0);
						}

						if (player.Velocity.Y > 0) {
							player.UpdateImagePositionY (tileRect.Top - player.Image.SourceRect.Height);
							player.Velocity = new Vector2 (player.Velocity.X, 0);
						}
					}
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
