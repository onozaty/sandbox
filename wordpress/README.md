公式サイトでのWordpressインストールのドキュメント

* https://developer.wordpress.org/advanced-administration/before-install/

## 環境

* Ubuntu 22.04

## ミドルウエアのインストール

### Apache

```
apt -y install apache2
```

### PHP

```
apt -y install php8.1
```

### MySQL

```
apt -y install mysql-server
```

## DBの作成

* DB: `wordpress`
* ユーザ: `wordpressuser`
* パスワード: `password`

``` sql
CREATE DATABASE wordpress;
CREATE USER wordpressuser@localhost IDENTIFIED BY 'password';
GRANT ALL PRIVILEGES ON wordpress.* TO wordpressuser@localhost;
```
