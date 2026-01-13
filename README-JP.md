# Claude Code 開発ワークフロー

**PIVループ**（Prime, Implement, Validate）手法を使用した、ポータブルなAI駆動型開発ワークフローフレームワーク。体系的な開発プロセスを提供する12個のスラッシュコマンドと5,110行のベストプラクティスドキュメントを含みます。

## これは何？

あらゆるソフトウェアプロジェクトに適用できる完全な開発ワークフローフレームワークです。以下を含みます：

- **PIVループワークフロー**: Prime → Plan → Execute → Validate → Commit
- **12個のスラッシュコマンド**: プロジェクト初期化からバグ修正まで
- **ベストプラクティスドキュメント**: FastAPI、React、SQLite、テスト、デプロイメントをカバーする5,110行
- **アーティファクト管理**: プラン、レビュー、レポートの構造化された出力保存
- **自己改善**: 組み込みのプロセス分析と改善提案

## クイックスタート

### 1. プロジェクトにコピー

```bash
# 新規プロジェクトディレクトリを作成
mkdir my-new-project
cd my-new-project

# ワークフローファイルをコピー
cp -r /path/to/this-repo/.claude .
cp -r /path/to/this-repo/.agents .
cp -r /path/to/this-repo/docs .
cp /path/to/this-repo/CLAUDE.md .

# Git初期化
git init
```

### 2. プロジェクトに合わせてカスタマイズ

`CLAUDE.md`を編集してプロジェクトに合わせます：
- 技術スタック
- プロジェクト構造
- コード規約
- テスト戦略

### 3. コマンド使用開始

```bash
# プロジェクトコンテキストをロード
/prime

# 新機能を計画
/plan-feature user-authentication

# 計画を実行
/execute .agents/plans/user-authentication.md

# 変更を検証
/validate

# コミット
/commit
```

## PIVループ

PIVループは機能開発への体系的なアプローチです：

```
Prime (理解) → Plan (設計) → Execute (構築) → Validate (検証) → Commit
     ↑                                                              ↓
     └────────────── 必要に応じてイテレーション ────────────────────┘
```

### Primeフェーズ
包括的なプロジェクトコンテキストをロード：
- プロジェクト構造とアーキテクチャ
- 主要ファイルとエントリポイント
- 技術スタック
- 現在の状態評価

### Planフェーズ
詳細な実装計画を作成：
- 機能要件分析
- コードベースインテリジェンス収集
- 外部リサーチとドキュメント調査
- 戦略的思考とリスク評価
- ワンパス実装のためのコンテキストリッチな計画

### Executeフェーズ
機能を体系的に実装：
- 計画を読んで理解
- 依存関係順にタスクを実装
- 継続的な検証（構文、インポート、型）
- テスト実装
- 検証コマンド実行

### Validateフェーズ
包括的な品質チェック：
- リンティング
- テスト実行
- カバレッジ分析
- ビルド検証
- 総合的な健全性評価

## 利用可能なコマンド

| コマンド | 引数 | 出力 | 目的 |
|---------|------|------|------|
| `/prime` | なし | - | プロジェクトコンテキストをロード |
| `/plan-feature` | [feature-name] | `.agents/plans/[feature-name].md` | 実装計画を作成 |
| `/execute` | [plan-path] | - | 計画を実行 |
| `/validate` | なし | - | 包括的な検証 |
| `/code-review` | なし | `.agents/code-reviews/[name].md` | 技術的コードレビュー |
| `/code-review-fix` | [file-or-description] | - | レビュー指摘を修正 |
| `/execution-report` | なし | `.agents/execution-reports/[feature-name].md` | 実装レポート |
| `/system-review` | [plan-path] [report-path] | `.agents/system-reviews/[feature-name]-review.md` | プロセス分析 |
| `/rca` | [github-issue-id] | `docs/rca/issue-[id].md` | 根本原因分析 |
| `/implement-fix` | [github-issue-id] | - | RCA修正を実装 |
| `/init-project` | なし | - | プロジェクト初期化 |
| `/create-prd` | [output-filename] | [output-filename] | PRD作成 |
| `/commit` | なし | - | Gitコミット作成 |

## ベストプラクティスドキュメント

`.claude/reference/`ディレクトリには5,110行のベストプラクティスが含まれています：

| ドキュメント | 使用タイミング |
|------------|--------------|
| `fastapi-best-practices.md` | APIエンドポイント構築、Pydanticスキーマ、依存性注入 |
| `react-frontend-best-practices.md` | コンポーネント、hooks、状態管理、フォーム |
| `sqlite-best-practices.md` | DBスキーマ設計、クエリ最適化、SQLAlchemyパターン |
| `testing-and-logging.md` | structlog設定、unit/integration/E2Eテストパターン |
| `deployment-best-practices.md` | Docker、本番ビルド、デプロイメント |

これらのドキュメントは：
- `/plan-feature`コマンドによって自動的に参照される
- `/code-review`が標準に対して検証するために使用
- いつでも手動参照が可能

## 開発シナリオ

### シナリオA: 新機能開発

```bash
# 1. プロジェクトを理解
/prime

# 2. 機能を計画
/plan-feature user-dashboard

# 3. 計画を実装
/execute .agents/plans/user-dashboard.md

# 4. 検証
/validate

# 5. コードレビュー（オプション）
/code-review

# 6. 必要に応じて問題を修正
/code-review-fix user-dashboard

# 7. コミット
/commit

# 8. 振り返り（オプション）
/execution-report
/system-review .agents/plans/user-dashboard.md .agents/execution-reports/user-dashboard.md
```

### シナリオB: GitHub Issue修正

