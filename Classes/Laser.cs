namespace Space_Invaders.Classes
{
	using System.Numerics;
	using Raylib_cs;

	using static Raylib_cs.Raylib;

	public class Laser
	{
		private Vector2 position;
		private int speed;
		private bool active;

		public Laser(Vector2 position, int speed)
		{
			this.Position = position;
			this.Speed = speed;
			this.Active = true;

		}
		public bool Active
		{
			get => active;
			set => active = value;
		}
		public Vector2 Position
		{
			get => position;
			set => position = value;
		}
		public int Speed
		{
			get => speed;
			set => speed = value;
		}

		public void Update()
		{
			if (this.Active)
			{
				if (this.Position.Y > GetScreenHeight() - 100 || this.Position.Y < 25)
				{
					this.Active = false;
					//std::cout << "Laser Inactive" << std::endl;
				}
			}

			this.position.Y += this.Speed;
		}

		public void Draw()
		{
			if (this.Active)
			{
				DrawRectangle((int)this.Position.X, (int)this.Position.Y, 4, 15, Color.Red);
			}
		}

		public Rectangle GetRect() => new Rectangle(this.position.X, this.position.Y, 4, 15);
	}
}
