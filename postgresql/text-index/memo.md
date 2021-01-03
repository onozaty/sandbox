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

## 対応策

もしも`text`型のカラムに対してINDEXを利用した検索を行いたい場合には、`md5`や`substring`などを利用して文字列のサイズを制限した関数INDEXを作成するといった方法があります。  
ただ、検索条件に関数INDEXと同じ条件を指定しなければならないため、かなり面倒です。

```sql
CREATE INDEX ON table1(md5(value));
```

```sql
INSERT INTO table1 
SELECT
  repeat((seq % 10)::text, seq)
FROM
  generate_series(1, 1000000) AS seq;
```

### データ準備

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