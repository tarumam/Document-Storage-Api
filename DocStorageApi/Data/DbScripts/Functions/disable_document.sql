CREATE OR REPLACE FUNCTION disable_document(
  p_document_id UUID
)
RETURNS integer AS $$
DECLARE
    rows_affected integer;
BEGIN
    UPDATE documents 
    SET    
        status = false,
        updated_at = current_timestamp
    WHERE 
        id = p_document_id AND status = true;

    GET DIAGNOSTICS rows_affected = ROW_COUNT;

    RETURN rows_affected;

    EXCEPTION 
        WHEN others THEN
            RAISE EXCEPTION 'An unknown error has occurred.';
END;
$$ LANGUAGE plpgsql;
