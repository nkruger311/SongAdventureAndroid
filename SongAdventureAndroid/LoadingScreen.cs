using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SongAdventureAndroid
{
	public class LoadingScreen : GameScreen
	{
		public Image Image {get;set;}

		public override void LoadContent()
		{
			base.LoadContent();

			Image = new Image();
			Image.Path = "Gameplay/UI/loadingscreenanimation";
			//Image.SourceRect = new Rectangle(0, 0, 400, 100);
			//Image.Position = new Vector2 ((ScreenManager.Instance.GraphicsDevice.Viewport.Width / 2) - 100,
			//	(ScreenManager.Instance.GraphicsDevice.Viewport.Height / 2) - 100);
			Image.LoadContent();

			Image.SourceRect = new Rectangle(0, 0, 100, 100);
			//Image.Position = new Vector2 ((ScreenManager.Instance.GraphicsDevice.Viewport.Width / 2) - 100,
			//	(ScreenManager.Instance.GraphicsDevice.Viewport.Height / 2) - 100);

			Image.IsActive = true;
			Image.ActivateEffect ("SpriteSheetEffect");
			Image.SpriteSheetEffect.IsActive = true;
			/* Turn the fade effect off */
			Image.FadeEffect.IsActive = false;
			/* Make the player face the camera */
			Image.SpriteSheetEffect.SwitchFrame = 250;
			Image.SpriteSheetEffect.AmountOfFrames = new Vector2(4, 1);
			Image.SpriteSheetEffect.DefaultFrame = Vector2.Zero;
			Image.SpriteSheetEffect.CurrentFrame = Image.SpriteSheetEffect.DefaultFrame;

			Image.Position = new Vector2 (Image.Position.X - 50, Image.Position.Y - 50);
		}

		public override void UnloadContent()
		{
			base.UnloadContent();
			Image.UnloadContent();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			Image.Update(gameTime);

			//if (InputManager.Instance.KeyPressed(Keys.Enter, Keys.Z))
			//if (InputManager.Instance.LeftMouseButtonDown())
			//if (InputManager.Instance.TouchPanelPressed())
			//	ScreenManager.Instance.ChangeScreens("TitleScreen");


		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Color backColor = new Color (89, 144, 25);
			//GraphicsDevice.Clear(new Microsoft.Xna.Framework.Color(89, 144, 25));
			ScreenManager.Instance.GraphicsDevice.Clear(backColor);
			Image.Draw(spriteBatch);
		}
	}
}
