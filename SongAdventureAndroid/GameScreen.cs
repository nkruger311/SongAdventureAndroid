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
    public class GameScreen
    {
        protected ContentManager _oContent;
        [XmlIgnore]
		public Type Type { get; set; }
		[XmlIgnore]
		public virtual bool IsInitializing { get; set; }

        public string XmlPath;

        public GameScreen()
        {
            Type = this.GetType();
            //XmlPath = "Load/Gameplay/Screens/" + Type.ToString().Replace("SongAdventureAndroid.", "") + ".xml";
            XmlPath = System.IO.Path.Combine(Globals.LoadGameplayScreensDirectory, Type.ToString().Replace("SongAdventureAndroid.", "") + ".xml");
        }

        public virtual void LoadContent()
        {
			_oContent = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
        }

        public virtual void UnloadContent()
        {
			_oContent.Unload();
        }
       
        public virtual void Update(GameTime gameTime)
        {
            InputManager.Instance.Update();
        }
        
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
