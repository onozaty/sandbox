## ドキュメント

* [Azure Functions のドキュメント \| Microsoft Learn](https://learn.microsoft.com/ja-jp/azure/azure-functions/)

## クイックスタート

Visual Studio 2022をインストールして試してみる。

* [クイック スタート: Visual Studio を使用して Azure で初めての C\# 関数を作成する \| Microsoft Learn](https://learn.microsoft.com/ja-jp/azure/azure-functions/functions-create-your-first-function-visual-studio?tabs=in-process)

## Azure Monitor Log にクエリを発行

SDK使うと簡単に書ける。

* [Azure Monitor Query client library for \.NET \- Azure for \.NET Developers \| Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/api/overview/azure/Monitor.Query-readme?view=azure-dotnet#logs-query)

`DefaultAzureCredential`を使うことで、ローカルでもAzure上でも同じように使える。

Azure上では、マネージドIDを利用して認証する。

* [マネージド ID \- Azure App Service \| Microsoft Learn](https://learn.microsoft.com/ja-jp/azure/app-service/overview-managed-identity?tabs=portal%2Chttp)

アプリ登録で、クライアントシークレットでもいける。  
その場合には、Log Analytics側でもアプリに対してロールを割り当てる必要があるので注意。

* [API アクセスと認証 \- Azure Monitor \| Microsoft Learn](https://learn.microsoft.com/ja-jp/azure/azure-monitor/logs/api/access-api?view=azuremgmtcdn-fluent-1.0.0#set-up-authentication)

## 環境変数

* [Azure Functions を使用する C\# クラス ライブラリ関数を開発する \| Microsoft Learn](https://learn.microsoft.com/ja-jp/azure/azure-functions/functions-dotnet-class-library?tabs=v4%2Ccmd#environment-variables)

ローカルの時には`local.settings.json`で設定。
Azure上では、構成→アプリケーション設定から設定しておく。

