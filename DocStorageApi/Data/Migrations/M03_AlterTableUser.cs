using DocStorageApi.Data.Entities;
using FluentMigrator;

namespace DocStorageApi.Data.Migrations
{
    [Migration(20230000003, description:"Alter table Users adding a new column 'salt'", BreakingChange = false)]
    public class M03_AlterTableUsers_20230000003 : Migration
    {
        public override void Down()
        {
            Delete.Column("salt").FromTable(_Entities.Users);
        }

        public override void Up()
        {
            // Set as nullable due to the post creation of the column
            Alter.Table(_Entities.Users)
            .AddColumn("salt").AsString().Nullable();
        }
    }
}
