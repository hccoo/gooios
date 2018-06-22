﻿// <auto-generated />
using Gooios.AuthorizationService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Gooios.AuthorizationService.Data.Migrations.PersistedGrantCustomDb
{
    [DbContext(typeof(PersistedGrantCustomDbContext))]
    partial class PersistedGrantCustomDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.PersistedGrant", b =>
                {
                    b.Property<string>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("key");

                    b.Property<string>("ClientId")
                        .HasColumnName("client_id");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnName("creation_time");

                    b.Property<string>("Data")
                        .HasColumnName("data");

                    b.Property<DateTime?>("Expiration")
                        .HasColumnName("expiration");

                    b.Property<string>("SubjectId")
                        .HasColumnName("subject_id");

                    b.Property<string>("Type")
                        .HasColumnName("type");

                    b.HasKey("Key");

                    b.ToTable("persisted_grants");
                });
#pragma warning restore 612, 618
        }
    }
}
