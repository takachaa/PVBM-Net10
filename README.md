# Claude Code Development Workflow

A portable, AI-driven development workflow framework using the **PIV Loop** (Prime, Implement, Validate) methodology. This framework provides systematic development processes with 12 slash commands and 5,110 lines of best practices documentation.

## What is This?

This is a complete development workflow framework that can be applied to any software project. It includes:

- **PIV Loop Workflow**: Prime → Plan → Execute → Validate → Commit
- **12 Slash Commands**: From project initialization to bug fixing
- **Best Practices Documentation**: 5,110 lines covering FastAPI, React, SQLite, testing, and deployment
- **Artifact Management**: Structured output storage for plans, reviews, and reports
- **Self-Improving**: Built-in process analysis and improvement suggestions

## Quick Start

### 1. Copy to Your Project

```bash
# Create your new project directory
mkdir my-new-project
cd my-new-project

# Copy workflow files
cp -r /path/to/this-repo/.claude .
cp -r /path/to/this-repo/.agents .
cp -r /path/to/this-repo/docs .
cp /path/to/this-repo/CLAUDE.md .

# Initialize Git
git init
```

### 2. Customize for Your Project

Edit `CLAUDE.md` to match your project:
- Technology stack
- Project structure
- Code conventions
- Testing strategy

### 3. Start Using Commands

```bash
# Load project context
/prime

# Plan a new feature
/plan-feature user-authentication

# Execute the plan
/execute .agents/plans/user-authentication.md

# Validate your changes
/validate

# Commit
/commit
```

## The PIV Loop

The PIV Loop is a systematic approach to feature development:

```
Prime (Understand) → Plan (Design) → Execute (Build) → Validate (Verify) → Commit
     ↑                                                                        ↓
     └──────────────────── Iterate as needed ────────────────────────────────┘
```

### Prime Phase
Load comprehensive project context:
- Project structure and architecture
- Key files and entry points
- Technology stack
- Current state assessment

### Plan Phase
Create detailed implementation plans:
- Feature requirements analysis
- Codebase intelligence gathering
- External research and documentation
- Strategic thinking and risk assessment
- Context-rich plans for one-pass implementation

### Execute Phase
Implement features systematically:
- Read and understand the plan
- Implement tasks in dependency order
- Continuous verification (syntax, imports, types)
- Test implementation
- Run validation commands

### Validate Phase
Comprehensive quality checks:
- Linting
- Test execution
- Coverage analysis
- Build verification
- Overall health assessment

## Available Commands

| Command | Arguments | Output | Purpose |
|---------|-----------|--------|---------|
| `/prime` | None | - | Load project context |
| `/plan-feature` | [feature-name] | `.agents/plans/[feature-name].md` | Create implementation plan |
| `/execute` | [plan-path] | - | Execute plan |
| `/validate` | None | - | Comprehensive validation |
| `/code-review` | None | `.agents/code-reviews/[name].md` | Technical code review |
| `/code-review-fix` | [file-or-description] | - | Fix review issues |
| `/execution-report` | None | `.agents/execution-reports/[feature-name].md` | Implementation report |
| `/system-review` | [plan-path] [report-path] | `.agents/system-reviews/[feature-name]-review.md` | Process analysis |
| `/rca` | [github-issue-id] | `docs/rca/issue-[id].md` | Root cause analysis |
| `/implement-fix` | [github-issue-id] | - | Implement RCA fix |
| `/init-project` | None | - | Initialize project |
| `/create-prd` | [output-filename] | [output-filename] | Create PRD |
| `/commit` | None | - | Create Git commit |

## Best Practices Documentation

The `.claude/reference/` directory contains 5,110 lines of best practices:

| Document | When to Use |
|----------|-------------|
| `fastapi-best-practices.md` | Building API endpoints, Pydantic schemas, dependency injection |
| `react-frontend-best-practices.md` | Components, hooks, state management, forms |
| `sqlite-best-practices.md` | DB schema design, query optimization, SQLAlchemy patterns |
| `testing-and-logging.md` | structlog setup, unit/integration/E2E testing patterns |
| `deployment-best-practices.md` | Docker, production builds, deployment |

These documents are:
- Automatically referenced by `/plan-feature` command
- Used by `/code-review` to validate against standards
- Available for manual reference anytime

## Development Scenarios

### Scenario A: New Feature Development

```bash
# 1. Understand the project
/prime

# 2. Plan the feature
/plan-feature user-dashboard

# 3. Implement the plan
/execute .agents/plans/user-dashboard.md

# 4. Validate
/validate

# 5. Code review (optional)
/code-review

# 6. Fix issues if needed
/code-review-fix user-dashboard

# 7. Commit
/commit

# 8. Reflect (optional)
/execution-report
/system-review .agents/plans/user-dashboard.md .agents/execution-reports/user-dashboard.md
```

