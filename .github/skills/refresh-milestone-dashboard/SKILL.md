---
name: refresh-milestone-dashboard
description: "Refresh the static milestone dashboard from live GitHub Issues and Milestones using gh CLI. Use when the user asks to update milestone tracking, overdue or upcoming deliverables, dashboard data, or the local milestone-control-room.html artifact."
argument-hint: "Optionally specify a milestone, issue, date, or delivery status to refresh or explain."
user-invocable: true
disable-model-invocation: false
---

# Refresh Milestone Dashboard

## Purpose

Keep `programmgmt/milestone-control-room.html` synchronized with the live demo data in `rob-foulkrod/ProjectHailMary`. The dashboard remains a single local HTML file. GitHub is the source of truth; the agent creates a static snapshot by rewriting only the embedded JavaScript data block.

## When To Use

Use this skill when asked to:

- refresh or update the milestone dashboard
- show current upcoming or overdue deliverables
- synchronize the local dashboard with GitHub Issues or Milestones
- update milestone status, dates, teams, blockers, or progress
- regenerate the local static dashboard data

## Preconditions

1. Confirm `programmgmt/milestone-control-room.html` exists.
2. Confirm `gh auth status` succeeds and includes repository access.
3. Confirm the dashboard contains these exact markers:
   - `// DATA_START`
   - `// DATA_END`
4. Do not modify CSS, page structure, filters, or drawer behavior during a data refresh.
5. Use the fixed dashboard reporting date unless the user explicitly requests a different demo date.

## Refresh Procedure

### 1. Read GitHub data

Run these commands from the repository root:

```powershell
gh api repos/rob-foulkrod/ProjectHailMary/milestones?state=all&per_page=100
gh issue list --repo rob-foulkrod/ProjectHailMary --state all --limit 100 --json number,title,state,body,labels,milestone,comments,updatedAt
```

Use the GitHub results to identify milestone dates, issue status, blockers, teams, and completion evidence. Do not invent current GitHub facts when the API does not provide them.

### 2. Map GitHub data to dashboard data

The generated JavaScript must preserve this shape:

```js
{
  id: "stable-slug",
  name: "Release name",
  description: "Short management-facing summary.",
  date: "YYYY-MM-DD",
  health: "green" | "yellow" | "red",
  team: "Pizza Experience" | "Platform Engineering" | "Quality Engineering",
  forecast: "High" | "Medium" | "Low",
  note: "Evidence, blocker, or decision context.",
  deliverables: [
    {
      title: "Issue title",
      issue: "#123",
      status: "ready" | "in progress" | "blocked" | "completed",
      team: "Owning team",
      date: "YYYY-MM-DD",
      detail: "Short delivery detail or dependency."
    }
  ]
}
```

Mapping rules:

- GitHub milestone `title` becomes `name`.
- GitHub milestone `due_on` becomes `date` in `YYYY-MM-DD` form.
- Issues assigned to a milestone become `deliverables`.
- Closed issues become `completed`.
- The `status: blocked` label becomes `blocked`.
- Other status labels map to `ready` or `in progress`.
- Issue numbers become the `issue` value.
- Existing blocker comments should inform `detail` and the milestone `note`.
- Use labels and issue content to infer team only when the repository data makes the mapping clear.
- Use `red` for overdue or materially blocked delivery, `yellow` for meaningful risk or low confidence, and `green` when the evidence supports the planned date.
- RAG is a management signal. Include the evidence in `note`; do not calculate health from a hidden formula.
- Preserve the fixed `DEMO_DATE` of `2026-08-11T00:00:00` unless explicitly asked to change it.
- Keep a small, readable static snapshot. Do not include every issue if it would make the demo harder to scan.

### 3. Rewrite only the marked block

In `programmgmt/milestone-control-room.html`, replace the content between `// DATA_START` and `// DATA_END` with:

```js
// DATA_START
const milestones = [
  /* generated milestone objects */
];
// DATA_END
```

The generated JavaScript must use valid syntax and must not contain unescaped backticks, `${...}` expressions, or raw line breaks inside quoted strings.

Never rewrite the whole HTML file. Never edit the CSS or event-handling code as part of a data refresh.

### 4. Validate the snapshot

Run the following checks:

```powershell
gh issue list --repo rob-foulkrod/ProjectHailMary --state all --limit 100 --json number,title,state,milestone,labels
dotnet build src/PizzaSales.ConsoleApp/PizzaSales.ConsoleApp.csproj
```

Also verify that:

- both data markers still exist and are in the correct order
- `const milestones = [` exists between the markers
- every milestone has a date and at least one deliverable
- every deliverable has a title, issue, status, team, and date
- dates use `YYYY-MM-DD`
- only one status value is used per deliverable
- the HTML contains no `fetch`, external script, or network dependency
- the fixed demo date remains unchanged unless requested

If browser automation is available, open the local HTML and test one filter and one detail drawer. If it is unavailable, report the static validation performed.

## Completion Report

Report:

- GitHub data source and refresh date
- milestones included
- overdue and upcoming counts
- blocked and completed deliverable counts
- whether the HTML data block was refreshed successfully
- validation performed and any data assumptions
