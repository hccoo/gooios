﻿// <auto-generated />
using Gooios.LogService.Entities;
using Gooios.LogService.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Gooios.LogService.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("Gooios.LogService.Entities.SystemLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("AppServiceName")
                        .IsRequired()
                        .HasColumnName("app_service_name")
                        .HasMaxLength(80);

                    b.Property<string>("ApplicationKey")
                        .IsRequired()
                        .HasColumnName("application_key")
                        .HasMaxLength(80);

                    b.Property<string>("BizData")
                        .IsRequired()
                        .HasColumnName("biz_data")
                        .HasMaxLength(2000);

                    b.Property<string>("CallerApplicationKey")
                        .HasColumnName("caller_application_key")
                        .HasMaxLength(80);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("created_on");

                    b.Property<string>("Exception")
                        .HasColumnName("exception")
                        .HasMaxLength(2000);

                    b.Property<int>("Level")
                        .HasColumnName("level");

                    b.Property<int>("LogThread")
                        .HasColumnName("log_thread");

                    b.Property<DateTime>("LogTime")
                        .HasColumnName("log_time");

                    b.Property<string>("Operation")
                        .IsRequired()
                        .HasColumnName("operation")
                        .HasMaxLength(200);

                    b.Property<string>("ReturnValue")
                        .HasColumnName("return_value")
                        .HasMaxLength(2000);

                    b.HasKey("Id");

                    b.ToTable("system_logs");
                });
#pragma warning restore 612, 618
        }
    }
}
