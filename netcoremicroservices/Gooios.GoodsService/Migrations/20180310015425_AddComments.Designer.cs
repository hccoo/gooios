﻿// <auto-generated />
using Gooios.GoodsService.Domains.Aggregates;
using Gooios.GoodsService.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Gooios.GoodsService.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20180310015425_AddComments")]
    partial class AddComments
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("Gooios.GoodsService.Domains.Aggregates.Comment", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnName("content")
                        .HasMaxLength(80);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnName("created_by")
                        .HasMaxLength(80);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("created_on");

                    b.Property<string>("GoodsId")
                        .IsRequired()
                        .HasColumnName("goods_id")
                        .HasMaxLength(80);

                    b.Property<string>("OrderId")
                        .IsRequired()
                        .HasColumnName("order_id")
                        .HasMaxLength(80);

                    b.HasKey("Id");

                    b.ToTable("comments");
                });

            modelBuilder.Entity("Gooios.GoodsService.Domains.Aggregates.CommentImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("CommentId")
                        .IsRequired()
                        .HasColumnName("comment_id")
                        .HasMaxLength(80);

                    b.Property<string>("ImageId")
                        .IsRequired()
                        .HasColumnName("image_id")
                        .HasMaxLength(80);

                    b.HasKey("Id");

                    b.ToTable("comment_images");
                });

            modelBuilder.Entity("Gooios.GoodsService.Domains.Aggregates.CommentTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("CommentId")
                        .IsRequired()
                        .HasColumnName("comment_id")
                        .HasMaxLength(80);

                    b.Property<string>("GoodsId")
                        .IsRequired()
                        .HasColumnName("goods_id")
                        .HasMaxLength(80);

                    b.Property<string>("TagId")
                        .IsRequired()
                        .HasColumnName("tag_id")
                        .HasMaxLength(80);

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnName("user_id")
                        .HasMaxLength(80);

                    b.HasKey("Id");

                    b.ToTable("comment_tags");
                });

            modelBuilder.Entity("Gooios.GoodsService.Domains.Aggregates.Goods", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Category");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnName("created_by")
                        .HasMaxLength(80);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("created_on");

                    b.Property<string>("Description")
                        .HasColumnName("description")
                        .HasMaxLength(4000);

                    b.Property<string>("Detail")
                        .HasColumnName("detail");

                    b.Property<string>("ItemNumber")
                        .IsRequired()
                        .HasColumnName("item_number")
                        .HasMaxLength(80);

                    b.Property<string>("LastUpdBy")
                        .IsRequired()
                        .HasColumnName("updated_by")
                        .HasMaxLength(80);

                    b.Property<DateTime?>("LastUpdOn")
                        .IsRequired()
                        .HasColumnName("updated_on");

                    b.Property<decimal>("MarketPrice")
                        .HasColumnName("market_price");

                    b.Property<string>("OptionalPropertyJsonObject")
                        .HasColumnName("optional_property_json_object");

                    b.Property<int>("Status")
                        .HasColumnName("status");

                    b.Property<int>("Stock")
                        .HasColumnName("stock");

                    b.Property<string>("StoreId")
                        .IsRequired()
                        .HasColumnName("store_id")
                        .HasMaxLength(80);

                    b.Property<string>("SubCategory");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("title")
                        .HasMaxLength(200);

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnName("unit");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnName("unit_price");

                    b.HasKey("Id");

                    b.ToTable("goods");
                });

            modelBuilder.Entity("Gooios.GoodsService.Domains.Aggregates.GoodsCategory", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasMaxLength(80);

                    b.Property<string>("ParentId")
                        .HasColumnName("parent_id")
                        .HasMaxLength(80);

                    b.HasKey("Id");

                    b.ToTable("goods_categories");
                });

            modelBuilder.Entity("Gooios.GoodsService.Domains.Aggregates.GoodsImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("created_on");

                    b.Property<string>("GoodsId")
                        .IsRequired()
                        .HasColumnName("goods_id")
                        .HasMaxLength(80);

                    b.Property<string>("ImageId")
                        .IsRequired()
                        .HasColumnName("image_id")
                        .HasMaxLength(80);

                    b.HasKey("Id");

                    b.ToTable("goods_images");
                });

            modelBuilder.Entity("Gooios.GoodsService.Domains.Aggregates.GrouponCondition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("GoodsId")
                        .IsRequired()
                        .HasColumnName("goods_id")
                        .HasMaxLength(80);

                    b.Property<int>("MoreThanNumber")
                        .HasColumnName("more_than_number");

                    b.Property<decimal>("Price")
                        .HasColumnName("price");

                    b.HasKey("Id");

                    b.ToTable("groupon_conditions");
                });

            modelBuilder.Entity("Gooios.GoodsService.Domains.Aggregates.OnlineGoods", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Category");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnName("created_by")
                        .HasMaxLength(80);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("created_on");

                    b.Property<string>("Description")
                        .HasColumnName("description")
                        .HasMaxLength(4000);

                    b.Property<string>("Detail")
                        .HasColumnName("detail");

                    b.Property<string>("ItemNumber")
                        .IsRequired()
                        .HasColumnName("item_number")
                        .HasMaxLength(80);

                    b.Property<string>("LastUpdBy")
                        .IsRequired()
                        .HasColumnName("updated_by")
                        .HasMaxLength(80);

                    b.Property<DateTime?>("LastUpdOn")
                        .IsRequired()
                        .HasColumnName("updated_on");

                    b.Property<decimal>("MarketPrice")
                        .HasColumnName("market_price");

                    b.Property<string>("OptionalPropertyJsonObject")
                        .HasColumnName("optional_property_json_object");

                    b.Property<int>("Status")
                        .HasColumnName("status");

                    b.Property<int>("Stock")
                        .HasColumnName("stock");

                    b.Property<string>("StoreId")
                        .IsRequired()
                        .HasColumnName("store_id")
                        .HasMaxLength(80);

                    b.Property<string>("SubCategory");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("title")
                        .HasMaxLength(200);

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnName("unit");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnName("unit_price");

                    b.HasKey("Id");

                    b.ToTable("online_goods");
                });

            modelBuilder.Entity("Gooios.GoodsService.Domains.Aggregates.OnlineGoodsImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("created_on");

                    b.Property<string>("GoodsId")
                        .IsRequired()
                        .HasColumnName("goods_id")
                        .HasMaxLength(80);

                    b.Property<string>("ImageId")
                        .IsRequired()
                        .HasColumnName("image_id")
                        .HasMaxLength(80);

                    b.HasKey("Id");

                    b.ToTable("online_goods_images");
                });

            modelBuilder.Entity("Gooios.GoodsService.Domains.Aggregates.OnlineGrouponCondition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("GoodsId")
                        .IsRequired()
                        .HasColumnName("goods_id")
                        .HasMaxLength(80);

                    b.Property<int>("MoreThanNumber")
                        .HasColumnName("more_than_number");

                    b.Property<decimal>("Price")
                        .HasColumnName("price");

                    b.HasKey("Id");

                    b.ToTable("online_groupon_conditions");
                });

            modelBuilder.Entity("Gooios.GoodsService.Domains.Aggregates.Tag", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("CategoryId")
                        .IsRequired()
                        .HasColumnName("category_id")
                        .HasMaxLength(80);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnName("created_by")
                        .HasMaxLength(80);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("created_on");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasMaxLength(80);

                    b.HasKey("Id");

                    b.ToTable("tags");
                });
#pragma warning restore 612, 618
        }
    }
}
