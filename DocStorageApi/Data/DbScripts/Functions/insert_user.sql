CREATE OR REPLACE FUNCTION insert_user(
    p_name varchar(50),
    p_password varchar(100),
    p_role varchar(10),
    p_status boolean
)
RETURNS uuid AS $$
DECLARE
    new_user_id uuid;
BEGIN

        INSERT INTO users (name, password, role, status)
        VALUES (p_name, p_password, p_role, p_status)
        RETURNING id INTO new_user_id;

        RETURN new_user_id;
    EXCEPTION 
        WHEN unique_violation THEN
            RAISE EXCEPTION 'The user with name % already exists.', p_name;
        WHEN others THEN
            RAISE EXCEPTION 'An unknown error has occurred. Additional information: %', p_additional_info;

END;
$$ LANGUAGE plpgsql;
