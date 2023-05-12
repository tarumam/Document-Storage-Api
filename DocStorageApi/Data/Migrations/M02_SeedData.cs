using FluentMigrator;

namespace DocStorageApi.Data.Migrations
{
    [Migration(202304260002, "Seed data")]
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

                                INSERT INTO users (name, password, salt, status, role)
                                VALUES
                                    ('admin@admin.com', 'b57889e85cae8be473c001c572672dc9b9b0c0ee0eb6579f968934ed253aa0cd', '9d84e9d5-904f-4f55-ae0a-7c47ca818e6a', true, 'Admin'),
                                    ('manager@manager.com', '7a0d0aed730feacdb9197ab8fb8fc6f1f203c0d843544ff773e4f74d3b67d4e4', '599366e2-b5b3-4011-8fe9-5b14450ec0c1', true, 'Manager'),
                                    ('regular@regular.com', '9575339efa0a9fccc91dbc503a5edf7892439ebfb9b5ad417c405334365e307d', '57363b8f-325a-4db8-9874-66b3b2a30144', true, 'Regular');

                                WITH admin_id AS (
                                    SELECT id FROM users u WHERE name = 'admin@admin.com'
                                ), sys_admin_group_id AS (
                                    SELECT id FROM access_groups ag WHERE name = 'System Admins'
                                ), sr_manager_group_id AS (
                                    SELECT id FROM access_groups ag WHERE name = 'Senior Managers'
                                ), op_group_id AS (
                                    SELECT id FROM access_groups ag WHERE name = 'Operational'
                                )
                                INSERT INTO user_access_groups (user_id, access_group_id, granted_by_user)
                                VALUES
                                    ((SELECT id FROM Users WHERE name = 'admin@admin.com'), (SELECT id FROM sys_admin_group_id), (SELECT id FROM admin_id)),
                                    ((SELECT id FROM Users WHERE name = 'manager@manager.com'), (SELECT id FROM sr_manager_group_id), (SELECT id FROM admin_id)),
                                    ((SELECT id FROM Users WHERE name = 'regular@regular.com'), (SELECT id FROM op_group_id), (SELECT id FROM admin_id));

                                INSERT INTO documents (file_path, name, category, description, posted_at, status, created_by_user) 
                                VALUES
                                    ('5df6a2a6-e5c4-4a4b-bb63-9e94b3cc4cf2.txt', 'testFileName', 'category', 'description', current_timestamp, true, (SELECT id FROM Users WHERE name = 'admin@admin.com')),
                                    ('invalid_file_1', 'invalidFileName1', 'category', 'description', current_timestamp, true, (SELECT id FROM Users WHERE name = 'admin@admin.com')),
                                    ('invalid_file_2', 'invalidFileName2', 'category', 'description', current_timestamp, true, (SELECT id FROM Users WHERE name = 'admin@admin.com'));

                                INSERT INTO document_access_groups (access_group_id, document_id, granted_by_user)
                                VALUES
                                    ((SELECT id FROM access_groups WHERE name = 'System Admins'), (SELECT id FROM documents WHERE file_path = '5df6a2a6-e5c4-4a4b-bb63-9e94b3cc4cf2.txt'), (SELECT id FROM users WHERE name = 'admin@admin.com')),
                                    ((SELECT id FROM access_groups WHERE name = 'Operational'), (SELECT id FROM documents WHERE file_path = 'invalid_file_1'), (SELECT id FROM users WHERE name = 'admin@admin.com')),
                                    ((SELECT id FROM access_groups WHERE name = 'Senior Managers'), (SELECT id FROM documents WHERE file_path = 'invalid_file_2'), (SELECT id FROM users WHERE name = 'admin@admin.com'));


                                INSERT INTO document_access_users (user_id, document_id, granted_by_user)
                                    SELECT u.id, d.id, (SELECT id FROM Users WHERE name = 'admin@admin.com')
                                    FROM users u
                                    INNER JOIN documents d ON d.file_path = 'invalid_file_1'
                                    WHERE u.name = 'manager@manager.com';

                                INSERT INTO document_access_users (user_id, document_id, granted_by_user)
                                    SELECT u.id, d.id, (SELECT id FROM Users WHERE name = 'admin@admin.com')
                                    FROM users u
                                    INNER JOIN documents d ON d.file_path = 'invalid_file_2'
                                    WHERE u.name = 'regular@regular.com';
                            end $$;
                        ");
        }
    }
}
