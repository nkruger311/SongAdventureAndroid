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
    public class Map
    {
		public event EventHandler OnMapChanging;

        [XmlElement("Layer")]
        public List<Layer> Layers;

		public Vector2 PlayerStartingPosition;

        [XmlIgnore]
        public List<Songism> Songisms;

		[XmlIgnore]
		public List<MapEntrance> MapEntrances;
        //SongismOptionDialog dialog;

		[XmlIgnore]
		public List<NPC> NPCs;

		[XmlIgnore]
		public string MapName;

        public Map()
        {
            Layers = new List<Layer>();
        }

        void songism_OnClick(object sender, EventArgs e)
        {
			if ((!((Songism)sender).HasPrerequisiteSongism) || (((Songism)sender).HasPrerequisiteSongism && Inventory.Instance.PrerequisiteSongismHasBeenDiscovered (((Songism)sender).PrerequisiteSongism)) &&
				(!((Songism)sender).HasPrerequisiteInventoryItem) || (((Songism)sender).HasPrerequisiteInventoryItem && Inventory.Instance.PrerequisiteInventoryItemHasBeenDiscovered (((Songism)sender).PrerequisiteInventoryItem))) {
				XmlManager<Songism> currentSongismSaver = new XmlManager<Songism> ();
				//currentSongismSaver.Save("Load/Gameplay/Songisms/CurrentSongism.xml", (Songism)sender);
				currentSongismSaver.Save (System.IO.Path.Combine (Globals.LoadGameplaySongismsDirectory, "CurrentSongism.xml"), (Songism)sender);

				ScreenManager.Instance.ChangeScreens ("SongismScreen");
			} else {
				// TODO
			}
        }

        public void LoadContent()
        {
            for (int i = 0; i < Layers.Count; i++)
            {
                Layers[i].LoadContent(Layers[i].TileDimensions);
            }

            XmlManager<List<Songism>> songismLoader = new XmlManager<List<Songism>>();
			XmlManager<List<MapEntrance>> xmMapEntranceLoader = new XmlManager<List<MapEntrance>> ();
            //Songisms = songismLoader.Load("Load/Gameplay/Songisms/Map1.xml");
           // Songisms = songismLoader.Load(System.IO.Path.Combine(Globals.LoadGameplaySongismsDirectory, "downtown_los_angeles.xml"));

			//DowntownLosAngeles
			if (System.IO.File.Exists (System.IO.Path.Combine (Globals.LoadGameplaySongismsDirectory, MapName + ".xml"))) {
				Songisms = songismLoader.Load (System.IO.Path.Combine (Globals.LoadGameplaySongismsDirectory, MapName + ".xml"));
			} else {
				Songisms = new List<Songism> ();
			}

			if (System.IO.File.Exists (System.IO.Path.Combine (Globals.LoadGameplayMapEntrancesDirectory, MapName + ".xml"))) {
				MapEntrances = xmMapEntranceLoader.Load (System.IO.Path.Combine (Globals.LoadGameplayMapEntrancesDirectory, MapName + ".xml"));
			} else {
				MapEntrances = new List<MapEntrance> ();
			}

            foreach (Songism songism in Songisms)
            {
                songism.LoadContent();
                songism.Image.DeactivateEffect("FadeEffect");
                songism.Image.DeactivateEffect("SpriteSheetEffect");

                songism.OnClick +=songism_OnClick;
            }

			foreach (MapEntrance mapEntrance in MapEntrances) {
				mapEntrance.LoadContent ();
				mapEntrance.Image.DeactivateEffect ("FadeEffect");
				mapEntrance.Image.DeactivateEffect ("SpriteSheetEffect");

				mapEntrance.OnClick += MapEntrance_OnClick;
				mapEntrance.OnPlayerEnter += MapEntrance_OnPlayerEnter;
			}

			XmlManager<List<NPC>> xmlNpcLoader = new XmlManager<List<NPC>> ();

			if (System.IO.File.Exists (System.IO.Path.Combine (Globals.LoadGameplayNPCDirectory, MapName + ".xml"))) {
				NPCs = xmlNpcLoader.Load (System.IO.Path.Combine (Globals.LoadGameplayNPCDirectory, MapName + ".xml"));
			} else {
				NPCs = new List<NPC> ();
			}

			foreach (NPC npc in NPCs) {
				npc.LoadContent ();
				npc.Image.DeactivateEffect("FadeEffect");
				npc.Image.DeactivateEffect("SpriteSheetEffect");

				npc.InteractionImage.DeactivateEffect("FadeEffect");
				npc.InteractionImage.DeactivateEffect("SpriteSheetEffect");

				npc.MissionCompleteInteractionImage.DeactivateEffect("FadeEffect");
				npc.MissionCompleteInteractionImage.DeactivateEffect("SpriteSheetEffect");

				npc.OnClick += Npc_OnClick;
				npc.OnIsActiveChanged += Npc_OnIsActiveChanged;
			}
        }

        void MapEntrance_OnClick (object sender, EventArgs e)
        {
			ChangeMaps (((MapEntrance)sender).NextMapName);
        }

		private void MapEntrance_OnPlayerEnter (object sender, EventArgs e)
		{
			ChangeMaps (((MapEntrance)sender).NextMapName);
		}

		private void ChangeMaps(string nextMapName)
		{
			XmlManager<GameProgress> xmGameProgess = new XmlManager<GameProgress> ();
			GameProgress gpProgress = xmGameProgess.Load (System.IO.Path.Combine (Globals.LoadGameplayDirectory, "game_progress.xml"));

			gpProgress.CurrentMapName = nextMapName;
			xmGameProgess.Save (System.IO.Path.Combine (Globals.LoadGameplayDirectory, "game_progress.xml"), gpProgress);

			OnMapChanging (this, null);
		}

        void Npc_OnIsActiveChanged (object sender, EventArgs e)
        {
			XmlManager<List<NPC>> xmlNpcSaver = new XmlManager<List<NPC>> ();
			//xmlNpcSaver.Save (System.IO.Path.Combine (Globals.LoadGameplayNPCDirectory, "downtown_los_angeles.xml"), NPCs);
			xmlNpcSaver.Save (System.IO.Path.Combine (Globals.LoadGameplayNPCDirectory, MapName + ".xml"), NPCs);
        }

        void Npc_OnClick (object sender, EventArgs e)
        {
			if (!((NPC)sender).CurrentlyInteracting) {
				((NPC)sender).CurrentlyInteracting = true;


			} else {
				((NPC)sender).CurrentlyInteracting = false;
			}

			//InputManager.Instance.ResetInputState ();
        }

        public void UnloadContent()
        {
            foreach (Layer l in Layers)
                l.UnloadContent();

            XmlManager<List<Songism>> songismSaver = new XmlManager<List<Songism>>();
            //songismSaver.Save("Load/Gameplay/Songisms/Map1.xml", Songisms);
            //songismSaver.Save(System.IO.Path.Combine(Globals.LoadGameplaySongismsDirectory, "downtown_los_angeles.xml"), Songisms);
			songismSaver.Save(System.IO.Path.Combine(Globals.LoadGameplaySongismsDirectory, MapName + ".xml"), Songisms);

            foreach (Songism songism in Songisms)
                songism.UnloadContent();

			foreach (MapEntrance mapEntrance in MapEntrances) {
				mapEntrance.UnloadContent ();
			}

			XmlManager<List<NPC>> xmlNpcSaver = new XmlManager<List<NPC>> ();
			//xmlNpcSaver.Save (System.IO.Path.Combine (Globals.LoadGameplayNPCDirectory, "downtown_los_angeles.xml"), NPCs);
			xmlNpcSaver.Save (System.IO.Path.Combine (Globals.LoadGameplayNPCDirectory, MapName + ".xml"), NPCs);

			foreach (NPC npc in NPCs)
				npc.UnloadContent ();

            if (ScreenManager.Instance.IsTransitioning)
                ScreenManager.Instance.IsGuessingSongism = (ScreenManager.Instance.NewScreen.GetType().Name.Equals("SongismScreen"));
        }

        public void Update(GameTime gameTime, ref Player player)
        {
            if (ScreenManager.Instance.GameplayScreenActive)
            {
                /* If the Gameplay screen is active and we're set as guessing the songism, this means we just got back from guessing */
                if (ScreenManager.Instance.IsGuessingSongism)
                {
                    //Camera2D.Instance.Position = player.Position;

                    ScreenManager.Instance.IsGuessingSongism = false;
                    XmlManager<Songism> songismLoader = new XmlManager<Songism>();
                    Songism currentSongism = songismLoader.Load(System.IO.Path.Combine(Globals.LoadGameplaySongismsDirectory, "CurrentSongism.xml"));

                    if (currentSongism.Discovered)
                    {
                        foreach (Songism songism in Songisms)
                        {
                            if (songism.Name.Equals(currentSongism.Name))
                            {
                                songism.Discovered = currentSongism.Discovered;

								if (songism.Name == "Frolic Room" && songism.Image.Path == "Gameplay/Songisms/frolic_room_dark") {
									songism.Image.UnloadContent ();

									songism.Image.Path = "Gameplay/Songisms/frolic_room_light";



									songism.Image.LoadContent ();

									songism.Image.Alpha= 1;

									songism.Image.Position = new Vector2 (448, 64);
									songism.Image.SourceRect = new Rectangle (448, 64, 256, 128);
									songism.Image.SourceRect = new Rectangle (0, 0, songism.Image.SourceRect.Width, songism.Image.SourceRect.Height);
									songism.Image.IsActive = true;

									songism.Image.FadeEffect.IsActive = false;
									songism.Image.SpriteSheetEffect.IsActive = false;
								}
                                break;
                            }
                        }

						XmlManager<List<Songism>> songismSaver = new XmlManager<List<Songism>>();
						songismSaver.Save (System.IO.Path.Combine (Globals.LoadGameplaySongismsDirectory, MapName + ".xml"), Songisms);
                    }
                }

                foreach (Layer l in Layers)
                    l.Update(gameTime, ref player);

				foreach (MapEntrance mapEntrance in MapEntrances)
					mapEntrance.Update (gameTime, ref player);

                foreach (Songism songism in Songisms)
                    songism.Update(gameTime, ref player);

				bool fInteracting = false;
				foreach (NPC npc in NPCs) {
					if (npc.IsActive) {
						npc.Update (gameTime, ref player);
						if (npc.CurrentlyInteracting)
							fInteracting = true;
					}
				}

				ScreenManager.Instance.IsInteractingWithNpc = fInteracting;
            }

            //if (dialog != null)
            //{
            //    dialog.Update(gameTime);

            //    if (dialog.DialogResult == DialogResult.AddToSongBook)
            //    {
            //        /* 
            //            The player correctly guessed the name of the songism
            //            Let's set the discovered property to 'true' and add it to the SongBook
            //        */
            //        currentSongism.Discovered = true;
            //        Inventory.Instance.SongBook.Add(currentSongism);
                    
            //        dialog.UnloadContent();
            //        dialog = null;
            //        currentSongism = null;
            //    }
            //    else if (dialog.DialogResult == DialogResult.Cancel)
            //    {
            //        dialog.UnloadContent();
            //        dialog = null;
            //        currentSongism = null;
            //    }
            //}
        }

        public void Draw(SpriteBatch spriteBatch, string drawType)
        {
            if (ScreenManager.Instance.GameplayScreenActive)
            {
                foreach (Layer l in Layers)
                    l.Draw(spriteBatch, drawType);

				foreach (MapEntrance mapEntrance in MapEntrances)
					mapEntrance.Draw (spriteBatch);

                foreach (Songism songism in Songisms)
                    songism.Draw(spriteBatch);

				foreach (NPC npc in NPCs) {
					if (npc.IsActive) {
						npc.Draw (spriteBatch);
					}
				}
            }
            //if (dialog != null)
            //    dialog.Draw(spriteBatch);
        }
    }
}
