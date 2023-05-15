CREATE OR REPLACE FUNCTION insert_access_group (
  p_name varchar(50),
  p_status bool
)
RETURNS uuid AS $$
DECLARE
    v_access_group_id uuid;
BEGIN
    INSERT INTO document_access_users (name, status, created_at, updated_at)
    VALUES (p_name, p_status, current_timestamp, current_timestamp)
    ON CONFLICT (name) DO NOTHING
    RETURNING id INTO v_access_group_id;

    IF v_access_group_id IS NULL THEN
        RAISE EXCEPTION 'The group name provided is unavailable.';
    END IF;

  RETURN new_user_id;
END;
$$ LANGUAGE plpgsql;
