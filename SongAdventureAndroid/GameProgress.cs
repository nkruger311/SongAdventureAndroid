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
    public class GameProgress
    {
		public string CurrentMapName;
        public Vector2 PlayerPosition;
        
		public GameProgress()
        {
            
        }
    }
}