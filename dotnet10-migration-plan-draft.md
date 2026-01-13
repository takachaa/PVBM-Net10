# StentSoftwareDownloadWebApp .NET 8 → .NET 10 移行計画（草案）

## 概要

StentSoftwareDownloadWebApp バックエンドを .NET 8 から .NET 10 へ移行し、C# 14 の新機能を積極的に活用しながら Clean Architecture を洗練させる。

## 現状

| 項目 | 現在の状態 |
|------|-----------|
| フレームワーク | .NET 8.0 |
| 言語バージョン | C# 12（暗黙） |
| アーキテクチャ | Clean Architecture（Domain, Infrastructure, Api） |
| 認証 | ASP.NET Core Identity + Cookie Session |
| DB | PostgreSQL + EF Core 8.0.2 |
| 主要パッケージ | Identity 8.0.2, Npgsql 8.0.2, Serilog 9.0.0 |

## 移行要件

- [x] .NET 10 + C# 14 へアップグレード
- [x] Identity + Cookie Session 認証を維持
- [x] PostgreSQL を維持（EF Core バージョンアップのみ）
- [x] Controller ベースの API を維持（Minimal API への移行なし）
- [x] C# 13/14 の新機能を積極活用
- [x] Clean Architecture パターンを洗練
- [ ] 機能追加なし
- [ ] テスト追加なし

---

## Phase 1: プロジェクトファイル更新

### 1.1 Directory.Build.props

```xml
<Project>
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <LangVersion>14</LangVersion>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
</Project>
```

### 1.2 Directory.Packages.props

```xml
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
  <ItemGroup>
    <!-- ASP.NET Core / Identity -->
    <PackageVersion Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="10.0.0" />
    <PackageVersion Include="Microsoft.AspNetCore.Identity.UI" Version="10.0.0" />
    <PackageVersion Include="Microsoft.AspNetCore.OpenApi" Version="10.0.0" />

    <!-- EF Core -->
    <PackageVersion Include="Microsoft.EntityFrameworkCore" Version="10.0.0" />
    <PackageVersion Include="Microsoft.EntityFrameworkCore.Abstractions" Version="10.0.0" />
    <PackageVersion Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.0">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageVersion>
    <PackageVersion Include="Microsoft.EntityFrameworkCore.Relational" Version="10.0.0" />
    <PackageVersion Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.0">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageVersion>

    <!-- PostgreSQL -->
    <PackageVersion Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="10.0.0" />

    <!-- Logging -->
    <PackageVersion Include="Serilog" Version="4.2.0" />
    <PackageVersion Include="Serilog.AspNetCore" Version="10.0.0" />
    <PackageVersion Include="Serilog.Sinks.Seq" Version="9.0.0" />

    <!-- API Documentation -->
    <PackageVersion Include="Swashbuckle.AspNetCore" Version="8.0.0" />

    <!-- Authentication -->
    <PackageVersion Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="10.0.0" />

    <!-- Utilities -->
    <PackageVersion Include="SendGrid" Version="9.29.3" />
  </ItemGroup>
</Project>
```

**削除するパッケージ:**
- `Microsoft.AspNetCore.Identity` (2.2.0) - 古いバージョン、不要
- `Microsoft.EntityFrameworkCore.SqlServer` - PostgreSQL のみ使用
- `AutoMapper.Extensions.Microsoft.DependencyInjection` - 未使用
- `FluentValidation.AspNetCore` - 未使用
- `Twilio` - 未使用

---

## Phase 2: 破壊的変更への対応

### 2.1 EF Core 10 Breaking Changes

| 変更点 | 対応 |
|--------|------|
| Application Name が接続文字列に注入される | 明示的に `Application Name` を追加 |

**appsettings.json 更新:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=stentsoftwaredb;Username=stent;Password=stentpassword;Application Name=StentSoftwareDownloadWebApp"
  }
}
```

### 2.2 ASP.NET Core 10 Breaking Changes

現在の実装は既に `WebApplication.CreateBuilder()` を使用しており、主要な破壊的変更の影響は最小限。

---

## Phase 3: C# 14 新機能の適用

### 3.1 `field` キーワード（プロパティバッキングフィールド）

**対象ファイル:** `Domain/Abstractions/Result.cs`

```csharp
// Before
public bool IsSuccess { get; private set; }
public List<string> ErrorMessages { get; private set; }

// After - field キーワードでバリデーション追加
public bool IsSuccess { get; private init; }
public IReadOnlyList<string> ErrorMessages
{
    get => field;
    private init => field = value ?? [];
}
```

### 3.2 コレクション式の活用

**対象ファイル:** `Domain/Abstractions/Result.cs`

```csharp
// Before
public static Result SuccessResult() => new Result(true, new List<string>());
public static Result Failure(string errorMessage) => new Result(false, new List<string> { errorMessage });

