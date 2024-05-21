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

	public class Alien
	{
		private static Texture2D?[] alienImages = { null, null, null };
		private static (int, int)[] alienDimension = {(0, 0), (0, 0), (0, 0)};
		private int @type;
		private Vector2 position;

        public Alien(int @type, Vector2 position)
        {
			this.Type = @type;
			this.position = position;

			if (Alien.alienImages[this.Type - 1] is null)
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

					switch (this.Type)
					{
						case 1:
							sb.Append("graphics/alien_1.png");
							break;

						case 2:
							sb.Append("graphics/alien_2.png");
							break;

						case 3:
							sb.Append("graphics/alien_3.png");
							break;
						default:
							sb.Append("graphics/alien_1.png");
							break;
					}


					byte[] bytes = Encoding.ASCII.GetBytes(sb.ToString());

					unsafe
					{
						fixed (byte* p = bytes)
						{
							sbyte* sp = (sbyte*)p;
							Texture2D tex = LoadTexture(sp);

							if (this.Type > 0 && this.Type < 4)
							{
								Alien.alienImages[this.Type - 1] = tex;
								Alien.alienDimension[this.Type - 1] = (tex.Width, tex.Height);
								
							}
							else
							{
								Alien.alienImages[0] = tex;
								Alien.alienDimension[0] = (tex.Width, tex.Height);
							}
						}
					}
				}
			}				
		}

		public int Type
		{
			get => this.@type;
			set => this.@type = value;
		}
		public static Texture2D?[] AlienImages => Alien.alienImages;

		public Vector2 Position
		{
			get => position;
			set => position = value;
		}
		public static (int, int)[] AlienDimension => alienDimension;

		public void Update(int direction)
		{
			this.position.X += direction;
		}

		public void Draw()
		{
			DrawTextureV((Texture2D)Alien.alienImages[this.Type - 1], this.position, Color.Blue);
		}

		public Rectangle GetRect() => new Rectangle(this.Position.X, this.Position.Y, Alien.AlienDimension[this.Type - 1].Item1, Alien.AlienDimension[this.Type - 1].Item2);
	}
}