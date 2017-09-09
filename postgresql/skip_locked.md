# SKIP LOCKED

SKIP LOCKED は PostgreSQL 9.5 から入った新機能。

* [What's new in PostgreSQL 9.5 - PostgreSQL wiki](https://wiki.postgresql.org/wiki/What%27s_new_in_PostgreSQL_9.5#SKIP_LOCKED "What's new in PostgreSQL 9.5 - PostgreSQL wiki")
* [PostgreSQL: Documentation: 9.6: SELECT](https://www.postgresql.org/docs/9.6/static/sql-select.html#SQL-FOR-UPDATE-SHARE "PostgreSQL: Documentation: 9.6: SELECT")

これを使うと、FOR UPDATEの際に既に行ロックが取得されているレコードを除外することができる。すなわち、他のトランザクションによる行ロックを待たなくて良くなる。

## サンプル

idというカラムを持つidsというテーブルを作成し、3レコード作成しておく。

```
testdb=> CREATE TABLE ids AS SELECT generate_series(1, 3) AS id;
SELECT 3
testdb=> SELECT * FROM ids;
 id
----
  1
  2
  3
(3 rows)
```

ひとつ目のトランザクションでは、FOR UPDATEでid=1の行をロックする。

```
testdb=> BEGIN;
BEGIN
testdb=> SELECT * FROM ids WHERE id = 1 FOR UPDATE;
 id
----
  1
(1 row)
```

ふたつ目のトランザクションでは、全行をFOR UPDATEで取得する。id=1のレコードがロックされているので、全行とろうとしてもロックで取得できない。

```
testdb=> SELECT * FROM ids FOR UPDATE NOWAIT;
ERROR:  could not obtain lock on row in relation "ids"
```

これをSKIP LOCKEDを付けると、ロックされているid=1のレコードを除外して取得できる。

```
testdb=> SELECT * FROM ids FOR UPDATE SKIP LOCKED;
 id
----
  2
  3
(2 rows)
```

## キューとして

これが便利なのは、キューとして使うときだと思う。というか、それ以外で使い道を思いついていない。

SKIP LOCKEDを使うことによって、他のトランザクションで取得されている行以外を他のトランザクションの完了を待たずに取得できる。すなわち待ちが少なくて済む。

ということで、キューとしての使用方法を試してみる。queuesというテーブルを作り、idの昇順で取り出すことにする。

```
testdb=> CREATE TABLE queues (
testdb(>   id SERIAL,
testdb(>   message TEXT,
testdb(>   PRIMARY KEY(id)
testdb(> );
CREATE TABLE
testdb=> INSERT INTO queues(message) SELECT 'message' || id FROM generate_series(1, 3) AS id;
INSERT 0 3
testdb=> SELECT * FROM queues;
 id | message
----+----------
  1 | message1
  2 | message2
  3 | message3
(3 rows)
```

ORDER BY + LIMIT 1で優先度順で先頭の1件取り出し、FOR UPDATE SKIP LOCKED を付けてあげるだけ。

1つめのトランザクションで実行。
```
testdb=> BEGIN;
BEGIN
testdb=> SELECT * FROM queues ORDER BY id LIMIT 1 FOR UPDATE SKIP LOCKED;
 id | message
----+----------
  1 | message1
(1 row)
```

1つめのトランザクションが完了する前に、2つめのトランザクションで同じSQLを実行。待つことも無くLOCKされていない行から取得できる。

```
testdb=> BEGIN;
BEGIN
testdb=> SELECT * FROM queues ORDER BY id LIMIT 1 FOR UPDATE SKIP LOCKED;
 id | message
----+----------
  2 | message2
(1 row)
```

ただ、これだとqueuesテーブルから取り出したレコードを消せていないので、取り出した行を別途DELETEしてあげる必要がある。

そのまま同一のトランザクション内で取り出したレコードに対してDELETEでも問題ないが、RETURINGを使うことによって、該当行の内容取得とDELETEをひとつのクエリで実行できる。

```
testdb=> DELETE FROM queues WHERE id = (SELECT id FROM queues ORDER BY id LIMIT
1 FOR UPDATE SKIP LOCKED)
testdb-> RETURNING *;
 id | message
----+----------
  2 | message2
(1 row)

DELETE 1
```

RETURNINGは、INSERTやUPDATE、DELETEで処理したレコードの情報を取得する構文。シーケンスによって払い出されたものをINSERTの戻りで取得したいときなどに良く使う。とても強力な構文。

ということで、とても手軽にキューが出来た。

似たような方法で、Advisory Locks(pg_try_advisory_lock)を使う方法があるが、ORDER BYと組み合わせるとpg_try_advisory_lockを全行評価、すなわち全行ロックしてしまうので、優先度付けた取り出しが難しい。なので、9.5以降ならば、SKIP LOCKEDを使った形のほうが簡単だと思う。

* [PostgreSQL: Documentation: 9.6: Explicit Locking](https://www.postgresql.org/docs/9.6/static/explicit-locking.html#ADVISORY-LOCKS "PostgreSQL: Documentation: 9.6: Explicit Locking")
* [PostgreSQL で簡易に MQ - Mi manca qualche giovedi`?](http://d.hatena.ne.jp/n_shuyo/20090415/mq "PostgreSQL で簡易に MQ - Mi manca qualche giovedi`?")
