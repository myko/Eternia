using EterniaGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.Ui;

namespace EterniaXna.Screens
{
    public class DefeatScreen: MenuScreen
    {
        private readonly Player player;
        private readonly Battle battle;

        public DefeatScreen(Player player, Battle battle)
        {
            this.player = player;
            this.battle = battle;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            var okButton = CreateButton("New Encounter", new Vector2(Width / 2 - 100, Height - 300));
            okButton.Width = 200;
            okButton.Click += okButton_Click;
            Controls.Add(okButton);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.DrawString(Font, "You were defeated!", new Vector2(Width/2 - 100, 400), Color.White, 0.2f);
        }

        void okButton_Click(object sender, System.EventArgs e)
        {
            VictoryScreen.SaveActors(ScreenManager, player);

            ScreenManager.AddScreen(new SelectEncounterScreen(player));
            ScreenManager.RemoveScreen(this);
        }
    }
}
