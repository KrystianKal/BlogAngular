﻿// <auto-generated />
using System;
using BlogBackend.Modules.Common.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BlogAngular.Migrations
{
    [DbContext(typeof(BlogDbContext))]
    [Migration("20240816112908_FixArticleFavoritedTable")]
    partial class FixArticleFavoritedTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ArticleTag", b =>
                {
                    b.Property<Guid>("ArticlesArticleId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TagsTagId")
                        .HasColumnType("uuid");

                    b.HasKey("ArticlesArticleId", "TagsTagId");

                    b.HasIndex("TagsTagId");

                    b.ToTable("ArticleTag");
                });

            modelBuilder.Entity("BlogAngular.Modules.Articles.Domain.Article", b =>
                {
                    b.Property<Guid>("ArticleId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("FavoritesCount")
                        .HasColumnType("integer");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("ArticleId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("BlogAngular.Modules.Articles.Domain.ArticleFavorited", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ArticleId")
                        .HasColumnType("uuid")
                        .HasColumnName("ArticleId");

                    b.HasKey("UserId", "ArticleId");

                    b.HasIndex("ArticleId");

                    b.ToTable("ArticleFavoriteds");
                });

            modelBuilder.Entity("BlogAngular.Modules.Articles.Domain.Comment", b =>
                {
                    b.Property<Guid>("CommentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ArticleId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("CommentId");

                    b.HasIndex("ArticleId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("BlogAngular.Modules.Articles.Domain.Tag", b =>
                {
                    b.Property<Guid>("TagId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("TagId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("BlogAngular.Modules.Profiles.Profile", b =>
                {
                    b.Property<Guid>("ProfileId")
                        .HasColumnType("uuid");

                    b.Property<string>("Bio")
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<string>("ProfileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("ProfileId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("BlogAngular.Modules.Profiles.ProfileFollow", b =>
                {
                    b.Property<Guid>("FollowerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("FollowingId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("FollowedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("FollowerId", "FollowingId");

                    b.HasIndex("FollowingId");

                    b.ToTable("ProfileFollow");
                });

            modelBuilder.Entity("BlogAngular.Modules.Users.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ArticleTag", b =>
                {
                    b.HasOne("BlogAngular.Modules.Articles.Domain.Article", null)
                        .WithMany()
                        .HasForeignKey("ArticlesArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogAngular.Modules.Articles.Domain.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BlogAngular.Modules.Articles.Domain.Article", b =>
                {
                    b.HasOne("BlogAngular.Modules.Users.User", null)
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BlogAngular.Modules.Articles.Domain.ArticleFavorited", b =>
                {
                    b.HasOne("BlogAngular.Modules.Articles.Domain.Article", "Article")
                        .WithMany("ArticleFavoriteds")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogAngular.Modules.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlogAngular.Modules.Articles.Domain.Comment", b =>
                {
                    b.HasOne("BlogAngular.Modules.Articles.Domain.Article", null)
                        .WithMany("Comments")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BlogAngular.Modules.Profiles.Profile", b =>
                {
                    b.HasOne("BlogAngular.Modules.Users.User", null)
                        .WithOne()
                        .HasForeignKey("BlogAngular.Modules.Profiles.Profile", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BlogAngular.Modules.Profiles.ProfileFollow", b =>
                {
                    b.HasOne("BlogAngular.Modules.Profiles.Profile", "Follower")
                        .WithMany("Following")
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogAngular.Modules.Profiles.Profile", "Following")
                        .WithMany("Followers")
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Follower");

                    b.Navigation("Following");
                });

            modelBuilder.Entity("BlogAngular.Modules.Articles.Domain.Article", b =>
                {
                    b.Navigation("ArticleFavoriteds");

                    b.Navigation("Comments");
                });

            modelBuilder.Entity("BlogAngular.Modules.Profiles.Profile", b =>
                {
                    b.Navigation("Followers");

                    b.Navigation("Following");
                });
#pragma warning restore 612, 618
        }
    }
}
