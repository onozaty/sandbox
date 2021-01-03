下記のような形で`text`型のカラムにINDEXを作成した場合、大きなテキストを入れるとエラーになることがあります。

```sql
CREATE TABLE table1 (
  value text
);

CREATE INDEX ON table1(value);
```

投入したテキストのサイズによってエラーの内容が異なります。

```sql
testdb=# INSERT INTO table1 VALUES(repeat('a', 300000));
ERROR:  index row size 3456 exceeds btree version 4 maximum 2704 for index "table1_value_idx"
DETAIL:  Index row references tuple (0,1) in relation "table1".
HINT:  Values larger than 1/3 of a buffer page cannot be indexed.
Consider a function index of an MD5 hash of the value, or use full text indexing.
```

```sql
testdb=# INSERT INTO table1 VALUES(repeat('a', 1000000));
ERROR:  index row requires 11464 bytes, maximum size is 8191
```

## データ準備

```sql
CREATE TABLE table1 (
  id integer,
  value text
);

INSERT INTO table1 
SELECT
  seq,
  repeat(seq::text, seq % 1000)
FROM
  generate_series(1, 10000000) AS seq;

INSERT INTO table1 VALUES(-1, repeat('a', 300000));
INSERT INTO table1 VALUES(-2, repeat('a', 1000000));
```

```sql
testdb=# \timing
Timing is on.
testdb=# CREATE TABLE table1 (
testdb(#   id integer,
testdb(#   value text
testdb(# );
CREATE TABLE
Time: 6.143 ms
testdb=# INSERT INTO table1
testdb-# SELECT
testdb-#   seq,
testdb-#   repeat(seq::text, seq % 1000)
testdb-# FROM
testdb-#   generate_series(1, 10000000) AS seq;
INSERT 0 10000000
Time: 273198.073 ms (04:33.198)
testdb=# INSERT INTO table1 VALUES(-1, repeat('a', 300000));
INSERT 0 1
Time: 17.072 ms
testdb=# INSERT INTO table1 VALUES(-2, repeat('a', 1000000));
INSERT 0 1
Time: 25.221 ms
```

```sql
testdb=# SELECT count(*) FROM table1;
  count
----------
 10000002
(1 row)

Time: 2829.149 ms (00:02.829)
```

```sql
testdb=# EXPLAIN ANALYZE SELECT id FROM table1 WHERE value = '1';
                                                        QUERY PLAN
--------------------------------------------------------------------------------------------------------------------------
 Gather  (cost=1000.00..515777.44 rows=1 width=4) (actual time=3106.957..3110.776 rows=1 loops=1)
   Workers Planned: 2
   Workers Launched: 2
   ->  Parallel Seq Scan on table1  (cost=0.00..514777.34 rows=1 width=4) (actual time=2072.206..3104.169 rows=0 loops=3)
         Filter: (value = '1'::text)
         Rows Removed by Filter: 3333334
 Planning Time: 0.058 ms
 Execution Time: 3110.792 ms
(8 rows)

Time: 3111.104 ms (00:03.111)
```

```sql
testdb=# SELECT id FROM table1 WHERE value = repeat('a', 1000000);
 id
----
 -2
(1 row)

Time: 3248.298 ms (00:03.248)
```

## 通常のb-tree INDEX

```sql
CREATE INDEX ON table1(value);
```

エラーになる。

```sql
testdb=# CREATE INDEX ON table1(value);
ERROR:  index row requires 11464 bytes, maximum size is 8191
CONTEXT:  parallel worker
Time: 271992.175 ms (04:31.992)
```

## md5関数INDEX

```sql
CREATE INDEX ON table1(md5(value));
```

```sql
testdb=# CREATE INDEX ON table1(md5(value));
CREATE INDEX
Time: 94736.893 ms (01:34.737)
```

```sql
testdb=# SELECT id FROM table1 WHERE value = repeat('a', 1000000) AND md5(value) = md5(repeat('a', 1000000));
 id
----
 -2
(1 row)

Time: 54.118 ms
```

```sql
testdb=# EXPLAIN ANALYZE SELECT id FROM table1 WHERE value = '1' AND md5(value) = md5('1');
                                                          QUERY PLAN
------------------------------------------------------------------------------------------------------------------------------
 Bitmap Heap Scan on table1  (cost=1819.56..146875.59 rows=1 width=4) (actual time=1.648..1.650 rows=1 loops=1)
   Recheck Cond: (md5(value) = 'c4ca4238a0b923820dcc509a6f75849b'::text)
   Filter: (value = '1'::text)
   Heap Blocks: exact=1
   ->  Bitmap Index Scan on table1_md5_idx  (cost=0.00..1819.56 rows=50000 width=0) (actual time=0.844..0.845 rows=1 loops=1)
         Index Cond: (md5(value) = 'c4ca4238a0b923820dcc509a6f75849b'::text)
 Planning Time: 0.096 ms
 Execution Time: 1.670 ms
(8 rows)

Time: 12.226 ms
```

