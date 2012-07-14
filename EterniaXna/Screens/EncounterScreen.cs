using System;
using System.Collections.Generic;
using System.Linq;
using EterniaGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myko.Xna.Ui;
using Myko.Xna.SkinnedModel;
using EterniaGame.Actors;
using EterniaXna.EditorForms;

namespace EterniaXna.Screens
{
    public class EncounterScreen: GameScreen
    {
        class ScrollingText
        {
            public Actor Source { get; set; }
            public Actor Target { get; set; }
            public string Text { get; set; }
            public Vector2 Position { get; set; }
            public float Alpha { get; set; }
            public SpriteFont Font { get; set; }
            public Color Color { get; set; }
            public float Speed { get; set; }

            public void Update(GameTime time)
            {
                Position = Position + new Vector2(0, (float)-time.ElapsedGameTime.TotalSeconds * Speed);
                Alpha -= (float)time.ElapsedGameTime.TotalSeconds * 0.8f;
            }
        }

        class GraphicEffect
        {
            public Vector2 Position { get; set; }
            public float Scale { get; set; }
            public float Alpha { get; set; }
            public float Age { get; set; }

            public void Update(GameTime time)
            {
                Age += (float)time.ElapsedGameTime.TotalSeconds;
                Scale = 1f + 10f * Age;
                Alpha = 1f - 4f * Age;
            }
        }

        private readonly Player player;
        private readonly EncounterDefinition encounterDefinition;
        private readonly Battle battle;

        List<Turn> turns;
        List<Actor> selectedActors;
        Actor mouseOverActor;
        List<ScrollingText> scrollingTexts = new List<ScrollingText>();
        List<GraphicEffect> graphicEffects = new List<GraphicEffect>();
        List<ParticleSystem> particleSystems = new List<ParticleSystem>();

        List<ActorModel> actorModels;
        List<ProjectileModel> projectileModels;
        MapModel mapModel;

        Random random = new Random();
        SpriteFont kootenayFont;
        SpriteFont kootenaySmallFont;
        Texture2D healthBarTexture;
        Texture2D defaultAbilityTexture;
        Model levelModel;
        Texture2D levelTexture;
        Texture2D splashTexture;
        Effect billboardEffect;
        Effect mapEffect;
        Texture2D selectionTexture;
        
        Vector2 cameraPosition;
        float cameraDistance = 10f;
        Matrix view;
        Matrix projection;

        List<Button> abilityButtons;
        List<Button> targettingStrategyButtons;

        Texture2D destinationTexture;
        bool isPaused = true;
        bool isBenchmarking = false;

        EncounterForm encounterForm;
        float fps = 60.0f;

        public EncounterScreen(Player player, EncounterDefinition encounterDefinition, Battle battle)
        {
            this.player = player;
            this.encounterDefinition = encounterDefinition;
            this.battle = battle;
            //encounterDefinition.Map = new Map(18, 16);

            battle.Start();

            turns = new List<Turn>();

            cameraPosition = new Vector2(-2, -5);

            abilityButtons = new List<Button>();
            targettingStrategyButtons = new List<Button>();
            selectedActors = new List<Actor>();
            actorModels = new List<ActorModel>();
            projectileModels = new List<ProjectileModel>();
        }

        void abilityButton_Click(Button button)
        {
            if (button != null)
            {
                var abilityButton = button.Content as AbilityButton;
                if (abilityButton != null)
                {
                    var ability = abilityButton.Ability;
                    ability.Enabled = !ability.Enabled;
                }
            }
        }

        void targettingStrategyButton_Click(Button button)
        {
            if (button != null)
            {
                var strategyButton = button.Content as TargetingStrategyButton;
                if (strategyButton != null)
                {
                    var actor = strategyButton.Actor;
                    actor.TargettingStrategy = strategyButton.TargetingStrategy.Value;
                }
            }
        }

