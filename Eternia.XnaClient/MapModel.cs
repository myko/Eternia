using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EterniaGame;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Eternia.XnaClient
{
    public struct MapVertex
    {
        public Vector3 Position;
        public Vector2 TextureCoordinate;
        public Vector4 TextureBlends;

        public MapVertex(Vector3 position, Vector2 textureCoordinate, Vector4 textureBlends)
        {
            Position = position;
            TextureCoordinate = textureCoordinate;
            TextureBlends = textureBlends;
        }
    }

    public class MapChunk
    {
        public List<MapVertex> Vertices { get; set; }
        public string Texture0 { get; set; }
        public string Texture1 { get; set; }
        public string Texture2 { get; set; }
        public string Texture3 { get; set; }

        public MapChunk()
        {
            Vertices = new List<MapVertex>();
        }
    }

    public class MapModel
    {
        private Map map;
        private float scale = 2f;
        private List<MapChunk> chunks = new List<MapChunk>();

        public MapModel(Map map)
        {
            this.map = map;

            BuildChunks();
            OptimizeChunks();
        }

        public void BuildChunks()
        {
            chunks.Clear();
            MapChunk chunk;

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    var index = x + y * map.Width;
                    if (index >= map.Tiles.Length)
                        continue;
                    var tile = map.Tiles[index];
                    if (tile == null)
                        continue;

                    float fx1 = scale * (x + 0);
                    float fy1 = scale * (y + 0);

                    float fx2 = scale * (x + 0.5f);
                    float fy2 = scale * (y + 0.5f);

                    float fx3 = scale * (x + 1);
                    float fy3 = scale * (y + 1);

                    float fz1 = scale * tile.Height;

                    //  ---------
                    // |\ A | B /|
                    // | \  |  / |
                    // | H\ | /  |
                    // |   \|/ C |
                    // |----+----|
                    // |G  /|\ D |
                    // |  / | \  |
                    // | /F | E\ |
                    // |/   |   \|
                    //  ---------

                    chunk = new MapChunk();
                    
                    // H
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz1, fy2), new Vector2(0.0f, 0.5f), new Vector4(0.5f, 0.5f, 0, 0)));
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz1, fy3), new Vector2(0.0f, 0.0f), new Vector4(0.25f, 0.25f, 0.25f, 0.25f)));
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz1, fy2), new Vector2(0.5f, 0.5f), new Vector4(1, 0, 0, 0)));

                    // A
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz1, fy3), new Vector2(0.0f, 0.0f), new Vector4(0.25f, 0.25f, 0.25f, 0.25f)));
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz1, fy3), new Vector2(0.5f, 0.0f), new Vector4(0.5f, 0, 0, 0.5f)));
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz1, fy2), new Vector2(0.5f, 0.5f), new Vector4(1, 0, 0, 0)));

                    chunk.Texture0 = tile.FloorTexture;
                    chunk.Texture1 = tile.WallTexture;
                    chunk.Texture2 = tile.WallTexture;
                    chunk.Texture3 = tile.WallTexture;
                    if (tile.Height == tile.Left.Height)
                        chunk.Texture1 = tile.Left.FloorTexture;
                    if (tile.Height == tile.Above.Left.Height)
                        chunk.Texture2 = tile.Above.Left.FloorTexture;
                    if (tile.Height == tile.Above.Height)
                        chunk.Texture3 = tile.Above.FloorTexture;

                    chunks.Add(chunk);

                    chunk = new MapChunk();

                    // B
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz1, fy3), new Vector2(0.5f, 0.0f), new Vector4(0.5f, 0.5f, 0, 0)));
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz1, fy3), new Vector2(1.0f, 0.0f), new Vector4(0.25f, 0.25f, 0.25f, 0.25f)));
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz1, fy2), new Vector2(0.5f, 0.5f), new Vector4(1, 0, 0, 0)));
                                                                    
                    // C                                            
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz1, fy3), new Vector2(1.0f, 0.0f), new Vector4(0.25f, 0.25f, 0.25f, 0.25f)));
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz1, fy2), new Vector2(1.0f, 0.5f), new Vector4(0.5f, 0, 0, 0.5f)));
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz1, fy2), new Vector2(0.5f, 0.5f), new Vector4(1, 0, 0, 0)));

                    chunk.Texture0 = tile.FloorTexture;
                    chunk.Texture1 = tile.WallTexture;
                    chunk.Texture2 = tile.WallTexture;
                    chunk.Texture3 = tile.WallTexture;
                    if (tile.Height == tile.Above.Height)
                        chunk.Texture1 = tile.Above.FloorTexture;
                    if (tile.Height == tile.Above.Right.Height)
                        chunk.Texture2 = tile.Above.Right.FloorTexture;
                    if (tile.Height == tile.Right.Height)
                        chunk.Texture3 = tile.Right.FloorTexture;

                    chunks.Add(chunk);

                    chunk = new MapChunk();

                    // D
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz1, fy2), new Vector2(1.0f, 0.5f), new Vector4(0.5f, 0.5f, 0, 0)));
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz1, fy1), new Vector2(1.0f, 1.0f), new Vector4(0.25f, 0.25f, 0.25f, 0.25f)));
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz1, fy2), new Vector2(0.5f, 0.5f), new Vector4(1, 0, 0, 0)));
                                                                    
                    // E                                            
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz1, fy1), new Vector2(1.0f, 1.0f), new Vector4(0.25f, 0.25f, 0.25f, 0.25f)));
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz1, fy1), new Vector2(0.5f, 1.0f), new Vector4(0.5f, 0, 0, 0.5f)));
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz1, fy2), new Vector2(0.5f, 0.5f), new Vector4(1, 0, 0, 0)));

                    chunk.Texture0 = tile.FloorTexture;
                    chunk.Texture1 = tile.WallTexture;
                    chunk.Texture2 = tile.WallTexture;
                    chunk.Texture3 = tile.WallTexture;
                    if (tile.Height == tile.Right.Height)
                        chunk.Texture1 = tile.Right.FloorTexture;
                    if (tile.Height == tile.Right.Below.Height)
                        chunk.Texture2 = tile.Right.Below.FloorTexture;
                    if (tile.Height == tile.Below.Height)
                        chunk.Texture3 = tile.Below.FloorTexture;

                    chunks.Add(chunk);

                    chunk = new MapChunk();

                    // F
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz1, fy1), new Vector2(0.5f, 1.0f), new Vector4(0.5f, 0.5f, 0, 0)));
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz1, fy1), new Vector2(0.0f, 1.0f), new Vector4(0.25f, 0.25f, 0.25f, 0.25f)));
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz1, fy2), new Vector2(0.5f, 0.5f), new Vector4(1, 0, 0, 0)));
                                                                    
                    // G                                            
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz1, fy1), new Vector2(0.0f, 1.0f), new Vector4(0.25f, 0.25f, 0.25f, 0.25f)));
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz1, fy2), new Vector2(0.0f, 0.5f), new Vector4(0.5f, 0, 0, 0.5f)));
                    chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz1, fy2), new Vector2(0.5f, 0.5f), new Vector4(1, 0, 0, 0)));

                    chunk.Texture0 = tile.FloorTexture;
                    chunk.Texture1 = tile.WallTexture;
                    chunk.Texture2 = tile.WallTexture;
                    chunk.Texture3 = tile.WallTexture;
                    if (tile.Height == tile.Below.Height)
                        chunk.Texture1 = tile.Below.FloorTexture;
                    if (tile.Height == tile.Below.Left.Height)
                        chunk.Texture2 = tile.Below.Left.FloorTexture;
                    if (tile.Height == tile.Left.Height)
                        chunk.Texture3 = tile.Left.FloorTexture;

                    chunks.Add(chunk);
                    
                    if (tile.Height > tile.Below.Height)
                    {
                        chunk = new MapChunk();

                        float fz2 = scale * (tile.Height - 0.25f);
                        float fz3 = scale * (tile.Below.Height + 0.25f);
                        float fz4 = scale * (tile.Below.Height);
                        float v = tile.Height - tile.Below.Height - 1;

                        // TOP
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz2, fy1), new Vector2(0.0f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz1, fy1), new Vector2(0.0f, 0.00f), new Vector4(0.33f, 0.33f, 0, 0.33f)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz2, fy1), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz1, fy1), new Vector2(0.0f, 0.00f), new Vector4(0.33f, 0.33f, 0, 0.33f)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz1, fy1), new Vector2(0.5f, 0.00f), new Vector4(0.5f, 0.5f, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz2, fy1), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                                                                        
                        // MIDDLE                                       
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz3, fy1), new Vector2(0.0f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz2, fy1), new Vector2(0.0f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz3, fy1), new Vector2(0.5f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz2, fy1), new Vector2(0.0f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz2, fy1), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz3, fy1), new Vector2(0.5f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                                                                        
                        // BOTTOM                                       
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz3, fy1), new Vector2(0.0f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz3, fy1), new Vector2(0.5f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz4, fy1), new Vector2(0.0f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz3, fy1), new Vector2(0.5f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz4, fy1), new Vector2(0.5f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz4, fy1), new Vector2(0.0f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));

                        chunk.Texture0 = tile.WallTexture;
                        chunk.Texture1 = tile.FloorTexture;
                        chunk.Texture2 = tile.Below.FloorTexture;

                        if (tile.Height == tile.Left.Height)
                            chunk.Texture3 = tile.Left.FloorTexture;
                        else
                            chunk.Texture3 = tile.WallTexture;

                        chunks.Add(chunk);

                        chunk = new MapChunk();

                        // TOP
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz1, fy1), new Vector2(0.5f, 0.00f), new Vector4(0.5f, 0.5f, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz1, fy1), new Vector2(1.0f, 0.00f), new Vector4(0.33f, 0.33f, 0, 0.33f)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz2, fy1), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz1, fy1), new Vector2(1.0f, 0.00f), new Vector4(0.33f, 0.33f, 0, 0.33f)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz2, fy1), new Vector2(1.0f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz2, fy1), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                                                                        
                        // MIDDLE                                       
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz3, fy1), new Vector2(0.5f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz2, fy1), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz3, fy1), new Vector2(1.0f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz2, fy1), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz2, fy1), new Vector2(1.0f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz3, fy1), new Vector2(1.0f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                                                                        
                        // BOTTOM                                       
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz4, fy1), new Vector2(0.5f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz3, fy1), new Vector2(0.5f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz4, fy1), new Vector2(1.0f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz3, fy1), new Vector2(0.5f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz3, fy1), new Vector2(1.0f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz4, fy1), new Vector2(1.0f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));
                        
                        chunk.Texture0 = tile.WallTexture;
                        chunk.Texture1 = tile.FloorTexture;
                        chunk.Texture2 = tile.Below.FloorTexture;

                        if (tile.Height == tile.Right.Height)
                            chunk.Texture3 = tile.Right.FloorTexture;
                        else
                            chunk.Texture3 = tile.WallTexture;

                        chunks.Add(chunk);
                    }

                    if (tile.Height > tile.Left.Height)
                    {
                        chunk = new MapChunk();

                        float fz2 = scale * (tile.Height - 0.25f);
                        float fz3 = scale * (tile.Left.Height + 0.25f);
                        float fz4 = scale * (tile.Left.Height);
                        float v = tile.Height - tile.Left.Height - 1;

                        // TOP
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz2, fy3), new Vector2(0.0f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz1, fy3), new Vector2(0.0f, 0.00f), new Vector4(0.33f, 0.33f, 0, 0.33f)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz2, fy2), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz1, fy3), new Vector2(0.0f, 0.00f), new Vector4(0.33f, 0.33f, 0, 0.33f)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz1, fy2), new Vector2(0.5f, 0.00f), new Vector4(0.5f, 0.5f, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz2, fy2), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                                                                        
                        // MIDDLE                                       
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz3, fy3), new Vector2(0.0f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz2, fy3), new Vector2(0.0f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz3, fy2), new Vector2(0.5f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz2, fy3), new Vector2(0.0f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz2, fy2), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz3, fy2), new Vector2(0.5f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                                                                        
                        // BOTTOM                                       
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz3, fy3), new Vector2(0.0f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz3, fy2), new Vector2(0.5f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz4, fy3), new Vector2(0.0f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz3, fy2), new Vector2(0.5f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz4, fy2), new Vector2(0.5f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz4, fy3), new Vector2(0.0f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));

                        chunk.Texture0 = tile.WallTexture;
                        chunk.Texture1 = tile.FloorTexture;
                        chunk.Texture2 = tile.Left.FloorTexture;

                        if (tile.Height == tile.Above.Height)
                            chunk.Texture3 = tile.Above.FloorTexture;
                        else
                            chunk.Texture3 = tile.WallTexture;

                        chunks.Add(chunk);

                        chunk = new MapChunk();

                        // TOP
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz1, fy2), new Vector2(0.5f, 0.00f), new Vector4(0.5f, 0.5f, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz1, fy1), new Vector2(1.0f, 0.00f), new Vector4(0.33f, 0.33f, 0, 0.33f)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz2, fy2), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz1, fy1), new Vector2(1.0f, 0.00f), new Vector4(0.33f, 0.33f, 0, 0.33f)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz2, fy1), new Vector2(1.0f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz2, fy2), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                                                                        
                        // MIDDLE                                       
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz3, fy2), new Vector2(0.5f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz2, fy2), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz3, fy1), new Vector2(1.0f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz2, fy2), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz2, fy1), new Vector2(1.0f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz3, fy1), new Vector2(1.0f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                                                                        
                        // BOTTOM                                       
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz4, fy2), new Vector2(0.5f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz3, fy2), new Vector2(0.5f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz4, fy1), new Vector2(1.0f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz3, fy2), new Vector2(0.5f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz3, fy1), new Vector2(1.0f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz4, fy1), new Vector2(1.0f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));
                        
                        chunk.Texture0 = tile.WallTexture;
                        chunk.Texture1 = tile.FloorTexture;
                        chunk.Texture2 = tile.Left.FloorTexture;

                        if (tile.Height == tile.Right.Height)
                            chunk.Texture3 = tile.Below.FloorTexture;
                        else
                            chunk.Texture3 = tile.WallTexture;

                        chunks.Add(chunk);
                    }

                    if (tile.Height > tile.Above.Height)
                    {
                        float fz2 = scale * (tile.Height - 0.25f);
                        float fz3 = scale * (tile.Above.Height + 0.25f);
                        float fz4 = scale * (tile.Above.Height);
                        float v = tile.Height - tile.Above.Height - 1;

                        chunk = new MapChunk();

                        // TOP
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz2, fy3), new Vector2(0.0f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz1, fy3), new Vector2(0.0f, 0.00f), new Vector4(0.33f, 0.33f, 0, 0.33f)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz2, fy3), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz1, fy3), new Vector2(0.0f, 0.00f), new Vector4(0.33f, 0.33f, 0, 0.33f)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz1, fy3), new Vector2(0.5f, 0.00f), new Vector4(0.5f, 0.5f, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz2, fy3), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                                                                        
                        // MIDDLE                                       
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz3, fy3), new Vector2(0.0f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz2, fy3), new Vector2(0.0f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz3, fy3), new Vector2(0.5f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz2, fy3), new Vector2(0.0f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz2, fy3), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz3, fy3), new Vector2(0.5f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                                                                        
                        // BOTTOM                                       
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz3, fy3), new Vector2(0.0f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz3, fy3), new Vector2(0.5f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz4, fy3), new Vector2(0.0f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz3, fy3), new Vector2(0.5f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx1, fz4, fy3), new Vector2(0.5f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz4, fy3), new Vector2(0.0f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));

                        chunk.Texture0 = tile.WallTexture;
                        chunk.Texture1 = tile.FloorTexture;
                        chunk.Texture2 = tile.Above.FloorTexture;

                        if (tile.Height == tile.Right.Height)
                            chunk.Texture3 = tile.Right.FloorTexture;
                        else
                            chunk.Texture3 = tile.WallTexture;

                        chunks.Add(chunk);

                        chunk = new MapChunk();

                        // TOP
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz1, fy3), new Vector2(0.5f, 0.00f), new Vector4(0.5f, 0.5f, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz1, fy3), new Vector2(1.0f, 0.00f), new Vector4(0.33f, 0.33f, 0, 0.33f)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz2, fy3), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz1, fy3), new Vector2(1.0f, 0.00f), new Vector4(0.33f, 0.33f, 0, 0.33f)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz2, fy3), new Vector2(1.0f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz2, fy3), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                                                                        
                        // MIDDLE                                       
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz3, fy3), new Vector2(0.5f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz2, fy3), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz3, fy3), new Vector2(1.0f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz2, fy3), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz2, fy3), new Vector2(1.0f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz3, fy3), new Vector2(1.0f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                                                                        
                        // BOTTOM                                       
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz4, fy3), new Vector2(0.5f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz3, fy3), new Vector2(0.5f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz4, fy3), new Vector2(1.0f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz3, fy3), new Vector2(0.5f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz3, fy3), new Vector2(1.0f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx2, fz4, fy3), new Vector2(1.0f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));
                        
                        chunk.Texture0 = tile.WallTexture;
                        chunk.Texture1 = tile.FloorTexture;
                        chunk.Texture2 = tile.Above.FloorTexture;

                        if (tile.Height == tile.Left.Height)
                            chunk.Texture3 = tile.Left.FloorTexture;
                        else
                            chunk.Texture3 = tile.WallTexture;

                        chunks.Add(chunk);
                    }

                    if (tile.Height > tile.Right.Height)
                    {
                        float fz2 = scale * (tile.Height - 0.25f);
                        float fz3 = scale * (tile.Right.Height + 0.25f);
                        float fz4 = scale * (tile.Right.Height);
                        float v = tile.Height - tile.Right.Height - 1;

                        chunk = new MapChunk();

                        // TOP
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz2, fy2), new Vector2(0.0f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz1, fy2), new Vector2(0.0f, 0.00f), new Vector4(0.33f, 0.33f, 0, 0.33f)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz2, fy3), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz1, fy2), new Vector2(0.0f, 0.00f), new Vector4(0.33f, 0.33f, 0, 0.33f)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz1, fy3), new Vector2(0.5f, 0.00f), new Vector4(0.5f, 0.5f, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz2, fy3), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                                                                        
                        // MIDDLE                                       
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz3, fy2), new Vector2(0.0f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz2, fy2), new Vector2(0.0f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz3, fy3), new Vector2(0.5f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz2, fy2), new Vector2(0.0f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz2, fy3), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz3, fy3), new Vector2(0.5f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                                                                        
                        // BOTTOM                                       
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz3, fy2), new Vector2(0.0f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz3, fy3), new Vector2(0.5f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz4, fy2), new Vector2(0.0f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz3, fy3), new Vector2(0.5f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz4, fy3), new Vector2(0.5f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz4, fy2), new Vector2(0.0f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));

                        chunk.Texture0 = tile.WallTexture;
                        chunk.Texture1 = tile.FloorTexture;
                        chunk.Texture2 = tile.Right.FloorTexture;

                        if (tile.Height == tile.Below.Height)
                            chunk.Texture3 = tile.Below.FloorTexture;
                        else
                            chunk.Texture3 = tile.WallTexture;

                        chunks.Add(chunk);

                        chunk = new MapChunk();

                        // TOP
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz1, fy1), new Vector2(0.5f, 0.00f), new Vector4(0.5f, 0.5f, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz1, fy2), new Vector2(1.0f, 0.00f), new Vector4(0.33f, 0.33f, 0, 0.33f)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz2, fy1), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz1, fy2), new Vector2(1.0f, 0.00f), new Vector4(0.33f, 0.33f, 0, 0.33f)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz2, fy2), new Vector2(1.0f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz2, fy1), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                                                                        
                        // MIDDLE                                       
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz3, fy1), new Vector2(0.5f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz2, fy1), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz3, fy2), new Vector2(1.0f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz2, fy1), new Vector2(0.5f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz2, fy2), new Vector2(1.0f, 0.25f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz3, fy2), new Vector2(1.0f, 0.75f + v), new Vector4(1, 0, 0, 0)));
                                                                        
                        // BOTTOM                                       
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz4, fy1), new Vector2(0.5f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz3, fy1), new Vector2(0.5f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz4, fy2), new Vector2(1.0f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));
                                                                        
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz3, fy1), new Vector2(0.5f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz3, fy2), new Vector2(1.0f, 0.75f), new Vector4(1, 0, 0, 0)));
                        chunk.Vertices.Add(new MapVertex(new Vector3(fx3, fz4, fy2), new Vector2(1.0f, 1.00f), new Vector4(0.5f, 0, 0.5f, 0)));

                        chunk.Texture0 = tile.WallTexture;
                        chunk.Texture1 = tile.FloorTexture;
                        chunk.Texture2 = tile.Right.FloorTexture;

                        if (tile.Height == tile.Right.Height)
                            chunk.Texture3 = tile.Above.FloorTexture;
                        else
                            chunk.Texture3 = tile.WallTexture;

                        chunks.Add(chunk);
                    }
                }
            }
        }
        
        public void OptimizeChunks()
        {
            var textures = chunks.SelectMany(x => new[] { x.Texture0, x.Texture1, x.Texture2, x.Texture3 }).Distinct().ToList();
            var textureCombos = 0;

            for (int i = 0; i < textures.Count; i++)
            {
                for (int j = i; j < textures.Count; j++)
                {
                    for (int k = j; k < textures.Count; k++)
                    {
                        for (int l = k; l < textures.Count; l++)
                        {
                            var similarChunks = 0;
                            var textureCombo = new[] { textures[i], textures[j], textures[k], textures[l] }.ToArray();
                            
                            foreach (var chunk in chunks)
                            {
                                var chunkCombo = new[] { chunk.Texture0, chunk.Texture1, chunk.Texture2, chunk.Texture3 };
                                var chunkTextures = chunkCombo.Distinct();

                                var valid = true;
                                foreach (var chunkTexture in chunkTextures)
                                {
                                    if (chunkCombo.Count(x => x == chunkTexture) != textureCombo.Count(x => x == chunkTexture))
                                        valid = false;
                                }
                                if (valid)
                                    similarChunks++;
                            }

                            //System.Diagnostics.Debug.WriteLine(string.Join(", ", textureCombo) + ": " + similarChunks + " chunks.");
                            if (similarChunks > 0)
                                textureCombos++;
                        }
                    }
                }
            }
            System.Diagnostics.Debug.WriteLine(textureCombos + " texture combinations.");

            System.Diagnostics.Debug.WriteLine("Optimizing " + chunks.Count + " chunks (" + chunks.Average(x => x.Vertices.Count).ToString("0") + " avg verts.)");

            var newChunks = new List<MapChunk>();

            while (chunks.Any())
            {
                var chunk = chunks.First();

                newChunks.Add(chunk);
                chunks.Remove(chunk);

                var identicalChunks = chunks
                    .Where(x => x.Texture0 == chunk.Texture0 && x.Texture1 == chunk.Texture1 && x.Texture2 == chunk.Texture2 && x.Texture3 == chunk.Texture3)
                    .ToList();

                foreach (var otherChunk in identicalChunks)
                {
                    chunk.Vertices.AddRange(otherChunk.Vertices);
                    chunks.Remove(otherChunk);
                }

                MergeSimilarChunks(chunk,
                    chunk.Texture1, chunk.Texture0, chunk.Texture2, chunk.Texture3,
                    x => new Vector4(x.Y, x.X, x.Z, x.W));

                MergeSimilarChunks(chunk,
                    chunk.Texture2, chunk.Texture1, chunk.Texture0, chunk.Texture3,
                    x => new Vector4(x.Z, x.Y, x.X, x.W));

                MergeSimilarChunks(chunk,
                    chunk.Texture3, chunk.Texture1, chunk.Texture2, chunk.Texture0,
                    x => new Vector4(x.W, x.Y, x.Z, x.X));

                MergeSimilarChunks(chunk,
                    chunk.Texture1, chunk.Texture0, chunk.Texture3, chunk.Texture2,
                    x => new Vector4(x.Y, x.X, x.W, x.Z));

                MergeSimilarChunks(chunk,
                    chunk.Texture0, chunk.Texture1, chunk.Texture3, chunk.Texture2,
                    x => new Vector4(x.X, x.Y, x.W, x.Z));

                MergeSimilarChunks(chunk,
                    chunk.Texture0, chunk.Texture2, chunk.Texture1, chunk.Texture3,
                    x => new Vector4(x.X, x.Z, x.Y, x.W));
            }

            chunks = newChunks;

            System.Diagnostics.Debug.WriteLine(newChunks.Count + " total chunks (" + newChunks.Average(x => x.Vertices.Count).ToString("0") + " avg verts.)");
        }

        private void MergeSimilarChunks(MapChunk chunk, string texture0, string texture1, string texture2, string texture3, Func<Vector4, Vector4> swizzle)
        {
            var similarChunks = chunks
                .Where(x => x.Texture0 == texture0 && x.Texture1 == texture1 && x.Texture2 == texture2 && x.Texture3 == texture3)
                .ToList();

            //if (similarChunks.Count > 0)
            //    System.Diagnostics.Debug.WriteLine("Found " + similarChunks.Count + " similar chunks.");

            foreach (var otherChunk in similarChunks)
            {
                for (int i = 0; i < otherChunk.Vertices.Count; i++)
                {
                    var vertex = otherChunk.Vertices[i];
                    chunk.Vertices.Add(new MapVertex
                    {
                        Position = vertex.Position,
                        TextureCoordinate = vertex.TextureCoordinate,
                        TextureBlends = swizzle(vertex.TextureBlends)
                    });
                }
                chunks.Remove(otherChunk);
            }
        }

        public void Draw(GraphicsDevice device, Matrix view, Matrix projection, Effect mapEffect, ContentManager contentManager)
        {
            var vertexElements = new VertexElement[] 
                { 
                    new VertexElement(0, 0, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Position, 0),
                    new VertexElement(0, 12, VertexElementFormat.Vector2, VertexElementMethod.Default, VertexElementUsage.TextureCoordinate, 0),
                    new VertexElement(0, 20, VertexElementFormat.Vector4, VertexElementMethod.Default, VertexElementUsage.Color, 0),
                    new VertexElement(0, 36, VertexElementFormat.Single, VertexElementMethod.Default, VertexElementUsage.Color, 1),
                };

            device.VertexDeclaration = new VertexDeclaration(device, vertexElements);

            mapEffect.Parameters["View"].SetValue(view);
            mapEffect.Parameters["Projection"].SetValue(projection);
            mapEffect.Parameters["World"].SetValue(Matrix.CreateTranslation(map.Width / 2f * -scale, map.Height / 2f * -scale, 0));

            foreach (var chunk in chunks)
            {
                mapEffect.Parameters["Texture0"].SetValue(contentManager.Load<Texture2D>(chunk.Texture0));
                mapEffect.Parameters["Texture1"].SetValue(contentManager.Load<Texture2D>(chunk.Texture1));
                mapEffect.Parameters["Texture2"].SetValue(contentManager.Load<Texture2D>(chunk.Texture2));
                mapEffect.Parameters["Texture3"].SetValue(contentManager.Load<Texture2D>(chunk.Texture3));

                DrawPrimitives(device, mapEffect, chunk.Vertices);
            }
        }

        private static void DrawPrimitives(GraphicsDevice device, Effect mapEffect, List<MapVertex> vertices)
        {
            mapEffect.Begin();

            mapEffect.CurrentTechnique.Passes[0].Begin();
            device.DrawUserPrimitives<MapVertex>(PrimitiveType.TriangleList, vertices.ToArray(), 0, vertices.Count / 3);
            mapEffect.CurrentTechnique.Passes[0].End();

            mapEffect.End();
        }
    }
}
