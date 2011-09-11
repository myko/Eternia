using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Myko.Xna.Ui;
using EterniaXna.Screens;
using EterniaGame;
using System.IO;
using System.Xml.Serialization;

namespace EterniaXna
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class EterniaXna : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        StorageDevice storage;

        public EterniaXna()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferMultiSampling = true;

            screenManager = new ScreenManager(this);

            Components.Add(screenManager);
            Components.Add(new GamerServicesComponent(this));
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

            if (storage == null && !Guide.IsVisible)
                Guide.BeginShowStorageDeviceSelector(result =>
                {
                    storage = Guide.EndShowStorageDeviceSelector(result);
                    Services.AddService(typeof(StorageDevice), storage);
                }, null);

            Player player = null;
            using (var container = storage.OpenContainer("Eternia"))
            {
                if (Directory.Exists(container.Path))
                {
                    var filename = Path.Combine(container.Path, "Player.xml");
                    if (File.Exists(filename))
                    {
                        FileStream stream = File.Open(filename, FileMode.Open, FileAccess.Read);

                        // Read the data from the file
                        XmlSerializer serializer = new XmlSerializer(typeof(Player));
                        try
                        {
                            player = (Player)serializer.Deserialize(stream);
                        }
                        catch
                        {
                            System.Diagnostics.Debug.WriteLine("Corrupt Player.xml file found.");
                        }

                        // Close the file
                        stream.Close();
                        
                        foreach (var hero in player.Heroes)
                        {
                            if (!player.UnlockedTargetingStrategies.Contains(hero.TargettingStrategy))
                                hero.TargettingStrategy = player.UnlockedTargetingStrategies[0];
                        }
                    }
                }
            }

            if (player == null)
                player = Player.CreateWithDefaults();

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
            GraphicsDevice.Clear(Color.DarkSlateGray);

            base.Draw(gameTime);
        }
    }
}
