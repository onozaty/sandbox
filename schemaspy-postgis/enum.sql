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
  )
SELECT
  c.table_name,
  c.column_name,
  CASE
    WHEN c.data_type = 'USER-DEFINED'
    AND ev.enum_name IS NOT NULL THEN ev.enum_name || '(' || ev.enum_labels || ')'
    ELSE c.udt_name
  END AS column_type,
  c.udt_name AS short_column_type
FROM
  information_schema.columns c
  LEFT JOIN enum_values ev ON c.table_schema = ev.table_schema
  AND c.udt_name = ev.enum_name
WHERE
  c.table_schema = 'public';
