CREATE OR REPLACE FUNCTION remove_user_access_group (
    p_user_id uuid, 
    p_access_group_id uuid
) 
RETURNS integer 
AS $$
DECLARE
    rows_affected integer;
BEGIN
    IF EXISTS (
        SELECT 1 FROM user_access_groups uag
        JOIN document_access_groups dag  on uag.id = dag.access_group_id
        WHERE uag.user_id = p_user_id AND uag.access_group_id = p_access_group_id
    ) THEN
        RAISE EXCEPTION 'Cannot delete the record because there are dependent records in document_access_users table';
    ELSE
        DELETE FROM 
            user_access_groups
        WHERE 
            user_id = p_user_id 
            AND access_group_id = p_access_group_id;

        GET DIAGNOSTICS rows_affected = ROW_COUNT;
    END IF;

    RETURN rows_affected;

EXCEPTION 

    WHEN foreign_key_violation THEN
        RAISE EXCEPTION 'Foreign key violation error: %', SQLERRM;
        RETURN -1;
END;
$$ LANGUAGE plpgsql;