using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopify.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddProductsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_product_categories_category_id",
                table: "product");

            migrationBuilder.DropPrimaryKey(
                name: "pk_product",
                table: "product");

            migrationBuilder.RenameTable(
                name: "product",
                newName: "products");

            migrationBuilder.RenameIndex(
                name: "ix_product_category_id",
                table: "products",
                newName: "ix_products_category_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_products",
                table: "products",
                column: "id");

            migrationBuilder.CreateTable(
                name: "image",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_image", x => x.id);
                    table.ForeignKey(
                        name: "fk_image_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_image_product_id",
                table: "image",
                column: "product_id");

            migrationBuilder.AddForeignKey(
                name: "fk_products_categories_category_id",
                table: "products",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_products_categories_category_id",
                table: "products");

            migrationBuilder.DropTable(
                name: "image");

            migrationBuilder.DropPrimaryKey(
                name: "pk_products",
                table: "products");

            migrationBuilder.RenameTable(
                name: "products",
                newName: "product");

            migrationBuilder.RenameIndex(
                name: "ix_products_category_id",
                table: "product",
                newName: "ix_product_category_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_product",
                table: "product",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_product_categories_category_id",
                table: "product",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
