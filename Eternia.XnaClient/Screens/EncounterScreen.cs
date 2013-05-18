﻿using System;
using System.Collections.Generic;
using System.Linq;
using Eternia.Game;
using Eternia.Game.Actors;
using Eternia.XnaClient;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myko.Xna.Ui;

namespace EterniaXna.Screens
{
    public class EncounterScreen: GameScreen
    {
        private readonly Player player;
        private readonly EncounterDefinition encounterDefinition;
        private readonly Battle battle;

        List<Turn> turns;
        List<Actor> selectedActors;
        Actor mouseOverActor;

        Random random = new Random();
        SpriteFont kootenayFont;
        SpriteFont kootenaySmallFont;
        Texture2D healthBarTexture;
        Texture2D defaultAbilityTexture;
        ScrollingTextSystem scrollingTextSystem;

        List<Button> abilityButtons;
        //List<Button> targettingStrategyButtons;
        List<Button> orderQueueButtons;

        AbilityButton isTargetting;
        bool isPaused = true;
        bool isBenchmarking = false;

        float fps = 60.0f;

        Scene scene;

        public EncounterScreen(Player player, EncounterDefinition encounterDefinition, Battle battle)
        {
            this.player = player;
            this.encounterDefinition = encounterDefinition;
            this.battle = battle;

            battle.Start();

            turns = new List<Turn>();

            abilityButtons = new List<Button>();
            //targettingStrategyButtons = new List<Button>();
            orderQueueButtons = new List<Button>();
            selectedActors = new List<Actor>();
        }

        void abilityButton_Click(Button button)
        {
            if (button != null)
            {
                var abilityButton = button.Content as AbilityButton;
                if (abilityButton != null)
                {
                    var ability = abilityButton.Ability;
                    var actor = abilityButton.Actor;

                    if (ability.TargettingType == TargettingTypes.Self)
                        IssueOrder(actor, new Order(ability, actor, actor));
                    else
                        isTargetting = abilityButton;
                }
            }
        }

