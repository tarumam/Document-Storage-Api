CREATE OR REPLACE FUNCTION assign_group_to_document(
  p_access_group_id UUID,
  p_document_id UUID,
  p_granted_by_user UUID
)
RETURNS integer 
AS $$
DECLARE
    rows_affected integer;
BEGIN
    INSERT INTO document_access_groups (access_group_id, document_id, granted_by_user)
    VALUES (p_access_group_id, p_document_id, p_granted_by_user);

    GET DIAGNOSTICS rows_affected = ROW_COUNT;

    IF rows_affected = 0 THEN
      RAISE EXCEPTION 'Failed to insert into document_access_groups.';
    END IF;

    return rows_affected;

  EXCEPTION 
    WHEN unique_violation THEN
      RAISE EXCEPTION 'An attempt was made to add a duplicate record.';
    WHEN foreign_key_violation THEN
      RAISE EXCEPTION 'A foreign key constraint was violated.';
    WHEN others THEN
      RAISE EXCEPTION 'An error occurred while inserting into document_access_groups.';

END;
$$ LANGUAGE plpgsql;
