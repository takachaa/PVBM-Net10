# Claude Code Development Workflow

A portable development workflow framework for building software projects with AI assistance using the PIV (Prime, Implement, Validate) Loop methodology.

## Overview

This workflow provides a systematic approach to software development using Claude Code. It includes slash commands for all phases of development, best practices documentation, and structured artifact management.

## Quick Reference

### Essential Commands

```bash
/prime                              # Load project context
/plan-feature [name]                # Plan a new feature
/execute [plan-path]                # Execute a plan
/validate                           # Run all validations
/commit                             # Create Git commit
```

### Full Command List

| Command | Purpose |
|---------|---------|
| `/prime` | Load project context and understand codebase |
| `/plan-feature [name]` | Create detailed implementation plan |
| `/execute [plan-path]` | Implement features from plan |
| `/validate` | Run linting, tests, and builds |
| `/code-review` | Technical code review |
| `/code-review-fix [description]` | Fix code review issues |
| `/execution-report` | Generate implementation report |
| `/system-review [plan] [report]` | Analyze process adherence |
| `/rca [issue-id]` | Root cause analysis for GitHub issues |
| `/implement-fix [issue-id]` | Implement fix from RCA |
| `/init-project` | Initialize project dependencies |
| `/create-prd [filename]` | Create product requirements document |
| `/commit` | Create semantic Git commit |

## PIV Loop Workflow

The PIV Loop is the core development process:

```
Prime (Understand) → Plan (Design) → Execute (Build) → Validate (Verify) → Commit
     ↑                                                                        ↓
     └──────────────────── Iterate as needed ────────────────────────────────┘
```

### 1. Prime Phase - Understanding
- Run `/prime` to load comprehensive project context
- Analyzes project structure, tech stack, key files
- Provides architectural overview and current state

### 2. Plan Phase - Design
- Run `/plan-feature [name]` to create implementation plan
- 5-phase planning process:
  1. Feature requirements understanding
  2. Codebase intelligence gathering
  3. External research & documentation
  4. Strategic thinking & risk assessment
  5. Plan structure generation
- Outputs to `.agents/plans/[name].md`

### 3. Execute Phase - Implementation
- Run `/execute [plan-path]` to implement the plan
- Systematic implementation with continuous verification
- Tests are implemented alongside features
- Validation commands run automatically

### 4. Validate Phase - Quality Assurance
- Run `/validate` for comprehensive checks
- Includes linting, testing, coverage, builds
- Provides overall health assessment

### 5. Commit Phase - Version Control
- Run `/commit` to create semantic Git commit
- Automatic commit message generation
- Tagged with appropriate prefixes (feat, fix, docs, etc.)

## Best Practices Documentation

The `.claude/reference/` directory contains comprehensive best practices:

### Available Documents

| Document | Lines | Topics Covered |
|----------|-------|----------------|
| `fastapi-best-practices.md` | 923 | Project structure, routing, Pydantic, dependencies, error handling, async patterns, testing, security |
| `react-frontend-best-practices.md` | 1,319 | Component architecture, state management, React Router, TanStack Query, Tailwind CSS, forms, performance |
| `sqlite-best-practices.md` | 898 | Schema design, connection management, transactions, query optimization, indexing, SQLAlchemy integration |
| `testing-and-logging.md` | 899 | structlog configuration, testing pyramid, unit/integration/E2E patterns, React component testing |
| `deployment-best-practices.md` | 1,071 | Local development, production deployment, Docker, CI/CD, monitoring, performance optimization |

### How to Use

1. **Automatic Reference**: `/plan-feature` automatically reads relevant best practices
2. **Manual Reference**: Read documents when implementing specific features
3. **Validation**: `/code-review` validates against documented standards
4. **Customization**: Edit documents to add project-specific patterns

## Workflow Artifacts

### Directory Structure

```
.agents/
├── plans/                    # Feature implementation plans
├── code-reviews/             # Code review results
├── execution-reports/        # Implementation retrospectives
└── system-reviews/           # Process improvement analysis

docs/
└── rca/                      # Root cause analysis documents
```

