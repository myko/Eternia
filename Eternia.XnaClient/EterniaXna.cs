using System;
using System.IO;
using Eternia.Game;
using EterniaXna.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.Ui;
using Newtonsoft.Json;

namespace Eternia.XnaClient
{
    public class EterniaXna : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        public EterniaXna()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);
            //graphics.PreferredBackBufferWidth = (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 9) / 10;
            //graphics.PreferredBackBufferHeight = ((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 50) * 9) / 10;

            graphics.PreferredBackBufferWidth = (1920 * 9) / 10;
            graphics.PreferredBackBufferHeight = ((1080 - 50) * 9) / 10;

            graphics.PreferMultiSampling = true;
            graphics.SynchronizeWithVerticalRetrace = false;

            IsFixedTimeStep = false;
                        
            screenManager = new ScreenManager(this);

            Components.Add(screenManager);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;

            base.Initialize();

            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            Player player = null;
            var containerPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Eternia");

            if (Directory.Exists(containerPath))
            {
                var filename = Path.Combine(containerPath, "Player.xml");
                if (File.Exists(filename))
                {
                    FileStream stream = File.Open(filename, FileMode.Open, FileAccess.Read);
                    StreamReader reader = new StreamReader(stream);

                    // Read the data from the file
                    try
                    {
                        var json = reader.ReadToEnd();
                        player = JsonConvert.DeserializeObject<Player>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
                    }
                    catch
                    {
                        System.Diagnostics.Debug.WriteLine("Corrupt Player.xml file found.");
                    }

                    // Close the file
                    stream.Close();
                }
            }

            if (player == null)
                player = Player.CreateWithDefaults();

            foreach (var hero in player.Heroes)
            {
                if (!player.UnlockedTargetingStrategies.Contains(hero.TargettingStrategy))
                    hero.TargettingStrategy = player.UnlockedTargetingStrategies[0];
            }

            screenManager.AddScreen(new TitleScreen(player));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(0.03f, 0.03f, 0.10f));

            base.Draw(gameTime);
        }
    }
}
