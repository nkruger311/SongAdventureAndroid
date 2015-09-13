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
    class DialogButton
    {
        public event EventHandler OnButtonDown;
        public event EventHandler OnButtonRelease;
        public event EventHandler OnDialogClosing;

		public string ButtonName {get;set;}
		public Image Image {get;set;}
        public string Text
        {
            get { return Image.Text; }
            set { Image.Text = value; }
        }
        private int _iPrevFrameY = -1;
        private bool _bEnabled;

        public DialogButton(bool enabled)
        {
            Image = new Image();
            Image.Path = "Load/Gameplay/UI/buttonsheet";
            Image.SourceRect = new Rectangle(0, 0, 190, 49);
            Text = String.Empty;
			_bEnabled = enabled;
        }

        public void LoadContent()
        {
            Image.LoadContent();
            Image.SpriteSheetEffect.AmountOfFrames = new Vector2(1, 2);
            Image.SpriteSheetEffect.CurrentFrame = new Vector2(0, 0);
            Image.SourceRect = new Rectangle ((int)Image.Position.X, (int)Image.Position.Y, Image.SourceRect.Width, Image.SourceRect.Height);
            int FrameHeight = Image.SpriteSheetEffect.FrameHeight;
            int FrameWidth = Image.SpriteSheetEffect.FrameWidth;
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            Image.IsActive = true;

			Image.SourceRect = new Rectangle ((int)Image.Position.X, (int)Image.Position.Y, Image.SourceRect.Width, Image.SourceRect.Height);

			if (_bEnabled)
            {
                if (InputManager.Instance.TouchPanelDown() && Image.SourceRect.Contains(InputManager.Instance.TransformedTouchPosition))
                {
                    if (!InputManager.Instance.InputDisabled)
                    {
                        Image.SpriteSheetEffect.CurrentFrame.Y = 1;
                    }
                }
                else
                    Image.SpriteSheetEffect.CurrentFrame.Y = 0;

                if (InputManager.Instance.TouchPanelReleased() && Image.SourceRect.Contains(InputManager.Instance.PrevTransformedTouchPosition))
                {
                    if (!InputManager.Instance.InputDisabled)
                    {
                        Image.SpriteSheetEffect.CurrentFrame.Y = 0;
                        OnButtonRelease(this, null);
                    }
                }
            }
            else
                Image.SpriteSheetEffect.CurrentFrame.Y = 1;

            Image.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }
}
