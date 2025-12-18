using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back_Quiz.Migrations
{
    /// <inheritdoc />
    public partial class CorrectedModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuizTemplates_QuizTemplateId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_QuizTemplateId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "QuizTemplateId",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "QuizTemplates",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "QuizTemplates",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestionCount",
                table: "QuizTemplates",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeLimitSeconds",
                table: "QuizTemplates",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "QuizTemplates");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "QuizTemplates");

            migrationBuilder.DropColumn(
                name: "QuestionCount",
                table: "QuizTemplates");

            migrationBuilder.DropColumn(
                name: "TimeLimitSeconds",
                table: "QuizTemplates");

            migrationBuilder.AddColumn<Guid>(
                name: "QuizTemplateId",
                table: "Questions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizTemplateId",
                table: "Questions",
                column: "QuizTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuizTemplates_QuizTemplateId",
                table: "Questions",
                column: "QuizTemplateId",
                principalTable: "QuizTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