        void orderQueueButton_Click(Button button)
        {
            if (button != null)
            {
                var abilityButton = button.Content as OrderButton;
                if (abilityButton != null)
                {
                    var order = abilityButton.Order;
                    abilityButton.Actor.Orders.Remove(order);
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
            kootenayFont = ContentManager.Load<SpriteFont>(@"Fonts\Kootenay");
            kootenaySmallFont = ContentManager.Load<SpriteFont>(@"Fonts\KootenaySmall");
            
            scene = new Scene(ScreenManager.GraphicsDevice);
            scene.LoadContent(ContentManager);
            scene.Nodes.Add(new Level(ScreenManager.GraphicsDevice, ContentManager.Load<Model>(@"Models\Levels\pillarlevel")));
            scene.Nodes.Add(new ParticleSystem(ScreenManager.GraphicsDevice, ContentManager.Load<Effect>(@"Shaders\Particle"))
            {
                BlendState = BlendState.NonPremultiplied,
                Texture = ContentManager.Load<Texture2D>(@"Sprites\fog"),
                RotationSpeed = 0.2f,
                SpawnRate = 0,
                MaxParticles = 200,
                Emitter = () => new Particle()
                {
                    Position = random.NextVector3(-40, 40) - new Vector3(0, 2, 0),
                    Alpha = random.Between(0.1f, 0.16f),
                    Scale = random.Between(15f, 18f),
                    Angle = random.Between(0f, 6f),
                    LifeSpan = float.PositiveInfinity
                },
            });
            scrollingTextSystem = new ScrollingTextSystem(scene, SpriteBatch, kootenaySmallFont, kootenayFont);
            scene.Nodes.Add(scrollingTextSystem);

            // TODO: This is map specific
            AddFire(new Vector3(-10.5f, 0.5f, -10.5f));
            AddFire(new Vector3(10.5f, 0.5f, -10.5f));
            AddFire(new Vector3(10.5f, 0.5f, 10.5f));
            AddFire(new Vector3(-10.5f, 0.5f, 10.5f));

            healthBarTexture = ContentManager.Load<Texture2D>(@"Interface\healthbar");
            defaultAbilityTexture = ContentManager.Load<Texture2D>(@"Icons\INV_Misc_QuestionMark");

            var buttonTexture = ContentManager.Load<Texture2D>(@"Interface\button1");
            var buttonMouseOverTexture = ContentManager.Load<Texture2D>(@"Interface\button1-mouseover");

            var pauseButton = new Button
            {
                Content = "Unpause",
                Position = new Vector2(Width / 2 - 220, 4),
                Background = new Color(230, 140, 60),
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
                Background = new Color(230, 140, 60),
                BackgroundTexture = buttonTexture,
                MouseOverTexture = buttonMouseOverTexture,
                Width = 150,
                Height = 40,
                ZIndex = 0.1f,
            };
            benchButton.Click += () => isBenchmarking = !isBenchmarking;
            //Controls.Add(benchButton);

            for (int i = 0; i < 10; i++)
            {
                var abilityButton = new Button();
                abilityButton.Position = new Vector2((Width / 2) - 300 + i * 40, Height - 80);
                abilityButton.Width = 32;
                abilityButton.Height = 32;
                abilityButton.Background = Color.Transparent;
                abilityButton.Click += () => abilityButton_Click(abilityButton);
                abilityButtons.Add(abilityButton);
                Controls.Add(abilityButton);
            }
            
            for (int i = 0; i < 10; i++)
            {
                var orderQueueButton = new Button();
                orderQueueButton.Position = new Vector2((Width / 2) - 300 + i * 65, Height - 125);
                orderQueueButton.Width = 32;
                orderQueueButton.Height = 32;
                orderQueueButton.Background = Color.Transparent;
                orderQueueButton.Click += () => orderQueueButton_Click(orderQueueButton);
                orderQueueButtons.Add(orderQueueButton);
                Controls.Add(orderQueueButton);
            }

            //for (int i = 0; i < player.UnlockedTargetingStrategies.Count; i++)
            //{
            //    var targettingStrategyButton = new Button();
            //    targettingStrategyButton.Position = new Vector2((Width / 2) + 300 + i * 40, Height - 80);
            //    targettingStrategyButton.Width = 32;
            //    targettingStrategyButton.Height = 32;
            //    targettingStrategyButton.Background = Color.TransparentBlack;
            //    targettingStrategyButton.Click += () => targettingStrategyButton_Click(targettingStrategyButton);
            //    targettingStrategyButtons.Add(targettingStrategyButton);
            //    Controls.Add(targettingStrategyButton);
            //}

            base.LoadContent();
        }

        // TODO: This is map specific
        private void AddFire(Vector3 position)
        {
            scene.Nodes.Add(new ParticleSystem(ScreenManager.GraphicsDevice, ContentManager.Load<Effect>(@"Shaders\Particle"))
            {
                BlendState = BlendState.Additive,
                Texture = ContentManager.Load<Texture2D>(@"Sprites\fire"),
                AlphaFunc = p => p.InverseAgeFraction * 0.3f,
                ScaleFunc = p => p.InverseAgeFraction * 0.6f,
                RotationSpeed = 1f,
                SpawnRate = 0.0005f,
                MaxParticles = 300,
                Emitter = () => new Particle()
                {
                    Position = position + random.NextVector3(-0.3f, 0.3f),
                    Velocity = new Vector3(0, 1, 0) * random.Between(0.9f, 2.5f),
                    Alpha = random.Between(0.2f, 0.8f),
                    LifeSpan = random.Between(0.75f, 1.5f)
                },
            });
            scene.Nodes.Add(new ParticleSystem(ScreenManager.GraphicsDevice, ContentManager.Load<Effect>(@"Shaders\Particle"))
            {
                BlendState = BlendState.Additive,
                Texture = ContentManager.Load<Texture2D>(@"Sprites\light"),
                SpawnRate = 0,
                MaxParticles = 1,
                AlphaFunc = p => 0.25f + (float)Math.Sin(p.Age * 10f + random.NextDouble()) * 0.04f,
                ScaleFunc = p => 8 + (float)Math.Sin(p.Age * 20f + random.NextDouble()) * 0.2f,
                Emitter = () => new Particle()
                {
                    Position = position + new Vector3(0, 1, 0),
                    Angle = random.Between(0f, 6f),
                    LifeSpan = float.PositiveInfinity,
                },
            });
        }

        private MouseState? dragStart;
        private MouseState? dragEnd;
        private bool isPressingLeft;

        public override void HandleInput(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var keyboardState = Keyboard.GetState();
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var mouseWorldLocation = scene.Unproject(mouseState);
                    
            FindMouseOverActor(mouseState, mouseWorldLocation);

            var form = System.Windows.Forms.Form.FromHandle(ScreenManager.Game.Window.Handle);
            if (isTargetting != null)
            {
                if (isTargetting.Ability.TargettingType == TargettingTypes.Location)
                {
                    if (form.Cursor != System.Windows.Forms.Cursors.Cross)
                        form.Cursor = System.Windows.Forms.Cursors.Cross;
                }
                else if (mouseOverActor != null)
                {
                    if ((isTargetting.Ability.TargettingType == TargettingTypes.Friendly && mouseOverActor.Faction != Factions.Friend) ||
                        (isTargetting.Ability.TargettingType == TargettingTypes.Hostile && mouseOverActor.Faction != Factions.Enemy))
                    {
                        if (form.Cursor != System.Windows.Forms.Cursors.No)
                            form.Cursor = System.Windows.Forms.Cursors.No;
                    }
                    else
                    {
                        if (form.Cursor != System.Windows.Forms.Cursors.Cross)
                            form.Cursor = System.Windows.Forms.Cursors.Cross;
                    }
                }
                else
                {
                    if (form.Cursor != System.Windows.Forms.Cursors.SizeAll)
                        form.Cursor = System.Windows.Forms.Cursors.SizeAll;
                }
            }
            else if (mouseOverActor != null)
            {
                if (form.Cursor != System.Windows.Forms.Cursors.Hand)
                    form.Cursor = System.Windows.Forms.Cursors.Hand;
            }
            else
            {
                if (form.Cursor != System.Windows.Forms.Cursors.Arrow)
                    form.Cursor = System.Windows.Forms.Cursors.Arrow;
            }

            if (mouseState.LeftButton == ButtonState.Pressed && !isPressingLeft || dragStart.HasValue)
            {
                isPressingLeft = true;

                if (!Controls.Any(x => x.IsMouseOver))
                {
                    if (isTargetting != null)
                    {
                        var ability = isTargetting.Ability;
                        var actor = isTargetting.Actor;
                        var target = mouseOverActor;

                        if (ability.TargettingType == TargettingTypes.Self)
                            IssueOrder(actor, new Order(ability, actor, actor));
                        else if (ability.TargettingType == TargettingTypes.Hostile && target != null && target.Faction != actor.Faction)
                            IssueOrder(actor, new Order(ability, actor, target));
                        else if (ability.TargettingType == TargettingTypes.Friendly && target != null && target.Faction == actor.Faction)
                            IssueOrder(actor, new Order(ability, actor, target));
                        else if (ability.TargettingType == TargettingTypes.Location)
                            IssueOrder(actor, new Order(ability, actor, null, mouseWorldLocation));

                        isTargetting = null;
                    }
                    else
                    {
                        if (keyboardState.IsKeyUp(Keys.LeftShift))
                            selectedActors.Clear();
                        if (mouseOverActor != null && !selectedActors.Contains(mouseOverActor))
                            selectedActors.Add(mouseOverActor);

                        if (dragStart.HasValue)
                        {
                            if (Math.Abs(mouseState.X - dragStart.Value.X) > 10 || Math.Abs(mouseState.Y - dragStart.Value.Y) > 10)
                                dragEnd = mouseState;
                        }
                        else
                            dragStart = mouseState;

                        if (dragStart.HasValue && dragEnd.HasValue)
                        {
                            var x1 = Math.Min(dragStart.Value.X, dragEnd.Value.X);
                            var x2 = Math.Max(dragStart.Value.X, dragEnd.Value.X);
                            var y1 = Math.Min(dragStart.Value.Y, dragEnd.Value.Y);
                            var y2 = Math.Max(dragStart.Value.Y, dragEnd.Value.Y);

                            selectedActors.Clear();
                            foreach (var actor in battle.Actors.Where(x => x.IsAlive && x.Faction == Factions.Friend))
                            {
                                var pos = scene.Project(actor.Position);
                                if (pos.X > x1 && pos.X < x2 && pos.Y > y1 && pos.Y < y2)
                                    selectedActors.Add(actor);
                            }
                        }
                    }
                }
                else
                {
                    isTargetting = null;
                }
            }

            if (mouseState.LeftButton == ButtonState.Released)
            {
                isPressingLeft = false;
                dragStart = null;
                dragEnd = null;
            }

            if (mouseState.RightButton == ButtonState.Pressed && selectedActors.Any())
            {
                isTargetting = null;

                if (mouseOverActor != null)
                {
                    foreach (var actor in selectedActors)
                    {
                        if (keyboardState.IsKeyDown(Keys.LeftShift))
                            actor.QueueTargetActor(mouseOverActor);
                        else
                            actor.TargetActor(mouseOverActor);
                    }
                }
                else
                {
                    foreach (var actor in selectedActors)
                    {
                        actor.OrderMoveTo(mouseWorldLocation);
                    }
                }
            }

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                battle.Forfeit();
            }

            scene.HandleInput(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            selectedActors.RemoveAll(x => !x.IsAlive);

            foreach (var actor in battle.Actors.Where(x => !scene.Nodes.OfType<ActorModel>().Any(y => y.Actor == x)))
            {
                var actorTexture = ContentManager.SafeLoad<Texture2D>(@"Models\Actors\" + actor.TextureName + "_portrait", @"Models\Actors\warrior_portrait");
                var actorModel = new ActorModel(actor, actorTexture, ContentManager, ScreenManager.GraphicsDevice, ContentManager.Load<Effect>(@"Shaders\Particle"));

                actorModel.Nodes.Add(new Shadow(ScreenManager.GraphicsDevice, ContentManager.Load<Effect>(@"Shaders\billboard"), ContentManager.Load<Texture2D>(@"Sprites\shadow"), actor));
                actorModel.Nodes.Add(new RangeIndicator(actorModel, ScreenManager.GraphicsDevice, ContentManager.Load<Effect>(@"Shaders\billboard"), ContentManager.Load<Texture2D>(@"Interface\circlearea")));
                actorModel.Nodes.Add(new ActorWidgets(ScreenManager.GraphicsDevice, ContentManager.Load<Effect>(@"Shaders\billboard"), ContentManager.Load<Texture2D>(@"Interface\selection"), ContentManager.Load<Texture2D>(@"Interface\destination"), ContentManager.Load<Texture2D>(@"Interface\destination2"), actorModel));
                actorModel.Nodes.Add(new AbilityAnimation(actor, ScreenManager.GraphicsDevice, ContentManager.Load<Effect>(@"Shaders\billboard"), ContentManager.Load<Texture2D>(@"Sprites\attack_sword_1")));
                
                scene.Nodes.Add(actorModel);
            }

            foreach (var actorModel in scene.Nodes.OfType<ActorModel>().OrderBy(a => a.Actor.Position.Y))
            {
                actorModel.IsMouseOver = actorModel.Actor == mouseOverActor;
                actorModel.IsTargeted = selectedActors.Any(x => x.Targets.FirstOrDefault() == actorModel.Actor);
                actorModel.HasSelection = selectedActors.Any();
                actorModel.IsSelected = selectedActors.Contains(actorModel.Actor);
            }

            // TODO: Projectiles should not all look the same
            foreach (var projectile in battle.Projectiles.Where(x => !scene.Nodes.OfType<ProjectileModel>().Any(y => y.Projectile == x)))
            {
                var proj = projectile;

                var smokeTrail = new ParticleSystem(ScreenManager.GraphicsDevice, ContentManager.Load<Effect>(@"Shaders\Particle"))
                {
                    Position = projectile.Position,
                    Texture = ContentManager.Load<Texture2D>(@"Sprites\smoke_particle"),
                    SpawnRate = 0.005f,
                    Forces = { new Vector3(random.Between(-0.5f, 0.5f), random.Between(-2.5f, 0.5f), random.Between(-0.5f, 0.5f)) },
                    AlphaFunc = p => p.InverseAgeFraction * 0.5f,
                    ScaleFunc = p => 0.2f + p.Age,
                    RotationSpeed = 1f,
                    Emitter = () => new Particle
                    {
                        Position = new Vector3(projectile.Position.X, projectile.Position.Z, projectile.Position.Y),
                        LifeSpan = 0.8f,
                        Angle = random.Between(0, 6)
                    }
                };

                var fireBall = new ParticleSystem(ScreenManager.GraphicsDevice, ContentManager.Load<Effect>(@"Shaders\Particle"))
                {
                    Position = projectile.Position,
                    Texture = ContentManager.Load<Texture2D>(@"Sprites\fireball"),
                    BlendState = BlendState.Additive,
                    SpawnRate = 0.005f,
                    AlphaFunc = p => p.InverseAgeFraction * 0.8f,
                    ScaleFunc = p => p.InverseAgeFraction * 0.7f,
                    RotationSpeed = 1f,
                    Emitter = () => new Particle
                    {
                        Position = new Vector3(projectile.Position.X, projectile.Position.Z, projectile.Position.Y),
                        LifeSpan = 0.2f,
                        Angle = random.Between(0, 6)
                    }
                };
                
                scene.Nodes.Add(new ProjectileModel()
                {
                    Projectile = projectile,
                    Model = ContentManager.Load<Model>(@"Models\Objects\" + projectile.ModelName),
                    Texture = ContentManager.Load<Texture2D>(@"Models\Objects\" + projectile.TextureName),
                    Nodes = { smokeTrail, fireBall }
                });
            }

            if (!isPaused)
            {
                float deltaTime = isBenchmarking ? 0.5f : (float)gameTime.ElapsedGameTime.TotalSeconds;
                var turn = battle.Run(deltaTime);
                turns.Add(turn);

                while (battle.GraphicEffects.Any())
                {
                    var graphicsEffect = battle.GraphicEffects.Dequeue();

                    if (graphicsEffect is BillboardDefinition)
                    {
                        var billboardDefinition = graphicsEffect as BillboardDefinition;

                        scene.Nodes.Add(new Billboard(ScreenManager.GraphicsDevice, ContentManager.Load<Effect>(@"Shaders\Billboard"))
                        {
                            Position = billboardDefinition.Position,
                            Texture = ContentManager.SafeLoad<Texture2D>(@"Sprites\" + billboardDefinition.TextureName, BlankTexture),
                            LifeTime = billboardDefinition.LifeTime,
                            Angle = billboardDefinition.Angle,

                            AngleFunc = billboardDefinition.AngleFunc.ToFunc(),
                            AlphaFunc = billboardDefinition.AlphaFunc.ToFunc(),
                            ScaleFunc = billboardDefinition.ScaleFunc.ToFunc(),
                        });
                    }

                    if (graphicsEffect is ParticleSystemDefinition)
                    {
                        var particleSystemDefinition = graphicsEffect as ParticleSystemDefinition;

                        scene.Nodes.Add(new ParticleSystem(ScreenManager.GraphicsDevice, ContentManager.Load<Effect>(@"Shaders\Particle"))
                        {
                            Position = new Vector3(particleSystemDefinition.Position.X, 1f, particleSystemDefinition.Position.Y),
                            Texture = ContentManager.SafeLoad<Texture2D>(@"Sprites\" + particleSystemDefinition.TextureName, BlankTexture),
                            SpawnRate = 0.0001f,
                            BlendState = BlendState.Additive,
                            LifeSpan = particleSystemDefinition.LifeSpan,

                            AngleFunc = p => particleSystemDefinition.AngleFunc.ToFunc()(p.AgeFraction),
                            AlphaFunc = p => particleSystemDefinition.AlphaFunc.ToFunc()(p.AgeFraction),
                            ScaleFunc = p => particleSystemDefinition.ScaleFunc.ToFunc()(p.AgeFraction),

                            Emitter = () => new Particle()
                            {
                                Position = new Vector3(particleSystemDefinition.Position.X, 1f, particleSystemDefinition.Position.Y),
                                Velocity = random.NextUnitVector2().Project(1f) * random.Between(1f, 4f),
                                LifeSpan = 1f,
                            }
                        });
                    }
                }
            }

            scrollingTextSystem.Filter = selectedActors.FirstOrDefault();

            UpdateAbilityButtons();
            UpdateOrderQueueButtons();
            //UpdateStrategyButtons();

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

            scene.Update(gameTime, isPaused);
        }

        private void IssueOrder(Actor actor, Order order)
        {
            if (isPaused)
                actor.Orders.Add(order);
            else
                actor.Orders.Insert(0, order);
        }

        private void FindMouseOverActor(MouseState mouseState, Vector2 mouseWorldLocation)
        {
            if (mouseState.X > 0 && mouseState.X < 150 && mouseState.Y > 0 && mouseState.Y < 50 * battle.Actors.Count)
                mouseOverActor = battle.Actors[mouseState.Y / 50];
            else
                mouseOverActor = null;

            foreach (var actor in battle.Actors.Where(x => x.IsAlive))
            {
                if ((actor.Position - mouseWorldLocation).Length() < actor.Diameter)
                    mouseOverActor = actor;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //DrawHelpers();

            scene.Draw();

            foreach (var actorModel in scene.Nodes.OfType<ActorModel>().OrderBy(a => a.Actor.Position.Y))
            {
                if (selectedActors.Any() && actorModel.Actor.IsAlive)
                {
                    var v = scene.Project(actorModel.Actor.Position);
                    DrawHealthBar((int)(v.X - 30), (int)(v.Y + 40), 60, 5, actorModel.Actor.HealthFraction, Color.GreenYellow);
                    //if (battle.Actors.Any(x => x != actorModel.Actor && x.Faction == Factions.Enemy && x.Targets.Any() && x.Targets.Peek() == actorModel.Actor))
                    //    SpriteBatch.Draw(ContentManager.Load<Texture2D>(@"Interface\aggro"), new Rectangle((int)(v.X + 52), (int)(v.Y - 110), 24, 24), Color.White, 0.01f);
                }
            }

            DrawThreatList();
            DrawAuras();

            DrawNamePlates();

            DrawCombatLog();

            if (dragStart.HasValue && dragEnd.HasValue)
            {
                var x1 = Math.Min(dragStart.Value.X, dragEnd.Value.X);
                var x2 = Math.Max(dragStart.Value.X, dragEnd.Value.X);
                var y1 = Math.Min(dragStart.Value.Y, dragEnd.Value.Y);
                var y2 = Math.Max(dragStart.Value.Y, dragEnd.Value.Y);
                SpriteBatch.Draw(BlankTexture, new Rectangle(x1, y1, x2 - x1, y2 - y1), Color.Green * 0.33f);
            }

            if (isTargetting != null && isTargetting.Ability.TargettingType == TargettingTypes.Location)
            {
                var position = scene.Unproject(Mouse.GetState());
                scene.DrawBillboard(new Vector3(position.X, 0.05f, position.Y), ContentManager.Load<Texture2D>(@"Interface\circlearea"), isTargetting.Ability.Area);
            }

            SpriteBatch.DrawString(Font, scene.CountNodes().ToString(), new Vector2(Width - 50, Height - Font.LineSpacing * 2), Color.White);
            SpriteBatch.DrawString(Font, fps.ToString("0"), new Vector2(Width - 50, Height - Font.LineSpacing), Color.White);
            fps = (float)((fps + (1000.0 / gameTime.ElapsedGameTime.TotalMilliseconds)) / 2.0);
        }

        private void DrawHelpers()
        {
            scene.DrawHelperBox(new Vector3(0, 0, 0), Color.Gray, ContentManager);
            scene.DrawHelperBox(new Vector3(1, 0, 0), Color.Red, ContentManager);
            scene.DrawHelperBox(new Vector3(0, 1, 0), Color.Green, ContentManager);
            scene.DrawHelperBox(new Vector3(0, 0, 1), Color.Blue, ContentManager);
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
                    var abilityTexture = ContentManager.SafeLoad<Texture2D>(@"Icons\" + ability.TextureName, defaultAbilityTexture);

                    abilityButtons[i].Content = new AbilityButton(abilityButtons[i], selectedActor, ability, abilityTexture, BlankTexture, kootenayFont);
                    abilityButtons[i].Tooltip = new AbilityTooltip(selectedActor, ability) { Font = kootenaySmallFont };
                }
            }
        }

        private void UpdateOrderQueueButtons()
        {
            orderQueueButtons.ForEach(button => { button.Content = null; button.Tooltip = null; });

            if (selectedActors.Any())
            {
                var selectedActor = selectedActors.First();
                for (int i = 0; i < selectedActor.Orders.Count; i++)
                {
                    var order = selectedActor.Orders.ElementAt(i);
                    var abilityTextureFileName = @"Icons\" + order.Ability.TextureName;
                    var abilityTexture = ContentManager.SafeLoad<Texture2D>(abilityTextureFileName, defaultAbilityTexture);
                    var targetTexture = order.TargetActor != null ? ContentManager.SafeLoad<Texture2D>(@"Models\Actors\" + order.TargetActor.TextureName + "_portrait") : ContentManager.SafeLoad<Texture2D>(@"Models\Actors\" + selectedActor.TextureName + "_portrait");

                    orderQueueButtons[i].Content = new OrderButton(orderQueueButtons[i], selectedActor, order, abilityTexture, BlankTexture, targetTexture, kootenayFont);
                    orderQueueButtons[i].Tooltip = new AbilityTooltip(selectedActor, order.Ability) { Font = kootenaySmallFont };
                }
            }
        }

        //private void UpdateStrategyButtons()
        //{
        //    targettingStrategyButtons.ForEach(button => { button.Content = null; button.Tooltip = null; });

        //    if (selectedActors.Any(x => x.PlayerControlled))
        //    {
        //        var selectedActor = selectedActors.First(x => x.PlayerControlled);
        //        var texture = ContentManager.Load<Texture2D>("Icons\\Ability_thunderbolt");
                
        //        var strategies = TargetingStrategy.All().Where(x => player.UnlockedTargetingStrategies.Contains(x.Value)).ToArray();
        //        for (int i = 0; i < strategies.Length; i++)
        //        {
        //            targettingStrategyButtons[i].Content = new TargetingStrategyButton(targettingStrategyButtons[i], selectedActor, strategies[i], texture);
        //            var smallFont = ContentManager.Load<SpriteFont>("Fonts\\kootenaySmall");
        //            var t = strategies[i].Name + "\n" + strategies[i].Description;
        //            var ts = smallFont.MeasureString(t);
        //            targettingStrategyButtons[i].Tooltip = new Border(t) { Font = smallFont, Width = ts.X + 20, Height = ts.Y + 20 };
        //        }
        //    }
        //}
        
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
                    actorText += " > " + actor.Targets.First().Name; // string.Join(", ", actor.Targets.Select(x => x.Name).ToArray());

                SpriteBatch.Draw(ContentManager.Load<Texture2D>(@"Models\Actors\" + actor.TextureName + "_portrait"), new Rectangle(0, i * 50, 50, 50), color);
                if (battle.Actors.Any(x => x != actor && x.Faction == Factions.Enemy && x.Targets.Any() && x.Targets.Peek() == actor))
                    SpriteBatch.Draw(ContentManager.Load<Texture2D>(@"Interface\aggro"), new Rectangle(0, i * 50, 24, 24), Color.White, 0.01f);
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
                    DrawHealthBar(50, 25 + i * 50 + 20, 100, 5, (actor.CastingProgress.Duration - actor.CastingProgress.Current) / actor.CastingProgress.Duration, Color.Goldenrod);
                    if (isFirstSelected)
                    {
                        var abilityTextureFileName = @"Icons\" + actor.CurrentOrder.Ability.TextureName;
                        var abilityTexture = ContentManager.SafeLoad<Texture2D>(abilityTextureFileName, defaultAbilityTexture);
                        SpriteBatch.Draw(abilityTexture, new Rectangle((int)Width / 2 - 25, (int)Height - 200, 50, 50), Color.White);
                        DrawHealthBar((int)Width / 2 - 100, (int)Height - 140, 200, 10, (actor.CastingProgress.Duration - actor.CastingProgress.Current) / actor.CastingProgress.Duration, Color.Goldenrod);
                    }
                }
                SpriteBatch.DrawString(kootenaySmallFont, actor.CurrentHealth.ToString("0") + "/" + actor.MaximumHealth.ToString("0"), new Vector2(55, 25 + i * 50), Color.White, ZIndex + 0.003f);
            }
        }

        private void DrawHealthBar(int x, int y, int width, int height, float fraction, Color color)
        {
            if (width > 0)
            {
                SpriteBatch.Draw(healthBarTexture, new Rectangle(x, y, width, height), Color.DarkGray, ZIndex + 0.001f);
                if (fraction > 0)
                    SpriteBatch.Draw(healthBarTexture, new Rectangle(x, y, (int)(fraction * width), height), color, ZIndex + 0.002f);
            }
        }

        private void pauseButton_Click(Button pauseButton)
        {
            isPaused = !isPaused;
            pauseButton.Content = isPaused ? "Unpause" : "Pause";
        }
    }
}
