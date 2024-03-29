﻿// <auto-generated />
using Gooios.OrderService.Domains.Aggregates;
using Gooios.OrderService.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Gooios.OrderService.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20200213014402_FixOrderTitleCanNull")]
    partial class FixOrderTitleCanNull
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("Gooios.OrderService.Domains.Aggregates.DeliveryNote", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("CarrierName")
                        .IsRequired()
                        .HasColumnName("carrier_name")
                        .HasMaxLength(80);

                    b.Property<string>("CarrierPhone")
                        .IsRequired()
                        .HasColumnName("carrier_phone")
                        .HasMaxLength(80);

                    b.Property<string>("Consignee")
                        .IsRequired()
                        .HasColumnName("consignee")
                        .HasMaxLength(80);

                    b.Property<string>("ConsigneeMobile")
                        .IsRequired()
                        .HasColumnName("consignee_mobile")
                        .HasMaxLength(20);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnName("created_by")
                        .HasMaxLength(80);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("created_on");

                    b.Property<string>("DeliveryAddress")
                        .IsRequired()
                        .HasColumnName("delivery_address")
                        .HasMaxLength(80);

                    b.Property<string>("DeliveryNoteNo")
                        .IsRequired()
                        .HasColumnName("delivery_note_no")
                        .HasMaxLength(80);

                    b.Property<string>("OrderId")
                        .IsRequired()
                        .HasColumnName("order_id")
                        .HasMaxLength(80);

                    b.Property<decimal>("ShippingAmount")
                        .HasColumnName("shipping_amount");

                    b.Property<int>("ShippingMethod")
                        .HasColumnName("shipping_method");

                    b.HasKey("Id");

                    b.ToTable("delivery_notes");
                });

            modelBuilder.Entity("Gooios.OrderService.Domains.Aggregates.Order", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("ActivityId")
                        .HasColumnName("activity_id")
                        .HasMaxLength(80);

                    b.Property<string>("Area")
                        .IsRequired()
                        .HasColumnName("area")
                        .HasMaxLength(80);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnName("city")
                        .HasMaxLength(80);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnName("created_by")
                        .HasMaxLength(80);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("created_on");

                    b.Property<string>("CustomerMobile")
                        .IsRequired()
                        .HasColumnName("customer_mobile")
                        .HasMaxLength(80);

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnName("customer_name")
                        .HasMaxLength(80);

                    b.Property<string>("InvoiceRemark")
                        .HasColumnName("invoice_remark")
                        .HasMaxLength(500);

                    b.Property<int>("InvoiceType")
                        .HasColumnName("invoice_type");

                    b.Property<string>("Mark")
                        .IsRequired()
                        .HasColumnName("mark")
                        .HasMaxLength(80);

                    b.Property<string>("OrderNo")
                        .IsRequired()
                        .HasColumnName("order_no")
                        .HasMaxLength(80);

                    b.Property<string>("OrganizationId")
                        .IsRequired()
                        .HasColumnName("organization_id")
                        .HasMaxLength(80);

                    b.Property<decimal>("PayAmount")
                        .HasColumnName("pay_amount");

                    b.Property<string>("Postcode")
                        .IsRequired()
                        .HasColumnName("post_code")
                        .HasMaxLength(80);

                    b.Property<decimal>("PreferentialAmount")
                        .HasColumnName("preferential_amount");

                    b.Property<string>("Province")
                        .IsRequired()
                        .HasColumnName("province")
                        .HasMaxLength(80);

                    b.Property<string>("Remark")
                        .HasColumnName("remark")
                        .HasMaxLength(500);

                    b.Property<decimal>("ShippingCost")
                        .HasColumnName("shipping_cost");

                    b.Property<int>("Status")
                        .HasColumnName("status");

                    b.Property<string>("StreetAddress")
                        .IsRequired()
                        .HasColumnName("street_address")
                        .HasMaxLength(200);

                    b.Property<decimal>("Tax")
                        .HasColumnName("tax");

                    b.Property<string>("Title")
                        .HasColumnName("title")
                        .HasMaxLength(200);

                    b.Property<decimal>("TotalAmount")
                        .HasColumnName("total_amount");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnName("updated_by")
                        .HasMaxLength(80);

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnName("updated_on");

                    b.HasKey("Id");

                    b.ToTable("orders");
                });

            modelBuilder.Entity("Gooios.OrderService.Domains.Aggregates.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int>("Count")
                        .HasColumnName("count");

                    b.Property<string>("ObjectId")
                        .IsRequired()
                        .HasColumnName("object_id")
                        .HasMaxLength(80);

                    b.Property<string>("ObjectNo")
                        .IsRequired()
                        .HasColumnName("object_no")
                        .HasMaxLength(80);

                    b.Property<string>("OrderId")
                        .IsRequired()
                        .HasColumnName("order_id")
                        .HasMaxLength(80);

                    b.Property<string>("PreviewPictureUrl")
                        .IsRequired()
                        .HasColumnName("preview_picture_url")
                        .HasMaxLength(200);

                    b.Property<string>("SelectedProperties")
                        .IsRequired()
                        .HasColumnName("selected_properties")
                        .HasMaxLength(2000);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("title")
                        .HasMaxLength(200);

                    b.Property<decimal>("TradeUnitPrice")
                        .HasColumnName("trade_unit_price");

                    b.HasKey("Id");

                    b.ToTable("order_items");
                });

            modelBuilder.Entity("Gooios.OrderService.Domains.Aggregates.OrderTrace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnName("created_by")
                        .HasMaxLength(80);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("created_on");

                    b.Property<bool>("IsSuccess")
                        .HasColumnName("is_success");

                    b.Property<string>("OrderId")
                        .IsRequired()
                        .HasColumnName("order_id")
                        .HasMaxLength(80);

                    b.Property<int>("Status")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.ToTable("order_traces");
                });
#pragma warning restore 612, 618
        }
    }
}
