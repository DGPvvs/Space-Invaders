namespace Space_Invaders
{
	using Raylib_cs;
	using Space_Invaders.Classes;
	using System.Linq;
	using System.Numerics;
	using System.Text;
	using static Common.Constants;
	using static Raylib_cs.Raylib;

	public class StartUp
	{
		static void Main(string[] args)
		{			
			Color gray = new  Color( 29, 29, 27, 255);


			InitWindow(WINDOW_WIDTH + OFFSET, WINDOW_HEIGHT + 2 * OFFSET, GAME_TITLE);
			InitAudioDevice();

			Font font;
			Texture2D spaceshipImage;
			unsafe
			{
				StringBuilder sbFont = new StringBuilder();
				StringBuilder sbSpaceship = new StringBuilder();

				sbyte* name = GetApplicationDirectory();
				int i = 0;
				while (name[i] != '\0')
				{
					sbFont.Append((char)name[i]);
					sbSpaceship.Append((char)name[i]);
					i++;
				}

				sbFont.Append("../../../font / monogram.ttf");
				sbSpaceship.Append("../../../graphics/spaceship.png");

				byte[] bytesFont = Encoding.ASCII.GetBytes(sbFont.ToString());

				unsafe
				{
					fixed (byte* p = bytesFont)
					{
						int codepoint = 0;
						sbyte* sp = (sbyte*)p;
						font = LoadFontEx(sp, 64, &codepoint, 0);
					}
				}

				byte[] bytesSpaceship = Encoding.ASCII.GetBytes(sbSpaceship.ToString());

				unsafe
				{
					fixed (byte* p = bytesSpaceship)
					{
						sbyte* sp = (sbyte*)p;
						spaceshipImage = LoadTexture(sp);
					}
				}
			}

			SetTargetFPS(TARGET_FPS);

			Game game = new Game();

			while (!WindowShouldClose())
			{
				UpdateMusicStream(game.Music);
				game.HandleInput();
				game.Update();

				BeginDrawing();
				ClearBackground(gray);

				DrawRectangleRoundedLines(new Rectangle(10, 10, 780, 780), .18f, 20, 2, Color.Yellow);
				DrawLineEx(new Vector2(25, 730), new Vector2(775, 730), 3, Color.Yellow);

				if (game.Run)
				{
					DrawTextEx(font, "LEVEL 1", new Vector2(570, 740), 34, 2, Color.Yellow);
				}
				else
				{
					DrawTextEx(font, "GAME OVER", new Vector2(570, 740), 34, 2, Color.Yellow);
				}

				float x = 50.0f;
				for (int i = 0; i < game.Lives; i++)
				{
					DrawTextureV(spaceshipImage, new Vector2(x, 745), Color.Yellow);
					x += 50;
				}

				DrawTextEx(font, "SCORE", new Vector2(50, 15), 34, 2, Color.Yellow);

				byte[] scoreText = FormetWithLeadingZeros(game.Score, 5);

				unsafe
				{
					fixed (byte* p = scoreText)
					{
						sbyte* sp = (sbyte*)p;
						DrawTextEx(font, sp, new Vector2(50, 40), 34, 2, Color.Yellow);
					}
				}

				DrawTextEx(font, "HIGH-SCORE", new Vector2(540, 15), 34, 2, Color.Yellow);

				scoreText = FormetWithLeadingZeros(game.HightScore, 5);

				unsafe
				{
					fixed (byte* p = scoreText)
					{
						sbyte* sp = (sbyte*)p;
						DrawTextEx(font, sp, new Vector2(540, 40), 34, 2, Color.Yellow);
					}
				}


				game.Draw();

				EndDrawing();
			}

			CloseAudioDevice();
			CloseWindow();
		}

		static byte[] FormetWithLeadingZeros(int number, int width)
		{
			int leadingZeroes = width - number.ToString().Length;

			string result = new string('0', leadingZeroes) + number.ToString();

			byte[] bytes = Encoding.ASCII.GetBytes(result.ToString());
			return bytes;
		}
	}
}
