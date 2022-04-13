# Microsoft Certified: Azure Fundamentals

* [Microsoft Certified: Azure Fundamentals \- Learn \| Microsoft Docs](https://docs.microsoft.com/ja-jp/learn/certifications/azure-fundamentals/)

AZ-900: Microsoft Azure の基礎 に合格すると認定される。

* [試験 AZ\-900: Microsoft Azure Fundamentals \- Learn \| Microsoft Docs](https://docs.microsoft.com/ja-jp/learn/certifications/exams/az-900)

Microsoft Azure Virtual Training Day: Azure Fundamentals を受講すると、無料で受験できる。

## 学習コンテンツ

### ラーニングパス

* [Azure の基礎 第 1 部:Azure の主要概念に関する説明 \(AZ\-900\) \- Learn \| Microsoft Docs](https://docs.microsoft.com/ja-jp/learn/paths/az-900-describe-cloud-concepts/)
* [Azure の基礎 第 2 部:Azure の主要サービスに関する説明 \(AZ\-900\) \- Learn \| Microsoft Docs](https://docs.microsoft.com/ja-jp/learn/paths/az-900-describe-core-azure-services/)
* [Azure の基礎 第 3 部:Azure のコア ソリューションおよび管理ツールに関する説明 \(AZ\-900\) \- Learn \| Microsoft Docs](https://docs.microsoft.com/ja-jp/learn/paths/az-900-describe-core-solutions-management-tools-azure/)
* [Azure の基礎 第 4 部:一般的なセキュリティ機能およびネットワーク セキュリティ機能に関する説明 \(AZ\-900\) \- Learn \| Microsoft Docs](https://docs.microsoft.com/ja-jp/learn/paths/az-900-describe-general-security-network-security-features/)
* [Azure の基礎 第 5 部:ID、ガバナンス、プライバシー、およびコンプライアンス機能に関する説明 \(AZ\-900\) \- Learn \| Microsoft Docs](https://docs.microsoft.com/ja-jp/learn/paths/az-900-describe-identity-governance-privacy-compliance-features/)
* [Azure の基礎 第 6 部:Azure Cost Management およびサービス レベル アグリーメントに関する説明 \(AZ\-900\) \- Learn \| Microsoft Docs](https://docs.microsoft.com/ja-jp/learn/paths/az-900-describe-azure-cost-management-service-level-agreements/)

## メモ

### Azure の主要概念に関する説明

* Azure portal
* Azure Marketplace

* クラウド コンピューティングの種類
    * プライベートクラウド
    * パブリッククラウド
    * ハイブリッドクラウド
* クラウドの利点
    * 高可用性
    * スケーラビリティ
    * 弾力性
        * 自動スケーリング
    * 機敏性
        * 迅速にデプロイ
    * geo ディストリビューション
        * 使用しているリージョンで常に最高のパフォーマンスを保証
    * ディザスター リカバリー
* 費用
    * 資本的支出 (CapEx)
        * 物理インフラストラクチャに費やす初期費用
    * 運用費 (OpEx)
        * 現在サービスや製品に費やしているコスト
* クラウドサービスモデル
    * IaaS
        * Azure Virtual Machines
    * PaaS
        * Azure App Services
    * SaaS
        * Microsoft Office 365

* リソースの組織構造
    * リソース
        * 作成するサービスのインスタンス
    * リソース グループ
        * Azureリソースをデプロイして管理するための論理的なグループ
    * サブスクリプション
    * 管理グループ

* 特殊な Azure リージョン
    * US DoD 中部、US Gov バージニア、US Gov アイオワなど
        * 米国の政府機関やパートナー用にネットワークを物理的および論理的に分離した Azure インスタンス
    * 中国東部、中国北部など
        * Microsoft と 21Vianet 間の特異なパートナーシップを通じて利用
* Azure Availability Zones 可用性ゾーン
    * Azure リージョン内の物理的に分離されたデータセンター
* リージョンのペア
    * 自動的にペアのリージョンにフェールオーバー
* Azure Resource Manager
    * Azure のデプロイおよび管理サービス
* リソースグループ
    * リソースは1つのリソースグループにのみ存在
* Azure サブスクリプション
    * Azureの利用にはサブスクリプションが必要
    * Azureアカウントに複数のサブスクリプションを紐づけられる
    * コストはサブスクリプション単位
* Azure 管理グループ
    * 管理グループはネストできる
    * 管理グループとサブスクリプションの親は1つだけ

### Azure の主要サービスに関する説明

* Azure Virtual Machines
    * IaaS
    * スケーリング
        * 仮想マシン スケール セット
            * 自動的にスケーリング
        * Azure Batch
            * 大規模で並列にバッチジョブを実行
* Azure App Service
    * PaaS
    * Webアプリ、APIアプリ、WebJobs、モバイルアプリ
* Container Instances
    * スケーリングや負荷分散などは無い
* Azure Kubernetes Service
* Functions
    * サーバレス
    * イベント契機で実行
    * 関数の実行中に使用されたCPU時間で課金
* Azure Logic Apps
    * Functionsと似ているが、こちらはコードを書くのでは無くワークフローを定義
* Azure Virtual Desktop


* Azure Virtual Network
    * Azureリソース間の通信
        * 仮想ネットワーク
        * サービスエンドポイント
    * オンプレミスリソースとの通信
        * Azure VPN Gateway
            * ポイント対サイト仮想プライベート ネットワーク
            * サイト間仮想プライベート ネットワーク
                * Azureのネットワークがローカルネットワークにあるように
        * Azure ExpressRoute
            * インターネット経由しない
                * 環境間(IP VPN)接続、ポイントツーポイントのイーサネット接続、共有施設での接続プロバイダーによる仮想交差接続
            * 接続モデル
                * CloudExchange コロケーション
                * ポイント ツー ポイントのイーサネット接続
                * Any-to-Any 接続
            * レイヤー3接続
    * ネットワーク トラフィックのルーティング
        * ルートテーブル
        * Border Gateway Protocol
            * VPNと共に
    * ネットワーク トラフィックのフィルター処理
        * ネットワーク セキュリティ グループ
        * ネットワーク仮想アプライアンス
    * 仮想ネットワークのピアリング
        * 仮想ネットワーク同士をリンク

* Azure Storage
    * Azure ストレージ アカウント作成から
    * Disk Storage
        * Azure 仮想マシンのディスクとして機能
    * Azure Blob Storage
        * オブジェクトストレージ
        * アクセス層
            * ホットアクセス層
            * クールアクセス層
            * アーカイブアクセス層
    * Azure Files
        * ファイル共有

* データベース
    * Azure Cosmos DB
        * NoSQL
        * SQL、MongoDB、Cassandra、Tables、Gremlin API がサポート
    * Azure SQL Database
        * SQL Server
    * Azure Database for MySQL
    * Azure Database for PostgreSQL
        * Hyperscale (Citus) で分散も
    * Azure SQL Managed Instance
        * Azure SQL Database よりオプション等の設定が柔軟
* 分析サービス
    * Azure Synapse Analytics
        * エンタープライズ データ ウェアハウスとビッグ データ分析を組み合わせた無制限の分析サービス
    * Azure HDInsight
        * Apache Spark、Apache Hadoop、Apache Kafka、Apache HBase、Apache Storm、Machine Learning Services などを使って分析
    * Azure Databricks
        * Apache Spark でのビッグ データ分析
    * Azure Data Lake Analytics
        * 超並列データ変換処理プログラムを、U-SQL、R、Python、.NET で容易に開発および実行

* Azure IoT サービス
    * Azure IoT Hub
        * IoTアプリケーションとそれが管理するデバイス間の双方向通信のための中央メッセージハブ
        * レポート管理のためのダッシュボードが必要なければこれで十分
            * REST APIでもレポート取得できる
    * Azure IoT Central
        * ダッシュボード。IoT Hub上に構築
        * デバイステンプレート使うと、サービス側でコーディングすることなくデバイスを接続できる
    * Azure Sphere
        * エンドツーエンドの安全性の高い IoT ソリューションを作成
        * 構成
            * Azure Sphere マイクロコントローラー ユニット (MCU)
            * カスタマイズされた Linux オペレーティング システム
            * Azure Sphere Security Service

* Azure Machine Learning
    * データに接続して、モデルをトレーニング＋テスト
* Azure Cognitive Services
    * 事前に構築された機械学習モデルを使用
    * カテゴリ
        * 言語
            * 言語判定、翻訳など
        * 音声
            * 音声↔テキストの変換
        * 視覚
            * 画像コンテンツの識別
        * 決定
    * Personalizerサービス
        * ユーザのアクションを監視して予測
* Azure Bot Service
    * 人間と同様に質問を理解して返答する仮想エージェントを作成するためのプラットフォーム

* Azure Functions
    * プログラミング言語で書く
    * イベントに応答して実行される1つのメソッドまたは関数をホストできる
        * HTTP要求、キューの新しいメッセージ、タイマーのメッセージなど
    * 自動的にスケーリング
* Azure Logic Apps
    * ローコードまたはノーコード開発プラットフォーム
    * ワークフローを定義
        * APIを介して多数のサービスを接続

* Azure DevOps Services
    * 様々なサービス
        * Azure Repos
        * Azure Boards
        * Azure Pipelines
        * Azure Artifacts
        * Azure Test Plans
    * 細かなアクセス制御ができる
    * プロジェクト管理とレポートで優れる
* GitHub、GitHub Actions
    * OSSでの歴史、パブリックならば
* Azure DevTest Labs
    * 仮想マシンの構築が含まれる。様々なOSでビルド、テストができる。

* Azure portal
    * ユーザインタフェースがある
* Azure mobile app
    * スマートフォンやタブレットからアクセス
* Azure PowerShell
* Azure CLI
    * bash
    * PowerShellとCLIは同じような機能を提供。慣れた方で。
    * 1回きりの用途に向いている
* Azure Resource Manager テンプレート (ARM テンプレート)
    * JSON形式でリソースを定義
    * 繰り返しや並列して実施する場合に有効
    * 検証ステップがある

* Azure Advisor
    * レコメンテーション
        * クラウド環境を改善するための新しいレコメンデーションが利用可能になったときに、アラートを受け取れる
    * カテゴリ
        * 信頼性
        * セキュリティ
        * パフォーマンス
        * コスト
        * オペレーショナル エクセレンス
    * デプロイ済みのリソースの分析
* Azure Monitor
    * メトリックとログデータに基づいて収集、分析、視覚化、場合によってアクションを実行
        * アラートを設定
    * Azure Application Insights
* Azure Service Health
    * Azure のサービス、リージョン、リソースの正常性に関するパーソナライズされたビューを提供
    * インシデントについてサービス停止の正式な根本原因分析 (RCA) が提供

* Azure Security Center
    * Azure とオンプレミスの両方ですべてのサービスにわたってセキュリティ体制を可視化できる監視サービス
        * 許可されたアプリケーションの一覧を定義
        * マルウェアを検出して VM へのインストールをブロック
* Azure Sentinel
    * セキュリティ情報およびイベント管理 (SIEM) システム
    * データ収集、脅威を検出
* Azure Key Vault

* Azure Dedicated Host
    * 専用の物理サーバ上で仮想マシンをホスト

* セキュリティ体制
    * 機密性、整合性、可用性
* Azure Firewall
    * Azure Firewall を使用すると、送信 HTTP/S トラフィックを指定した完全修飾ドメイン名 (FQDN) の一覧に制限できる
    * 外部との通信
* Azure DDoS Protection
* ネットワーク セキュリティ グループ
     * ネットワーク セキュリティ グループ規則を使用すると、送信元と送信先の IP アドレス、ポート、およびプロトコルによって、リソースで送受信されるトラフィックをフィルター処理できる
     * 仮想ネットワーク間
* Azure Application Gateway Web アプリケーション ファイアウォール

* 認証
* 認可、承認
* Azure Active Directory
    * 提供サービス
        * 認証
        * シングルサインオン
        * アプリケーション管理
        * デバイス管理
    * Azure AD Connect で既存のActive Directoryとも接続
* 多要素認証
    * カテゴリ
        * ユーザが知っていること
            * メールアドレス、パスワード
        * ユーザが持っているもの
            * スマートフォン
        * ユーザ自身の特徴
            * 生体認証
* 条件付きアクセス
    * 承認されたデバイスからのみアクセスを許可など

* ロールベースのアクセス制御 (Azure RBAC)
    * アクセス許可を定義するロールを作成できる
    * Azure portal のアクセス制御(IAM)で行う
* リソースロック
    * 誤った変更を防ぐ
    * ロックのレベル
        * CanNotDelete 
        * ReadOnly
* リソースタグ
    * タグにより、リソースに関する追加情報(メタデータ)が提供される
    * Azure Policy を使用して、リソースのプロビジョニング時に適切なタグが確実に割り当てられるようにすることもできる
* Azure Policy
    * ポリシーに準拠していないものがわかったり、準拠しないものが作れなくなる
        * 特定のAzureリージョンにはVMがデプロイできないようにするなど
* Azure Blueprints
    * 標準に沿った組み込みのBlueprintsがある
* クラウド導入フレームワーク
    * サブスクリプション ガバナンス戦略

* コンプライアンス認証
* Microsoft プライバシーに関する声明
    * Microsoft が収集する個人データ、Microsoft がそれを使用する方法、およびその目的が記載
* オンライン サービス条件
    * Microsoft とお客様の間の法的契約
* Data Protection Addendum
    * オンライン サービスのデータ処理およびセキュリティの条項
* トラストセンター
    * セキュリティ、プライバシー、およびコンプライアンスに関するリソースにアクセス
* Azureコンプライアンスドキュメント
    * Azureサブスクリプションに適用できる一般的な標準の参照ブループリントやポリシー定義
* Azure Government
    * Microsoft Azure サービスの別のインスタンス
    * 米国連邦政府機関、州と地方の自治体、およびそれらのソリューションプロバイダーのセキュリティとコンプライアンスのニーズに対応
* Azure China 21Vianet
    * 中国に拠点を置くクラウドサービスの物理的に分離されたインスタンス

* 総保有コスト(TCO)計算ツール
    * オンプレからAzureへの移行について計算
* 料金計算ツール
* Azure Advisor
    * 使用状況を監視、不要なリソースを削除
* Azure Reservations
    * 予約することで割引
* Azure Cost Management + Billing
    * リソース最適化のための推奨事項を受け取る
* 使用していない仮想マシンの割り当て解除でコスト節約
    * CPUやネットワークの料金がかからなくなる
* サブスクリプションの使用制限を使用して、想定外の支出を防ぐ
* 低コストの場所およびリージョンを選択
* タグを適用してコストの所有者を識別

* SLA
    * ダウンタイムの割合
* サービスクレジット
    * サービスがSLAどおりに機能しなかった場合の補償として割引
* Azureの状態にて、Azureサービスの正常性を確認
* 冗長性を追加して可用性を高める