```bash
# 1. Issueを分析
/rca 42

# 2. 修正を実装
/implement-fix 42

# 3. 検証してコミット
/validate
/commit
```

### シナリオC: プロジェクト初期化

```bash
# 新規プロジェクトをセットアップ
/init-project
```

## ワークフローアーティファクト

ワークフローは以下のディレクトリに構造化された出力を作成します：

```
.agents/
├── plans/                    # 機能実装計画
├── code-reviews/             # コードレビュー結果
├── execution-reports/        # 実装の振り返り
└── system-reviews/           # プロセス改善分析

docs/
└── rca/                      # 根本原因分析ドキュメント
```

## ワークフロー最適化のヒント

### コンテキストが王様
- `/plan-feature`は情報密度の高い計画を生成
- ワンパス実装のための十分なコンテキストを提供
- 外部リサーチと既存パターン分析を組み合わせ

### 並列分析
- `/plan-feature`は特化したサブエージェントを並列で使用
- 効率的なリサーチと分析

### 継続的検証
- `/execute`は継続的に検証（構文、インポート、型）
- 早期エラー検出

### 自己改善ループ
- `/system-review`がプロセス遵守を分析
- CLAUDE.mdとコマンドへの改善提案
- 継続的なワークフロー改善

## 例：タスク管理アプリの構築

```bash
# 1. プロジェクトセットアップ
mkdir task-manager && cd task-manager
# ワークフローファイルをコピー（上記参照）
git init

# 2. コンテキストをロード
/prime

# 3. PRDを作成
/create-prd task-manager-prd.md

# 4. バックエンド基盤
/plan-feature backend-foundation
/execute .agents/plans/backend-foundation.md
/validate
/commit

# 5. フロントエンド基盤
/plan-feature frontend-foundation
/execute .agents/plans/frontend-foundation.md
/validate
/commit

# 6. タスクCRUD機能
/plan-feature task-crud
/execute .agents/plans/task-crud.md
/validate
/code-review
/code-review-fix task-crud
/validate
/commit

# 7. 振り返りと改善
/execution-report
/system-review .agents/plans/task-crud.md .agents/execution-reports/task-crud.md
```

## トラブルシューティング

**Q: /plan-featureの出力が長すぎてトークン制限に達する**
**A:** 機能を小さなサブ機能に分割してください。大きな機能は複数の小さな計画に分割すべきです。

**Q: /executeが途中で止まる**
**A:** 計画の依存関係順序を確認してください。必要に応じて一部を手動で実装してください。

**Q: /validate中にテストが失敗する**
**A:** `/code-review-fix`を使用して問題を特定・修正してください。または手動でデバッグしてください。

**Q: 既存プロジェクトでこれを使用できますか？**
**A:** はい。`.claude/`、`.agents/`、`docs/`ディレクトリをコピーし、プロジェクトに合わせて`CLAUDE.md`をカスタマイズしてください。

**Q: ベストプラクティスドキュメントをカスタマイズできますか？**
**A:** はい。`.claude/reference/`内のMarkdownファイルを直接編集してください。プロジェクト固有のパターンを追加できます。

## プロジェクト構造

```
.
├── .agents/                  # ワークフローアーティファクト保存
│   ├── plans/               # 機能計画
│   ├── code-reviews/        # レビュー結果
│   ├── execution-reports/   # 実装レポート
│   └── system-reviews/      # プロセス分析
├── .claude/                  # ワークフローコア
│   ├── commands/            # スラッシュコマンド定義
│   │   ├── core_piv_loop/  # PIVループコマンド
│   │   ├── validation/     # 検証コマンド
│   │   └── github_bug_fix/ # バグ修正ワークフロー
│   ├── reference/          # ベストプラクティス（5,110行）
│   ├── PRD_example.md      # PRD例（habit-tracker）
│   └── settings.local.json # Claude Code設定
├── docs/
│   └── rca/                # 根本原因分析ドキュメント
├── CLAUDE.md               # プロジェクト固有使用ガイド
└── README.md               # このファイル
```

## 設定

`.claude/settings.local.json`を編集してbashコマンド権限を設定：

```json
{
  "permissions": {
    "allow": [
      "Bash(tree:*)",
      "Bash(wc:*)",
      "Bash(uv run:*)",
      "Bash(uv sync:*)",
      "Bash(npm run:*)",
      "Bash(npm install:*)",
      "Bash(git:*)",
      "Bash(gh:*)",
      "Bash(curl:*)",
      "Bash(git add:*)"
    ]
  }
}
```

## 哲学

**コンテキストが王様**: ワークフローはワンパス実装のための豊富なコンテキストの提供を重視します。計画には必要な情報がすべて含まれており、やり取りを減らし自律的な実行を可能にします。

**体系的プロセス**: PIVループはすべての機能で一貫した品質を保証します。すべての機能は同じ構造化されたアプローチに従います。

**自己改善**: 組み込みの振り返り機能により、実際の開発経験に基づいてワークフローを継続的に改善できます。

**ポータブル**: このフレームワークは、ワークフローファイルをコピーしてプロジェクト固有のドキュメントをカスタマイズするだけで、どのプロジェクトにも適用できます。

## 貢献

このワークフローフレームワークはhabit-trackerプロジェクトから生まれました。自由に：
- ニーズに合わせてコマンドをカスタマイズ
- ベストプラクティスドキュメントを拡張
- プロジェクト固有のパターンを追加
- 改善をコミュニティに共有

## ライセンス

このワークフローフレームワークはあらゆるプロジェクトで使用できるよう提供されています。

## 参考

このワークフローを実証した元のhabit-trackerアプリケーションについては、Gitの履歴を参照するか、利用可能な場合は`workshop`ブランチを確認してください。
