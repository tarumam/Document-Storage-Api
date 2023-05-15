CREATE OR REPLACE FUNCTION insert_document(
    p_file_path varchar(1000),
    p_name varchar(50),
    p_category varchar(500),
    p_description varchar(500),
    p_posted_at timestamp with time zone,
    p_status boolean,
    p_created_by_user UUID
)
RETURNS uuid AS $$
DECLARE
    new_document_id uuid;
BEGIN
        INSERT INTO documents(
            file_path, 
            name, 
            category, 
            description, 
            posted_at, 
            status, 
            created_by_user
        )
        VALUES(
            p_file_path, 
            p_name, 
            p_category, 
            p_description, 
            p_posted_at, 
            p_status, 
            p_created_by_user
        )
        ON CONFLICT DO NOTHING
        RETURNING id INTO new_document_id;
        
        RETURN new_document_id;

END;
$$ LANGUAGE plpgsql;