### Scenario B: GitHub Issue Fix

```bash
# 1. Analyze the issue
/rca 42

# 2. Implement the fix
/implement-fix 42

# 3. Validate and commit
/validate
/commit
```

### Scenario C: Project Initialization

```bash
# Set up a new project
/init-project
```

## Workflow Artifacts

The workflow creates structured outputs in these directories:

```
.agents/
├── plans/                    # Feature implementation plans
├── code-reviews/             # Code review results
├── execution-reports/        # Implementation retrospectives
└── system-reviews/           # Process improvement analysis

docs/
└── rca/                      # Root cause analysis documents
```

## Workflow Optimization Tips

### Context is King
- `/plan-feature` generates information-dense plans
- Provides sufficient context for one-pass implementation
- Combines external research with existing pattern analysis

### Parallel Analysis
- `/plan-feature` uses specialized subagents in parallel
- Efficient research and analysis

### Continuous Verification
- `/execute` continuously verifies (syntax, imports, types)
- Early error detection

### Self-Improvement Loop
- `/system-review` analyzes process adherence
- Suggests improvements to CLAUDE.md and commands
- Continuous workflow improvement

## Example: Building a Task Manager App

```bash
# 1. Project setup
mkdir task-manager && cd task-manager
# Copy workflow files (see above)
git init

# 2. Load context
/prime

# 3. Create PRD
/create-prd task-manager-prd.md

# 4. Backend foundation
/plan-feature backend-foundation
/execute .agents/plans/backend-foundation.md
/validate
/commit

# 5. Frontend foundation
/plan-feature frontend-foundation
/execute .agents/plans/frontend-foundation.md
/validate
/commit

# 6. Task CRUD features
/plan-feature task-crud
/execute .agents/plans/task-crud.md
/validate
/code-review
/code-review-fix task-crud
/validate
/commit

# 7. Reflect and improve
/execution-report
/system-review .agents/plans/task-crud.md .agents/execution-reports/task-crud.md
```

## Troubleshooting

**Q: /plan-feature output too long, hitting token limits**
**A:** Break down the feature into smaller sub-features. Large features should be split into multiple smaller plans.

**Q: /execute stops midway**
**A:** Review the plan for correct dependency order. Implement some parts manually if needed.

**Q: Tests fail during /validate**
**A:** Use `/code-review-fix` to identify and fix issues. Or debug manually.

**Q: Can I use this with existing projects?**
**A:** Yes. Copy `.claude/`, `.agents/`, and `docs/` directories, then customize `CLAUDE.md` to match your project.

**Q: Can I customize best practices documentation?**
**A:** Yes. Edit Markdown files in `.claude/reference/` directly. Add project-specific patterns.

## Project Structure

```
.
├── .agents/                  # Workflow artifact storage
│   ├── plans/               # Feature plans
│   ├── code-reviews/        # Review results
│   ├── execution-reports/   # Implementation reports
│   └── system-reviews/      # Process analysis
├── .claude/                  # Workflow core
│   ├── commands/            # Slash command definitions
│   │   ├── core_piv_loop/  # PIV loop commands
│   │   ├── validation/     # Validation commands
│   │   └── github_bug_fix/ # Bug fix workflow
│   ├── reference/          # Best practices (5,110 lines)
│   ├── PRD_example.md      # Example PRD (habit-tracker)
│   └── settings.local.json # Claude Code configuration
├── docs/
│   └── rca/                # Root cause analysis docs
├── CLAUDE.md               # Project-specific usage guide
└── README.md               # This file
```

## Configuration

Edit `.claude/settings.local.json` to configure bash command permissions:

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

## Philosophy

**Context is King**: The workflow emphasizes providing rich context for one-pass implementation. Plans contain all necessary information, reducing back-and-forth and enabling autonomous execution.

**Systematic Process**: The PIV loop ensures consistent quality across all features. Every feature follows the same structured approach.

**Self-Improving**: Built-in reflection and system review capabilities allow the workflow to continuously improve based on real development experiences.

**Portable**: This framework can be applied to any project by simply copying the workflow files and customizing the project-specific documentation.

## Contributing

This workflow framework originated from the habit-tracker project. Feel free to:
- Customize commands for your needs
- Extend best practices documentation
- Add project-specific patterns
- Share improvements back to the community

## License

This workflow framework is provided as-is for use in any project.

## Reference

For the original habit-tracker application that demonstrated this workflow, see the Git history or check the `workshop` branch if available.
