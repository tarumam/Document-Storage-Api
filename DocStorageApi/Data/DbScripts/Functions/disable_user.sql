CREATE OR REPLACE FUNCTION disable_user (
  p_user_id UUID
)
RETURNS integer AS $$
DECLARE
    rows_affected integer;
BEGIN
    UPDATE users 
    SET     
        status = false,
        updated_at = current_timestamp
    WHERE 
        id = p_user_id AND status = true;

    GET DIAGNOSTICS rows_affected = ROW_COUNT;

    RETURN rows_affected;
END;
$$ LANGUAGE plpgsql;