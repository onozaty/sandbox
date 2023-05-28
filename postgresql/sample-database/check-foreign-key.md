* PostgreSQL 15.3

## メモ

### pg_constraint

```
dvdrental=# \d pg_constraint
                Table "pg_catalog.pg_constraint"
     Column     |     Type     | Collation | Nullable | Default
----------------+--------------+-----------+----------+---------
 oid            | oid          |           | not null |
 conname        | name         |           | not null |
 connamespace   | oid          |           | not null |
 contype        | "char"       |           | not null |
 condeferrable  | boolean      |           | not null |
 condeferred    | boolean      |           | not null |
 convalidated   | boolean      |           | not null |
 conrelid       | oid          |           | not null |
 contypid       | oid          |           | not null |
 conindid       | oid          |           | not null |
 conparentid    | oid          |           | not null |
 confrelid      | oid          |           | not null |
 confupdtype    | "char"       |           | not null |
 confdeltype    | "char"       |           | not null |
 confmatchtype  | "char"       |           | not null |
 conislocal     | boolean      |           | not null |
 coninhcount    | integer      |           | not null |
 connoinherit   | boolean      |           | not null |
 conkey         | smallint[]   |           |          |
 confkey        | smallint[]   |           |          |
 conpfeqop      | oid[]        |           |          |
 conppeqop      | oid[]        |           |          |
 conffeqop      | oid[]        |           |          |
 confdelsetcols | smallint[]   |           |          |
 conexclop      | oid[]        |           |          |
 conbin         | pg_node_tree | C         |          |
Indexes:
    "pg_constraint_oid_index" PRIMARY KEY, btree (oid)
    "pg_constraint_conname_nsp_index" btree (conname, connamespace)
    "pg_constraint_conparentid_index" btree (conparentid)
    "pg_constraint_conrelid_contypid_conname_index" UNIQUE CONSTRAINT, btree (conrelid, contypid, conname)
    "pg_constraint_contypid_index" btree (contypid)
```

* https://www.postgresql.jp/document/15/html/catalog-pg-constraint.html

### information_schema.table_constraints

```
dvdrental=# \d information_schema.table_constraints
                       View "information_schema.table_constraints"
       Column       |               Type                | Collation | Nullable | Default
--------------------+-----------------------------------+-----------+----------+---------
 constraint_catalog | information_schema.sql_identifier |           |          |
 constraint_schema  | information_schema.sql_identifier |           |          |
 constraint_name    | information_schema.sql_identifier |           |          |
 table_catalog      | information_schema.sql_identifier |           |          |
 table_schema       | information_schema.sql_identifier |           |          |
 table_name         | information_schema.sql_identifier |           |          |
 constraint_type    | information_schema.character_data |           |          |
 is_deferrable      | information_schema.yes_or_no      |           |          |
 initially_deferred | information_schema.yes_or_no      |           |          |
 enforced           | information_schema.yes_or_no      |           |          |
 nulls_distinct     | information_schema.yes_or_no      |           |          |
```

* https://www.postgresql.jp/document/15/html/infoschema-table-constraints.html

