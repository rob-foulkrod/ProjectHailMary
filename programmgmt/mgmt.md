# Project Hail Mary Management Demo Notes

## Current Demo Assets

- GitHub Project: [Project Hail Mary Delivery Dashboard](https://github.com/users/rob-foulkrod/projects/34)
- Milestone 1: `Release 1.0 - Pizza Ordering` due 2026-08-21 (upcoming)
- Milestone 2: `Release 1.1 - Engineering Reliability` due 2026-08-07 (overdue)
- Initiative #13: `Improve Pizza Ordering Experience` (intended Green example)
- Initiative #14: `Stabilize Engineering Environment` (intended Red and blocked example)
- Existing epics: #1 through #4, each with two linked sub-issues

The project is linked to this repository and includes these custom fields:

| Field | Demo use |
| --- | --- |
| Work type | Initiative, Epic, or Story |
| Health | Green, Yellow, or Red executive status |
| Team | Pizza Experience, Platform Engineering, or Quality Engineering |
| Estimate | Relative workload value |
| Target date | Planned delivery date |
| Forecast confidence | High, Medium, or Low confidence in the date |
| Scope change | Baseline or Added after planning |
| Risk level | Low, Medium, or High delivery risk |

Existing issue labels provide a second source of signals: `status: ready`, `status: in progress`, `status: blocked`, `status: completed`, `risk`, and `scope-change`.

## Suggested Seed Data

Add issues #1 through #14 to Project #34. Use this small, intentionally mixed data set:

| Item | Work type | Health | Team | Estimate | Target date | Forecast | Scope | Risk | Milestone |
| --- | --- | --- | --- | ---: | --- | --- | --- | --- | --- |
| #13 Pizza Ordering Experience | Initiative | Green | Pizza Experience | 13 | 2026-08-21 | High | Baseline | Low | Release 1.0 |
| #1 README | Epic | Green | Pizza Experience | 3 | 2026-08-16 | High | Baseline | Low | Release 1.0 |
| #4 Unit testing | Epic | Yellow | Quality Engineering | 8 | 2026-08-21 | Medium | Baseline | Medium | Release 1.0 |
| #3 Princess Donut requests | Epic | Yellow | Pizza Experience | 5 | 2026-08-22 | Medium | Added after planning | Medium | Release 1.0 |
| #14 Engineering Environment | Initiative | Red | Platform Engineering | 13 | 2026-08-07 | Low | Baseline | High | Release 1.1 |
| #2 Rocky environment escape | Epic | Red | Platform Engineering | 8 | 2026-08-07 | Low | Baseline | High | Release 1.1 |
| #7 Investigate access | Story | Red | Platform Engineering | 5 | 2026-08-12 | Low | Baseline | High | Release 1.1 |
| #10 First Donut enhancement | Story | Yellow | Pizza Experience | 3 | 2026-08-22 | Low | Added after planning | Medium | Release 1.0 |

Keep #5 closed and `status: completed`. Keep #7 and #10 blocked, with their existing blocker comments. This provides completed work, in-progress work, planned work, blocked work, a scope change, and an overdue release.

## Project Views To Create

1. **Executive Summary**: group by `Health`; show Status, Target date, Forecast confidence, Risk level, and Milestone.
2. **Release Readiness**: group by Milestone; sort by Target date; filter out completed work if a focused forecast is needed.
3. **Team Capacity**: group by Team; show Estimate and Status.
4. **Risk And Blockers**: filter `Risk level` is High or Status is blocked; show Parent issue and Updated.
5. **Scope Changes**: filter Scope change is `Added after planning`; group by Team.
6. **Aging Work**: sort by Created ascending; filter Status is In Progress or blocked; show Updated and Target date.
7. **Roadmap**: use the roadmap layout with Target date, grouped by Team or Milestone.

## Executive Walkthrough

Start on **Executive Summary** and answer three questions:

1. **Are we on track?**
   - The Pizza Ordering initiative is Green with a high-confidence upcoming release.
   - The Engineering Environment initiative is Red because Release 1.1 is overdue.

2. **What is at risk?**
   - #7 is blocked on a deployment-permission inventory.
   - #10 is blocked on backlog triage.
   - #3 is added scope and should be reviewed before committing to Release 1.0.

3. **What decisions or escalations are needed?**
   - Ask Platform Engineering for the access-audit inventory and an owner/date for #7.
   - Decide whether #3 and #10 remain in Release 1.0 or move to a later release.
   - Confirm capacity in Quality Engineering before committing #4's test coverage scope.

## Facilitation Notes

- Treat RAG as a management signal, not a calculation. Explain the evidence behind the color.
- Use milestones for release dates and the Target date field for item-level commitments.
- A blocked status needs an owner, dependency, and next review date. The seeded blocker comments model this.
- Scope changes should stay visible after they are accepted; do not simply relabel them as baseline.
- Aging is visible from the built-in Created and Updated fields, even without a separate stale-work field.
