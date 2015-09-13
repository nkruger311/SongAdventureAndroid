// next step is tutorial 8
// youtube.com/CodeingMadeEasy


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SongAdventureAndroid
{
    public class ScreenManager
    {
        private static ScreenManager instance;
        /* Used to delay the acceptance of any input until screen transitioning is complete */
        int _transitionInputDelay = 100;
        int _transitionInputDelayCounter = 0;

		[XmlIgnore]
		public bool SpriteBatchHasBegun;
		[XmlIgnore]
		public bool KeepSpriteBatchOpen;

        [XmlIgnore]
        public Vector2 Dimensions { get; set; }
        [XmlIgnore]
        public ContentManager Content { private set; get; }
        XmlManager<GameScreen> xmlGameScreenManager;

        [XmlIgnore]
        public GameScreen CurrentScreen, NewScreen;
        [XmlIgnore]
        public GraphicsDevice GraphicsDevice;
        [XmlIgnore]
        public SpriteBatch SpriteBatch;
        //[XmlIgnore]
        //public GameWindow GameWindow;
        [XmlIgnore]
        public AndroidGameWindow GameWindow;

        //public Image Image;
        [XmlIgnore]
        public bool IsTransitioning { get; private set; }

		private bool _fTransitionStarted;

		//private Image loadingImage;

        [XmlIgnore]
        public bool GameplayScreenActive
        {
            get { return CurrentScreen.Type.Name.Equals("GameplayScreen"); }
        }

        [XmlIgnore]
        public bool IsGuessingSongism { get; set; }

		[XmlIgnore]
		public bool IsInteractingWithNpc { get; set; }

        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                {
                    XmlManager<ScreenManager> xml = new XmlManager<ScreenManager>();
                    instance = xml.Load(Path.Combine(Globals.LoadDirectory, "ScreenManager.xml"));
                }

                return instance;
            }
        }

        public void ChangeScreens(string screenName)
        {
            NewScreen = (GameScreen)Activator.CreateInstance(Type.GetType("SongAdventureAndroid." + screenName));
            //Image.IsActive = true;
            //Image.FadeEffect.Increase = true;
            //Image.Alpha = 0.0f;
            IsTransitioning = true;
			_fTransitionStarted = true;
            _transitionInputDelayCounter = 0;
            InputManager.Instance.InputDisabled = true;
        }

        private void Transition(GameTime gameTime)
        {
            if (IsTransitioning)
            {


                //Image.Update(gameTime);
                //if (Image.Alpha == 1.0f)
				if (_fTransitionStarted)
                {
					_fTransitionStarted = false;

                    CurrentScreen.UnloadContent();
                    CurrentScreen = NewScreen;
                    xmlGameScreenManager.Type = CurrentScreen.Type;
                    if (File.Exists(CurrentScreen.XmlPath))
                        CurrentScreen = xmlGameScreenManager.Load(CurrentScreen.XmlPath);
                    CurrentScreen.LoadContent();
                }
                //else if (Image.Alpha == 0.0f)
				else
                {
					//Image.IsActive = false;
					if (!CurrentScreen.IsInitializing) {
					//	Image.IsActive = false;
						IsTransitioning = false;
					}
                }
            }
            else if (InputManager.Instance.InputDisabled)
            {
                _transitionInputDelayCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (_transitionInputDelayCounter > _transitionInputDelay)
                    InputManager.Instance.InputDisabled = false;
            }
        }

        public ScreenManager()
        {
            //Dimensions = new Vector2(640, 480);
            //CurrentScreen = new SplashScreen();
            ////CurrentScreen = new GameplayScreen();
            ////currentScreen = new SongismScreen();
            //xmlGameScreenManager = new XmlManager<GameScreen>();
            //xmlGameScreenManager.Type = CurrentScreen.Type;
            ////CurrentScreen = xmlGameScreenManager.Load("Load/SplashScreen.xml");
            //CurrentScreen = xmlGameScreenManager.Load(Path.Combine(Globals.LoadGameplayScreensDirectory, "SplashScreen.xml"));



            /* Start game at splash screen */
            //Dimensions = new Vector2(640, 480);
            //CurrentScreen = new SplashScreen();
            //xmlGameScreenManager = new XmlManager<GameScreen>();
            //xmlGameScreenManager.Type = CurrentScreen.Type;
            //CurrentScreen = xmlGameScreenManager.Load(Path.Combine(Globals.LoadGameplayScreensDirectory, "SplashScreen.xml"));

			/* Start game at title screen */
			Dimensions = new Vector2(640, 480);
			CurrentScreen = new TitleScreen();
			//CurrentScreen = new LoadingScreen();
			xmlGameScreenManager = new XmlManager<GameScreen>();
			xmlGameScreenManager.Type = CurrentScreen.Type;

            /* Start game at gameplay screen */
            //Dimensions = new Vector2(640, 480);
            //CurrentScreen = new GameplayScreen();
            //xmlGameScreenManager = new XmlManager<GameScreen>();
            //xmlGameScreenManager.Type = CurrentScreen.Type;

        }

        public void LoadContent(ContentManager Content)
        {
            this.Content = new ContentManager(Content.ServiceProvider, "Content");
            CurrentScreen.LoadContent();
            
			//Image.Scale = new Vector2(GameWindow.ClientBounds.Width, GameWindow.ClientBounds.Height);
            //Image.LoadContent();



			/*
			loadingImage = new Image();
			loadingImage.Path = "Gameplay/UI/loadingscreenanimation";
			//Image.SourceRect = new Rectangle(0, 0, 400, 100);
			//Image.Position = new Vector2 ((ScreenManager.Instance.GraphicsDevice.Viewport.Width / 2) - 100,
			//	(ScreenManager.Instance.GraphicsDevice.Viewport.Height / 2) - 100);
			loadingImage.LoadContent();

			loadingImage.SourceRect = new Rectangle(0, 0, 100, 100);
			//Image.Position = new Vector2 ((ScreenManager.Instance.GraphicsDevice.Viewport.Width / 2) - 100,
			//	(ScreenManager.Instance.GraphicsDevice.Viewport.Height / 2) - 100);

			loadingImage.IsActive = true;
			loadingImage.ActivateEffect ("SpriteSheetEffect");
			loadingImage.SpriteSheetEffect.IsActive = true;

			loadingImage.FadeEffect.IsActive = false;

			loadingImage.SpriteSheetEffect.SwitchFrame = 250;
			loadingImage.SpriteSheetEffect.AmountOfFrames = new Vector2(4, 1);
			loadingImage.SpriteSheetEffect.DefaultFrame = Vector2.Zero;
			loadingImage.SpriteSheetEffect.CurrentFrame = loadingImage.SpriteSheetEffect.DefaultFrame;

			loadingImage.Position = new Vector2 (loadingImage.Position.X - 50, loadingImage.Position.Y - 50);
			*/
        }

        public void UnloadContent()
        {
            CurrentScreen.UnloadContent();
            //Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
			CurrentScreen.Update(gameTime);
            Transition(gameTime);
			//loadingImage.Update (gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
			
			CurrentScreen.Draw(spriteBatch);
			if (IsTransitioning)
			{
				//Color backColor = new Color (89, 144, 25);
				//ScreenManager.Instance.GraphicsDevice.Clear(backColor);

				//loadingImage.Draw (spriteBatch);
                //Image.Draw(spriteBatch);
			}
        }
    }
}
