CREATE OR REPLACE FUNCTION update_access_group (
    p_id uuid,
    p_name varchar(50),
    p_status boolean
)
RETURNS integer AS $$
DECLARE
    rows_affected integer;
BEGIN
    UPDATE access_groups 
    SET 
        name = p_name,
        status = p_status,
        updated_at = current_timestamp
    WHERE
        id = p_id;
    
    GET DIAGNOSTICS rows_affected = ROW_COUNT;
    
    RETURN rows_affected;
EXCEPTION 
    WHEN unique_violation THEN
        RAISE NOTICE 'The access group name already exists.';
        RETURN -1;
    WHEN foreign_key_violation THEN
        RAISE NOTICE 'The access group does not exist.';
        RETURN -2;
    WHEN others THEN
        RAISE NOTICE 'An unknown error has occurred.';
        RETURN 0;
END;
$$ LANGUAGE plpgsql;