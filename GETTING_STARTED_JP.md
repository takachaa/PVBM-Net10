# 新規プロジェクト開始時のPRD作成と初期セットアップ手順

## 概要

Claude Code開発ワークフローを使って新しいプロジェクトを開始する際の完全な手順ガイド。PRD作成から最初の機能実装までをカバーします。

## PRD作成に使用するコマンド

**`/create-prd [output-filename]`**

- **目的**: 会話履歴から包括的なProduct Requirements Document（PRD）を生成
- **引数**: 出力ファイル名（省略時は`PRD.md`）
- **出力**: 15セクション構造の詳細なPRD

### PRDの構成（15セクション）

1. Executive Summary - 製品概要
2. Mission - ミッションステートメント
3. Target Users - ターゲットユーザー
4. MVP Scope - スコープ（✅In Scope, ❌Out of Scope）
5. User Stories - ユーザーストーリー
6. Core Architecture & Patterns - アーキテクチャ
7. Tools/Features - 機能仕様
8. Technology Stack - 技術スタック
9. Security & Configuration - セキュリティ設定
10. API Specification - API仕様
11. Success Criteria - 成功基準
12. Implementation Phases - 実装フェーズ
13. Future Considerations - 将来の検討事項
14. Risks & Mitigations - リスクと対策
15. Appendix - 付録

## 完全な新規プロジェクト開始手順

### フェーズ1: プロジェクト準備（会話による要件定義）

#### ステップ1: プロジェクトアイデアをClaudeと議論

```
会話例:
---
ユーザー: タスク管理アプリを作りたいです。
         個人用で、タスクの作成・編集・削除、
         優先度設定、期限管理ができるものです。

Claude: 素晴らしいアイデアですね！いくつか質問させてください：

1. 技術スタックの希望はありますか？
   - バックエンド: FastAPI / Express / Django など
   - フロントエンド: React / Vue / Svelte など
   - データベース: SQLite / PostgreSQL / MongoDB など

2. 認証は必要ですか？
   - ローカル単一ユーザー
   - マルチユーザー（要認証）

3. MVP（最小限の製品）に含めたい機能は？
   - 必須機能
   - 後回しにできる機能

4. デプロイ方法は？
   - ローカル開発のみ
   - 本番環境へのデプロイ予定
```

#### ステップ2: 要件を詳細化

Claudeとの会話で以下を明確にします：
- **コア機能**: MVPに絶対必要なもの
- **技術選択**: 使用する技術スタックと理由
- **ユーザー像**: 誰が、どのように使うか
- **成功基準**: 何ができれば成功か
- **制約事項**: 技術的制約、時間的制約

### フェーズ2: PRD生成

#### ステップ3: /create-prdコマンドを実行

```bash
/create-prd task-manager-prd.md
```

**実行内容:**
1. Claudeが会話履歴全体をレビュー
2. 要件を15セクション構造に整理
3. 不足情報は合理的な推測で補完
4. 包括的なPRDドキュメントを生成

**生成されるPRD例:**
```markdown
# Task Manager - Product Requirements Document

## 1. Executive Summary
個人用タスク管理アプリケーション...

## 2. Mission
シンプルで効率的なタスク管理を提供...

## 4. MVP Scope
### In Scope
- ✅ タスクの作成・編集・削除
- ✅ 優先度設定（高・中・低）
- ✅ 期限管理
- ✅ 完了状態の管理

### Out of Scope
- ❌ タスクの共有機能
- ❌ リマインダー通知
- ❌ カテゴリ/タグ機能
...
```

#### ステップ4: PRDのレビューと調整

生成されたPRDを確認：
1. **内容の正確性**: 意図通りの要件が含まれているか
2. **スコープの妥当性**: MVP範囲が適切か
3. **技術選択**: 技術スタックが適切か
4. **実装フェーズ**: 段階的な実装計画が現実的か

不足や修正が必要な場合：
- Claudeとさらに議論して要件を明確化
- 再度`/create-prd`を実行して更新版を生成

### フェーズ3: 開発環境セットアップ

#### ステップ5: ワークフローファイルをコピー

```bash
# プロジェクトディレクトリ作成
mkdir task-manager
cd task-manager

# ワークフローファイルをコピー
cp -r /path/to/PVBM-framework/.claude .
cp -r /path/to/PVBM-framework/.agents .
cp -r /path/to/PVBM-framework/docs .
cp /path/to/PVBM-framework/CLAUDE.md .

# Git初期化
git init

# 生成したPRDを配置
mv /path/to/task-manager-prd.md .
```

#### ステップ6: CLAUDE.mdをカスタマイズ

`CLAUDE.md`を編集してプロジェクト固有の情報を追加：

```markdown
# Task Manager

## Tech Stack
- Backend: Python 3.11+, FastAPI, SQLAlchemy, SQLite
- Frontend: React 18, Vite, TanStack Query, Tailwind CSS
- Testing: pytest, Vitest

## Project Structure
task-manager/
├── backend/
│   ├── app/
│   │   ├── main.py
│   │   ├── models.py
│   │   └── routers/
│   └── tests/
├── frontend/
│   └── src/
└── task-manager-prd.md

## Code Conventions
- Python: PEP8, type hints required
- React: Functional components with hooks
- API: RESTful design
```

### フェーズ4: 開発開始（PIVループ）

#### ステップ7: プロジェクトコンテキストをロード

```bash
/prime
```

**実行内容:**
- プロジェクト構造を分析
- PRDを読み込み
- 技術スタックを理解
- 現在の状態を評価

