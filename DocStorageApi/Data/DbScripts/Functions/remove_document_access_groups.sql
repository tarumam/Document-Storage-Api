CREATE OR REPLACE FUNCTION remove_document_access_groups(
    p_document_id uuid,
    p_access_group_id uuid
)
RETURNS integer AS $$
DECLARE
    rows_affected integer;
BEGIN
    DELETE FROM document_access_groups
    WHERE document_id = p_document_id AND access_group_id = p_access_group_id;

	GET DIAGNOSTICS rows_affected = ROW_COUNT;

    RETURN rows_affected;

EXCEPTION 
    WHEN others THEN
        RAISE NOTICE 'An unknown error has occurred.';
        RETURN -1;
END;
$$ LANGUAGE plpgsql;