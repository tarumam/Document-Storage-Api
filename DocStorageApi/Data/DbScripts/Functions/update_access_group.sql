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
END;
$$ LANGUAGE plpgsql;