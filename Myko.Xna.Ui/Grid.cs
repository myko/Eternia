using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Myko.Xna.Ui
{
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

        public List<Size> Rows { get; private set; }
        public List<Size> Columns { get; private set; }
        public GridCellCollection Cells { get; set; }

        public Grid()
        {
            Rows = new List<Size>();
            Columns = new List<Size>();
            Cells = new GridCellCollection(this);
        }

        public override void HandleInput(Vector2 position, GameTime gameTime)
        {
            DoControlsWithLayout(position, gameTime, (c, p) => c.HandleInput(c.Position + p, gameTime));

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
            DoControlsWithLayout(position, gameTime, (c, p) => c.Draw(c.Position + p, gameTime));

            base.Draw(position, gameTime);
        }

        private void DoControlsWithLayout(Vector2 position, GameTime gameTime, Action<Control, Vector2> action)
        {
            var fixedHeight = Rows.Where(x => x.Type == SizeTypes.Fixed).Sum(x => x.Value);
            var fillHeight = ActualHeight - fixedHeight;
            if (Rows.Count(x => x.Type == SizeTypes.Fill) > 0)
                fillHeight /= Rows.Where(x => x.Type == SizeTypes.Fill).Sum(x => x.Value);

            var fixedWidth = Columns.Where(x => x.Type == SizeTypes.Fixed).Sum(x => x.Value);
            var fillWidth = ActualWidth - fixedWidth;
            if (Columns.Count(x => x.Type == SizeTypes.Fill) > 0)
                fillWidth /= Columns.Where(x => x.Type == SizeTypes.Fill).Sum(x => x.Value);

            float fy = position.Y;
            for (int row = 0; row < Rows.Count; row++)
            {
                var rowHeight = Rows[row].Type == SizeTypes.Fixed ? Rows[row].Value : fillHeight * Rows[row].Value;
                float fx = position.X;

                for (int column = 0; column < Columns.Count; column++)
                {
                    var columnWidth = Columns[column].Type == SizeTypes.Fixed ? Columns[column].Value : fillWidth * Columns[column].Value;

                    foreach (var control in Cells.Controls(row, column))
                    {
                        var controlPosition = new Vector2(
                            (float)Math.Round(fx + columnWidth / 2f - control.ActualWidth / 2f),
                            (float)Math.Round(fy + rowHeight / 2f - control.ActualHeight / 2f));
                        action(control, controlPosition);
                    }

                    fx += columnWidth;
                }

                fy += rowHeight;
            }
        }
    }
}
