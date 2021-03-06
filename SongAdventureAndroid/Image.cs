﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SongAdventureAndroid
{
    public class Image
    {
		public float Alpha { get; set; }
		public string Text { get; set; }
		public string FontName { get; set; }
		public string Path { get; set; }
		public Globals.TextAlignment TextAlignment { get; set; }
        [XmlIgnore]
		public Vector2 TextAnimationTravel { get; set; }
        [XmlIgnore]
		public Color TextColor { get; set; }
		public Vector2 Position { get; set; }
		public Vector2 Scale { get; set; }
		public Rectangle SourceRect { get; set; }
		public bool IsActive { get; set; }

        [XmlIgnore]
        public Texture2D Texture;
        Vector2 origin;
        ContentManager content;
        RenderTarget2D renderTarget;
        SpriteFont font;
        Dictionary<string, ImageEffect> effectDictionary;
        public string Effects;

        public FadeEffect FadeEffect;
        public SpriteSheetEffect SpriteSheetEffect;

        private void SetEffect<T>(ref T effect)
        {
            if (effect == null)
                effect = (T)Activator.CreateInstance(typeof(T));
            else
            {
                (effect as ImageEffect).IsActive = true;
                var obj = this;
                (effect as ImageEffect).LoadContent(ref obj);
            }

            if (!effectDictionary.ContainsKey(effect.GetType().ToString().Replace("SongAdventureAndroid.", "")))
                effectDictionary.Add(effect.GetType().ToString().Replace("SongAdventureAndroid.", ""), (effect as ImageEffect));
        }

        public void ActivateEffect(string effect)
        {
            if (effectDictionary.ContainsKey(effect))
            {
                effectDictionary[effect].IsActive = true;
                var obj = this;
                effectDictionary[effect].LoadContent(ref obj);
            }
        }

        public void DeactivateEffect(string effect)
        {
            if (effectDictionary.ContainsKey(effect))
            {
                effectDictionary[effect].IsActive = false;
                effectDictionary[effect].UnloadContent();
            }
        }

        public Image()
        {
            Path = Text = Effects = String.Empty;
            TextAlignment = Globals.TextAlignment.Left;
            TextColor = Color.White;
            FontName = "Fonts/GameFont";
            Position = Vector2.Zero;
            TextAnimationTravel = Vector2.Zero;
            Scale = Vector2.One;
            Alpha = 1.0f;
            SourceRect = Rectangle.Empty;
            effectDictionary = new Dictionary<string, ImageEffect>();
        }

        public void LoadContent()
        {
			content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            if (Path != String.Empty)
				Texture = content.Load<Texture2D>(Path);

			//((Microsoft.Xna.Framework.AndroidGameActivity)Game1.Activity).RunOnUiThread (() => font = content.Load<SpriteFont> (FontName));
			font = content.Load<SpriteFont>(FontName);

            Vector2 dimensions = Vector2.Zero;

			dimensions = Vector2.Zero;

            if (Texture != null)
            {
                //dimensions.X += Texture.Width;

				//((Microsoft.Xna.Framework.AndroidGameActivity)Game1.Activity).RunOnUiThread (() => dimensions.X = Math.Max(Texture.Width, font.MeasureString(Text).X));
				//((Microsoft.Xna.Framework.AndroidGameActivity)Game1.Activity).RunOnUiThread (() => dimensions.Y = Math.Max(Texture.Height, font.MeasureString(Text).Y));

				dimensions.X = Math.Max(Texture.Width, font.MeasureString(Text).X);
				dimensions.Y = Math.Max(Texture.Height, font.MeasureString(Text).Y);
            }
            else
            {
				dimensions.X = font.MeasureString(Text).X;
				dimensions.Y = font.MeasureString(Text).Y;
            }

            //dimensions.X += font.MeasureString(Text).X;

            if (SourceRect == Rectangle.Empty)
                SourceRect = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);

			renderTarget = new RenderTarget2D(ScreenManager.Instance.GraphicsDevice, (int)dimensions.X, (int)dimensions.Y);

            //ScreenManager.Instance.GraphicsDevice.SetRenderTarget(renderTarget);
            //ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);

			ScreenManager.Instance.GraphicsDevice.SetRenderTarget (renderTarget);
			ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);

			ScreenManager.Instance.SpriteBatch.Begin();
            if (Texture != null)
				ScreenManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White);

            //if (TextAlignment == Globals.TextAlignment.Right)
            //    ScreenManager.Instance.SpriteBatch.DrawString(font, Text, new Vector2(dimensions.X - font.MeasureString(Text).X, dimensions.Y - font.MeasureString(Text).Y), Color.White);
            //else if (TextAlignment == Globals.TextAlignment.Center)
            //    ScreenManager.Instance.SpriteBatch.DrawString(font, Text, new Vector2((dimensions.X - font.MeasureString(Text).X) / 2, (dimensions.Y - font.MeasureString(Text).Y) / 2), Color.White);
            //else
            //    ScreenManager.Instance.SpriteBatch.DrawString(font, Text, Vector2.Zero, Color.White);

            //ScreenManager.Instance.SpriteBatch.End();
			ScreenManager.Instance.SpriteBatch.End();

			Texture = renderTarget;

			ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);

            SetEffect<FadeEffect>(ref FadeEffect);
            SetEffect<SpriteSheetEffect>(ref SpriteSheetEffect);

            if (Effects != String.Empty)
            {
                string[] split = Effects.Split(':');
                foreach (string item in split)
                    ActivateEffect(item);
            }
        }

		public void LoadContentThreadSafe()
		{
			content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

			if (Path != String.Empty)
				Texture = content.Load<Texture2D>(Path);

			//((Microsoft.Xna.Framework.AndroidGameActivity)Game1.Activity).RunOnUiThread (() => font = content.Load<SpriteFont> (FontName));
			font = content.Load<SpriteFont>(FontName);

			Vector2 dimensions = Vector2.Zero;

			dimensions = Vector2.Zero;

			if (Texture != null)
			{
				//dimensions.X += Texture.Width;

				//((Microsoft.Xna.Framework.AndroidGameActivity)Game1.Activity).RunOnUiThread (() => dimensions.X = Math.Max(Texture.Width, font.MeasureString(Text).X));
				//((Microsoft.Xna.Framework.AndroidGameActivity)Game1.Activity).RunOnUiThread (() => dimensions.Y = Math.Max(Texture.Height, font.MeasureString(Text).Y));

				dimensions.X = Math.Max(Texture.Width, font.MeasureString(Text).X);
				dimensions.Y = Math.Max(Texture.Height, font.MeasureString(Text).Y);
			}
			else
			{
				dimensions.X = font.MeasureString(Text).X;
				dimensions.Y = font.MeasureString(Text).Y;
			}

			//dimensions.X += font.MeasureString(Text).X;

			if (SourceRect == Rectangle.Empty)
				SourceRect = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);

			renderTarget = new RenderTarget2D(ScreenManager.Instance.GraphicsDevice, (int)dimensions.X, (int)dimensions.Y);

			//ScreenManager.Instance.GraphicsDevice.SetRenderTarget(renderTarget);
			//ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);

			((Microsoft.Xna.Framework.AndroidGameActivity)Game1.Activity).RunOnUiThread (() => ScreenManager.Instance.GraphicsDevice.SetRenderTarget (renderTarget));
			((Microsoft.Xna.Framework.AndroidGameActivity)Game1.Activity).RunOnUiThread (() => ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent));

			//ScreenManager.Instance.GraphicsDevice.SetRenderTarget (renderTarget);
			//ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);


			//if (ScreenManager.Instance.SpriteBatchHasBegun) {
			//	ScreenManager.Instance.SpriteBatch.Draw (Texture, Vector2.Zero, Color.White);
			//} else {
			//	((Microsoft.Xna.Framework.AndroidGameActivity)Game1.Activity).RunOnUiThread (() => System.Threading.Thread.Sleep (1000));
			//
			//	ScreenManager.Instance.SpriteBatch.Begin();
			//	if (Texture != null)
			//		ScreenManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White);
			//	//((Microsoft.Xna.Framework.AndroidGameActivity)Game1.Activity).RunOnUiThread (() => ScreenManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White));
			//
			//	//if (TextAlignment == Globals.TextAlignment.Right)
			//	//    ScreenManager.Instance.SpriteBatch.DrawString(font, Text, new Vector2(dimensions.X - font.MeasureString(Text).X, dimensions.Y - font.MeasureString(Text).Y), Color.White);
			//	//else if (TextAlignment == Globals.TextAlignment.Center)
			//	//    ScreenManager.Instance.SpriteBatch.DrawString(font, Text, new Vector2((dimensions.X - font.MeasureString(Text).X) / 2, (dimensions.Y - font.MeasureString(Text).Y) / 2), Color.White);
			//	//else
			//	//    ScreenManager.Instance.SpriteBatch.DrawString(font, Text, Vector2.Zero, Color.White);
			//
			//	((Microsoft.Xna.Framework.AndroidGameActivity)Game1.Activity).RunOnUiThread (() => ScreenManager.Instance.SpriteBatch.End());
			//}


			//((Microsoft.Xna.Framework.AndroidGameActivity)Game1.Activity).RunOnUiThread (() => ScreenManager.Instance.SpriteBatch.Begin());
			//ScreenManager.Instance.SpriteBatch.Begin();
			if (Texture != null)
				ScreenManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White);
				//((Microsoft.Xna.Framework.AndroidGameActivity)Game1.Activity).RunOnUiThread (() => ScreenManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White));

			//if (TextAlignment == Globals.TextAlignment.Right)
			//    ScreenManager.Instance.SpriteBatch.DrawString(font, Text, new Vector2(dimensions.X - font.MeasureString(Text).X, dimensions.Y - font.MeasureString(Text).Y), Color.White);
			//else if (TextAlignment == Globals.TextAlignment.Center)
			//    ScreenManager.Instance.SpriteBatch.DrawString(font, Text, new Vector2((dimensions.X - font.MeasureString(Text).X) / 2, (dimensions.Y - font.MeasureString(Text).Y) / 2), Color.White);
			//else
			//    ScreenManager.Instance.SpriteBatch.DrawString(font, Text, Vector2.Zero, Color.White);

			//ScreenManager.Instance.SpriteBatch.End();
			//((Microsoft.Xna.Framework.AndroidGameActivity)Game1.Activity).RunOnUiThread (() => ScreenManager.Instance.SpriteBatch.End());

			Texture = renderTarget;

			((Microsoft.Xna.Framework.AndroidGameActivity)Game1.Activity).RunOnUiThread (() => ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null));
			//ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);

			SetEffect<FadeEffect>(ref FadeEffect);
			SetEffect<SpriteSheetEffect>(ref SpriteSheetEffect);

			if (Effects != String.Empty)
			{
				string[] split = Effects.Split(':');
				foreach (string item in split)
					ActivateEffect(item);
			}
		}

        public void UnloadContent()
        {
            content.Unload();
            foreach (var effect in effectDictionary)
                DeactivateEffect(effect.Key);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var effect in effectDictionary)
            {
                if (effect.Value.IsActive)
                    effect.Value.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
			origin = new Vector2(SourceRect.Width / 2, SourceRect.Height / 2);
			spriteBatch.Draw(Texture, Position + origin, SourceRect, Color.White * Alpha, 0.0f, origin, Scale, SpriteEffects.None, 0.0f);
        }

        public void StoreEffects()
        {
            Effects = String.Empty;
            foreach (var effect in effectDictionary)
            {
                if (effect.Value.IsActive)
                    Effects += effect.Key + ":";
            }

            Effects = Effects.TrimEnd(':');
        }

        public void RestoreEffects()
        {
            foreach (var effect in effectDictionary)
                DeactivateEffect(effect.Key);

            string[] split = Effects.Split(':');
            foreach (string s in split)
                ActivateEffect(s);
        }

        public void AddText(string text)
        {
            Text = text;

            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            if (Path != String.Empty)
                Texture = content.Load<Texture2D>(Path);

            font = content.Load<SpriteFont>(FontName);

            Vector2 dimensions = Vector2.Zero;

            if (Texture != null)
            {
                dimensions.X = Math.Max(Texture.Width, font.MeasureString(Text).X);
                dimensions.Y = Math.Max(Texture.Height, font.MeasureString(Text).Y);
            }
            else
            {
                dimensions.X = font.MeasureString(Text).X;
                dimensions.Y = font.MeasureString(Text).Y;
            }

            if (SourceRect == Rectangle.Empty)
                SourceRect = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);

            renderTarget = new RenderTarget2D(ScreenManager.Instance.GraphicsDevice, (int)dimensions.X, (int)dimensions.Y);
			ScreenManager.Instance.GraphicsDevice.SetRenderTarget(renderTarget);
			ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);
			ScreenManager.Instance.SpriteBatch.Begin();
            if (Texture != null)
				ScreenManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White);

            if (SpriteSheetEffect.IsActive)
            {
                if (TextAlignment == Globals.TextAlignment.Right)
                {
                    for (int i = 0; i < SpriteSheetEffect.AmountOfFrames.X; i++)
                    {
                        for (int j = 0; j < SpriteSheetEffect.AmountOfFrames.Y; j++)
                        {
							ScreenManager.Instance.SpriteBatch.DrawString(font, Text, new Vector2(SpriteSheetEffect.FrameWidth - font.MeasureString(Text).X, SpriteSheetEffect.FrameHeight - font.MeasureString(Text).Y) +
                            (new Vector2(SpriteSheetEffect.FrameWidth * i, SpriteSheetEffect.FrameHeight * j) +
                            new Vector2(TextAnimationTravel.X * i, TextAnimationTravel.Y * j)),
								TextColor);
                        }
                    }
                    //ScreenManager.Instance.SpriteBatch.DrawString(font, Text, new Vector2(SpriteSheetEffect.FrameWidth - font.MeasureString(Text).X, SpriteSheetEffect.FrameHeight - font.MeasureString(Text).Y), Color.White);
                }
                else if (TextAlignment == Globals.TextAlignment.Center)
                {
                    for (int i = 0; i < SpriteSheetEffect.AmountOfFrames.X; i ++)
                    {
                        for (int j = 0; j < SpriteSheetEffect.AmountOfFrames.Y; j++)
                        {
							ScreenManager.Instance.SpriteBatch.DrawString(font, Text, new Vector2((SpriteSheetEffect.FrameWidth - font.MeasureString(Text).X) / 2, (SpriteSheetEffect.FrameHeight - font.MeasureString(Text).Y) / 2) +
                            (new Vector2(SpriteSheetEffect.FrameWidth * i, SpriteSheetEffect.FrameHeight * j) +
                            new Vector2(TextAnimationTravel.X * i, TextAnimationTravel.Y * j)),
								TextColor);
                        }
                    }
                    //ScreenManager.Instance.SpriteBatch.DrawString(font, Text, new Vector2((SpriteSheetEffect.FrameWidth - font.MeasureString(Text).X) / 2, (SpriteSheetEffect.FrameHeight - font.MeasureString(Text).Y) / 2), Color.White);
                }
                else
                {
                    for (int i = 0; i < SpriteSheetEffect.AmountOfFrames.X; i++)
                    {
                        for (int j = 0; j < SpriteSheetEffect.AmountOfFrames.Y; j++)
                        {
							ScreenManager.Instance.SpriteBatch.DrawString(font, Text, Vector2.Zero +
                            (new Vector2(SpriteSheetEffect.FrameWidth * i, SpriteSheetEffect.FrameHeight * j) +
                            new Vector2(TextAnimationTravel.X * i, TextAnimationTravel.Y * j)),
								TextColor);
                        }
                    }
                    //ScreenManager.Instance.SpriteBatch.DrawString(font, Text, Vector2.Zero, Color.White);
                }
            }
            else
            {
                if (TextAlignment == Globals.TextAlignment.Right)
					ScreenManager.Instance.SpriteBatch.DrawString(font, Text, new Vector2(dimensions.X - font.MeasureString(Text).X, dimensions.Y - font.MeasureString(Text).Y), TextColor);
                else if (TextAlignment == Globals.TextAlignment.Center)
					ScreenManager.Instance.SpriteBatch.DrawString(font, Text, new Vector2((dimensions.X - font.MeasureString(Text).X) / 2, (dimensions.Y - font.MeasureString(Text).Y) / 2), TextColor);
                else
					ScreenManager.Instance.SpriteBatch.DrawString(font, Text, Vector2.Zero, TextColor);
            }

			try
			{
				ScreenManager.Instance.SpriteBatch.End();
			}
			catch (Exception ex) {
				string s = ex.Message;
			}

            Texture = renderTarget;

			ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);
        }

		public void AddTextThreadSafe(string text)
		{
			Text = text;

			content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

			if (Path != String.Empty)
				Texture = content.Load<Texture2D>(Path);

			font = content.Load<SpriteFont>(FontName);

			Vector2 dimensions = Vector2.Zero;

			if (Texture != null)
			{
				dimensions.X = Math.Max(Texture.Width, font.MeasureString(Text).X);
				dimensions.Y = Math.Max(Texture.Height, font.MeasureString(Text).Y);
			}
			else
			{
				dimensions.X = font.MeasureString(Text).X;
				dimensions.Y = font.MeasureString(Text).Y;
			}

			if (SourceRect == Rectangle.Empty)
				SourceRect = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);

			renderTarget = new RenderTarget2D(ScreenManager.Instance.GraphicsDevice, (int)dimensions.X, (int)dimensions.Y);
			((Microsoft.Xna.Framework.AndroidGameActivity)Game1.Activity).RunOnUiThread(() => ScreenManager.Instance.GraphicsDevice.SetRenderTarget(renderTarget));
			((Microsoft.Xna.Framework.AndroidGameActivity)Game1.Activity).RunOnUiThread(() => ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent));
			//ScreenManager.Instance.SpriteBatch.Begin();
			if (Texture != null)
				ScreenManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White);

			if (SpriteSheetEffect.IsActive)
			{
				if (TextAlignment == Globals.TextAlignment.Right)
				{
					for (int i = 0; i < SpriteSheetEffect.AmountOfFrames.X; i++)
					{
						for (int j = 0; j < SpriteSheetEffect.AmountOfFrames.Y; j++)
						{
							ScreenManager.Instance.SpriteBatch.DrawString(font, Text, new Vector2(SpriteSheetEffect.FrameWidth - font.MeasureString(Text).X, SpriteSheetEffect.FrameHeight - font.MeasureString(Text).Y) +
								(new Vector2(SpriteSheetEffect.FrameWidth * i, SpriteSheetEffect.FrameHeight * j) +
									new Vector2(TextAnimationTravel.X * i, TextAnimationTravel.Y * j)),
								TextColor);
						}
					}
					//ScreenManager.Instance.SpriteBatch.DrawString(font, Text, new Vector2(SpriteSheetEffect.FrameWidth - font.MeasureString(Text).X, SpriteSheetEffect.FrameHeight - font.MeasureString(Text).Y), Color.White);
				}
				else if (TextAlignment == Globals.TextAlignment.Center)
				{
					for (int i = 0; i < SpriteSheetEffect.AmountOfFrames.X; i ++)
					{
						for (int j = 0; j < SpriteSheetEffect.AmountOfFrames.Y; j++)
						{
							ScreenManager.Instance.SpriteBatch.DrawString(font, Text, new Vector2((SpriteSheetEffect.FrameWidth - font.MeasureString(Text).X) / 2, (SpriteSheetEffect.FrameHeight - font.MeasureString(Text).Y) / 2) +
								(new Vector2(SpriteSheetEffect.FrameWidth * i, SpriteSheetEffect.FrameHeight * j) +
									new Vector2(TextAnimationTravel.X * i, TextAnimationTravel.Y * j)),
								TextColor);
						}
					}
					//ScreenManager.Instance.SpriteBatch.DrawString(font, Text, new Vector2((SpriteSheetEffect.FrameWidth - font.MeasureString(Text).X) / 2, (SpriteSheetEffect.FrameHeight - font.MeasureString(Text).Y) / 2), Color.White);
				}
				else
				{
					for (int i = 0; i < SpriteSheetEffect.AmountOfFrames.X; i++)
					{
						for (int j = 0; j < SpriteSheetEffect.AmountOfFrames.Y; j++)
						{
							ScreenManager.Instance.SpriteBatch.DrawString(font, Text, Vector2.Zero +
								(new Vector2(SpriteSheetEffect.FrameWidth * i, SpriteSheetEffect.FrameHeight * j) +
									new Vector2(TextAnimationTravel.X * i, TextAnimationTravel.Y * j)),
								TextColor);
						}
					}
					//ScreenManager.Instance.SpriteBatch.DrawString(font, Text, Vector2.Zero, Color.White);
				}
			}
			else
			{
				if (TextAlignment == Globals.TextAlignment.Right)
					ScreenManager.Instance.SpriteBatch.DrawString(font, Text, new Vector2(dimensions.X - font.MeasureString(Text).X, dimensions.Y - font.MeasureString(Text).Y), TextColor);
				else if (TextAlignment == Globals.TextAlignment.Center)
					ScreenManager.Instance.SpriteBatch.DrawString(font, Text, new Vector2((dimensions.X - font.MeasureString(Text).X) / 2, (dimensions.Y - font.MeasureString(Text).Y) / 2), TextColor);
				else
					ScreenManager.Instance.SpriteBatch.DrawString(font, Text, Vector2.Zero, TextColor);
			}

			try
			{
				//ScreenManager.Instance.SpriteBatch.End();
			}
			catch (Exception ex) {
				string s = ex.Message;
			}

			Texture = renderTarget;

			//ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);
			((Microsoft.Xna.Framework.AndroidGameActivity)Game1.Activity).RunOnUiThread (() => ScreenManager.Instance.GraphicsDevice.SetRenderTarget (null));
		}

		public void UpdateSourceRectPositionX(int x)
		{
			UpdateSourceRectPosition (x, (int)SourceRect.Y);
		}

		public void UpdateSourceRectPositionY(int y)
		{
			UpdateSourceRectPosition ((int)SourceRect.X, y);
		}

		public void UpdateSourceRectPosition(int x, int y)
		{
			SourceRect = new Rectangle((int)x, (int)y, SourceRect.Width, SourceRect.Height);
		}

		public void UpdatePositionX(int x)
		{
			UpdatePosition (x, (int)Position.Y);
		}

		public void UpdatePositionY(int y)
		{
			UpdatePosition ((int)Position.X, y);
		}

		public void UpdatePosition(int x, int y)
		{
			Position = new Vector2 (x, y);
		}
    }
}
