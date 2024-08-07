﻿// <auto-generated />
using System;
using CurrencyExchangeRates.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CurrencyExchangeRates.Database.Migrations
{
    [DbContext(typeof(CurrencyExchangeRateDbContext))]
    partial class CurrencyExchangeRateDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CurrencyExchangeRates.Models.Entities.CurrencyExchangeRate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("AskPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("BidPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ExchangeRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("FromCurrencyCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("LastRefreshed")
                        .HasColumnType("datetime2");

                    b.Property<string>("ToCurrencyCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("FromCurrencyCode", "ToCurrencyCode")
                        .IsUnique();

                    b.ToTable("CurrencyExchangeRates");
                });
#pragma warning restore 612, 618
        }
    }
}
