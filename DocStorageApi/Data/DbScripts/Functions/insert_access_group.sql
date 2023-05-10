CREATE OR REPLACE FUNCTION insert_access_group (
  p_name varchar(50),
  p_status bool
)
RETURNS integer AS $$
DECLARE
    v_access_group_id uuid;
    rows_affected integer;
BEGIN
  BEGIN
    INSERT INTO document_access_users (name, status, created_at, updated_at)
    VALUES (p_name, p_status, current_timestamp, current_timestamp)
    RETURNING id INTO v_access_group_id;

    GET DIAGNOSTICS rows_affected = ROW_COUNT;
  EXCEPTION 
    WHEN unique_violation THEN
      RAISE NOTICE 'The access group already exists.';
      rows_affected = -1;
    WHEN others THEN
      RAISE NOTICE 'An unknown error has occurred.';
      rows_affected = 0;
  END;

  RETURN rows_affected;
END;
$$ LANGUAGE plpgsql;