### Artifact Lifecycle

1. **Plans** (`.agents/plans/`): Created by `/plan-feature`, consumed by `/execute`
2. **Code Reviews** (`.agents/code-reviews/`): Created by `/code-review`, used by `/code-review-fix`
3. **Execution Reports** (`.agents/execution-reports/`): Created by `/execution-report`, used by `/system-review`
4. **System Reviews** (`.agents/system-reviews/`): Created by `/system-review`, inform workflow improvements
5. **RCA Documents** (`docs/rca/`): Created by `/rca`, used by `/implement-fix`

## Development Scenarios

### Scenario: New Feature

```bash
# 1. Understand the project
/prime

# 2. Plan the feature
/plan-feature user-authentication

# 3. Execute the plan
/execute .agents/plans/user-authentication.md

# 4. Validate
/validate

# 5. Optional: Code review
/code-review
/code-review-fix user-authentication

# 6. Commit
/commit

# 7. Optional: Reflect and improve
/execution-report
/system-review .agents/plans/user-authentication.md .agents/execution-reports/user-authentication.md
```

### Scenario: Bug Fix (GitHub Issue)

```bash
# 1. Root cause analysis
/rca 42

# 2. Implement fix
/implement-fix 42

# 3. Validate and commit
/validate
/commit
```

### Scenario: Project Setup

```bash
# Initialize dependencies and start servers
/init-project
```

## Workflow Philosophy

### Context is King
- Plans contain all information needed for one-pass implementation
- Reduces back-and-forth and enables autonomous execution
- Combines external research with existing pattern analysis

### Systematic Process
- PIV loop ensures consistent quality
- Every feature follows the same structured approach
- Repeatable and predictable outcomes

### Continuous Verification
- Validation happens throughout implementation
- Early error detection (syntax, imports, types)
- Multiple validation layers (lint, test, coverage, build, review)

### Self-Improving
- Built-in reflection capabilities (`/execution-report`)
- Process analysis and improvement suggestions (`/system-review`)
- Continuous workflow optimization

## Configuration

### Bash Command Permissions

Edit `.claude/settings.local.json` to configure allowed bash commands:

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

### Project-Specific Customization

Customize this file (CLAUDE.md) for your project:

1. **Technology Stack**: List your specific technologies
2. **Project Structure**: Document your directory layout
3. **Code Conventions**: Define coding standards and patterns
4. **Testing Strategy**: Specify test types and coverage requirements
5. **Deployment**: Document deployment processes

## Troubleshooting

### /plan-feature output too long
**Solution**: Break down the feature into smaller sub-features. Split large features into multiple focused plans.

### /execute stops midway
**Solution**: Review plan for correct dependency order. Implement some parts manually if needed.

### Tests fail during /validate
**Solution**: Use `/code-review-fix` to identify and fix issues, or debug manually.

### Applying to existing projects
**Solution**: Copy `.claude/`, `.agents/`, and `docs/` directories. Customize this CLAUDE.md file to match your project.

### Customizing best practices
**Solution**: Edit Markdown files in `.claude/reference/` directly. Add project-specific patterns and conventions.

## Getting Started

### For New Projects

1. Copy workflow files to your project
2. Customize CLAUDE.md for your tech stack
3. Run `/prime` to understand your project
4. Create PRD with `/create-prd` (optional)
5. Start developing with `/plan-feature`

### For Existing Projects

1. Copy `.claude/`, `.agents/`, `docs/` to your project
2. Edit CLAUDE.md to reflect your project specifics
3. Run `/prime` to understand existing codebase
4. Begin using workflow commands for new features

## Additional Resources

### Example PRD
- **.claude/PRD_example.md**: Example product requirements document (from habit-tracker project)
- Use as template for creating your own PRDs with `/create-prd`

## Notes

- All commands are defined in `.claude/commands/` directory
- Best practices docs in `.claude/reference/` can be customized
- Workflow is portable - copy to any project and start using
- Git history preserves original development of this workflow framework
