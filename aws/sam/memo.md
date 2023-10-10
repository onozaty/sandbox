* https://docs.aws.amazon.com/ja_jp/serverless-application-model/latest/developerguide/what-is-sam.html

Vagrant 上で試す。Ubuntu 22.04 を利用。

## Docker のインストール

* https://docs.docker.com/engine/install/ubuntu/

```
# Add Docker's official GPG key:
sudo apt-get update
sudo apt-get install ca-certificates curl gnupg
sudo install -m 0755 -d /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg
sudo chmod a+r /etc/apt/keyrings/docker.gpg

# Add the repository to Apt sources:
echo \
  "deb [arch="$(dpkg --print-architecture)" signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu \
  "$(. /etc/os-release && echo "$VERSION_CODENAME")" stable" | \
  sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
sudo apt-get update
```

```
sudo apt-get install docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
```

## SAM CLI のインストール

* https://docs.aws.amazon.com/serverless-application-model/latest/developerguide/install-sam-cli.html

```
wget https://github.com/aws/aws-sam-cli/releases/latest/download/aws-sam-cli-linux-x86_64.zip
```

```
unzip aws-sam-cli-linux-x86_64.zip -d sam-installation
```

```
sudo ./sam-installation/install
```

```
sam --version
```

## チュートリアル

下記でHello Worldサンプルアリケーションを試してみる。

* https://docs.aws.amazon.com/ja_jp/serverless-application-model/latest/developerguide/serverless-getting-started-hello-world.html

pipがインストールされている必要があるようなので、pipをインストールする。

```
sudo apt install python3-pip
```

あと、`template.yaml`の`Runtime: python3.9`を`Runtime: python3.10`に変える必要があった。  
(UbuntuでインストールされているのがPython 3.10だったので)

aws cli もインストールして、クレデンシャルを設定する必要があった。

* https://docs.aws.amazon.com/ja_jp/cli/latest/userguide/getting-started-install.html

### TypeScriptで

nodejsのインストール。

```
sudo apt update
sudo apt install nodejs npm
sudo npm -g install n
sudo n stable
sudo apt purge nodejs npm
sudo apt autoremove
```

`sam build`で`esbuild`が無いと怒られたのでインストール。

```
sudo npm install -g esbuild
```