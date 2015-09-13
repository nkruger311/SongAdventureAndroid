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

        public string ButtonName;
        public Image Image;
        public string Text
        {
            get { return Image.Text; }
            set { Image.Text = value; }
        }
        int prevFrameY = -1;
        bool enabled;

        public DialogButton(bool enabled)
        {
            Image = new Image();
            Image.Path = "Load/Gameplay/UI/buttonsheet";
            Image.SourceRect = new Rectangle(0, 0, 190, 49);
            Text = String.Empty;
            this.enabled = enabled;
        }

        public void LoadContent()
        {
            Image.LoadContent();
            Image.SpriteSheetEffect.AmountOfFrames = new Vector2(1, 2);
            Image.SpriteSheetEffect.CurrentFrame = new Vector2(0, 0);
            Image.SourceRect.X = (int)Image.Position.X;
            Image.SourceRect.Y = (int)Image.Position.Y;
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

            Image.SourceRect.X = (int)Image.Position.X;
            Image.SourceRect.Y = (int)Image.Position.Y;

            if (enabled)
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
