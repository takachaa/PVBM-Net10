# PRD: StentSoftwareDownloadWebApp .NET 10 Migration

## 1. Executive Summary

StentSoftwareDownloadWebApp は、ステントソフトウェアのダウンロード管理を行うWebアプリケーションのバックエンドシステムです。現在 .NET 8 で稼働しているこのシステムを .NET 10 へ移行し、最新のC# 14機能を活用しながらClean Architectureを洗練させます。

本移行プロジェクトでは、認証方式（ASP.NET Core Identity + Cookie Session）およびデータベース（PostgreSQL）は現状を維持しつつ、フレームワークとライブラリのバージョンアップ、コード品質の向上、アーキテクチャパターンの改善を実施します。機能追加は行わず、移行とリファクタリングに集中します。

**MVPゴール:** .NET 10環境で全既存機能が正常に動作し、C# 14の新機能を活用したコードベースへ移行完了すること。

## 2. Mission

### Product Mission Statement
StentSoftwareDownloadWebAppのバックエンドを最新の.NETプラットフォームへ安全に移行し、長期的な保守性とパフォーマンスを向上させる。

### Core Principles

1. **安全性優先** - 既存機能を壊さず、段階的に移行を進める
2. **最新技術の活用** - C# 14/.NET 10の新機能を積極的に採用
3. **アーキテクチャの一貫性** - Clean Architectureパターンを維持・洗練
4. **シンプルさ** - 過度な複雑化を避け、必要最小限の変更に留める
5. **検証可能性** - 各フェーズで動作確認を実施

## 3. Target Users

### Primary User Persona: システム管理者/開発者
- **役割:** StentSoftwareDownloadWebAppの保守・運用担当者
- **技術レベル:** .NET/C#に精通、クリーンアーキテクチャの理解あり
- **ニーズ:**
  - 最新フレームワークへの追従
  - セキュリティパッチの適用
  - 保守性の高いコードベース

### Secondary User Persona: エンドユーザー
- **役割:** ソフトウェアをダウンロードする顧客
- **技術レベル:** 一般的なWebアプリ利用者
- **ニーズ:**
  - 安定したダウンロード機能
  - 信頼性の高い認証
  - スムーズなユーザー体験

## 4. MVP Scope

### In Scope

**フレームワーク移行**
- ✅ .NET 8 から .NET 10 へのアップグレード
- ✅ C# 14 言語機能の有効化
- ✅ 全NuGetパッケージの.NET 10対応版への更新
- ✅ EF Core 10へのアップグレード

**コード改善**
- ✅ C# 14新機能の適用（field キーワード、コレクション式、Primary Constructor）
- ✅ Result パターンの record ベースへの書き換え
- ✅ DI登録の整理（ServiceCollectionExtensions）
- ✅ Program.cs の簡素化

**品質保証**
- ✅ 全APIエンドポイントの動作確認
- ✅ 認証フロー（登録、ログイン、2FA、パスワードリセット）の検証
- ✅ ビルド・起動確認

### Out of Scope

**機能追加**
- ❌ 新規API追加
- ❌ OpenID Connect / 外部IdP対応
- ❌ JWT認証の追加
- ❌ Minimal API への移行

**テスト**
- ❌ 単体テストの追加
- ❌ 統合テストの追加
- ❌ E2Eテストの追加

**インフラ**
- ❌ データベーススキーマの変更
- ❌ Docker/CI/CD設定の更新（別途対応）
- ❌ パフォーマンスチューニング

## 5. User Stories

### 開発者向けストーリー

**US-1: フレームワーク更新**
> As a 開発者, I want to .NET 10でアプリケーションをビルド・実行できる, so that 最新のセキュリティパッチと機能を利用できる

**例:** `dotnet build` が net10.0 ターゲットで成功し、`dotnet run` でアプリケーションが起動する

**US-2: 新言語機能の活用**
> As a 開発者, I want to C# 14の新機能をコードベースで使用できる, so that より簡潔で保守性の高いコードを書ける

**例:** Result.cs で `field` キーワードやコレクション式 `[]` を使用

**US-3: パッケージ互換性**
> As a 開発者, I want to 全ての依存パッケージが.NET 10と互換性がある状態にする, so that ビルドエラーや実行時エラーを回避できる

**例:** EF Core 10, Npgsql 10, Identity 10 等が正常に動作

### エンドユーザー向けストーリー

**US-4: 認証機能の継続**
> As a ユーザー, I want to 移行後も既存のアカウントでログインできる, so that サービスを継続利用できる

**例:** 既存ユーザーがメール/パスワードでログインし、2FAを完了できる

**US-5: ダウンロード機能の継続**
> As a ユーザー, I want to 移行後もソフトウェアをダウンロードできる, so that 必要なソフトウェアを入手できる

**例:** 認証後、Windowsインストーラーをダウンロードできる

## 6. Core Architecture & Patterns

### High-Level Architecture

現状のClean Architectureを維持:

