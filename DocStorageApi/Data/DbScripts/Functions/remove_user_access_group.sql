CREATE OR REPLACE FUNCTION remove_user_access_group (
    p_user_id uuid, 
    p_access_group_id uuid
) 
RETURNS integer 
AS $$
DECLARE
    rows_affected integer;
BEGIN
   
        DELETE FROM 
            user_access_groups
        WHERE 
            user_id = p_user_id 
            AND access_group_id = p_access_group_id;

        GET DIAGNOSTICS rows_affected = ROW_COUNT;

    RETURN rows_affected;
END;
$$ LANGUAGE plpgsql;