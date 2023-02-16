## Fromでユーザのデフォルトを設定

```
If(
    EditForm1.Mode = FormMode.New,
    {
        DisplayName: User().FullName,
        Claims: Concatenate(
            "i:0#.f|membership|",
            User().Email
        ),
        '@odata.type': "#Microsoft.Azure.Connectors.SharePoint.SPListExpandedUser",
        Email: User().Email
    },
    ThisItem.{列名}
)
```

`FullName`は姓名逆になる場合があるので、その場合には`Office365ユーザー`コネクタ使って取得すると良い。

```
Office365ユーザー.UserProfile(User().Email)
```

## JSONのパース

* [Power Apps の ParseJSON 関数 \(実験的\) \- Power Platform \| Microsoft Learn](https://learn.microsoft.com/ja-jp/power-platform/power-fx/reference/function-parsejson)

設定から「近日公開の機能」→「実験段階」→「ParseJSON関数と型のないオブジェクト」をオンに。

型変換を書いてあげる必要あり。

```json
[
  { "id": 1, "name": "One"},
  { "id": 2, "name": "Two"}
]
```

```
ForAll( ParseJSON( JsonString ), { id: Value(ThisRecord.id), name: Text(ThisRecord.name) })
```