        public override void LoadContent()
        {
            billboardEffect = ContentManager.Load<Effect>(@"Shaders\Billboard");
            mapEffect = ContentManager.Load<Effect>(@"Shaders\Map");
            kootenayFont = ContentManager.Load<SpriteFont>(@"Fonts\Kootenay");
            kootenaySmallFont = ContentManager.Load<SpriteFont>(@"Fonts\KootenaySmall");
            healthBarTexture = ContentManager.Load<Texture2D>(@"Interface\healthbar");
            destinationTexture = ContentManager.Load<Texture2D>(@"Interface\destination");
            selectionTexture = ContentManager.Load<Texture2D>(@"Interface\selection");
            defaultAbilityTexture = ContentManager.Load<Texture2D>(@"Icons\INV_Misc_QuestionMark");

            levelModel = ContentManager.Load<Model>(@"Models\Levels\pointdread");
            levelTexture = ContentManager.Load<Texture2D>(@"Models\Levels\2136");

            splashTexture = ContentManager.Load<Texture2D>(@"Models\Objects\splash_diffuse");

            var buttonTexture = ContentManager.Load<Texture2D>(@"Interface\button");
            var buttonMouseOverTexture = ContentManager.Load<Texture2D>(@"Interface\button-mouseover");

            var pauseButton = new Button
            {
                Content = "Unpause",
                Position = new Vector2(Width / 2 - 220, 4),
                Background = new Color(100, 140, 120),
                BackgroundTexture = buttonTexture,
                MouseOverTexture = buttonMouseOverTexture,
                Width = 150,
                Height = 40,
                ZIndex = 0.1f,
            };
            pauseButton.Click += () => pauseButton_Click(pauseButton);
            Controls.Add(pauseButton);

            var benchButton = new Button
            {
                Content = "Benchmark",
                Position = new Vector2(Width / 2 + 20, 4),
                Background = new Color(100, 140, 120),
                BackgroundTexture = buttonTexture,
                MouseOverTexture = buttonMouseOverTexture,
                Width = 150,
                Height = 40,
                ZIndex = 0.1f,
            };
            benchButton.Click += () => isBenchmarking = !isBenchmarking;
            Controls.Add(benchButton);

            for (int i = 0; i < 10; i++)
            {
                var abilityButton = new Button();
                abilityButton.Position = new Vector2((Width / 2) - 200 + i * 40, Height - 50);
                abilityButton.Width = 32;
                abilityButton.Height = 32;
                abilityButton.Background = Color.TransparentBlack;
                abilityButton.Click += () => abilityButton_Click(abilityButton);
                abilityButtons.Add(abilityButton);
                Controls.Add(abilityButton);
            }

            for (int i = 0; i < player.UnlockedTargetingStrategies.Count; i++)
            {
                var targettingStrategyButton = new Button();
                targettingStrategyButton.Position = new Vector2((Width / 2) - 200 + i * 40, Height - 100);
                targettingStrategyButton.Width = 32;
                targettingStrategyButton.Height = 32;
                targettingStrategyButton.Background = Color.TransparentBlack;
                targettingStrategyButton.Click += () => targettingStrategyButton_Click(targettingStrategyButton);
                targettingStrategyButtons.Add(targettingStrategyButton);
                Controls.Add(targettingStrategyButton);
            }

            base.LoadContent();
        }

        public override void HandleInput(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var keyboardState = Keyboard.GetState();
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            FindMouseOverActor(mouseState);

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (!Controls.Any(x => x.IsMouseOver))
                {
                    if (keyboardState.IsKeyUp(Keys.LeftShift))
                        selectedActors.Clear();
                    if (mouseOverActor != null && !selectedActors.Contains(mouseOverActor))
                        selectedActors.Add(mouseOverActor);
                }
            }

            if (mouseState.RightButton == ButtonState.Pressed && selectedActors.Any())
            {
                if (mouseOverActor != null)
                {
                    selectedActors.Where(x => x.IsAlive).ToList().ForEach(x =>
                    {
                        if (keyboardState.IsKeyUp(Keys.LeftShift))
                            x.Targets.Clear();
                        if (!x.Targets.Contains(mouseOverActor))
                            x.Targets.Enqueue(mouseOverActor);
                    });
                }
                else
                {
                    selectedActors.Where(x => x.IsAlive).ToList().ForEach(x =>
                    {
                        x.Destination = Unproject(mouseState);
                        x.Targets.Clear();
                    });
                }
            }

            cameraDistance = Math.Min(40f, Math.Max(5f, 25f - mouseState.ScrollWheelValue * 0.01f));

            if (keyboardState.IsKeyDown(Keys.Left))
                cameraPosition.X = Math.Max(-20, cameraPosition.X - 20f * deltaTime);

            if (keyboardState.IsKeyDown(Keys.Right))
                cameraPosition.X = Math.Min(20, cameraPosition.X + 20f * deltaTime);