// After
public static Result SuccessResult() => new(true, []);
public static Result Failure(string errorMessage) => new(false, [errorMessage]);
```

### 3.3 Primary Constructor の活用

**対象ファイル:** `Domain/Entities/ApplicationUser.cs`, DTOs

```csharp
// Before
public class TwoFactorAuth
{
    public string UserId { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    // ...
}

// After - Primary Constructor
public class TwoFactorAuth(string userId, string code, DateTime expiresAt)
{
    public string UserId { get; set; } = userId;
    public string Code { get; set; } = code;
    public DateTime ExpiresAt { get; set; } = expiresAt;
    // ...
}
```

### 3.4 Null 条件付き代入（C# 14）

**対象ファイル:** `Domain/Services/AuthenticationService.cs`

```csharp
// Before
if (user != null)
{
    user.LastLoginAt = DateTime.UtcNow;
}

// After
user?.LastLoginAt = DateTime.UtcNow;
```

---

## Phase 4: アーキテクチャの洗練

### 4.1 Result パターンの改善

**`Domain/Abstractions/Result.cs` の改善:**

```csharp
namespace Domain.Abstractions;

public record Result(bool IsSuccess, IReadOnlyList<string> ErrorMessages)
{
    public static Result Success() => new(true, []);
    public static Result Failure(params string[] errors) => new(false, errors);
    public static Result Failure(IEnumerable<string> errors) => new(false, errors.ToList());
}

public record Result<T>(bool IsSuccess, T? Value, IReadOnlyList<string> ErrorMessages)
    : Result(IsSuccess, ErrorMessages)
{
    public static Result<T> Success(T value) => new(true, value, []);
    public static new Result<T> Failure(params string[] errors) => new(false, default, errors);
    public static new Result<T> Failure(IEnumerable<string> errors) => new(false, default, errors.ToList());
}
```

### 4.2 DI 登録の整理

**`Infrastructure/Extensions/ServiceCollectionExtensions.cs` 作成:**

```csharp
namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITwoFactorAuthRepository, TwoFactorAuthRepository>();
        services.AddScoped<IEmailRepository, SendGridEmailRepository>();
        services.AddScoped<IInstallerFileRepository, InstallerFileRepository>();
        services.AddScoped<IInstallHistoryRepository, InstallHistoryRepository>();

        return services;
    }

    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IInstallerDownloadService, InstallerDownloadService>();
        services.AddScoped<IContactService, ContactService>();

        return services;
    }
}
```

### 4.3 Program.cs の簡素化

```csharp
var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services));

// Services
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddDomainServices()
    .AddIdentityConfiguration()
    .AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware pipeline
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.Services.ApplyMigrationsAndSeed(app.Logger);
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<PasswordExpirationMiddleware>();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
```

---

## Phase 5: 実装ステップ

### Step 1: 準備
```bash
git checkout -b feature/dotnet10-migration
```

### Step 2: SDK インストール確認
```bash
dotnet --list-sdks
# .NET 10 SDK がインストールされていることを確認
```

### Step 3: プロジェクトファイル更新
1. `Directory.Build.props` を更新（net10.0, LangVersion 14）
2. `Directory.Packages.props` を更新（パッケージバージョン）
3. 不要パッケージを削除

### Step 4: パッケージ復元
```bash
dotnet restore
dotnet build
```

### Step 5: 破壊的変更対応
1. 接続文字列に `Application Name` 追加

### Step 6: コード更新
1. `Result.cs` を record ベースに書き換え
2. C# 14 の新機能を適用
3. DI 登録を `ServiceCollectionExtensions` に整理
4. `Program.cs` を簡素化

### Step 7: ビルド・動作確認
```bash
dotnet build
dotnet run --project Api/Api.csproj
```

---

## Phase 6: 検証

### ビルド検証
```bash
dotnet clean
dotnet restore
dotnet build --configuration Release
```

### 機能検証チェックリスト
- [ ] アプリケーション起動
- [ ] DB 接続
- [ ] ユーザー登録
- [ ] メール確認
- [ ] ログイン（2FA 含む）
- [ ] パスワードリセット
- [ ] インストーラーダウンロード（認証済み）
- [ ] ログアウト

### API エンドポイント検証
```bash
# Swagger UI で確認
# http://localhost:5000/swagger
```

---

## 変更対象ファイル一覧

| ファイル | 変更内容 |
|---------|---------|
| `Directory.Build.props` | net10.0, LangVersion 14 |
| `Directory.Packages.props` | パッケージバージョン更新 |
| `appsettings.json` | 接続文字列に Application Name 追加 |
| `Domain/Abstractions/Result.cs` | record ベースに書き換え |
| `Api/Program.cs` | DI 整理、簡素化 |
| `Infrastructure/Extensions/ServiceCollectionExtensions.cs` | 新規作成（DI 登録整理） |
| `Domain/Entities/*.cs` | Primary Constructor 適用（任意） |
| `Domain/Dtos/*.cs` | record 構文の活用（任意） |

---

## リスクと対策

| リスク | 対策 |
|--------|------|
| Npgsql 10.0 の互換性問題 | プレビュー版の場合は 9.x 系で一時対応 |
| 既存データベースとの互換性 | マイグレーション不要（スキーマ変更なし） |
| サードパーティパッケージの未対応 | Swashbuckle 等は最新版を確認 |

## ロールバック計画

問題発生時:
```bash
git checkout main
# または
git revert --no-commit HEAD~N
```
