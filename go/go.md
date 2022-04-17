## go mod

```
go mod init github.com/onozaty/xxxxx
go build
```

## goreleaser

```
goreleaser init
```

`.goreleaser.yml`は、Windowsのアーカイブの設定だけ変える。

```yaml
    format_overrides:
      - goos: windows
        format: zip
```

試すだけ。

```
goreleaser --snapshot --skip-publish --rm-dist
```

tag切ってリリース。

```
set GITHUB_TOKEN=xxxxxxxxxx
goreleaser --rm-dist
```

