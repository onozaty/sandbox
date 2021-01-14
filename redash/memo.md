# Redash

* https://github.com/getredash/redash

## CentOSでのインストール

Docker 使ったインストール方法が下記で記載されている。

* https://redash.io/help/open-source/dev-guide/docker

事前準備で Docker のインストール。

```
yum-config-manager --add-repo https://download.docker.com/linux/centos/docker-ce.repo
yum install -y docker-ce
```

```
curl -L "https://github.com/docker/compose/releases/download/1.27.4/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
chmod +x /usr/local/bin/docker-compose
```

Node.jsのインストール。(LTSの14)

```
curl -sL https://rpm.nodesource.com/setup_14.x | bash -
yum install -y nodejs
```

Redashをクローンするために、git のインストール。

```
yum install -y git
```

```
git clone https://github.com/getredash/redash.git
cd redash/
```

```
docker-compose up -d
```

エラーになった。

```
INFO: pip is looking at multiple versions of atsd-client to determine which version is compatible with other requirements. This could take a while.
ERROR: Cannot install -r requirements_all_ds.txt (line 12), -r requirements_all_ds.txt (line 21) and -r requirements_all_ds.txt (line 34) because these package versions have conflicting dependencies.
ERROR: ResolutionImpossible: for help visit https://pip.pypa.io/en/latest/user_guide/#fixing-conflicting-dependencies

The conflict is caused by:
    atsd-client 3.0.5 depends on python-dateutil
    azure-kusto-data 0.0.35 depends on python-dateutil>=2.8.0
    dql 0.5.26 depends on python-dateutil<2.7.0

To fix this you could try to:
1. loosen the range of package versions you've specified
2. remove package versions to allow pip attempt to solve the dependency conflict

ERROR: Service 'server' failed to build : The command '/bin/sh -c if [ "x$skip_ds_deps" = "x" ] ; then pip install -r requirements_all_ds.txt ; else echo "Skipping pip install -r requirements_all_ds.txt" ; fi' returned a non-zero code: 1
```

## Ubuntuでのインストール

Ubuntu だと、下記にあるスクリプトで全部インストールできる模様なので、そちらに切り替える。

* https://github.com/getredash/setup

```
sudo apt-get install git
```

インストール先のディレクトリを作っておく。

```
sudo mkdir /opt/redash
```

```
git clone https://github.com/getredash/setup.git
sudo sh setup/setup.sh
```

インストール完了。

```
Creating redash_adhoc_worker_1     ... done
Creating redash_server_1           ... done
Creating redash_scheduled_worker_1 ... done
Creating redash_scheduler_1        ... done
Creating redash_nginx_1            ... done
```

5000番ポートでアクセスできる。

* http://192.168.33.10:5000/
