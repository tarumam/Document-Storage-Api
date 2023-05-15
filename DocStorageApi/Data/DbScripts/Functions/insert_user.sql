CREATE OR REPLACE FUNCTION insert_user(
    p_name varchar(50),
    p_password varchar(100),
    p_role varchar(10),
    p_salt varchar(10),
    p_status boolean
)
RETURNS uuid AS $$
DECLARE
    new_user_id uuid;
BEGIN
    INSERT INTO users (name, password, role, salt, status)
        VALUES (p_name, p_password, p_role, p_salt, p_status)
        ON CONFLICT DO NOTHING
        RETURNING id INTO new_user_id;

    RETURN new_user_id;
END;
$$ LANGUAGE plpgsql;