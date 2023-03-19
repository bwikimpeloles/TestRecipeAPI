﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestRecipeAPI.Data;

#nullable disable

namespace TestRecipeAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230317065245_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TestRecipeAPI.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("TestRecipeAPI.Entities.Favourite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AccountId")
                        .HasColumnType("int");

                    b.Property<bool?>("FavouriteBool")
                        .HasColumnType("bit");

                    b.Property<int?>("TestRecipeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("TestRecipeId");

                    b.ToTable("Favourites");
                });

            modelBuilder.Entity("TestRecipeAPI.Entities.TestRecipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("Favourite")
                        .HasColumnType("int");

                    b.Property<string>("Ingredient")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Instruction")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TestRecipes");
                });

            modelBuilder.Entity("TestRecipeAPI.Entities.Favourite", b =>
                {
                    b.HasOne("TestRecipeAPI.Entities.Account", "Account")
                        .WithMany("FavouriteRel")
                        .HasForeignKey("AccountId");

                    b.HasOne("TestRecipeAPI.Entities.TestRecipe", "TestRecipe")
                        .WithMany("FavouriteRel")
                        .HasForeignKey("TestRecipeId");

                    b.Navigation("Account");

                    b.Navigation("TestRecipe");
                });

            modelBuilder.Entity("TestRecipeAPI.Entities.Account", b =>
                {
                    b.Navigation("FavouriteRel");
                });

            modelBuilder.Entity("TestRecipeAPI.Entities.TestRecipe", b =>
                {
                    b.Navigation("FavouriteRel");
                });
#pragma warning restore 612, 618
        }
    }
}