下記のような形で`text`型のカラムにINDEXを作成した場合、大きなテキストを入れるとエラーになることがあります。

```sql
CREATE TABLE table1 (
  value text
);

CREATE INDEX ON table1(value);
```

投入したテキストのサイズによってエラーの内容がことなります。

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
