namespace Space_Invaders.Classes
{
	using Raylib_cs;
	using System.Collections.Generic;
	using System.Numerics;
	using System.Text;
	using static Raylib_cs.Raylib;

	public class SpaceShip
	{
		private Texture2D image;
		private Vector2 position;
		private double lastFireTime;
		private Sound laserSound;
		private List <Laser> lasers;


		public SpaceShip()
		{
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

				sb.Append("graphics/spaceship.png");
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

			this.laserSound = LoadSound("music/laser.ogg");

			this.position.X = (GetScreenWidth() - this.image.Width) / 2;
			this.position.Y = GetScreenHeight() - this.image.Height - 100;

			this.Lasers = new List<Laser>();
			this.lastFireTime = .0;

		}

		public List<Laser> Lasers
		{
			get => lasers;
			set => lasers = value;
		}

		public void Draw()
		{
			DrawTextureV(this.image, this.position, Color.Yellow);
		}

		public void MoveLeft()
		{
			this.position.X -= 7;

			if (this.position.X < 25)
			{
				this.position.X = 25;
			}
		}

		public Rectangle GetRect() => new Rectangle(this.position.X, this.position.Y, this.image.Width, this.image.Height);

		public void MoveRight()
		{
			this.position.X += 7;
			if (this.position.X > GetScreenWidth() - this.image.Width - 25)
			{
				this.position.X = GetScreenWidth() - this.image.Width - 25;
			}
		}

		public void FireLaser()
		{
			if (GetTime() - this.lastFireTime >= .35)
			{
				Vector2 vec = new Vector2(this.position.X + this.image.Width / 2 - 2, this.position.Y);
				this.Lasers.Add(new Laser(vec, -6));
				this.lastFireTime = GetTime();
				PlaySound(this.laserSound);
			}
		}

		public void Reset()
		{
			this.position.X = (GetScreenWidth() - this.image.Width) / 2.0f;
			this.position.Y = GetScreenHeight() - this.image.Height - 100;
			this.Lasers.Clear();
		}
	}
}
