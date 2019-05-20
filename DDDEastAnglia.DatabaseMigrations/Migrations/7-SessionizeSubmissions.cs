using FluentMigrator;

namespace DDDEastAnglia.DatabaseMigrations.Migrations
{
    [Migration(20190610)]
    public class SessionizeSubmissions : Migration
    {
        public override void Up()
        {
            Alter.Table("Conferences")
                .AddColumn("SessionizeId").AsString(10).Nullable()
                .AddColumn("SessionizeName").AsString(50).Nullable();
        }

        public override void Down()
        {
            Delete.Column("SessionizeName")
                .FromTable("Conferences");

            Delete.Column("SessionizeId")
                .FromTable("Conferences");
        }
    }
}