```
┌─────────────────────────────────────────────────────┐
│                    Api Layer                         │
│  (Controllers, Middlewares, Program.cs)             │
├─────────────────────────────────────────────────────┤
│                  Domain Layer                        │
│  (Entities, Services, Repositories Interfaces,      │
│   DTOs, Abstractions)                               │
├─────────────────────────────────────────────────────┤
│               Infrastructure Layer                   │
│  (EF Core, Repository Implementations,              │
│   External Integrations)                            │
└─────────────────────────────────────────────────────┘
```

### Directory Structure

```
StentSoftwareDownloadWebApp/
├── Api/
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── DownloadController.cs
│   │   └── ContactController.cs
│   ├── Middlewares/
│   │   ├── ExceptionMiddleware.cs
│   │   └── PasswordExpirationMiddleware.cs
│   ├── Program.cs
│   └── appsettings.json
├── Domain/
│   ├── Abstractions/
│   │   └── Result.cs          # record ベースに改善
│   ├── Entities/
│   │   ├── ApplicationUser.cs
│   │   ├── TwoFactorAuth.cs
│   │   └── InstallHistory.cs
│   ├── Services/
│   ├── ServiceInterfaces/
│   ├── Repositories/
│   ├── Dtos/
│   └── Constants/
├── Infrastructure/
│   ├── EFCore/
│   │   └── Data/
│   ├── Repositories/
│   ├── Extensions/
│   │   └── ServiceCollectionExtensions.cs  # 新規作成
│   └── Migrations/
├── Directory.Build.props      # net10.0, LangVersion 14
└── Directory.Packages.props   # 中央パッケージ管理
```

### Key Design Patterns

| パターン | 適用箇所 | 説明 |
|---------|---------|------|
| Clean Architecture | 全体 | 3層分離（Api, Domain, Infrastructure） |
| Repository Pattern | データアクセス | IUserRepository, ITwoFactorAuthRepository 等 |
| Result Pattern | サービス層 | 成功/失敗を明示的に表現 |
| Dependency Injection | 全層 | ServiceCollectionExtensions で整理 |

## 7. Tools/Features

### 7.1 認証機能（既存維持）

| 機能 | エンドポイント | 説明 |
|-----|---------------|------|
| ユーザー登録 | POST /api/auth/register | メール確認必須 |
| ログイン | POST /api/auth/login | Cookie Session発行 |
| 2FA | POST /api/auth/send-2fa-code, verify-2fa-code | コードベース認証 |
| パスワードリセット | POST /api/auth/forgot-password, reset-password | メールリンク |
| プロフィール | GET/PUT /api/auth/mypage | ユーザー情報管理 |

### 7.2 ダウンロード機能（既存維持）

| 機能 | エンドポイント | 説明 |
|-----|---------------|------|
| Windowsインストーラー | GET /api/download/windows | 認証必須、履歴記録 |

### 7.3 問い合わせ機能（既存維持）

| 機能 | エンドポイント | 説明 |
|-----|---------------|------|
| 問い合わせ送信 | POST /api/contact/inquiry | SendGrid経由 |

## 8. Technology Stack

### Backend

| カテゴリ | 現行 | 移行後 |
|---------|------|--------|
| Framework | .NET 8.0 | .NET 10.0 |
| Language | C# 12 | C# 14 |
| Web Framework | ASP.NET Core 8 | ASP.NET Core 10 |

### Dependencies

| パッケージ | 現行バージョン | 移行後バージョン |
|-----------|---------------|-----------------|
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 8.0.2 | 10.0.0 |
| Microsoft.EntityFrameworkCore | 8.0.2 | 10.0.0 |
| Npgsql.EntityFrameworkCore.PostgreSQL | 8.0.2 | 10.0.0 |
| Serilog.AspNetCore | 9.0.0 | 10.0.0 |
| Swashbuckle.AspNetCore | 6.5.0 | 8.0.0 |
| SendGrid | 9.29.2 | 9.29.3 |

### 削除するパッケージ

| パッケージ | 理由 |
|-----------|------|
| Microsoft.AspNetCore.Identity 2.2.0 | 古いバージョン、不要 |
| AutoMapper.Extensions.Microsoft.DependencyInjection | 未使用 |
| FluentValidation.AspNetCore | 未使用 |
| Twilio | 未使用 |

### Database

- **DBMS:** PostgreSQL（変更なし）
- **ORM:** Entity Framework Core 10
- **Migrations:** 既存マイグレーション維持

## 9. Security & Configuration

### 認証・認可（変更なし）

- **方式:** ASP.NET Core Identity
- **セッション:** Cookie ベース（5分有効、スライディング）
- **パスワードポリシー:** 8文字以上、大文字/小文字/数字/記号必須
- **アカウントロックアウト:** 10回失敗で15分ロック
- **2FA:** コードベース（10分有効）

### Configuration

