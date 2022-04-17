## git commit

変更されたファイルをインデックスに追加してコミット。

```
git commit -a
```

## git merge

マージした情報を残す場合、--no-ff を付与。

```
git merge --no-ff branch1
```

mergeの時の動きは、下記がわかりやすい。
* [git mergeの履歴の違いを図にして整理してみた - Qiita](http://qiita.com/digdagdag/items/f8fc75edbe258ef32c98 "git mergeの履歴の違いを図にして整理してみた - Qiita")

ツリー構造を確認。
```
git log --graph
```

## git tag

tag一覧。
```
git tag
```

注釈付きのtag作成。
```
git tag -a v1.0 -m 'v1.0 release'
```

特定のコミットにtag付ける場合、チェックサムを最後に付ける。
```
git tag -a v1.0 -m 'v1.0 release' 172476bbe8bb899f8fd7979d268284cb21df833d
```

タグをリモートに反映。
```
git push origin --tags
```

## git branch

branch切る。
```
git checkout -b fix/hoge
```
上と同じ。
```
git branch fix/hoge
git checkout fix/hoge
```

## git diff

ワーキングツリーとインデックスの差分。
```
git diff
```

`--cached`でインデックスとHEADの差分。(addしたものとHEADを比較したいとき)
```
git diff --cached
```

直前のコミットのdiff
```
git diff HEAD^ HEAD
```

## 変更を戻す

ローカルでの変更を元に戻す。
```
git checkout .
```

addしたのを戻す。
```
git reset HEAD
```

## git config

とりあえずまず最初に。
```
git config --global user.name "nanashi gonbe"
git config --global user.email nanashi@example.com
```
