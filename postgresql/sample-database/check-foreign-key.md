* PostgreSQL 15.3

## メモ

### ARRAY を番号振ったうえで、行として扱う方法

これができると楽なので考える。

```sql
CREATE TABLE table1 AS 
SELECT 
  *
FROM
(
  VALUES
    (1, ARRAY['a', 'b']),
    (3, ARRAY['a', 'b', 'b', 'a']),
    (2, ARRAY['z', 'y', 'x'])
) AS t (id, names)
;
```

```sql
SELECT
  *
FROM
  table1
;
```

```
 id |   names
----+-----------
  1 | {a,b}
  3 | {a,b,b,a}
  2 | {z,y,x}
(3 rows)
```

単にunnestする。

```sql
SELECT
  *
  , unnest(names) AS name
FROM
  table1
;
```

```
 id |   names   | name
----+-----------+------
  1 | {a,b}     | a
  1 | {a,b}     | b
  3 | {a,b,b,a} | a
  3 | {a,b,b,a} | b
  3 | {a,b,b,a} | b
  3 | {a,b,b,a} | a
  2 | {z,y,x}   | z
  2 | {z,y,x}   | y
  2 | {z,y,x}   | x
(9 rows)
```

これにARRAY内での順序に基づく番号を振りたい。

```
 id |   names   | name | order
----+-----------+------+-------
  1 | {a,b}     | a    |     1
  1 | {a,b}     | b    |     2
  2 | {z,y,x}   | z    |     1
  2 | {z,y,x}   | y    |     2
  2 | {z,y,x}   | x    |     3
  3 | {a,b,b,a} | a    |     1
  3 | {a,b,b,a} | b    |     2
  3 | {a,b,b,a} | b    |     3
  3 | {a,b,b,a} | a    |     4
```

#### unnest + row_number

とりあえず、全体で通番振る。  
unnest したものにWindows関数の`row_number`使って全体に対して通番振る形。


```sql
SELECT
  *
  , row_number() OVER() AS order
FROM (
  SELECT
    *
    , unnest(names) AS name
  FROM
    table1
) temp
;
```

```
 id |   names   | name | order
----+-----------+------+-------
  1 | {a,b}     | a    |     1
  1 | {a,b}     | b    |     2
  3 | {a,b,b,a} | a    |     3
  3 | {a,b,b,a} | b    |     4
  3 | {a,b,b,a} | b    |     5
  3 | {a,b,b,a} | a    |     6
  2 | {z,y,x}   | z    |     7
  2 | {z,y,x}   | y    |     8
  2 | {z,y,x}   | x    |     9
(9 rows)
```

ARRAYでの順序は崩れずに全体としての通番となった。ただ、ORDER BY指定していないので、これが必ずこの順番になることが保証されるのかはわからなかった。
unnestは配列の順番に展開されることは保証されている(ドキュメント上にも記載)が、その後にrow_numberでのクエリで保証されるかがわからない。  
(Sortが発生しないので、大丈夫そうな気はするが、保証されるものなのか判断できない)

OVER で PARTITION BY 指定したら、各ARRAY毎の順番になりそうだなとも思ったが、これだと順序が狂う場合があった。 PARTITION BY を行うことでSortが発生するため。

```sql
SELECT
  *
  , row_number() OVER(PARTITION BY id) AS order
FROM (
  SELECT
    *
    , unnest(names) AS name
  FROM
    table1
) temp
;
```

```
 id |   names   | name | order
----+-----------+------+-------
  1 | {a,b}     | a    |     1
  1 | {a,b}     | b    |     2
  2 | {z,y,x}   | x    |     1
  2 | {z,y,x}   | z    |     2
  2 | {z,y,x}   | y    |     3
  3 | {a,b,b,a} | a    |     1
  3 | {a,b,b,a} | a    |     2
  3 | {a,b,b,a} | b    |     3
  3 | {a,b,b,a} | b    |     4
(9 rows)
```

全体に対して`row_number() OVER()`で振った後に、さらに`row_number() OVER(PARTITION BY id ORDER BY order)`のような形で、期待する結果を作ることはできた。

```sql
SELECT
  id
  , names
  , name
  , row_number() OVER(PARTITION BY id ORDER BY overall_order) AS order
FROM (
  SELECT
    *
    , row_number() OVER() AS overall_order
  FROM (
    SELECT
      *
      , unnest(names) AS name
    FROM
      table1
  ) temp1
) temp2
;
```

```
 id |   names   | name | order
----+-----------+------+-------
  1 | {a,b}     | a    |     1
  1 | {a,b}     | b    |     2
  2 | {z,y,x}   | z    |     1
  2 | {z,y,x}   | y    |     2
  2 | {z,y,x}   | x    |     3
  3 | {a,b,b,a} | a    |     1
  3 | {a,b,b,a} | b    |     2
  3 | {a,b,b,a} | b    |     3
  3 | {a,b,b,a} | a    |     4
(9 rows)
```

#### 再帰SQL

配列の先頭から順に再帰SQLで辿っていく。

```sql
WITH RECURSIVE previous AS(
  -- 配列の先頭を起点に(PostgreSQLの配列のインデックスは1から)
  SELECT
    *
    , names[1] AS name
    , 1 AS order 
  FROM
    table1
  UNION ALL
  SELECT
    table1.*
    , table1.names[previous.order + 1] AS name
    , previous.order + 1 AS order 
  FROM
    previous
    INNER JOIN table1
      USING(id)
  WHERE
    table1.names[previous.order + 1] IS NOT NULL
)
SELECT
  *
FROM
  previous
ORDER BY
  previous.id
  , previous.order
;
```

```
 id |   names   | name | order
----+-----------+------+-------
  1 | {a,b}     | a    |     1
  1 | {a,b}     | b    |     2
  2 | {z,y,x}   | z    |     1
  2 | {z,y,x}   | y    |     2
  2 | {z,y,x}   | x    |     3
  3 | {a,b,b,a} | a    |     1
  3 | {a,b,b,a} | b    |     2
  3 | {a,b,b,a} | b    |     3
  3 | {a,b,b,a} | a    |     4
(9 rows)
```


### 使えそうなテーブル

#### pg_constraint

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

#### pg_index

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

#### pg_class

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

#### pg_attribute

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

#### information_schema.table_constraints

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

#### information_schema.key_column_usage

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

