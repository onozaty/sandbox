WITH
  enum_values AS (
    SELECT
      pg_namespace.nspname AS table_schema,
      pg_type.typname AS enum_name,
      string_agg(
        pg_enum.enumlabel,
        ', '
        ORDER BY
          pg_enum.enumsortorder
      ) AS enum_labels
    FROM
      pg_type
      INNER JOIN pg_enum ON (pg_type.oid = pg_enum.enumtypid)
      INNER JOIN pg_namespace ON (pg_type.typnamespace = pg_namespace.oid)
    WHERE
      pg_type.typtype = 'e'
    GROUP BY
      pg_namespace.nspname,
      pg_type.typname
  ),
  geometry_info AS (
    SELECT
      f_table_schema AS table_schema,
      f_table_name AS table_name,
      f_geometry_column AS column_name,
      type || ', ' || srid AS geom_details
    FROM
      geometry_columns
  )
SELECT
  c.table_name,
  c.column_name,
  CASE
    WHEN c.data_type = 'USER-DEFINED'
    AND ev.enum_name IS NOT NULL THEN ev.enum_name || '(' || ev.enum_labels || ')'
    WHEN c.data_type = 'USER-DEFINED'
    AND c.udt_name = 'geometry' THEN 'geometry(' || g.geom_details || ')'
    ELSE c.udt_name
  END AS column_type,
  c.udt_name AS short_column_type
FROM
  information_schema.columns c
  LEFT JOIN enum_values ev ON c.table_schema = ev.table_schema
  AND c.udt_name = ev.enum_name
  LEFT JOIN geometry_info g ON c.table_schema = g.table_schema
  AND c.table_name = g.table_name
  AND c.column_name = g.column_name
WHERE
  c.table_schema = 'public';