**出力例:**
```
プロジェクト: task-manager
状態: 新規プロジェクト（コードベースなし）
PRD: task-manager-prd.md
技術スタック: FastAPI + React + SQLite
次のステップ: バックエンド基盤構築を推奨
```

#### ステップ8: 最初の機能を計画

```bash
/plan-feature backend-foundation
```

**実行内容:**
1. PRDの「Implementation Phases」を参照
2. バックエンド基盤に必要な要素を特定
3. FastAPIベストプラクティスを参照
4. 詳細な実装計画を生成

**出力先:** `.agents/plans/backend-foundation.md`

**計画内容例:**
```markdown
# Backend Foundation - Implementation Plan

## Overview
FastAPI + SQLAlchemy + SQLiteでバックエンド基盤を構築

## Tasks
1. プロジェクト構造作成
   - backend/app/main.py
   - backend/app/database.py
   - backend/app/models.py

2. FastAPIアプリケーション初期化
   - CORSミドルウェア設定
   - ルーター設定

3. SQLAlchemy設定
   - データベース接続
   - Baseモデル定義

4. 基本的なヘルスチェックエンドポイント
   - GET /health

## Validation
- uvicorn起動確認
- /healthエンドポイント動作確認
```

#### ステップ9: 計画を実装

```bash
/execute .agents/plans/backend-foundation.md
```

**実行内容:**
- 計画を読み込み
- タスクを順番に実装
- 継続的に検証（構文、インポート）
- テストも同時に実装

#### ステップ10: 検証

```bash
/validate
```

**検証項目:**
- バックエンドリンティング: `ruff check`
- バックエンドテスト: `pytest`
- カバレッジ: `pytest --cov`

#### ステップ11: コミット

```bash
/commit
```

**実行内容:**
- git statusとgit diffを確認
- 適切なコミットメッセージを生成
- `feat: implement backend foundation`などのタグ付き

### フェーズ5: 反復開発

同じPIVループを繰り返します：

```bash
# フロントエンド基盤
/plan-feature frontend-foundation
/execute .agents/plans/frontend-foundation.md
/validate
/commit

# タスクCRUD機能
/plan-feature task-crud
/execute .agents/plans/task-crud.md
/validate
/code-review
/commit

# 優先度機能
/plan-feature priority-system
/execute .agents/plans/priority-system.md
/validate
/commit
```

## PRD作成前に必要な情報

`/create-prd`を実行する前に、会話で明確にすべき項目：

### 必須情報
- [ ] **製品概要**: 何を作るか（1-2文）
- [ ] **コア機能**: 絶対必要な機能3-5個
- [ ] **技術スタック**: 使用する言語・フレームワーク
- [ ] **ユーザー**: 誰が使うか

### 推奨情報
- [ ] **MVPスコープ**: 最初のバージョンに含める/含めない機能
- [ ] **成功基準**: どうなれば成功か
- [ ] **制約**: 技術的制約、時間的制約
- [ ] **デプロイ方法**: ローカル / クラウド
- [ ] **認証**: 必要 / 不要

### オプション情報（PRD生成時に推測可能）
- [ ] セキュリティ要件
- [ ] パフォーマンス要件
- [ ] 将来的な拡張計画

## よくある質問

### Q1: PRDを作らずに開発を始められますか？

**A:** 可能ですが、推奨しません。理由：
- PRDがあると`/plan-feature`がより正確な計画を生成
- スコープクリープ（機能肥大化）を防げる
- プロジェクトの方向性が明確になる

小規模プロジェクト（1-2日）なら省略も可。

### Q2: PRDは後から変更できますか？

**A:** はい。プロジェクト進行中に：
1. 新しい要件をClaudeと議論
2. `/create-prd task-manager-prd.md`で上書き
3. または手動でPRDを編集

### Q3: PRD生成に会話はどれくらい必要？

**A:** 最小限の場合：
- 5-10往復の会話でMVPレベルのPRD生成可能

包括的な場合：
- 20-30往復でプロダクションレベルのPRD生成

### Q4: /create-prdの代わりに手動でPRDを書けますか？

**A:** 可能です。`PRD_example.md`をテンプレートとして：
```bash
cp .claude/PRD_example.md my-project-prd.md
# 手動で編集
```

ただし、`/create-prd`は：
- 会話から自動抽出
- 一貫性のある構造
- 見落としの防止

というメリットがあります。

## まとめ：理想的な新規プロジェクト開始フロー

```bash
# 1. Claudeと会話して要件定義（10-20往復）
ユーザー: 「〇〇アプリを作りたい」
Claude: 「技術スタックは？」「認証は？」「MVPスコープは？」
ユーザー: （質問に回答）

# 2. PRD生成
/create-prd my-project-prd.md

# 3. PRDレビュー
（生成されたPRDを確認、必要に応じて再生成）

# 4. ワークフローセットアップ
mkdir my-project && cd my-project
cp -r /path/to/PVBM-framework/{.claude,.agents,docs,CLAUDE.md} .
git init

# 5. CLAUDE.mdカスタマイズ
（技術スタック、プロジェクト構造、規約を記載）

# 6. 開発開始
/prime
/plan-feature backend-foundation
/execute .agents/plans/backend-foundation.md
/validate
/commit

# 7. 反復（PIVループ）
/plan-feature [次の機能]
/execute .agents/plans/[次の機能].md
/validate
/commit
```

この流れに従うことで、体系的で質の高いプロジェクト開発が可能になります。
