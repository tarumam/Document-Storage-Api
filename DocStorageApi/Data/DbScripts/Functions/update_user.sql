CREATE OR REPLACE FUNCTION update_user(
    p_user_id uuid,
    p_name varchar(50),
    p_password varchar(1000),
    p_role varchar(50),
    p_status bool
)
RETURNS integer AS $$
DECLARE
    rows_affected integer;
BEGIN

    UPDATE users
    SET 
        name = p_name,
        password = p_password,
        role = p_role,
        status = p_status,
        updated_at = current_timestamp
    WHERE id = p_user_id;

    GET DIAGNOSTICS rows_affected = ROW_COUNT;

    RETURN rows_affected;
END;
$$ LANGUAGE plpgsql;