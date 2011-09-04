using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Myko.Xna.Ui
{
    public enum GridSizeTypes
    { 
        Fill,
        Fixed
    }

    public class GridSize
    {
        public readonly GridSizeTypes type;
        public readonly float size;

        public GridSize(GridSizeTypes type, float size)
        {
            this.type = type;
            this.size = size;
        }

        public static GridSize Fill()
        {
            return new GridSize(GridSizeTypes.Fill, 1);
        }

        public static GridSize Fill(float factor)
        {
            return new GridSize(GridSizeTypes.Fill, factor);
        }

        public static GridSize Fixed(float size)
        {
            return new GridSize(GridSizeTypes.Fixed, size);
        }
    }


    public class Grid: Control
    {
        public class GridCellCollection
        {
            private readonly Grid grid;
            internal readonly Dictionary<int, Dictionary<int, ControlCollection>> cells;

            public ControlCollection this[int row, int column]
            {
                get 
                {
                    if (!cells.ContainsKey(row))
                        cells.Add(row, new Dictionary<int, ControlCollection>());

                    var columns = cells[row];
                    if (!columns.ContainsKey(column))
                        columns.Add(column, new ControlCollection(grid));

                    return columns[column]; 
                }
            }

            public GridCellCollection(Grid grid)
            {
                this.grid = grid;
                this.cells = new Dictionary<int, Dictionary<int, ControlCollection>>();
            }

            public IEnumerable<ControlCollection> All()
            {
                return cells.Values.SelectMany(x => x.Values);
            }

            public IEnumerable<Control> Controls(int row, int column)
            {
                return cells
                    .Where(x => x.Key == row)
                    .SelectMany(x => x.Value)
                    .Where(x => x.Key == column)
                    .SelectMany(x => x.Value);
            }
        }

        public List<GridSize> Rows { get; private set; }
        public List<GridSize> Columns { get; private set; }
        public GridCellCollection Cells { get; set; }

        public Grid()
        {
            Rows = new List<GridSize>();
            Columns = new List<GridSize>();
            Cells = new GridCellCollection(this);
        }

        public override void HandleInput(Vector2 position, GameTime gameTime)
        {
            DoControlsWithLayout(position, gameTime, (c, p) => c.HandleInput(p, gameTime));

            base.HandleInput(position, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var control in Cells.All().SelectMany(x => x))
            {
                control.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(Vector2 position, GameTime gameTime)
        {
            DoControlsWithLayout(position, gameTime, (c, p) => c.Draw(p, gameTime));

            base.Draw(position, gameTime);
        }

        private void DoControlsWithLayout(Vector2 position, GameTime gameTime, Action<Control, Vector2> action)
        {
            var fixedHeight = Rows.Where(x => x.type == GridSizeTypes.Fixed).Sum(x => x.size);
            var fillHeight = Height - fixedHeight;
            if (Rows.Count(x => x.type == GridSizeTypes.Fill) > 0)
                fillHeight /= Rows.Where(x => x.type == GridSizeTypes.Fill).Sum(x => x.size);

            var fixedWidth = Columns.Where(x => x.type == GridSizeTypes.Fixed).Sum(x => x.size);
            var fillWidth = Width - fixedWidth;
            if (Columns.Count(x => x.type == GridSizeTypes.Fill) > 0)
                fillWidth /= Columns.Where(x => x.type == GridSizeTypes.Fill).Sum(x => x.size);

            float fy = position.Y;
            for (int row = 0; row < Rows.Count; row++)
            {
                var rowHeight = Rows[row].type == GridSizeTypes.Fixed ? Rows[row].size : fillHeight * Rows[row].size;
                float fx = position.X;

                for (int column = 0; column < Columns.Count; column++)
                {
                    var columnWidth = Columns[column].type == GridSizeTypes.Fixed ? Columns[column].size : fillWidth * Columns[column].size;

                    foreach (var control in Cells.Controls(row, column))
                    {
                        var controlPosition = new Vector2(
                            (float)Math.Round(fx + columnWidth / 2f - control.Width / 2f), 
                            (float)Math.Round(fy + rowHeight / 2f - control.Height / 2f));
                        action(control, controlPosition);
                    }

                    fx += columnWidth;
                }

                fy += rowHeight;
            }
        }
    }
}
