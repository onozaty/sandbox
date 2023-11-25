## SQL/JSON パス式

- https://www.postgresql.jp/document/15/html/functions-json.html#FUNCTIONS-SQLJSON-PATH
- https://www.postgresql.jp/document/15/html/datatype-json.html#DATATYPE-JSONPATH

XPath のように、JSON から値を取り出す式を書ける。  
パス式は jsonpath データ型として実装されている。SQL では文字列リテラルとしてシングルクォートで囲んで記述。

下記の関数で利用できる。

- `jsonb_path_query`
- `jsonb_path_query_array`
- `jsonb_path_query_first`
- `jsonb_path_exists`
- `jsonb_path_match`

上記以外にも末尾に`_tz`が付いた同様の関数(`jsonb_path_query_tz`など)がある。  
これらは`datetime`メソッドを使った式で、タイムゾーンが省略されている場合に実行環境のタイムゾーンをデフォルトとして扱う。

### 基本

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

