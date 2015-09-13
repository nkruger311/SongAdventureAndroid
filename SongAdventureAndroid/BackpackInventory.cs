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
	public class BackpackInventory
	{
		public Image Image;

		public BackpackInventory ()
		{
			Image = new Image();
		}

		public void LoadContent()
		{
			Image = new Image();
			Image.Path = "Gameplay/UI/backpack";
			Image.LoadContent();

			Image.SourceRect = new Rectangle(0, 0, 64, 64);

			Image.IsActive = true;
			Image.DeactivateEffect ("SpriteSheetEffect");
			Image.SpriteSheetEffect.IsActive = false;

			Image.FadeEffect.IsActive = false;


			Image.TextAlignment = Globals.TextAlignment.Center;
			Image.TextColor = Color.White;

			Image.AddText(Inventory.Instance.SongBook.Count.ToString());
		}

		public void UnloadContent()
		{
			Image.UnloadContent();
		}

		public void Update(GameTime gameTime)
		{
			// We need to update the position so that it stays in the upper left corner
			Image.Position = new Vector2 (Camera2D.Instance.Position.X - (ScreenManager.Instance.GraphicsDevice.Viewport.Width / 2) + 10, Camera2D.Instance.Position.Y + (ScreenManager.Instance.GraphicsDevice.Viewport.Height / 2) - 64 - 10);

			Image.Update (gameTime);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			Image.Draw (spriteBatch);
		}
	}
}

