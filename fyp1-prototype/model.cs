﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace fyp1_prototype
{
	//	Class for table data / table columns
	//	Player class
	public class Player
	{
		[Key]
		[Column("ID")]
		public int Id { get; set; }

		[Column("username")]
		public string Username { get; set; }

		[Column("score")]
		public int Score { get; set; }

		[Column("date")]
		public string Date { get; set; }

		[Column("Password")]
		public string Password { get; set; }
	}

	//	Game class
	public class Game
	{
		[Key]
		[Column("ID")]
		public int Id { get; set; }

		[Column("lives")]
		public int Lives{ get; set; }

		[Column("playerGame")]
		public int PlayerGame{ get; set; }

		[Column("datetime")]
		public string DateTime { get; set; }

		[Column("time")]
		public string Time { get; set; }

		[Column("score")]
		public int Score { get; set; }

		[Column("itemGame")]
		public int ItemGame { get; set; }
	}

	//	Score class
	public class Score
	{
		[Key]
		[Column("ID")]
		public int Id { get; set; }

		[Column("value")]
		public int Value { get; set; }

		[Column("datetime")]
		public string DateTime { get; set; }

		[Column("playerScore")]
		public int PlayerScore { get; set; }
	}

	//	Item class
	public class Items
	{
		[Column("ID")]
		public int Id { get; set; }

		[Column("item_image_link")]
		public string ItemImageLink { get; set; }

		[Column("item_type")]
		public int ItemType { get; set; }
	}

	//	DbContext class to access tables
	public class DatabaseContext : DbContext
	{
		public DbSet<Player> Players { get; set; }
		public DbSet<Game> Game { get; set; }
		public DbSet<Score> Score { get; set; }
		public DbSet<Items> Items { get; set; }
	}

	//	Player class with public functions to interact with DbContext
	public class PlayersRepository
	{
		//	Constructor
		public PlayersRepository()
		{

		}

		//	Data transfer object (To save all column data)
		public class PlayerDto
		{
			public int Id { get; set; }
			public string Username { get; set; }
			public int Score { get; set; }
			public string Date { get; set; }
			public string Password { get; set; }
		}

		//	Get all players
		public List<PlayerDto> GetAllPlayers()
		{
			DatabaseContext dc = new DatabaseContext();
			return dc.Players.Select(player => new PlayerDto()
			{
				Id = player.Id,
				Username = player.Username,
				Score = player.Score,
				Date = player.Date,
				Password = player.Password
			}).ToList();
		}

		//	Get player with specific ID
		public List<PlayerDto> GetPlayerWithId(int id)
		{
			DatabaseContext dc = new DatabaseContext();
			return dc
				.Players
				.Where(player => player.Id == id)
				.Select(player => new PlayerDto()
				{
					Id = player.Id,
					Username = player.Username,
					Score = player.Score,
					Date = player.Date,
					Password = player.Password
				})
				.ToList();
		}

		//	Get player with specific Username
		public List<PlayerDto> GetPlayerWithUsername(string username)
		{
			DatabaseContext dc = new DatabaseContext();
			return dc
				.Players
				.Where(player => player.Username == username)
				.Select(player => new PlayerDto()
				{
					Id = player.Id,
					Username = player.Username,
					Score = player.Score,
					Date = player.Date,
					Password = player.Password
				})
				.ToList();
		}

		//	Get specific data of players with Data Transfer Object (Get specific column)
		public List<PlayerDto> GetAllPlayersName()
		{
			DatabaseContext dc = new DatabaseContext();
			return dc.Players
				.Select(player => new PlayerDto()
				{
					Id = player.Id,
					Username = player.Username,
					Score = player.Score
				})
				.ToList();
		}

		//	Get all scores of all players
		public List<int> GetAllPlayersScore()
		{
			DatabaseContext dc = new DatabaseContext();
			return dc.Players.Select(player => player.Score).ToList();
		}

		//	Add player to table
		public void AddPlayer(string name, string password)
		{
			DatabaseContext dc = new DatabaseContext();
			dc.Players.Add(new Player()
			{
				Username = name,
				Date = DateTime.Now.ToString("YYYY-MM-DD"),
				Password = "123456"
			});
			dc.SaveChanges();
		}

		//	Modify players' data
		public void ModifyPlayers()
		{
			DatabaseContext dc = new DatabaseContext();
			var players = dc.Players
				.Where(u => u.Score > 0)
				.ToList();

			foreach (var player in players)
			{
				//player.Score = 99999;
			}
			dc.SaveChanges();
		}

		//	Modify a player's score
		public void ModifyPlayerScore(int id, int score)
		{
			DatabaseContext dc = new DatabaseContext();
			var player = dc
				.Players
				.FirstOrDefault(p => p.Id == id);
			player.Score = score;
			dc.SaveChanges();
		}
	}

	//	Game class with public functions to interact with DbContext
	public class GameRepository
	{
		public GameRepository()
		{

		}

		public class GameDto
		{
			public int Id { get; set; }
			public int Lives { get; set; }
			public int PlayerGame { get; set; }
			public string DateTime { get; set; }
			public string Time { get; set; }
			public int Score { get; set; }
			public int ItemGame { get; set; }
		}

		public void AddGame(int id, int lives, int playerGame, string time, int score, int itemGame)
		{
			DatabaseContext dc = new DatabaseContext();
			dc.Game.Add(new Game()
			{
				Id = id,
				Lives = lives,
				PlayerGame = playerGame,
				DateTime = DateTime.Now.ToString("YYYY-MM-DD HH:MM:SS"),
				Time = time,
				Score = score,
				ItemGame = itemGame
			});
			dc.SaveChanges();
		}

		public void ModifyGame(int id, int lives, int playerGame, string time, int score, int itemGame)
		{
			DatabaseContext dc = new DatabaseContext();
			var game = dc.Game.FirstOrDefault(g => g.PlayerGame == playerGame);
			game.Id = id;
			game.Lives = lives;
			game.DateTime = DateTime.Now.ToString("YYYY-MM-DD HH:MM:SS");
			game.Time = time;
			game.Score = score;
			game.ItemGame = itemGame;
			dc.SaveChanges();
		}

		public List<GameDto> GetGame(int playerGame)
		{
			DatabaseContext dc = new DatabaseContext();
			return dc
				.Game
				.Where(game => game.PlayerGame == playerGame)
				.Select(game => new GameDto()
				{
					Id = game.Id,
					Lives = game.Lives,
					PlayerGame = game.Lives,
					DateTime = game.DateTime,
					Time = game.Time,
					Score = game.Score,
					ItemGame = game.ItemGame
				})
				.ToList();
		}
	}

	// Score class with public functions to interact with DbContext
	public class ScoreRepository
	{
		public ScoreRepository()
		{

		}

		public class ScoreDto
		{
			public int Id { get; set; }
			public int Value { get; set; }
			public string DateTime { get; set; }
			public int PlayerScore { get; set; }	//	Player ID foreign key
		}

		public void AddScore(int score, int playerID)
		{
			DatabaseContext dc = new DatabaseContext();
			dc.Score.Add(new Score()
			{
				Value = score,
				DateTime = DateTime.Now.ToString(),
				PlayerScore = playerID
			});
			dc.SaveChanges();
		}

		public List<ScoreDto> GetAllScore()
		{
			DatabaseContext dc = new DatabaseContext();
			return dc
				.Score
				.Select(score => new ScoreDto()
				{
					Value = score.Value,
					DateTime = score.DateTime,
					PlayerScore = score.PlayerScore
				})
				.ToList();
		}

	}

	//	Item class with public functions to interact with DbContext
	public class ItemsRepository
	{
		public ItemsRepository()
		{

		}

		public class ItemsDto
		{
			public int Id { get; set; }
			public string Item_Image_Link { get; set; }
			public int Item_Type { get; set; }
		}

		//	Get item of specific ID
		public List<ItemsDto> GetItem(int id)
		{
			DatabaseContext dc = new DatabaseContext();
			return dc
				.Items
				.Select(item => new ItemsDto()
				{
					Id = item.Id,
					Item_Image_Link = item.ItemImageLink,
					Item_Type = item.ItemType
				})
				.Where(item => item.Id == id)
				.ToList();
		}

		//	Get total count
		public int GetCount()
		{
			new ScoreRepository();
			DatabaseContext dc = new DatabaseContext();
			return dc.Items.Count();
		}
	}
}