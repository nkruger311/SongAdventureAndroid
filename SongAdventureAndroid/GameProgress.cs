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
		public string CurrentMapName { get; set;}
		public Vector2 PlayerPosition { get; set;}
        
		public GameProgress()
        {
            
        }
    }
}