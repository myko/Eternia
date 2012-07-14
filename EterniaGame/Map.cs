using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace EterniaGame
{
    public class MapTile
    {
        public int Height { get; set; }
        public string FloorTexture { get; set; }
        public string WallTexture { get; set; }

        [ContentSerializerIgnore]
        public MapTile Left { get; set; }
        [ContentSerializerIgnore]
        public MapTile Right { get; set; }
        [ContentSerializerIgnore]
        public MapTile Above { get; set; }
        [ContentSerializerIgnore]
        public MapTile Below { get; set; }

        public MapTile()
        {
            Left = Right = Above = Below = this;
        }
    }

    public class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public MapTile[] Tiles { get; set; }

        public Map()
        {
        }

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            Tiles = new MapTile[width * height];

            Randomizer r = new Randomizer();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tiles[x + y * width] = new MapTile();
                    Tiles[x + y * width].Height = (x == 0 || y == 0 || x == width - 1 || y == height - 1) ? 4 : 0;
                    Tiles[x + y * width].FloorTexture = r.From(@"MapTiles\grass1", @"MapTiles\grass2", @"MapTiles\dirt1", @"MapTiles\dirt2", @"MapTiles\dirt3", @"MapTiles\sand1", @"MapTiles\paving1");
                    Tiles[x + y * width].WallTexture = r.From(@"MapTiles\rock1");
                }
            }

            UpdateTileReferences();
        }

        public void UpdateTileReferences()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var index = x + y * Width;
                    if (index >= Tiles.Length)
                        continue;
                    var tile = Tiles[index];
                    if (tile == null)
                        continue;

                    if (x > 0)
                        tile.Left = Tiles[x - 1 + y * Width];

                    if (y > 0)
                        tile.Below = Tiles[x + (y - 1) * Width];

                    if (x < Width - 1)
                        tile.Right = Tiles[x + 1 + y * Width];

                    if (y < Height - 1)
                        tile.Above = Tiles[x + (y + 1) * Width];
                }
            }
        }
    }
}
