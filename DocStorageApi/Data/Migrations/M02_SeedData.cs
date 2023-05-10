using FluentMigrator;

namespace DocStorageApi.Data.Migrations
{
    [Migration(202304260002)]
    public class M02_SeedData_202304260002 : Migration
    {
        public override void Down()
        {

        }

        public override void Up()
        {
            Execute.Sql(@" DO $$
                            BEGIN
                                INSERT INTO access_groups (name, status)
                                VALUES
                                    ('System Admins', true),
                                    ('Senior Managers', true),
                                    ('Operational', true);

                                INSERT INTO users (name, password, status, role)
                                VALUES
                                    ('Admin', '473287f8298dba7163a897908958f7c0eae733e25d2e027992ea2edc9bed2fa8', true, 'Admin'),
                                    ('Manager', '473287f8298dba7163a897908958f7c0eae733e25d2e027992ea2edc9bed2fa8', true, 'Manager'),
                                    ('Regular', '473287f8298dba7163a897908958f7c0eae733e25d2e027992ea2edc9bed2fa8', true, 'Regular');

                                WITH admin_id AS (
                                    SELECT id FROM users u WHERE name = 'Admin'
                                ), sys_admin_group_id AS (
                                    SELECT id FROM access_groups ag WHERE name = 'System Admins'
                                ), sr_manager_group_id AS (
                                    SELECT id FROM access_groups ag WHERE name = 'Senior Managers'
                                ), op_group_id AS (
                                    SELECT id FROM access_groups ag WHERE name = 'Operational'
                                )
                                INSERT INTO user_access_groups (user_id, access_group_id, granted_by_user)
                                VALUES
                                    ((SELECT id FROM Users WHERE name = 'Admin'), (SELECT id FROM sys_admin_group_id), (SELECT id FROM admin_id)),
                                    ((SELECT id FROM Users WHERE name = 'Manager'), (SELECT id FROM sr_manager_group_id), (SELECT id FROM admin_id)),
                                    ((SELECT id FROM Users WHERE name = 'Regular'), (SELECT id FROM op_group_id), (SELECT id FROM admin_id));

                                INSERT INTO documents (file_path, name, category, description, posted_at, status, created_by_user) 
                                VALUES
                                    ('5df6a2a6-e5c4-4a4b-bb63-9e94b3cc4cf2.txt', 'testFileName', 'category', 'description', current_timestamp, true, (SELECT id FROM Users WHERE name = 'Admin')),
                                    ('invalid_file_1', 'invalidFileName1', 'category', 'description', current_timestamp, true, (SELECT id FROM Users WHERE name = 'Admin')),
                                    ('invalid_file_2', 'invalidFileName2', 'category', 'description', current_timestamp, true, (SELECT id FROM Users WHERE name = 'Admin'));

                                INSERT INTO document_access_groups (access_group_id, document_id, granted_by_user)
                                VALUES
                                    ((SELECT id FROM access_groups WHERE name = 'System Admins'), (SELECT id FROM documents WHERE file_path = '5df6a2a6-e5c4-4a4b-bb63-9e94b3cc4cf2.txt'), (SELECT id FROM users WHERE name = 'Admin')),
                                    ((SELECT id FROM access_groups WHERE name = 'Operational'), (SELECT id FROM documents WHERE file_path = 'invalid_file_1'), (SELECT id FROM users WHERE name = 'Admin')),
                                    ((SELECT id FROM access_groups WHERE name = 'Senior Managers'), (SELECT id FROM documents WHERE file_path = 'invalid_file_2'), (SELECT id FROM users WHERE name = 'Admin'));


                                INSERT INTO document_access_users (user_id, document_id, granted_by_user)
                                    SELECT u.id, d.id, (SELECT id FROM Users WHERE name = 'Admin')
                                    FROM users u
                                    INNER JOIN documents d ON d.file_path = 'invalid_file_1'
                                    WHERE u.name = 'Manager';

                                INSERT INTO document_access_users (user_id, document_id, granted_by_user)
                                    SELECT u.id, d.id, (SELECT id FROM Users WHERE name = 'Admin')
                                    FROM users u
                                    INNER JOIN documents d ON d.file_path = 'invalid_file_2'
                                    WHERE u.name = 'Regular';
                            end $$;
                        ");
        }
    }
}
