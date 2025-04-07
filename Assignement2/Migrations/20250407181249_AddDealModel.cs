using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assignment2.Migrations
{
    /// <inheritdoc />
    public partial class AddDealModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FoodDeliveryServiceId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deal_FoodDeliveryService_FoodDeliveryServiceId",
                        column: x => x.FoodDeliveryServiceId,
                        principalTable: "FoodDeliveryService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deal_FoodDeliveryServiceId",
                table: "Deal",
                column: "FoodDeliveryServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deal");
        }
    }
}
