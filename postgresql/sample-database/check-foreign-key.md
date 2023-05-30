* PostgreSQL 15.3

## メモ

### pg_constraint

制約に関する情報が格納されるテーブル。

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

### pg_index

インデックス情報が格納されるテーブル。

```
dvdrental=# \d pg_index
                     Table "pg_catalog.pg_index"
       Column        |     Type     | Collation | Nullable | Default
---------------------+--------------+-----------+----------+---------
 indexrelid          | oid          |           | not null |
 indrelid            | oid          |           | not null |
 indnatts            | smallint     |           | not null |
 indnkeyatts         | smallint     |           | not null |
 indisunique         | boolean      |           | not null |
 indnullsnotdistinct | boolean      |           | not null |
 indisprimary        | boolean      |           | not null |
 indisexclusion      | boolean      |           | not null |
 indimmediate        | boolean      |           | not null |
 indisclustered      | boolean      |           | not null |
 indisvalid          | boolean      |           | not null |
 indcheckxmin        | boolean      |           | not null |
 indisready          | boolean      |           | not null |
 indislive           | boolean      |           | not null |
 indisreplident      | boolean      |           | not null |
 indkey              | int2vector   |           | not null |
 indcollation        | oidvector    |           | not null |
 indclass            | oidvector    |           | not null |
 indoption           | int2vector   |           | not null |
 indexprs            | pg_node_tree | C         |          |
 indpred             | pg_node_tree | C         |          |
Indexes:
    "pg_index_indexrelid_index" PRIMARY KEY, btree (indexrelid)
    "pg_index_indrelid_index" btree (indrelid)
```

* https://www.postgresql.jp/document/15/html/catalog-pg-index.html

### pg_class

テーブル、インデックス、ビューなどの情報が格納されるテーブル。

```
dvdrental=# \d pg_class
                     Table "pg_catalog.pg_class"
       Column        |     Type     | Collation | Nullable | Default
---------------------+--------------+-----------+----------+---------
 oid                 | oid          |           | not null |
 relname             | name         |           | not null |
 relnamespace        | oid          |           | not null |
 reltype             | oid          |           | not null |
 reloftype           | oid          |           | not null |
 relowner            | oid          |           | not null |
 relam               | oid          |           | not null |
 relfilenode         | oid          |           | not null |
 reltablespace       | oid          |           | not null |
 relpages            | integer      |           | not null |
 reltuples           | real         |           | not null |
 relallvisible       | integer      |           | not null |
 reltoastrelid       | oid          |           | not null |
 relhasindex         | boolean      |           | not null |
 relisshared         | boolean      |           | not null |
 relpersistence      | "char"       |           | not null |
 relkind             | "char"       |           | not null |
 relnatts            | smallint     |           | not null |
 relchecks           | smallint     |           | not null |
 relhasrules         | boolean      |           | not null |
 relhastriggers      | boolean      |           | not null |
 relhassubclass      | boolean      |           | not null |
 relrowsecurity      | boolean      |           | not null |
 relforcerowsecurity | boolean      |           | not null |
 relispopulated      | boolean      |           | not null |
 relreplident        | "char"       |           | not null |
 relispartition      | boolean      |           | not null |
 relrewrite          | oid          |           | not null |
 relfrozenxid        | xid          |           | not null |
 relminmxid          | xid          |           | not null |
 relacl              | aclitem[]    |           |          |
 reloptions          | text[]       | C         |          |
 relpartbound        | pg_node_tree | C         |          |
Indexes:
    "pg_class_oid_index" PRIMARY KEY, btree (oid)
    "pg_class_relname_nsp_index" UNIQUE CONSTRAINT, btree (relname, relnamespace)
    "pg_class_tblspc_relfilenode_index" btree (reltablespace, relfilenode)
```

* https://www.postgresql.jp/document/15/html/catalog-pg-class.html

### pg_attribute

テーブルの列情報が格納されたテーブル。

```
dvdrental=# \d pg_attribute
               Table "pg_catalog.pg_attribute"
     Column     |   Type    | Collation | Nullable | Default
----------------+-----------+-----------+----------+---------
 attrelid       | oid       |           | not null |
 attname        | name      |           | not null |
 atttypid       | oid       |           | not null |
 attstattarget  | integer   |           | not null |
 attlen         | smallint  |           | not null |
 attnum         | smallint  |           | not null |
 attndims       | integer   |           | not null |
 attcacheoff    | integer   |           | not null |
 atttypmod      | integer   |           | not null |
 attbyval       | boolean   |           | not null |
 attalign       | "char"    |           | not null |
 attstorage     | "char"    |           | not null |
 attcompression | "char"    |           | not null |
 attnotnull     | boolean   |           | not null |
 atthasdef      | boolean   |           | not null |
 atthasmissing  | boolean   |           | not null |
 attidentity    | "char"    |           | not null |
 attgenerated   | "char"    |           | not null |
 attisdropped   | boolean   |           | not null |
 attislocal     | boolean   |           | not null |
 attinhcount    | integer   |           | not null |
 attcollation   | oid       |           | not null |
 attacl         | aclitem[] |           |          |
 attoptions     | text[]    | C         |          |
 attfdwoptions  | text[]    | C         |          |
 attmissingval  | anyarray  |           |          |
Indexes:
    "pg_attribute_relid_attnum_index" PRIMARY KEY, btree (attrelid, attnum)
    "pg_attribute_relid_attnam_index" UNIQUE CONSTRAINT, btree (attrelid, attname)
```

* https://www.postgresql.jp/document/15/html/catalog-pg-attribute.html

### information_schema.table_constraints

ユーザが所有する制約が参照できるビュー。

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

### information_schema.key_column_usage

制約によって制限をうけている全ての列が参照できるビュー。

```
dvdrental=# dvdrental=# \d information_schema.key_column_usage
                             View "information_schema.key_column_usage"
            Column             |                Type                | Collation | Nullable | Default
-------------------------------+------------------------------------+-----------+----------+---------
 constraint_catalog            | information_schema.sql_identifier  |           |          |
 constraint_schema             | information_schema.sql_identifier  |           |          |
 constraint_name               | information_schema.sql_identifier  |           |          |
 table_catalog                 | information_schema.sql_identifier  |           |          |
 table_schema                  | information_schema.sql_identifier  |           |          |
 table_name                    | information_schema.sql_identifier  |           |          |
 column_name                   | information_schema.sql_identifier  |           |          |
 ordinal_position              | information_schema.cardinal_number |           |          |
 position_in_unique_constraint | information_schema.cardinal_number |           |          |
```

* https://www.postgresql.jp/document/15/html/infoschema-key-column-usage.html

