using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Flatbuilder.DAL.Migrations
{
    public partial class FlatbuilderMigration18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Orders_OrderId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_OrderId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Rooms");

            migrationBuilder.CreateTable(
                name: "OrderRooms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoomId = table.Column<int>(nullable: false),
                    OrderId = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderRooms_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderRooms_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderRooms_OrderId",
                table: "OrderRooms",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderRooms_RoomId",
                table: "OrderRooms",
                column: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderRooms");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Rooms",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_OrderId",
                table: "Rooms",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Orders_OrderId",
                table: "Rooms",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
