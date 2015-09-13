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
    public enum DialogResult { None, AddToSongBook, Cancel }

    class SongismOptionDialog
    {
        public Image Image;
        public List<DialogButton> Buttons;
        public DialogResult DialogResult { get; set; }
        Songism currentSongism;

        public SongismOptionDialog(Songism songism)
        {
            Image = new Image();

            Image.Path = "Gameplay/UI/dialogbackground";
            Buttons = new List<DialogButton>();
            currentSongism = songism;
        }

        public void LoadContent()
        {
            Image.LoadContent();
            //Image.Scale = new Vector2(2.0f, 1.0f);
            //Image.Position = Vector2.Transform(new Vector2((ScreenManager.Instance.Dimensions.X / 2) - (Image.SourceRect.Width / 2), (ScreenManager.Instance.Dimensions.Y / 2) - (Image.SourceRect.Height / 2)), Matrix.Invert(Camera2D.Instance.CameraMatrix));
            Image.Position = Vector2.Transform(Vector2.Zero, Matrix.Invert(Camera2D.Instance.CameraMatrix));
            /* Turn the fade effect off */
            //Image.FadeEffect.IsActive = false;
            //Image.DeactivateEffect("FadeEffect");
            this.DialogResult = SongAdventureAndroid.DialogResult.None;
            
            for (int buttonIndex = 0; buttonIndex < Buttons.Count; buttonIndex++)
            {
                Buttons[buttonIndex].Image.Effects = "SpriteSheetEffect";
                Buttons[buttonIndex].LoadContent();

                if (buttonIndex > 0)
                    Buttons[buttonIndex].Image.Position = new Vector2(Image.Position.X, ((Image.Position.Y + 5) + ((Buttons[buttonIndex].Image.SourceRect.Height / 2) * buttonIndex) + (buttonIndex * 5)));
                else
                    Buttons[buttonIndex].Image.Position = new Vector2(Image.Position.X, Image.Position.Y + 5);

                Buttons[buttonIndex].OnButtonDown += button_OnButtonDown;
            }
        }

        void button_OnButtonDown(object sender, EventArgs e)
        {
            if (((DialogButton)sender).ButtonName == "Add")
            {
                string guess = "Time Bomb";
                if (string.Equals(currentSongism.Name, guess))
                    this.DialogResult = SongAdventureAndroid.DialogResult.AddToSongBook;
            }
            else if (((DialogButton)sender).ButtonName == "Cancel")
            {
                this.DialogResult = SongAdventureAndroid.DialogResult.Cancel;
            }
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            Image.IsActive = false;

            #region "Original Code"
            //if (Velocity.X == 0)
            //{
            //    //if (InputManager.Instance.KeyDown(Keys.Down))
            //    //{
            //    //    Velocity.Y = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //    //    Image.SpriteSheetEffect.CurrentFrame.Y = 0;
            //    //}
            //    //else if (InputManager.Instance.KeyDown(Keys.Up))
            //    //{
            //    //    Velocity.Y = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //    //    Image.SpriteSheetEffect.CurrentFrame.Y = 3;
            //    //}
            //    //else
            //    //    Velocity.Y = 0;

            //    if (InputManager.Instance.LeftMouseButtonDown())
            //    {
            //        if (InputManager.Instance.MousePosition.Y > Image.Position.Y)
            //        {
            //            Velocity.Y = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //            Image.SpriteSheetEffect.CurrentFrame.Y = 0;
            //        }
            //        else if (InputManager.Instance.MousePosition.Y < Image.Position.Y)
            //        {
            //            Velocity.Y = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //            Image.SpriteSheetEffect.CurrentFrame.Y = 3;
            //        }
            //        else
            //            Velocity.Y = 0;
            //    }
            //}

            //if (Velocity.Y == 0)
            //{
            //    //if (InputManager.Instance.KeyDown(Keys.Right))
            //    //{
            //    //    Velocity.X = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //    //    Image.SpriteSheetEffect.CurrentFrame.Y = 2;
            //    //}
            //    //else if (InputManager.Instance.KeyDown(Keys.Left))
            //    //{
            //    //    Velocity.X = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //    //    Image.SpriteSheetEffect.CurrentFrame.Y = 1;
            //    //}
            //    //else
            //    //    Velocity.X = 0;

            //    if (InputManager.Instance.LeftMouseButtonDown())
            //    {
            //        if (InputManager.Instance.MousePosition.X > Image.Position.X)
            //        {
            //            Velocity.X = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //            Image.SpriteSheetEffect.CurrentFrame.Y = 2;
            //        }
            //        else if (InputManager.Instance.MousePosition.X < Image.Position.X)
            //        {
            //            Velocity.X = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //            Image.SpriteSheetEffect.CurrentFrame.Y = 1;
            //        }
            //        else
            //            Velocity.X = 0;

            //        if (Velocity.X == 0 && Velocity.Y == 0)
            //            Image.IsActive = false;
            //    }
            //}
            #endregion // Original Code

            /* Checking for a mouse click, setting a destination */
            foreach (DialogButton button in Buttons)
            {
                button.Update(gameTime);
            }

            Image.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
            foreach (DialogButton button in Buttons)
                button.Draw(spriteBatch);
        }

        public void AddButton(string buttonName, string buttonImagePath, string buttonText, bool enabled)
        {
            DialogButton newButton = new DialogButton(enabled);
            Image buttonImage = new Image();
            buttonImage.Path = buttonImagePath;

            newButton.ButtonName = buttonName;
            newButton.Image = buttonImage;
            newButton.Text = buttonText;

            Buttons.Add(newButton);
        }
    }
}
