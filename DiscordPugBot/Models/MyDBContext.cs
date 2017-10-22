﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace OWPugs.Models
{
    public class MyDBContext : DbContext
    {
        public virtual DbSet<Maps> Maps { get; set; }
		public virtual DbSet<GameModes> GameModes { get; set; }
		public virtual DbSet<Users> Users { get; set; }
		public virtual DbSet<Matches> Matches { get; set; }
		public virtual DbSet<UsersMatchesRelation> UsersMatchesRelation { get; set; }

		private AppConfig _appConfig;


		public MyDBContext(IOptions<AppConfig> appConfig)
		{
			_appConfig = appConfig.Value;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
			optionsBuilder.UseMySql(@_appConfig.ConnectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			modelBuilder.Entity<UsersMatchesRelation>()
			.HasKey(t => new { t.MatchId, t.UserId});

			modelBuilder.Entity<UsersMatchesRelation>()
				.HasOne(pt => pt.Match)
				.WithMany(p => p.UserMatches)
				.HasForeignKey(pt => pt.MatchId);

			modelBuilder.Entity<UsersMatchesRelation>()
				.HasOne(pt => pt.User)
				.WithMany(t => t.UserMatches)
				.HasForeignKey(pt => pt.UserId);

			modelBuilder.Entity<Maps>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("0");
            });

			modelBuilder.Entity<GameModes>(entity =>
			{
				entity.Property(e => e.Id).HasColumnType("int(11)");

				entity.Property(e => e.Name)
					.IsRequired()
					.HasColumnType("varchar(255)")
					.HasDefaultValueSql("0");
			});

			modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.DiscordId).HasColumnType("bigint(20)");

				entity.Property(e => e.Info)				
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.Loses)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.PlayerHeroes)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.SkillRating)
                    .HasDefaultValueSql("1500");

				entity.Property(e => e.RatingsDeviation)
					.HasDefaultValueSql("350");

				entity.Property(e => e.Volatility)
					.HasDefaultValueSql("0.06");

				entity.Property(e => e.UserName).HasColumnType("varchar(128)");

                entity.Property(e => e.Wins)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

				entity.Property(e => e.WinsAsCaptain)
					.HasDefaultValueSql("0");

				entity.Property(e => e.LosesAsCaptain)
					.HasDefaultValueSql("0");
			});
        }
    }
}