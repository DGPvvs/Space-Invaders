namespace Space_Invaders.Classes
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Numerics;
	using System.Text;
	using System.Threading.Tasks;
	using Raylib_cs;

	using static Raylib_cs.Raylib;

	public class Block
	{
		private Vector2 position;

		public Block(Vector2 position)
        {
			this.position = position;            
        }

		public void Draw()
		{
			DrawRectangle((int)this.position.X, (int)this.position.Y, 3, 3, Color.Purple);
		}

		public Rectangle GetRect() => new Rectangle(this.position.X, this.position.Y, 3, 3);
	}
}
