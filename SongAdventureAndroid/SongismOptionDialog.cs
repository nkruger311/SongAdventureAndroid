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
            Image.Position = Vector2.Transform(Vector2.Zero, Matrix.Invert(Camera2D.Instance.CameraMatrix));
            /* Turn the fade effect off */
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
