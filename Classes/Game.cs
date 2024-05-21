namespace Space_Invaders.Classes
{
	using Raylib_cs;
	using System.Numerics;
	using System.Text;
	using static Common.Constants;
	using static Raylib_cs.Raylib;
	using static System.Net.Mime.MediaTypeNames;

	public class Game
	{
		private int alienDirection;
		private float lastAlienFired;

		private float mysteryShipSpawnInterval;
		private float tileLastSpawn;

		private int lives;
		private int score;
		private int hightScore;

		private bool run;

		private Music music;
		private Sound explosionSound;

		private SpaceShip spaceShip;
		private MysteryShip mysteryShip;

		private List<Obstacle> obstacles;
		private List<Alien> aliens;
		private List<Laser> alienLasers;

		public Game()
		{
			this.music = LoadMusicStream("music/music.ogg");
			this.explosionSound = LoadSound("music/explosion.ogg");
			PlayMusicStream(this.music);
			this.InitGame();
		}

		public SpaceShip SpaceShip
		{
			get => spaceShip;
			set => spaceShip = value;
		}
		public List<Obstacle> Obstacles
		{
			get => obstacles;
			set => obstacles = value;
		}
		public MysteryShip MysteryShip
		{
			get => mysteryShip;
			set => mysteryShip = value;
		}
		public bool Run
		{
			get => run;
			set => run = value;
		}

		public int Lives
		{
			get => lives;
			set => lives = value;
		}

		public int Score
		{
			get => score;
			set => score = value;
		}
		public int HightScore
		{
			get => hightScore;
			set => hightScore = value;
		}
		public Music Music
		{
			get => music;
			set => music = value;
		}

		public void Draw()
		{
			this.SpaceShip.Draw();

			foreach (var laser in this.SpaceShip.Lasers)
			{
				laser.Draw();
			}

			foreach (var obstacle in this.Obstacles)
			{
				obstacle.Draw();
			}

			foreach (var alien in this.aliens)
			{
				alien?.Draw();
			}

			foreach (var laser in this.alienLasers)
			{
				laser?.Draw();
			}

			this.MysteryShip.Draw();
		}

		public void HandleInput()
		{
			if (this.Run)
			{
				if (IsKeyDown(KeyboardKey.Left))
				{
					this.SpaceShip.MoveLeft();
				}
				else if (IsKeyDown(KeyboardKey.Right))
				{
					this.SpaceShip.MoveRight();
				}
				else if (IsKeyDown(KeyboardKey.Space))
				{
					this.SpaceShip.FireLaser();
				}
			}
		}

		public void Update()
		{
			if (this.Run)
			{
				double currentTime = GetTime();

				if (currentTime - this.tileLastSpawn > this.mysteryShipSpawnInterval)
				{
					this.mysteryShip.Spawn();
					this.tileLastSpawn = (float)GetTime();
					this.mysteryShipSpawnInterval = GetRandomValue(10, 20);
				}

				foreach (var laser in this.SpaceShip.Lasers)
				{
					laser.Update();
				}

				this.MoveAliens();
				this.AlienShootLaser();

				foreach (var laser in this.alienLasers)
				{
					laser.Update();
				}

				this.DeleteInactiveLasers();

				this.mysteryShip.Update();
				this.CheckForCollisions();
			}
			else
			{
				if (IsKeyDown(KeyboardKey.Enter))
				{
					this.Reset();
					this.InitGame();
				}
			}
		}

		private void Reset()
		{
			this.SpaceShip.Reset();
			this.alienLasers.Clear();
			this.aliens.Clear();
			this.Obstacles.Clear();
		}

		private void InitGame()
		{
			this.SpaceShip = new SpaceShip();
			this.MysteryShip = new MysteryShip();
			this.lives = 3;
			this.score = 0;
			this.hightScore = this.LoadHighScoreFromFile();

			this.Obstacles = this.CreateObstacles();

			this.aliens = this.CreateAliens();
			this.alienDirection = 1;
			this.lastAlienFired = .0f;
			this.alienLasers = new List<Laser>();

			this.tileLastSpawn = .0f;
			this.mysteryShipSpawnInterval = GetRandomValue(10, 20);
			this.Run = true;
		}

		private void CheckForCollisions()
		{
			//SpaseShip Lasers
			foreach (var laser in this.SpaceShip.Lasers)
			{
				//Hit aliens
				for (int it = this.aliens.Count - 1; it >= 0; it--)
				{
					if (CheckCollisionRecs(this.aliens[it].GetRect(), laser.GetRect()))
					{
						PlaySound(this.explosionSound);
						if (this.aliens[it].Type == 1)
						{
							this.score += 100;
						}
						else if (this.aliens[it].Type == 2)
						{
							this.score += 200;
						}
						else if (this.aliens[it].Type == 3)
						{
							this.score += 300;
						}

						this.CheckForHighScore();

						this.aliens.RemoveAt(it);
						laser.Active = false;
					}
				}

				//Hit obstacles
				for (int it = this.Obstacles.Count - 1; it >= 0; it--)
				{
					var obstacle = this.Obstacles[it];
					for (int itb = obstacle.Blocks.Count - 1; itb >= 0; itb--)
					{
						var block = obstacle.Blocks[itb];

						if (CheckCollisionRecs(block.GetRect(), laser.GetRect()))
						{
							obstacle.Blocks.RemoveAt(itb);
							laser.Active = false;
						}
					}
				}

				//Hit MysteryShip
				if (CheckCollisionRecs(this.MysteryShip.GetRect(), laser.GetRect()))
				{
					this.MysteryShip.Alive = false;
					laser.Active = false;
					this.score += 500;
					this.CheckForHighScore();
					PlaySound(this.explosionSound);
				}
			}

			//Alien lasers
			foreach (var laser in this.alienLasers)
			{
				if (CheckCollisionRecs(laser.GetRect(), this.SpaceShip.GetRect()))
				{
					laser.Active = false;
					this.lives--;

					if (this.lives == 0)
					{
						this.GameOver();
					}
				}

				//Hit obstacles
				for (int it = this.Obstacles.Count - 1; it >= 0; it--)
				{
					var obstacle = this.Obstacles[it];
					for (int itb = obstacle.Blocks.Count - 1; itb >= 0; itb--)
					{
						var block = obstacle.Blocks[itb];

						if (CheckCollisionRecs(block.GetRect(), laser.GetRect()))
						{
							obstacle.Blocks.RemoveAt(itb);
							laser.Active = false;
						}
					}
				}
			}

			//Alien collision with obstacle
			foreach (var alien in this.aliens)
			{
				foreach (var obstacle in this.Obstacles)
				{
					for (int i = obstacle.Blocks.Count - 1; i >= 0; i--)
					{
						if (CheckCollisionRecs(obstacle.Blocks[i].GetRect(), alien.GetRect()))
						{
							obstacle.Blocks.RemoveAt(i);
						}
					}
				}

				if (CheckCollisionRecs(alien.GetRect(), this.SpaceShip.GetRect()))
				{
					this.GameOver();
				}
			}
		}

		private void GameOver()
		{
			this.Run = false; ;
		}

		private void AlienShootLaser()
		{
			double currentTime = GetTime();

			if (currentTime - this.lastAlienFired >= ALIEN_LASER_SHOOT_INTERVAL && this.aliens.Count > 0)
			{

				int randomIndex = GetRandomValue(0, this.aliens.Count - 1);

				Alien alien = this.aliens[randomIndex];

				this.alienLasers.Add(new Laser(new Vector2(alien.Position.X + Alien.AlienDimension[alien.Type - 1].Item1 / 2, alien.Position.Y + Alien.AlienDimension[alien.Type - 1].Item2), 4));

				this.lastAlienFired = (float)GetTime();
			}
		}

		private void MoveDownAliens(int distance)
		{
			foreach (var alien in this.aliens)
			{
				float xPosition = alien.Position.X;
				float yPosition = alien.Position.Y + distance;
				alien.Position = new Vector2(xPosition, yPosition);
			}
		}

		private void MoveAliens()
		{
			foreach (var alien in this.aliens)
			{
				if (alien.Position.X + Alien.AlienDimension[alien.Type - 1].Item1 > GetScreenWidth() - 25)
				{
					this.alienDirection = -1;
					this.MoveDownAliens(SPEED_DOWN);
				}
				if (alien.Position.X < 25)
				{
					this.alienDirection = 1;
					this.MoveDownAliens(SPEED_DOWN);
				}

				alien.Update(this.alienDirection);
			}
		}

		private List<Alien> CreateAliens()
		{
			var temp = new List<Alien>();

			for (var row = 0; row < 5; row++)
			{
				for (var col = 0; col < 11; col++)
				{
					int alienType = 3;

					if (row == 1 || row == 2)
					{
						alienType = 2;
					}
					else if (row > 2)
					{
						alienType = 1;
					}

					float x = 75 + col * 55;
					float y = 110 + row * 55;
					temp.Add(new Alien(alienType, new Vector2(x, y)));

				}
			}

			return temp;
		}

		private List<Obstacle> CreateObstacles()
		{
			this.Obstacles = new List<Obstacle>();

			int obstacleWidth = Obstacle.Grid[0].Count * 3;
			float gap = (GetScreenWidth() - (4 * obstacleWidth)) / 5;
			float yPosition = GetScreenHeight() - 200;

			for (int i = 0; i < 4; i++)
			{
				float offsetX = (i + 1) * gap + i * obstacleWidth;
				this.Obstacles.Add(new Obstacle(new Vector2(offsetX, yPosition)));
			}

			return this.Obstacles;
		}

		private void DeleteInactiveLasers()
		{
			for (var i = this.SpaceShip.Lasers.Count - 1; i >= 0; i--)
			{
				if (!this.SpaceShip.Lasers[i].Active)
				{
					this.SpaceShip.Lasers.RemoveAt(i);
				}
			}

			for (var i = this.alienLasers.Count - 1; i >= 0; i--)
			{
				if (!this.alienLasers[i].Active)
				{
					this.alienLasers.RemoveAt(i);
				}
			}
		}

		private void CheckForHighScore()
		{
			if (this.score > this.hightScore)
			{
				this.hightScore = this.score;
				this.SaveHighScoreToFile(this.hightScore);
			}
		}

		private void SaveHighScoreToFile(int highScore)
		{
			using (StreamWriter sw = new StreamWriter("highscore.txt"))
			{
				sw.WriteLine(this.HightScore.ToString());				
			}
		}

		private int LoadHighScoreFromFile()
		{
			StringBuilder sb = new StringBuilder();
			string path = @"highscore.txt";
			if (!File.Exists(path))
			{
				this.SaveHighScoreToFile(0);
			}

			int result = 0;
			using (StreamReader sr = new StreamReader("highscore.txt"))
			{
				string line = string.Empty;
				while ((line = sr.ReadLine()) != null)
				{
					result = Convert.ToInt32(line);
				}
			}

			return result;
		}
	}
}
