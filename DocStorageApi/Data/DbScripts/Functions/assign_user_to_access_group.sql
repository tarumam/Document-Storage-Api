CREATE OR REPLACE FUNCTION assign_user_to_access_group(
  p_user_id UUID,
  p_access_group_id UUID,
  p_granted_by UUID
)
RETURNS integer AS $$
DECLARE
    rows_affected integer;
BEGIN
    
    INSERT INTO user_access_groups (user_id, access_group_id, granted_by_user)
    VALUES (p_user_id, p_access_group_id, p_granted_by)
    ON CONFLICT DO NOTHING;

    GET DIAGNOSTICS rows_affected = ROW_COUNT;

    RETURN rows_affected;
END;
$$ LANGUAGE plpgsql;
