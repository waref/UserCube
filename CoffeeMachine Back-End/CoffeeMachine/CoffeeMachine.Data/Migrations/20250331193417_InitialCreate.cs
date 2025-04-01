using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoffeeMachine.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDisabledAction = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoffeeCreationOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumEspressoShots = table.Column<int>(type: "int", nullable: true),
                    AddMilk = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeCreationOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoffeeMachineStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WaterLevel = table.Column<int>(type: "int", nullable: false),
                    BeanFeed = table.Column<int>(type: "int", nullable: false),
                    WasteCoffee = table.Column<int>(type: "int", nullable: false),
                    WaterTray = table.Column<int>(type: "int", nullable: false),
                    IsOn = table.Column<bool>(type: "bit", nullable: false),
                    IsMakingCoffee = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeMachineStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoffeeActionLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActionId = table.Column<int>(type: "int", nullable: false),
                    ActionTypeId = table.Column<int>(type: "int", nullable: false),
                    CoffeeCreationOptionsId = table.Column<int>(type: "int", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeActionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoffeeActionLogs_ActionTypes_ActionTypeId",
                        column: x => x.ActionTypeId,
                        principalTable: "ActionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoffeeActionLogs_Actions_ActionId",
                        column: x => x.ActionId,
                        principalTable: "Actions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoffeeActionLogs_CoffeeCreationOptions_CoffeeCreationOptionsId",
                        column: x => x.CoffeeCreationOptionsId,
                        principalTable: "CoffeeCreationOptions",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "ActionTypes",
                columns: new[] { "Id", "ActionName", "Description", "IsDisabledAction" },
                values: new object[,]
                {
                    { 1, "Turn off", "Turn off the machine if idle, no coffe will be surved !", false },
                    { 2, "Turn On", "Turing On the machine if off", false },
                    { 3, "Make Coffee", "making coffe if idle (On, not making coffe and no alert)", false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeActionLogs_ActionId",
                table: "CoffeeActionLogs",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeActionLogs_ActionTypeId",
                table: "CoffeeActionLogs",
                column: "ActionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeActionLogs_CoffeeCreationOptionsId",
                table: "CoffeeActionLogs",
                column: "CoffeeCreationOptionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoffeeActionLogs");

            migrationBuilder.DropTable(
                name: "CoffeeMachineStates");

            migrationBuilder.DropTable(
                name: "ActionTypes");

            migrationBuilder.DropTable(
                name: "Actions");

            migrationBuilder.DropTable(
                name: "CoffeeCreationOptions");
        }
    }
}
