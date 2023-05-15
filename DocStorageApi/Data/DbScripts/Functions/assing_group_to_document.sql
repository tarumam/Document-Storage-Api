CREATE OR REPLACE FUNCTION assign_group_to_document(
  p_access_group_id UUID,
  p_document_id UUID,
  p_granted_by_user UUID
)
RETURNS integer AS $$
DECLARE
    rows_affected integer;
BEGIN
    INSERT INTO document_access_groups (access_group_id, document_id, granted_by_user)
    VALUES (p_access_group_id, p_document_id, p_granted_by_user)
    ON CONFLICT DO NOTHING;

    GET DIAGNOSTICS rows_affected = ROW_COUNT;

    RETURN rows_affected;
END;
$$ LANGUAGE plpgsql;



