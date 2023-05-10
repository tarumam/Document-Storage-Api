CREATE OR REPLACE FUNCTION assign_user_to_document(
  p_user_id UUID,
  p_document_id UUID,
  p_granted_by_user UUID
)
RETURNS integer AS $$
DECLARE
    rows_affected integer;
BEGIN
    INSERT INTO document_access_users (user_id, document_id, granted_by_user)
    VALUES (p_user_id, p_document_id, p_granted_by_user);

    GET DIAGNOSTICS rows_affected = ROW_COUNT;

    RETURN rows_affected;

  EXCEPTION 
    WHEN unique_violation THEN
        RAISE EXCEPTION 'The user has already been granted access to this document.';
    WHEN foreign_key_violation  THEN
        RAISE EXCEPTION 'The user or document ID provided does not exist.';
    WHEN OTHERS THEN
        RAISE EXCEPTION 'An unexpected error occurred.';
END;
$$ LANGUAGE plpgsql;
