CREATE OR REPLACE FUNCTION remove_document_access_user (
    p_user_id UUID,
    p_document_id UUID
)
RETURNS integer AS $$
DECLARE
    rows_affected integer;
BEGIN
    DELETE FROM document_access_users
    WHERE user_id = p_user_id AND document_id = p_document_id;
    
    GET DIAGNOSTICS rows_affected = ROW_COUNT;

    RETURN rows_affected;
END;
$$ LANGUAGE plpgsql;
