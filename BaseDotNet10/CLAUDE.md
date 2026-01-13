# BaseDotNet10

.NET 10 + C# 14 ベースのClean Architectureテンプレートプロジェクト。

## 概要

このプロジェクトは、StentSoftwareDownloadWebAppを参考に作成された.NET 10のテンプレートです。

## 技術スタック

- **フレームワーク**: .NET 10.0
- **言語**: C# 14
- **認証**: ASP.NET Core Identity + Cookie Session
- **データベース**: PostgreSQL + Entity Framework Core 10
- **ログ**: Serilog + Seq
- **API ドキュメント**: Scalar
- **メール**: SendGrid

## アーキテクチャ

3層Clean Architecture:

```
Api/           - WebAPI層（Controllers, Middlewares）
Domain/        - ドメイン層（Entities, Services, Repositories Interface）
Infrastructure/ - インフラ層（EF Core, Repository実装）
```

## C# 14 新機能の活用

- **Primary Constructor**: サービス・リポジトリクラスで使用
- **record型**: DTOsとResultパターンで使用
- **コレクション式**: `[]` 構文を活用
- **required修飾子**: エンティティのプロパティで使用

## 開発コマンド

```bash
# Docker起動（PostgreSQL + Seq）
docker-compose up -d

# ビルド
dotnet build

# マイグレーション作成
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project Api

# マイグレーション適用
dotnet ef database update --project Infrastructure --startup-project Api

# 実行（開発）
dotnet run --project Api/Api.csproj
```

## Docker構成

| サービス | ポート | 説明 |
|---------|--------|------|
| basedotnet10-db | 5433 | PostgreSQL データベース |
| basedotnet10-seq | 5342, 8082 | Seq ログビューア (UI: http://localhost:8082) |

### データベース接続情報
- Host: localhost
- Port: 5433
- Database: stentsoftwaredb
- Username: stent
- Password: stentpassword

## APIエンドポイント

### 認証 (/api/auth)
- POST /register - ユーザー登録
- POST /login - ログイン
- POST /logout - ログアウト
- POST /send-2fa-code - 2FAコード送信
- POST /verify-2fa-code - 2FAコード検証
- POST /forgot-password - パスワードリセット要求
- POST /reset-password - パスワードリセット
- GET /confirm-email - メール確認
- GET/PUT /mypage - プロフィール

### ダウンロード (/api/download) [要認証]
- GET /windows - Windowsインストーラー

### 問い合わせ (/api/contact)
- POST /inquiry - 問い合わせ送信
