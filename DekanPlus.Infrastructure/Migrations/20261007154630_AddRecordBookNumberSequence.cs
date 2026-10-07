using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DekanPlus.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRecordBookNumberSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "record_book_number_seq");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "record_book_number_seq");
        }
    }
}
