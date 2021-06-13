# IDManager
概要
=======
* steamやoriginなどで使用しているIDを保存、表示するDiscordのBot

使用方法
=======
* DiscordのDeveloper PortalからBotのTokenを取得し、Program.csのTokenに代入し、コンパイル・実行する

コマンド一覧
=======
* add : サービスとIDを記録しておくコマンド   例：!id add Steam username
* del : 指定したサービスの記録を削除するコマンド   例：!id del Steam
* show : メンションした相手が登録しているIDの一覧を表示するコマンド
* vc : コマンドを実行したユーザーが接続しているボイスチャンネル内のユーザーの登録情報を一括表示する
* help : 説明の表示

想定環境
=======
* Windows10での使用を想定
* また、MyDocuments/Json/に.json形式のファイルを生成する
