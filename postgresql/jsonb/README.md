# SQL/JSON パス式

- https://www.postgresql.jp/document/15/html/functions-json.html#FUNCTIONS-SQLJSON-PATH
- https://www.postgresql.jp/document/15/html/datatype-json.html#DATATYPE-JSONPATH

JSONPath式で、XPath のように、JSON から値を取り出す式を書ける。  
JSONPathは下記のようなものになる。

* [JSONPath \- XPath for JSON](https://goessner.net/articles/JsonPath/)

PostgreSQLが全部に対応しているわけではなく(例えば`..`はPostgreSQLでは利用できない)、全て一緒ではないので要注意。
細かいところは、PostgreSQLのマニュアルを確認すること。

パス式は jsonpath データ型として実装されている。SQL では文字列リテラルとしてシングルクォートで囲んで記述。

下記の演算子と

* `@?`
* `@@`

下記の関数で利用できる。

- `jsonb_path_query`
- `jsonb_path_query_array`
- `jsonb_path_query_first`
- `jsonb_path_exists`
- `jsonb_path_match`

上記以外にも末尾に`_tz`が付いた同様の関数(`jsonb_path_query_tz`など)がある。  
これらは`datetime`メソッドを使った式で、タイムゾーンが省略されている場合に実行環境のタイムゾーンをデフォルトとして扱う。

## 基本

下記のような JSON を例として。

```json
{
  "id": 1,
  "name": "Taro",
  "email": "taro@example.com",
  "groups": [
    {
      "id": 1,
      "name": "GroupA"
    },
    {
      "id": 2,
      "name": "GroupB"
    }
  ]
}
```

```sql
CREATE TABLE jsons(
  json jsonb
);

INSERT INTO jsons VALUES ('{
  "id": 1,
  "name": "Taro",
  "email": "taro@example.com",
  "groups": [
    {
      "id": 1,
      "name": "GroupA"
    },
    {
      "id": 2,
      "name": "GroupB"
    }
  ]
}');
```

### アクセサ

JSON自体を指定する変数として`$`変数がある。
jsonpathとして`$`を指定すると、JSON全体が返却される。

```sql
test=> SELECT jsonb_path_query(json, '$') FROM jsons;
                                                       jsonb_path_query
------------------------------------------------------------------------------------------------------------------------------
 {"id": 1, "name": "Taro", "email": "taro@example.com", "groups": [{"id": 1, "name": "GroupA"}, {"id": 2, "name": "GroupB"}]}
(1 row)
```

`$`を起点として、`.key`アクセサで各要素を参照できる。  
`name`の値を取得する場合には、`$.name`と書く。

```sql
test=> SELECT jsonb_path_query(json, '$.name') FROM jsons;
 jsonb_path_query
------------------
 "Taro"
(1 row)
```

配列は`[]`で参照できる。

`groups`の2番目の`name`を取得する際には、`$.groups[1].name`と書く。  
JSONのオブジェクトのプロパティを参照しているのと同じ感じになる。

```sql
test=> SELECT jsonb_path_query(json, '$.groups[1].name') FROM jsons;
 jsonb_path_query
------------------
 "GroupB"
(1 row)
```

その他のアクセサは下記を参照。

* https://www.postgresql.jp/document/15/html/datatype-json.html#TYPE-JSONPATH-ACCESSORS

### 演算子とメソッド

jsonpathでは、演算子とメソッドも使える。

たとえば`id`の値に10加算する際には `+ 10` で加算できる。

```sql
test=> SELECT jsonb_path_query(json, '$.id + 10') FROM jsons;
 jsonb_path_query
------------------
 11
(1 row)
```

`size` メソッドで配列の要素数が取れる。

```sql
test=> SELECT jsonb_path_query(json, '$.groups.size()') FROM jsons;
 jsonb_path_query
------------------
 2
(1 row)
```

その他の演算子とメソッドは下記を参照。

* https://www.postgresql.jp/document/15/html/functions-json.html#FUNCTIONS-SQLJSON-OP-TABLE

### フィルター式

フィルター式では、クエリとして対象となった項目に対して、一致した条件のものに絞り込むことができる。  
パス `?` 条件 といった形で書く。
条件では `@` が現在の項目となるので、それに対して条件を記載する。

例えば、`groups`の`id`が`2`の項目を取り出したい場合には、下記のように書く。  
`[*]`は配列内の全項目を取り出すもの。

```sql
test=> SELECT jsonb_path_query(json, '$.groups[*] ? (@.id == 2)') FROM jsons;
      jsonb_path_query
-----------------------------
 {"id": 2, "name": "GroupB"}
(1 row)
```

その他のフィルター式は下記を参照。

* https://www.postgresql.jp/document/15/html/functions-json.html#FUNCTIONS-SQLJSON-FILTER-EX-TABLE

