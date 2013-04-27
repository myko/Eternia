using Eternia.Game;
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

            var grid = new Grid();
            grid.Width = Width;
            grid.Height = Height;
            grid.Rows.Add(GridSize.Fill());
            grid.Rows.Add(GridSize.Fill());
            grid.Columns.Add(GridSize.Fill());
            Controls.Add(grid);

            grid.Cells[0, 0].Add(new Label { Text = "You were defeated!" });

            var okButton = CreateButton("New Encounter", new Vector2(0, 0));
            okButton.Width = 200;
            okButton.Click += okButton_Click;
            grid.Cells[1, 0].Add(okButton);
        }

        void okButton_Click()
        {
            VictoryScreen.SaveActors(ScreenManager, player);

            ScreenManager.AddScreen(new TitleScreen(player));
            ScreenManager.RemoveScreen(this);
        }
    }
}
