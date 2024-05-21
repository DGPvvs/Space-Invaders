namespace Space_Invaders.Classes
{
	using Raylib_cs;
	using System.Numerics;
	using System.Text;

	using static Raylib_cs.Raylib;

	public class MysteryShip
	{
		private int speed;
		private bool alive;

		private Vector2 position;
		private Texture2D image;

		public MysteryShip()
        {
			this.Alive = false;
			unsafe
			{
				StringBuilder sb = new StringBuilder();

				sbyte* name = GetApplicationDirectory();
				int i = 0;
				while (name[i] != '\0')
				{
					sb.Append((char)name[i]);
					i++;
				}

				sb.Append("graphics/mystery.png");

				byte[] bytes = Encoding.ASCII.GetBytes(sb.ToString());

				unsafe
				{
					fixed (byte* p = bytes)
					{
						sbyte* sp = (sbyte*)p;
						this.image = LoadTexture(sp);
					}
				}
			}
		}

		public int Speed
		{
			get => speed;
			set => speed = value;
		}
		public bool Alive
		{
			get => alive;
			set => alive = value;
		}

		public Rectangle GetRect() => this.Alive ? new Rectangle(this.position.X, this.position.Y, this.image.Width, this.image.Height) : new Rectangle(this.position.X, this.position.Y, 0, 0);

		public void Draw()
		{
			if (this.Alive)
			{
				DrawTextureV(this.image, this.position, Color.LightGray);
			}
		}

		public void Update()
		{
			if (this.Alive)
			{
				this.position.X += this.speed;

				if (this.position.X > GetScreenWidth() - this.image.Width - 25 || this.position.X < 25)
				{
					this.Alive = false;
				}
			}
		}

		public void Spawn()
		{
			this.position.Y = 90;
			int side = GetRandomValue(0, 1);

			if (side == 0)
			{
				this.speed = 3;
				this.position.X = 25;
			}
			else
			{
				this.speed = -3;
				this.position.X = GetScreenWidth() - this.image.Width - 25;
			}

			this.Alive = true;
		}
	}
}
