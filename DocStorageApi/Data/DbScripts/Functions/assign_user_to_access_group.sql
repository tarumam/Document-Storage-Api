CREATE OR REPLACE FUNCTION assign_user_to_access_group(
  p_user_id UUID,
  p_access_group_id UUID,
  p_granted_by UUID
)
RETURNS integer AS $$
DECLARE
    rows_affected integer;
BEGIN
    INSERT INTO 
        user_access_groups (user_id, access_group_id, granted_by_user)
    VALUES 
        (p_user_id, p_access_group_id, p_granted_by);

    GET DIAGNOSTICS rows_affected = ROW_COUNT;

    RETURN rows_affected;
    
    EXCEPTION 
        WHEN unique_violation THEN
            RAISE NOTICE 'The user access group already exists.';
            RETURN -2;
        WHEN foreign_key_violation THEN
            RAISE NOTICE 'The user or access group does not exist.';
            RETURN -3;
        WHEN others THEN
            RAISE NOTICE 'An unknown error has occurred.';
            RETURN -1;

END;
$$ LANGUAGE plpgsql;