            if (keyboardState.IsKeyDown(Keys.Up))
                cameraPosition.Y = Math.Min(18, cameraPosition.Y + 20f * deltaTime);

            if (keyboardState.IsKeyDown(Keys.Down))
                cameraPosition.Y = Math.Max(-30, cameraPosition.Y - 20f * deltaTime);

            if (keyboardState.IsKeyDown(Keys.F2))
            {
                if (encounterForm == null || !encounterForm.Visible)
                {
                    encounterForm = new EncounterForm();
                    encounterForm.EncounterDefinition = encounterDefinition;
                    encounterForm.Show();
                }
            }
        }

        private Vector2 Unproject(MouseState mouseState)
        {
            var near = ScreenManager.GraphicsDevice.Viewport.Unproject(new Vector3(mouseState.X, mouseState.Y, 0), projection, view, Matrix.Identity);
            var far = ScreenManager.GraphicsDevice.Viewport.Unproject(new Vector3(mouseState.X, mouseState.Y, 1), projection, view, Matrix.Identity);
            var ray = Vector3.Normalize(far - near);

            var v = near + ray * -near.Z / ray.Z;
            return new Vector2(v.X, v.Y);
        }

        private Vector2 Project(Vector2 v)
        {
            var r = ScreenManager.GraphicsDevice.Viewport.Project(new Vector3(v, 0), projection, view, Matrix.Identity);
            return new Vector2(r.X, r.Y);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var actor in battle.Actors.Where(x => !actorModels.Any(y => y.Actor == x)))
            {
                var modelFileName = @"Models\Actors\" + actor.TextureName;
                if (!System.IO.File.Exists(System.IO.Path.Combine(ContentManager.RootDirectory, modelFileName)))
                    modelFileName = @"Models\Actors\heman";

                modelFileName = @"Models\Actors\token";
                actorModels.Add(new ActorModel(actor, ContentManager.Load<Model>(modelFileName)));
            }

