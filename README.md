# Oracle Forms to Blazor Migration

A modernization project migrating a legacy **Oracle Forms 6i** desktop application to a **.NET Blazor Server** application, with **Oracle** retained as the backend database.

## Overview

The original application is a small utility system built in Oracle Forms 6i, used internally for operational data entry — in this case, textile/weaving quality configuration. This project re-implements the same functionality on a modern stack while preserving the exact behavior and workflow users are already familiar with.

## Why this migration

Oracle Forms 6i is an aging platform with a shrinking support and hiring pool. This project moves the application to a stack (.NET / Blazor) that's easier to maintain, extend, and deploy, without disrupting the end-user experience or requiring changes to the underlying Oracle schema.

## Architecture

The original Forms application used a **base/subclass form design**: a parent form implemented shared behavior (Save, Next Record, Execute Query), and individual forms inherited from it, adding their own fields and logic.

This project recreates that same pattern in Blazor:

- **Base component class** — implements shared logic equivalent to the parent form's triggers/behavior
- **Shared toolbar component** — reusable Save / Next Record / Execute Query actions, consistent across all migrated forms
- **Individual form components** — inherit the base behavior and add their own fields, matching each original Form's layout and validation

This keeps the migrated app structurally close to the original, which reduces regression risk and makes behavior easy to verify against the legacy system.

## Tech stack

- **.NET 10 SDK**
- **Blazor Server**
- **Oracle.ManagedDataAccess.Core** for database connectivity
- **Oracle** (existing schema, unchanged)
- Connection strings managed via **User Secrets** (not committed to source control)

## Progress

- ✅ **Quality Setup form** — fully migrated and functional end-to-end (query, save, shared toolbar actions) against the live Oracle database. Manages loom/weaving quality configuration (quality code, warp/weft count, picks/ends per inch, width, weave, twill, colour, construction, brand, reed, etc.)
- 🔄 Refining trigger/calculated-field logic and field layout to match the original Forms behavior exactly
- ⏳ Remaining forms to be migrated using the same base/subclass pattern

## Notes

- Database schema is treated as source of truth — some assumptions from documentation/naming didn't hold (e.g. a field expected to be numeric was actually `VARCHAR2` in production), so fields are verified directly against the live schema before implementation.
- This repository is a personal portfolio reference for the migration approach; any client-specific data or credentials are excluded.