**appsettings.json 変更点:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=stentsoftwaredb;...;Application Name=StentSoftwareDownloadWebApp"
  }
}
```

### Security Scope

**In Scope:**
- ✅ 既存セキュリティ設定の維持
- ✅ パッケージの脆弱性対応（バージョンアップ）

**Out of Scope:**
- ❌ 新規セキュリティ機能の追加
- ❌ Passkey対応（.NET 10新機能）

## 10. API Specification

既存APIは全て維持。変更なし。

### 認証 API

```
POST /api/auth/register
POST /api/auth/login
POST /api/auth/logout
POST /api/auth/send-2fa-code
POST /api/auth/verify-2fa-code
POST /api/auth/forgot-password
POST /api/auth/reset-password
GET  /api/auth/confirm-email
POST /api/auth/resend-confirmation-email
GET  /api/auth/manage/info [Authorize]
GET  /api/auth/mypage [Authorize]
PUT  /api/auth/mypage [Authorize]
POST /api/auth/change-password [Authorize]
```

### ダウンロード API

```
GET /api/download/windows [Authorize]
```

### 問い合わせ API

```
POST /api/contact/inquiry
```

## 11. Success Criteria

### MVP Success Definition
.NET 10環境で全機能が正常動作し、本番デプロイ可能な状態。

### Functional Requirements

- ✅ `dotnet build` が成功する
- ✅ アプリケーションが起動する
- ✅ PostgreSQLに接続できる
- ✅ ユーザー登録→メール確認→ログイン が動作する
- ✅ 2FA（コード送信→検証）が動作する
- ✅ パスワードリセットが動作する
- ✅ インストーラーダウンロードが動作する
- ✅ Cookie Sessionが正常に機能する

### Quality Indicators

- コンパイル警告ゼロ
- 非推奨API使用ゼロ
- C# 14新機能を主要箇所で使用

### User Experience Goals

- 移行前後でエンドユーザー体験に変化なし
- レスポンス時間の維持（悪化なし）

## 12. Implementation Phases

### Phase 1: 準備・プロジェクトファイル更新

**Goal:** .NET 10でビルドできる状態にする

**Deliverables:**
- ✅ バックアップブランチ作成
- ✅ Directory.Build.props 更新（net10.0, LangVersion 14）
- ✅ Directory.Packages.props 更新（パッケージバージョン）
- ✅ 不要パッケージ削除
- ✅ `dotnet restore` 成功

**Validation:**
- `dotnet build` が成功する

### Phase 2: 破壊的変更対応

**Goal:** ランタイムエラーを解消

**Deliverables:**
- ✅ 接続文字列に Application Name 追加
- ✅ 非推奨APIの置き換え（該当箇所があれば）

**Validation:**
- アプリケーションが起動する
- DBに接続できる

### Phase 3: C# 14 / アーキテクチャ改善

**Goal:** コード品質向上

**Deliverables:**
- ✅ Result.cs を record ベースに書き換え
- ✅ コレクション式 `[]` の適用
- ✅ ServiceCollectionExtensions.cs 作成
- ✅ Program.cs 簡素化

**Validation:**
- ビルド成功
- 全APIエンドポイント動作確認

### Phase 4: 検証・完了

**Goal:** 本番デプロイ準備完了

**Deliverables:**
- ✅ 全機能検証チェックリスト完了
- ✅ Release ビルド確認
- ✅ ドキュメント更新

**Validation:**
- 機能検証チェックリスト100%完了
- PR作成・レビュー・マージ

## 13. Future Considerations

### Post-MVP Enhancements

| 機能 | 優先度 | 説明 |
|-----|--------|------|
| Passkey対応 | 中 | .NET 10 Identity新機能 |
| OpenID Connect | 低 | 外部IdP連携 |
| Minimal API移行 | 低 | 一部エンドポイントの軽量化 |

### Integration Opportunities

- Azure AD B2C 連携
- GitHub OAuth 連携

### Advanced Features

- レート制限の実装
- 監査ログの強化
- パフォーマンスモニタリング（Application Insights）

## 14. Risks & Mitigations

| リスク | 影響度 | 対策 |
|--------|--------|------|
| Npgsql 10.0 の互換性問題 | 高 | プレビュー版の場合は9.x系で一時対応。リリースノート確認 |
| 既存データとの互換性 | 高 | マイグレーション不要（スキーマ変更なし）。既存データでテスト |
| サードパーティパッケージ未対応 | 中 | 代替パッケージの調査、最新版の確認 |
| ビルド時間増加 | 低 | 影響は軽微。必要に応じてインクリメンタルビルド活用 |
| 本番環境での予期せぬ動作 | 中 | ステージング環境での十分なテスト |

## 15. Appendix

### Related Documents

- [移行計画草案](./dotnet10-migration-plan-draft.md)
- [.NET 10 Breaking Changes](https://learn.microsoft.com/en-us/dotnet/core/compatibility/10.0)
- [EF Core 10 What's New](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-10.0/whatsnew)

### Repository

- **GitHub:** https://github.com/takachaa/StentSoftwareDownloadWebApp.git
- **ブランチ戦略:** `feature/dotnet10-migration` から作業開始

### Key Dependencies Links

| パッケージ | NuGet |
|-----------|-------|
| EF Core | https://www.nuget.org/packages/Microsoft.EntityFrameworkCore |
| Npgsql | https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL |
| Identity | https://www.nuget.org/packages/Microsoft.AspNetCore.Identity.EntityFrameworkCore |
| Serilog | https://www.nuget.org/packages/Serilog.AspNetCore |
