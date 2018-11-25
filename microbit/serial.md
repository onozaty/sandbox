# シリアル通信

## ドライバのインストール

下記からインストール。

* [Windows serial configuration \- Handbook \| Mbed](https://os.mbed.com/handbook/Windows-serial-configuration)

インストールしたら、デバイスマネージャ上でポート番号確認。

## TeraTerm で接続

シリアルで先ほど確認したポートで接続。

接続したら、設定からレートを`115200`に変更。

## 加速度(絶対値)を出力

加速度(絶対値)を出力。

```javascript
basic.forever(function () {
    serial.writeValue("g", input.acceleration(Dimension.Strength))
})
```

うまく出力されない場合には、試しにUSB抜き差ししてみる。

何も動かしていない状態の場合、加速度(絶対値)は`1019`あたりだった。振ってみると、最大で`3000`くらいまでいく。
