using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "RiskRules",
                columns: table => new
                {
                    RiskRuleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RuleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConditionExpression = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskScore = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskRules", x => x.RiskRuleId);
                });

            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    ClaimId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClaimDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.ClaimId);
                    table.ForeignKey(
                        name: "FK_Claims_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimRiskRules",
                columns: table => new
                {
                    ClaimsClaimId = table.Column<int>(type: "int", nullable: false),
                    RiskRulesRiskRuleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimRiskRules", x => new { x.ClaimsClaimId, x.RiskRulesRiskRuleId });
                    table.ForeignKey(
                        name: "FK_ClaimRiskRules_Claims_ClaimsClaimId",
                        column: x => x.ClaimsClaimId,
                        principalTable: "Claims",
                        principalColumn: "ClaimId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaimRiskRules_RiskRules_RiskRulesRiskRuleId",
                        column: x => x.RiskRulesRiskRuleId,
                        principalTable: "RiskRules",
                        principalColumn: "RiskRuleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClaimRiskRules_RiskRulesRiskRuleId",
                table: "ClaimRiskRules",
                column: "RiskRulesRiskRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_CustomerId",
                table: "Claims",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaimRiskRules");

            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.DropTable(
                name: "RiskRules");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