INDEXのサイズは590,536,704バイト。

```sql
testdb=# SELECT *, pg_relation_size(indexname::regclass) FROM pg_indexes WHERE tablename = 'table1';
 schemaname | tablename |   indexname    | tablespace |                               indexdef                                | pg_relation_size
------------+-----------+----------------+------------+-----------------------------------------------------------------------+------------------
 public     | table1    | table1_md5_idx |            | CREATE INDEX table1_md5_idx ON public.table1 USING btree (md5(value)) |        590536704
(1 row)

Time: 13.024 ms
```

## substring関数INDEX (32文字)

```sql
CREATE INDEX ON table1(substring(value from 1 for 32));
```

```sql
testdb=# CREATE INDEX ON table1(substring(value from 1 for 32));
CREATE INDEX
Time: 78851.593 ms (01:18.852)
```

```sql
testdb=# SELECT id FROM table1 WHERE value = repeat('a', 1000000) AND substring(value from 1 for 32) = substring(repeat('a', 1000000) from 1 for 32);
 id
----
 -2
(1 row)

Time: 54.850 ms
```

```sql
testdb=# EXPLAIN ANALYZE SELECT id FROM table1 WHERE value = '1' AND substring(value from 1 for 32) = substring('1' from 1 for 32);
                                                             QUERY PLAN
------------------------------------------------------------------------------------------------------------------------------------
 Bitmap Heap Scan on table1  (cost=1815.56..146871.59 rows=1 width=4) (actual time=6.278..6.280 rows=1 loops=1)
   Recheck Cond: ("substring"(value, 1, 32) = '1'::text)
   Filter: (value = '1'::text)
   Heap Blocks: exact=1
   ->  Bitmap Index Scan on table1_substring_idx  (cost=0.00..1815.56 rows=50000 width=0) (actual time=6.267..6.267 rows=1 loops=1)
         Index Cond: ("substring"(value, 1, 32) = '1'::text)
 Planning Time: 0.092 ms
 Execution Time: 6.302 ms
(8 rows)

Time: 6.855 ms
```

INDEXのサイズは 588,972,032バイト。md5と合わせて32文字にしているので、ほとんど変わらない。

```sql
testdb=# SELECT *, pg_relation_size(indexname::regclass) FROM pg_indexes WHERE tablename = 'table1';
 schemaname | tablename |      indexname       | tablespace |                                          indexdef                                          | pg_relation_size
------------+-----------+----------------------+------------+--------------------------------------------------------------------------------------------+------------------
 public     | table1    | table1_substring_idx |            | CREATE INDEX table1_substring_idx ON public.table1 USING btree ("substring"(value, 1, 32)) |        588972032
(1 row)

Time: 2.016 ms
```

## substring関数INDEX (10文字)

```sql
CREATE INDEX ON table1(substring(value from 1 for 10));
```

```sql
testdb=# CREATE INDEX ON table1(substring(value from 1 for 10));
CREATE INDEX
Time: 66769.009 ms (01:06.769)
```

```sql
testdb=# SELECT id FROM table1 WHERE value = repeat('a', 1000000) AND substring(value from 1 for 10) = substring(repeat('a', 1000000) from 1 for 10);
 id
----
 -2
(1 row)

Time: 54.731 ms
```

```sql
testdb=# EXPLAIN ANALYZE SELECT id FROM table1 WHERE value = '1' AND substring(value from 1 for 10) = substring('1' from 1 for 10);
                                                             QUERY PLAN
------------------------------------------------------------------------------------------------------------------------------------
 Bitmap Heap Scan on table1  (cost=1147.44..146203.47 rows=1 width=4) (actual time=0.039..0.040 rows=1 loops=1)
   Recheck Cond: ("substring"(value, 1, 10) = '1'::text)
   Filter: (value = '1'::text)
   Heap Blocks: exact=1
   ->  Bitmap Index Scan on table1_substring_idx  (cost=0.00..1147.43 rows=50000 width=0) (actual time=0.033..0.033 rows=1 loops=1)
         Index Cond: ("substring"(value, 1, 10) = '1'::text)
 Planning Time: 0.091 ms
 Execution Time: 0.060 ms
(8 rows)

Time: 0.456 ms
```

INDEXのサイズは315,318,272バイト。文字数が減ったことによって、サイズも減っている。

```sql
testdb=# SELECT *, pg_relation_size(indexname::regclass) FROM pg_indexes WHERE tablename = 'table1';
 schemaname | tablename |      indexname       | tablespace |                                          indexdef                                          | pg_relation_size
------------+-----------+----------------------+------------+--------------------------------------------------------------------------------------------+------------------
 public     | table1    | table1_substring_idx |            | CREATE INDEX table1_substring_idx ON public.table1 USING btree ("substring"(value, 1, 10)) |        315318272
(1 row)

Time: 1.354 ms
```
