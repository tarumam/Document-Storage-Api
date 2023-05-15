using DocStorageApi.Data.Entities;
using FluentMigrator;

namespace DocStorageApi.Data.Migrations
{
    [Migration(20230000001, "Create intial database")]
    public class M01_InitialTables_20230000001 : Migration
    {
        public override void Down()
        {
            // With dependencies first
            Delete.Table(_Entities.DocumentAccessGroups);
            Delete.Table(_Entities.DocumentAccessUsers);
            Delete.Table(_Entities.Documents);
            Delete.Table(_Entities.UserAccessGroups);

            // No dependencies
            Delete.Table(_Entities.AccessGroups);
            Delete.Table(_Entities.Users);

            Execute.Sql("DROP EXTENSION IF EXISTS \"uuid-ossp\"");
        }

        public override void Up()
        {
            Execute.Sql("SET TIME ZONE 'UTC'");

            Execute.Sql("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"");

            Create.Table(_Entities.Users)
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey().WithDefault(SystemMethods.NewGuid)
            .WithColumn("name").AsString(50).NotNullable()
            .WithColumn("password").AsString(100).NotNullable()
            .WithColumn("token_id").AsString(100).Nullable()
            .WithColumn("status").AsBoolean()
            .WithColumn("role").AsString(10).NotNullable().WithDefaultValue("Regular")
            .WithColumn("created_at").AsDateTimeOffset().NotNullable().WithDefaultValue(SystemMethods.CurrentUTCDateTime)
            .WithColumn("updated_at").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);

            Create.UniqueConstraint("users_unique_name")
            .OnTable(_Entities.Users)
            .Columns("name");

            Create.Table(_Entities.AccessGroups)
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey().WithDefault(SystemMethods.NewGuid)
            .WithColumn("name").AsString(50).NotNullable()
            .WithColumn("status").AsBoolean()
            .WithColumn("created_at").AsDateTimeOffset().NotNullable().WithDefaultValue(SystemMethods.CurrentUTCDateTime)
            .WithColumn("updated_at").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);
            
            Create.UniqueConstraint("access_groups_unique_name")
            .OnTable(_Entities.AccessGroups)
            .Columns("name");

            Create.Table(_Entities.Documents)
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey().WithDefault(SystemMethods.NewGuid)
            .WithColumn("file_path").AsString(1000).NotNullable()
            .WithColumn("name").AsString(50).Nullable()
            .WithColumn("category").AsString(500).Nullable()
            .WithColumn("description").AsString(500).Nullable()
            .WithColumn("posted_at").AsDateTimeOffset().NotNullable()
            .WithColumn("status").AsBoolean()
            .WithColumn("created_by_user").AsGuid().NotNullable().ForeignKey("fk_users_documents", _Entities.Users, "id")
            .WithColumn("created_at").AsDateTimeOffset().NotNullable().WithDefaultValue(SystemMethods.CurrentUTCDateTime)
            .WithColumn("updated_at").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);

            Create.UniqueConstraint("documents_unique_file_path")
            .OnTable(_Entities.Documents)
            .Columns("file_path");

            // With FKs
            Create.Table(_Entities.UserAccessGroups)
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey().WithDefault(SystemMethods.NewGuid)
            .WithColumn("user_id").AsGuid().NotNullable().ForeignKey("fk_users_user_groups", _Entities.Users, "id")
            .WithColumn("access_group_id").AsGuid().NotNullable().ForeignKey("fk_groups_user_groups", _Entities.AccessGroups, "id")
            .WithColumn("granted_by_user").AsGuid().NotNullable().ForeignKey("fk_granted_by_user", _Entities.Users, "id")
            .WithColumn("created_at").AsDateTimeOffset().NotNullable().WithDefaultValue(SystemMethods.CurrentUTCDateTime);

            Create.UniqueConstraint("uc_user_access_groups_user_id_access_group_id")
            .OnTable(_Entities.UserAccessGroups)
            .Columns("user_id", "access_group_id");

            Create.Table(_Entities.DocumentAccessGroups)
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey().WithDefault(SystemMethods.NewGuid)
            .WithColumn("access_group_id").AsGuid().NotNullable().ForeignKey("fk_groups_documents_access_group", _Entities.AccessGroups, "id")
            .WithColumn("document_id").AsGuid().NotNullable().ForeignKey("fk_documents_document_access_user", _Entities.Documents, "id")
            .WithColumn("granted_by_user").AsGuid().NotNullable().ForeignKey("fk_granted_by_user", _Entities.Users, "id")
            .WithColumn("created_at").AsDateTimeOffset().NotNullable().WithDefaultValue(SystemMethods.CurrentUTCDateTime);

            Create.UniqueConstraint("uc_doc_access_groups_document_id_access_group_id")
            .OnTable(_Entities.DocumentAccessGroups)
            .Columns("document_id", "access_group_id");

            Create.Table(_Entities.DocumentAccessUsers)
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey().WithDefault(SystemMethods.NewGuid)
            .WithColumn("user_id").AsGuid().NotNullable().ForeignKey("fk_users_document_access_user", _Entities.Users, "id")
            .WithColumn("document_id").AsGuid().NotNullable().ForeignKey("fk_documents_doc_access_user", _Entities.Documents, "id")
            .WithColumn("granted_by_user").AsGuid().NotNullable().ForeignKey("fk_granted_by_user", _Entities.Users, "id")
            .WithColumn("created_at").AsDateTimeOffset().NotNullable().WithDefaultValue(SystemMethods.CurrentUTCDateTime);

            Create.UniqueConstraint("uc_doc_access_user_document_id_user_id")
            .OnTable(_Entities.DocumentAccessUsers)
            .Columns("document_id", "user_id");
        }
    }
}
