# Microsoft Certified: Azure Administrator Associate

* [Microsoft 認定:Azure Administrator Associate \- Certifications \| Microsoft Learn](https://learn.microsoft.com/ja-jp/certifications/azure-administrator/)

## 学習コンテンツ

### ラーニングパス

* [x] [AZ\-104:Azure 管理者向けの前提条件 \- Training \| Microsoft Learn](https://learn.microsoft.com/ja-jp/training/paths/az-104-administrator-prerequisites/)
* [x] [AZ\-104:Azure での ID とガバナンスの管理 \- Training \| Microsoft Learn](https://learn.microsoft.com/ja-jp/training/paths/az-104-manage-identities-governance/)
* [x] [AZ\-104:Azure でのストレージの実装と管理 \- Training \| Microsoft Learn](https://learn.microsoft.com/ja-jp/training/paths/az-104-manage-storage/)
* [x] [AZ\-104:Azure のコンピューティング リソースのデプロイと管理 \- Training \| Microsoft Learn](https://learn.microsoft.com/ja-jp/training/paths/az-104-manage-compute-resources/)
* [x] [AZ\-104:Azure 管理者向けの仮想ネットワークの構成と管理 \- Training \| Microsoft Learn](https://learn.microsoft.com/ja-jp/training/paths/az-104-manage-virtual-networks/)
* [x] [AZ\-104:Azure リソースの監視とバックアップ \- Training \| Microsoft Learn](https://learn.microsoft.com/ja-jp/training/paths/az-104-monitor-backup-resources/)

### 問題集

* [受講生30万講師のAZ\-104: Microsoft Azure Administrator模擬試験問題集（320問） \| Udemy](https://www.udemy.com/course/30az-104-microsoft-azure-administrator240/)

## メモ

* グループにライセンスを割り当てた場合、グループに紐づくユーザには割り当てられるが、グループに紐づくグループには伝播しない
* Azure Backup の Recovery Services コンテナーは、同じリージョンのものしかバックアップ対象にできない
    * 同じリソースグループでも、リージョンが異なったらダメ
* RDPは3389ポート、SMBは445ポート
* アラート処理ルールで「通知を表示しない」としても、Azureポータルには通知は表示される
    * アラート処理ルールで設定したアクションが実行されなくなるだけ
* Blobストレージの認証は、Azure Active Directory または SAS
* App Serviceでは、アプリケーションのログ記録と、Webサーバのログ記録の2種類のログ記録ができる
* App Serviceでのスロット交換で、アプリケーション周りの設定はスワップされる
* ドメインの所有権を確認するにはTXTレコードを見る
* サブドメインはNSレコード、Aレコードはホスト名とIPアドレス、PTRレコードは逆引き用
* Azure Container Networking Interface (CNI) ネットワークで、PodにIPアドレス振れる
* Azure Service Bus を使ってFIFOを実現するためには、セッションを有効にする
* Azure Migrateはオンプレミスの仮想マシンをAzureに移行するため
    * Data Migration Assistant はオンプレミスのSQL ServerをAzureに移行するため
* App ServiceのFreeプランだと、1日の使用可能なCPU時間が60分以内に制限される
    * Basicプランにすると無制限に
* リソースの作成日時を確認するには、アクティビティログを確認する
* 仮想ネットワークと紐づけるリソースは、同じリージョンにある必要がある
    * 同じリソースグループである必要はない
* ポイント対サイト間VPNでの認証方法は、Azure証明書認証 or Azure Active Directory
* ポイント対サイト間VPNでは、ルートベースの仮想ネットワークゲートウェイしか対応していない
    * ポリシーベースはダメ
* Azure AD Privileged Identity Management (PIM) を使用すると、Azure ADの組織で、ロールが割り当てられたり、アクティブ化されたりした場合などの重要なイベントが発生した際に通知を受けることができる
    * Azure AD Premium P2 ライセンスが必要
* Azure AD Identity Protection は、IDに関するリスクを検出、調査、修復するのに役立つ
* Azure Virtual WAN で複数のオンプレミスサイトとAzure間の接続を構成することができる
* 仮想ネットワークのピアリングした後に、サブネットを追加できない
    * いったんピアリング解除してから、サブネットを追加して、再度ピアリングする必要がある
* パブリックAzure DNSゾーンには、自動登録機能はない
    * 仮想ネットワークにリンクできるのは、プライベートDNSゾーンだけ
* Blobをアップロードするためには、ストレージBlobデータ寄稿者と、閲覧者の権限が必要
    * ストレージBlobデータ閲覧者は、リストも見えてしまうので、閲覧者の方だけでよい
* 管理グループやテナントルートグループにはロックやタグを適用できない
    * サブスクリプション配下だけ
* ポイント対サイトVPNが構成後にネットワークのトポロジが変更されてしまったら、VPNクライアントパッケージを再度ダウンロードしてインストールする必要がある
* リージョン間で Azure Backup 用の Recovery Services コンテナーを移動することはできない
* Log Analytics のクエリは、search in または テーブル名 から始められる
* Azure Network Watcherのトラフィック分析機能を有効化するには所有者、共同作成者、閲覧者、またはネットワーク共同作成者の権限が必要
    * 閲覧者でも良いのがポイント
* Premiumファイル共有は、FileStorageアカウントと呼ばれる特別な目的のストレージアカウントのみで利用が可能
* ホット、クール、アーカイブ間のストレージ データ階層は、Blob Storage および汎用v2(GPv2)アカウントのみ
* アーカイブアクセス層へのBLOBの移動は、LRS、GRS、または RA-GRS 用に構成されたストレージアカウントのみ
* アプリケーション セキュリティ グループを使用すると、複数の仮想マシンのネットワーク インターフェイスをグループ化し、NSG ルールのソースまたは宛先としてそのグループを使用できる
* Azureシステムルートの一部をカスタムのユーザー定義ルートでオーバーライドし、ルートテーブルにさらにカスタムルートを追加できる
* Log Analytics ワークスペースでの共有ダッシュボードにピン止めされたデータは、最大14日間のみ表示
* Azure Monitor、Azure Service Health、Azure Advisor にて、アクショングループを使用して、アラートについてユーザーに通知し、アクションを実行できる
* Azure Network Watcher はリージョン毎
* アラートで送信可能なSMSは5分に1件以下
    * Emailは1時間毎に100件以下
* Blobでオブジェクトレプリケーションをサポートするためには、バージョン管理を有効にする必要がある
* Azure Blob Storage で不変ポリシーを設定することで、一定期間変更/削除できないようにできる
* テンプレートを使用してデプロイする場合に指定できるのはリソースグループ
* 実行中の仮想マシンからディスクをデタッチできる(ホット除去)
* Azure Recovery Services の既定では、仮想マシンのバックアップは 30 日間保持
* Microsoft Azure Recovery Service (MARS) エージェントは、任意のサーバーのバックアップおよび回復サービスを実行するために必須
* セルフサービス パスワード リセット (SSPR)は、Azure AD Premium P1 と P2 で使える
* NSGは同じリージョン内のサブネット/NICにしか設定できない
* Basic Load Balancer は単一の可用性セット
    * Standard Load Balancer だと、ゾーン冗長になる
