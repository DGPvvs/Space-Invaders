namespace Space_Invaders.Classes
{
	using System.Collections.Generic;
	using System.Numerics;

	public class Obstacle
	{
		private Vector2 position;
		private List<Block> blocks;
		private static List<List<int>> grid = new List<List<int>>()
			{
				new List<int>() { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
				new List<int>() { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0},
				new List<int>() { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0},
				new List<int>() { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
				new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
				new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
				new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
				new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
				new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
				new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
				new List<int>() { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
				new List<int>() { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1},
				new List<int>() { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1}
			};

		public Obstacle(Vector2 position)
		{
			this.Position = position;
			this.Blocks = new List<Block>();			

			for (int row = 0; row < Obstacle.Grid.Count; row++)
			{
				for (int col = 0; col < Obstacle.Grid[row].Count; col++)
				{
					if (Obstacle.Grid[row][col] == 1)
					{
						float pos_x = this.Position.X + col * 3;
						float pos_y = this.Position.Y + row * 3;
						Block block = new Block(new Vector2(pos_x, pos_y));

						this.Blocks.Add(block);
					}
				}
			}
		}

		public Vector2 Position
		{
			get => position;
			set => position = value;
		}
		public List<Block> Blocks
		{
			get => blocks;
			set => blocks = value;
		}
		public static List<List<int>> Grid => Obstacle.grid;

		public void Draw()
		{
			foreach (var block in this.Blocks)
			{
				block.Draw();
			}
		}
	}
}
