using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibNoise.Generator;
using LibNoise.Utilities;
using System.Drawing;

namespace FantasyScape {
	class HeightMap {
		private static Dictionary<Vector2i, Bitmap> Hashmap = new Dictionary<Vector2i, Bitmap>();

		private static Perlin perlinModule = new Perlin();
		private static NoiseMap heightMap = new NoiseMap();
		private static NoiseMapBuilderPlane heightMapBuilder = new NoiseMapBuilderPlane();

		static RendererImage renderer = new RendererImage();

		static HeightMap() {
			renderer.ClearGradient();
			int min = 80;
			int max = 120;
			renderer.AddGradientPoint(-1, Color.FromArgb(255, min, min, min));
			renderer.AddGradientPoint(1, Color.FromArgb(255, max, max, max));
			heightMapBuilder.SetSourceModule(perlinModule);
			heightMapBuilder.SetDestNoiseMap(heightMap);
			heightMapBuilder.SetDestSize(Chunk.Size, Chunk.Size);
			renderer.SetSourceNoiseMap(heightMap);
		}

		static double Scale = Chunk.Size / 160.0;
		public static Bitmap GetHeightmap(int chunkx, int chunky) {
			Vector2i location = new Vector2i(chunkx, chunky);
			if (!Hashmap.ContainsKey(location)) {
				heightMapBuilder.SetBounds((chunkx) * Scale, (chunkx + 1) * Scale, (chunky) * Scale, (chunky + 1.0) * Scale);
				heightMapBuilder.Build();
				Hashmap[location] = renderer.Render();
			}
			return Hashmap[location];
		}
	}
}
