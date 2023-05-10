CREATE OR REPLACE FUNCTION update_token_id(p_user_id uuid, p_token_id varchar(100))
    RETURNS integer AS $$
DECLARE
    rows_affected integer;
BEGIN
    UPDATE users
    SET token_id = p_token_id, updated_at = current_timestamp
    WHERE id = p_user_id;

    GET DIAGNOSTICS rows_affected = ROW_COUNT;

    RETURN rows_affected;

EXCEPTION 
    WHEN others THEN
        RAISE NOTICE 'An unknown error has occurred.';
        RETURN -1;
END;
$$ LANGUAGE plpgsql;
