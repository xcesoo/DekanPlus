using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DekanPlus.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentConcurrencyToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // xmin is a PostgreSQL system column that every row already has, nothing to create.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
