using FluentMigrator;

namespace DDDEastAnglia.DatabaseMigrations.Migrations
{
    [Migration(20190609)]
    public class AddGuidIdToUsers : Migration
    {
        public override void Up()
        {
            Alter.Table("UserProfiles")
                .AddColumn("Id").AsGuid().NotNullable().WithDefault(SystemMethods.NewGuid).SetExistingRowsTo(SystemMethods.NewGuid);

        }

        public override void Down()
        {
            Delete.Column("Id")
                .FromTable("UserProfiles");
        }
    }
}