            foreach (var projectile in battle.Projectiles.Where(x => !projectileModels.Any(y => y.Projectile == x)))
            {
                var particleSystem = new ParticleSystem() { Position = projectile.Position, Texture = ContentManager.Load<Texture2D>(@"Sprites\smoke_particle") };

                particleSystems.Add(particleSystem);

                projectileModels.Add(new ProjectileModel() { 
                    Projectile = projectile, 
                    Model = ContentManager.Load<Model>(@"Models\Objects\" + projectile.ModelName), 
                    Texture = ContentManager.Load<Texture2D>(@"Models\Objects\" + projectile.TextureName), 
                    ParticleSystem = particleSystem });
            }
            projectileModels.RemoveAll(x => !x.Projectile.IsAlive);

            if (!isPaused)
            {
                float deltaTime = isBenchmarking ? 0.5f : (float)gameTime.ElapsedGameTime.TotalSeconds;
                var turn = battle.Run(deltaTime);
                foreach (var ev in turn.Events)
                {
                    if (selectedActors.Any())
                        if (!selectedActors.Any(x => ev.Actor == x || ev.Target == x))
                            continue;

                    var text = CreateScrollingTextForEvent(ev);
                    if (text != null)
                        scrollingTexts.Add(text);
                }
                turns.Add(turn);

                foreach (var actorModel in actorModels)
                {
                    actorModel.Update(gameTime, turn);
                }

                foreach (var projectile in projectileModels)
                {
                    projectile.Update(gameTime);
                }

                while (battle.GraphicEffects.Any())
                {
                    graphicEffects.Add(new GraphicEffect { Alpha = 1f, Scale = 1f, Position = battle.GraphicEffects.Dequeue().Position });
                }

                particleSystems.RemoveAll(ps => !ps.IsAlive && !ps.Particles.Any());
                particleSystems.ForEach(ps => ps.Update(gameTime));
            }

            scrollingTexts.RemoveAll(st => st.Alpha <= 0f);
            scrollingTexts.ForEach(st => st.Update(gameTime));

            graphicEffects.RemoveAll(ge => ge.Alpha <= 0f);
            graphicEffects.ForEach(ge => ge.Update(gameTime));

            UpdateAbilityButtons();
            UpdateStrategyButtons();

            if (battle.Actors.Count(a => a.IsAlive && a.Faction == Factions.Friend) == 0)
            {
                ScreenManager.AddScreen(new DefeatScreen(player, battle));
                ScreenManager.RemoveScreen(this);
            }

            if (battle.Actors.Count(a => a.IsAlive && a.Faction == Factions.Enemy) == 0)
            {
                ScreenManager.AddScreen(new VictoryScreen(player, encounterDefinition, battle, turns));
                ScreenManager.RemoveScreen(this);
            }
        }

        private void FindMouseOverActor(MouseState mouseState)
        {
            if (mouseState.X > 0 && mouseState.X < 150 && mouseState.Y > 0 && mouseState.Y < 50 * battle.Actors.Count)
                mouseOverActor = battle.Actors[mouseState.Y / 50];
            else
                mouseOverActor = null;

            foreach (var actor in battle.Actors.Where(x => x.IsAlive))
            {
                if ((actor.Position - Unproject(mouseState)).Length() < actor.Diameter)
                    mouseOverActor = actor;
            }
        }

        private ScrollingText CreateScrollingTextForEvent(Event ev)
        {
            if (ev.Target != null)
            {
                var damage = ev.Healing > 0f ? ev.Healing.ToString("0") : ev.Damage.ToString("0");
                var text = damage;

                switch (ev.CombatOutcome)
                {
                    case CombatOutcome.Miss:
                        text = "Miss";
                        break;
                    case CombatOutcome.Dodge:
                        text = "Dodge";
                        break;
                }

                return new ScrollingText()
                {
                    Source = ev.Actor,
                    Target = ev.Target,
                    Font = ev.CombatOutcome == CombatOutcome.Crit ? kootenayFont : kootenaySmallFont,
                    Text = text,
                    Color = ev.Healing > 0f ? Color.LightGreen : ev.Ability != null ? Color.Yellow : Color.White,
                    Alpha = 1f,
                    Position = Project(ev.Target.Position) + new Vector2((float)random.NextDouble() * 40f - 30f, -(ev.Target.Radius + 20f)),
                    Speed = random.Between(30f, 60f),
                };
            }

            return null;
        }

        public override void Draw(GameTime gameTime)
        {
            DrawWorld();

            DrawThreatList();
            DrawAuras();

            DrawScrollingTexts();

            DrawNamePlates();

            DrawCombatLog();

            fps = (float)((fps + (1000.0 / gameTime.ElapsedGameTime.TotalMilliseconds)) / 2.0);
        }

        private void DrawScrollingTexts()
        {
            scrollingTexts.ForEach(st =>
            {
                if (selectedActors.Any())
                    if (!selectedActors.Contains(st.Source) && !selectedActors.Contains(st.Target))
                        return;
                SpriteBatch.DrawString(st.Font, st.Text, st.Position, new Color(st.Color, st.Alpha));
            });
        }

        private void UpdateAbilityButtons()
        {
            abilityButtons.ForEach(button => { button.Content = null; button.Tooltip = null; });

            if (selectedActors.Any())
            {
                var selectedActor = selectedActors.First();
                for (int i = 0; i < selectedActor.Abilities.Count; i++)
                {
                    var ability = selectedActor.Abilities.ElementAt(i);
                    var abilityTexture = ContentManager.Load<Texture2D>(@"Icons\" + ability.TextureName) ?? defaultAbilityTexture;

                    abilityButtons[i].Content = new AbilityButton(abilityButtons[i], selectedActor, ability, abilityTexture, BlankTexture, kootenayFont);
                    abilityButtons[i].Tooltip = new AbilityTooltip(selectedActor, ability) { Font = kootenaySmallFont };
                }
            }
        }

        private void UpdateStrategyButtons()
        {
            targettingStrategyButtons.ForEach(button => { button.Content = null; button.Tooltip = null; });

            if (selectedActors.Any(x => x.PlayerControlled))
            {
                var selectedActor = selectedActors.First(x => x.PlayerControlled);
                var texture = ContentManager.Load<Texture2D>("Icons\\Ability_thunderbolt");
                
                var strategies = TargetingStrategy.All().Where(x => player.UnlockedTargetingStrategies.Contains(x.Value)).ToArray();
                for (int i = 0; i < strategies.Length; i++)
                {
                    targettingStrategyButtons[i].Content = new TargetingStrategyButton(targettingStrategyButtons[i], selectedActor, strategies[i], texture);
                    var smallFont = ContentManager.Load<SpriteFont>("Fonts\\kootenaySmall");
                    var t = strategies[i].Name + "\n" + strategies[i].Description;
                    var ts = smallFont.MeasureString(t);
                    targettingStrategyButtons[i].Tooltip = new Border(t) { Font = smallFont, Width = ts.X + 20, Height = ts.Y + 20 };
                }
            }
        }
        
        private void DrawThreatList()
        {
            if (selectedActors.Any())
            {
                var selectedActor = selectedActors.First();
                for (int i = 0; i < selectedActor.ThreatList.Count; i++)
                {
                    var threat = selectedActor.ThreatList[i];
                    SpriteBatch.DrawString(kootenaySmallFont, threat.Value.ToString() + " " + threat.Actor.Name, new Vector2(Width - 300, Height - 200 + i * 15), Color.White);
                }
            }
        }

        private void DrawAuras()
        {
            if (selectedActors.Any())
            {
                var selectedActor = selectedActors.First();
                for (int i = 0; i < selectedActor.Auras.Count; i++)
                {
                    var aura = selectedActor.Auras[i];
                    SpriteBatch.DrawString(kootenaySmallFont, string.Format("{0} {1:0}", aura.Name, aura.Duration), new Vector2(Width - 450, Height - 200 + i * 15), Color.White);
                }
            }
        }

        private void DrawCombatLog()
        {
            float y = Height - 15;
            foreach (var ev in turns.SelectMany(t => t.Events).Reverse().Take(10))
            {
                SpriteBatch.DrawString(kootenaySmallFont, ev.ToString(), new Vector2(0, y), Color.White);
                y -= 15;
            }
        }

        private void DrawWorld()
        {
            float aspectRatio = (float)ScreenManager.GraphicsDevice.Viewport.Width /
                    (float)ScreenManager.GraphicsDevice.Viewport.Height;

            view = Matrix.CreateLookAt(new Vector3(cameraPosition.X, cameraPosition.Y, cameraDistance),
                                              new Vector3(cameraPosition.X, cameraPosition.Y + 7f, 0),
                                              new Vector3(0, 0.9f, 0.1f));

            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                    aspectRatio,
                                                                    0.1f,
                                                                    1000f);

            ScreenManager.GraphicsDevice.RenderState.DepthBufferEnable = true;

            if (mapModel == null)
                mapModel = new MapModel(encounterDefinition.Map);

            mapModel.Draw(ScreenManager.GraphicsDevice, view, projection, mapEffect, ContentManager);

            //foreach (ModelMesh mesh in levelModel.Meshes)
            //{
            //    foreach (Effect effect in mesh.Effects)
            //    {
            //        (effect as BasicEffect).Texture = levelTexture;
            //        (effect as BasicEffect).TextureEnabled = true;

            //        effect.Parameters["World"].SetValue(Matrix.Identity);
            //        effect.Parameters["View"].SetValue(view);
            //        effect.Parameters["Projection"].SetValue(projection);
            //    }

            //    mesh.Draw();
            //}

            foreach (var actorModel in actorModels.OrderBy(a => a.Actor.Position.Y))
            {
                var color = Color.LightGray;
                var outlineColor = Color.White;

                if (actorModel.Actor == mouseOverActor)
                {
                    color = Color.White;
                    if (mouseOverActor.Faction == Factions.Friend)
                        outlineColor = Color.Green;
                    else
                        outlineColor = Color.Red;
                }

                if (selectedActors.Any())
                {
                    color = Color.Gray;
                    if (actorModel.Actor == mouseOverActor)
                        color = Color.LightGray;
                    if (selectedActors.Any(x => x.Targets.FirstOrDefault() == actorModel.Actor))
                        color = Color.Salmon;
                    if (selectedActors.Contains(actorModel.Actor))
                        color = Color.White;
                }

                if (selectedActors.Any() && actorModel.Actor.IsAlive)
                {
                    var v = Project(actorModel.Actor.Position);
                    DrawHealthBar((int)(v.X - 25), (int)(v.Y - 50), 50, 10, actorModel.Actor.HealthFraction, Color.GreenYellow);
                }

                actorModel.Draw(view, projection, color, ContentManager, ScreenManager.GraphicsDevice, actorModel.Actor == mouseOverActor, outlineColor);
            }

            foreach (var projectile in projectileModels)
            {
                projectile.Draw(view, projection);
            }

            foreach (var selectedActor in selectedActors)
            {
                var vertices = new VertexPositionTexture[6];
                vertices[0] = new VertexPositionTexture(new Vector3(-1, 1, 0), new Vector2(0, 0));
                vertices[1] = new VertexPositionTexture(new Vector3(1, 1, 0), new Vector2(1, 0));
                vertices[2] = new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 1));
                vertices[3] = new VertexPositionTexture(new Vector3(1, 1, 0), new Vector2(1, 0));
                vertices[4] = new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(1, 1));
                vertices[5] = new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 1));

                ScreenManager.GraphicsDevice.VertexDeclaration = new VertexDeclaration(ScreenManager.GraphicsDevice, VertexPositionTexture.VertexElements);
                ScreenManager.GraphicsDevice.RenderState.AlphaBlendEnable = true;
                ScreenManager.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
                ScreenManager.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
                ScreenManager.GraphicsDevice.RenderState.DepthBufferWriteEnable = false;

                billboardEffect.Parameters["View"].SetValue(view);
                billboardEffect.Parameters["Projection"].SetValue(projection);
                billboardEffect.Parameters["Alpha"].SetValue(1f);

                billboardEffect.Parameters["World"].SetValue(Matrix.CreateScale(selectedActor.Radius * 1.4f) * Matrix.CreateTranslation(new Vector3(selectedActor.Position, 0.04f)));
                if (selectedActor.Faction == Factions.Friend)
                    billboardEffect.Parameters["Diffuse"].SetValue(Color.Green.ToVector4());
                else
                    billboardEffect.Parameters["Diffuse"].SetValue(Color.Red.ToVector4());
                billboardEffect.Parameters["Texture"].SetValue(selectionTexture);

                billboardEffect.Begin();
                foreach (var pass in billboardEffect.CurrentTechnique.Passes)
                {
                    pass.Begin();
                    ScreenManager.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertices, 0, 2);
                    pass.End();
                }
                billboardEffect.End();

                if (selectedActor.Destination.HasValue)
                {
                    billboardEffect.Parameters["World"].SetValue(Matrix.CreateScale(selectedActor.Radius * 1.4f) * Matrix.CreateTranslation(new Vector3(selectedActor.Destination.Value, 0.04f)));
                    billboardEffect.Parameters["Diffuse"].SetValue(Color.White.ToVector4());
                    billboardEffect.Parameters["Texture"].SetValue(destinationTexture);

                    billboardEffect.Begin();
                    foreach (var pass in billboardEffect.CurrentTechnique.Passes)
                    {
                        pass.Begin();
                        ScreenManager.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertices, 0, 2);
                        pass.End();
                    }
                    billboardEffect.End();
                }

                ScreenManager.GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
            }

            foreach (var graphicEffect in graphicEffects)
            {
                var v = Project(graphicEffect.Position);
                float s = graphicEffect.Scale;

                var p = new Vector3(graphicEffect.Position, 1f);
                var vertices = new VertexPositionTexture[6];
                vertices[0] = new VertexPositionTexture(p + new Vector3(-1, 1, 0) * s, new Vector2(0, 0));
                vertices[1] = new VertexPositionTexture(p + new Vector3(1, 1, 0) * s, new Vector2(1, 0));
                vertices[2] = new VertexPositionTexture(p + new Vector3(-1, -1, 0) * s, new Vector2(0, 1));
                vertices[3] = new VertexPositionTexture(p + new Vector3(1, 1, 0) * s, new Vector2(1, 0));
                vertices[4] = new VertexPositionTexture(p + new Vector3(1, -1, 0) * s, new Vector2(1, 1));
                vertices[5] = new VertexPositionTexture(p + new Vector3(-1, -1, 0) * s, new Vector2(0, 1));

                billboardEffect.Parameters["World"].SetValue(Matrix.Identity);
                billboardEffect.Parameters["View"].SetValue(view);
                billboardEffect.Parameters["Projection"].SetValue(projection);
                billboardEffect.Parameters["Texture"].SetValue(splashTexture);
                billboardEffect.Parameters["Alpha"].SetValue(graphicEffect.Alpha);
                billboardEffect.Parameters["Diffuse"].SetValue(Color.White.ToVector4());

                ScreenManager.GraphicsDevice.VertexDeclaration = new VertexDeclaration(ScreenManager.GraphicsDevice, VertexPositionTexture.VertexElements);
                ScreenManager.GraphicsDevice.RenderState.AlphaBlendEnable = true;
                ScreenManager.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
                ScreenManager.GraphicsDevice.RenderState.DestinationBlend = Blend.One;
                ScreenManager.GraphicsDevice.RenderState.DepthBufferWriteEnable = false;

                billboardEffect.Begin();
                foreach (var pass in billboardEffect.CurrentTechnique.Passes)
                {
                    pass.Begin();
                    ScreenManager.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertices, 0, 2);
                    pass.End();
                }
                billboardEffect.End();

                ScreenManager.GraphicsDevice.RenderState.AlphaBlendEnable = true;
                ScreenManager.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
                ScreenManager.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
                ScreenManager.GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
            }

            foreach (var particleSystem in particleSystems)
            {
                particleSystem.Draw(ContentManager.Load<Effect>(@"Shaders\Particle"), view, projection, ScreenManager.GraphicsDevice);
            }

            SpriteBatch.DrawString(Font, fps.ToString("0"), new Vector2(Width - 50, Height - Font.LineSpacing), Color.White);
        }

        private void DrawNamePlates()
        {
            for (int i = 0; i < battle.Actors.Count; i++)
            {
                var actor = battle.Actors[i];
                var isSelected = selectedActors.Contains(actor);
                var isFirstSelected = selectedActors.FirstOrDefault() == actor;
                var color = Color.Gray;

                if (actor == mouseOverActor)
                    color = Color.LightGray;

                if (isSelected)
                    color = Color.White;

                if (selectedActors.Any())
                {
                    if (selectedActors.Any(x => x.Targets.FirstOrDefault() == actor))
                        color = Color.Salmon;
                }

                var actorText = actor.Name;
                if (actor.Targets.Any())
                    actorText += " > " + string.Join(", ", actor.Targets.Select(x => x.Name).ToArray());

                SpriteBatch.Draw(ContentManager.Load<Texture2D>(@"Models\Actors\" + actor.TextureName + "_portrait"), new Rectangle(0, i * 50, 50, 50), color);
                SpriteBatch.DrawString(kootenayFont, actorText, new Vector2(50, i * 50), color);
                if (actor.Faction == Factions.Enemy)
                    DrawHealthBar(50, 25 + i * 50, 100, 15, actor.HealthFraction, Color.Red);
                else if (actor.Faction == Factions.Friend)
                    DrawHealthBar(50, 25 + i * 50, 100, 15, actor.HealthFraction, Color.GreenYellow);
                else
                    DrawHealthBar(50, 25 + i * 50, 100, 15, actor.HealthFraction, Color.Yellow);
                if (actor.ResourceType == ActorResourceTypes.Mana)
                    DrawHealthBar(50, 25 + i * 50 + 15, 100, 5, actor.ManaFraction, Color.CornflowerBlue);
                else if (actor.ResourceType == ActorResourceTypes.Energy)
                    DrawHealthBar(50, 25 + i * 50 + 15, 100, 5, actor.EnergyFraction, Color.LightGoldenrodYellow);
                if (actor.CastingProgress != null)
                {
                    DrawHealthBar(50, 25 + i * 50 + 20, 100, 5, (actor.CastingProgress.Duration - actor.CastingProgress.Current) / actor.CastingProgress.Duration, Color.Yellow);
                    if (isFirstSelected)
                        DrawHealthBar((int)Width / 2 - 100, (int)Height - 140, 200, 10, (actor.CastingProgress.Duration - actor.CastingProgress.Current) / actor.CastingProgress.Duration, Color.Yellow);
                }
                SpriteBatch.DrawString(kootenaySmallFont, actor.CurrentHealth.ToString("0") + "/" + actor.MaximumHealth.ToString("0"), new Vector2(55, 25 + i * 50), Color.White, ZIndex + 0.003f);
            }
        }

        private void DrawHealthBar(int x, int y, int width, int height, float fraction, Color color)
        {
            SpriteBatch.Draw(healthBarTexture, new Rectangle(x, y, width, height), Color.DarkGray, ZIndex + 0.001f);
            SpriteBatch.Draw(healthBarTexture, new Rectangle(x, y, (int)(fraction * width), height), color, ZIndex + 0.002f);
        }

        private void pauseButton_Click(Button pauseButton)
        {
            isPaused = !isPaused;
            pauseButton.Content = isPaused ? "Unpause" : "Pause";
        }
    }
}
