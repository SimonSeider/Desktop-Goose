# Desktop Goose (Reverse-Engineered Source)

This repository contains a reverse-engineered/decompiled codebase of Desktop Goose.

## Branch Purpose

This is my personal branch for Desktop Goose improvements I want to make.

The main goals are:

- improve behavior and quality-of-life in areas I care about,
- fix bugs and rough edges I encounter while using or testing the app,
- keep the project evolving with practical, personal changes over time.

## What Changed In This Branch

The following changes are currently included:

### Multi-Monitor Support Fixes

- Replaced primary-monitor-only bounds usage with full virtual desktop bounds (`SystemInformation.VirtualScreen`).
- Updated goose spawn, movement targets, and clamping to operate across the full desktop space.
- Updated rendering coordinates so the goose is drawn correctly even when virtual desktop origin is not `(0, 0)` (for example, monitors placed to the left/top).

### Dialog / Window Stability Fixes

- Updated collectible window dialog startup to run on an STA thread.
- Fixed task positioning/clamping logic for dialog dragging so windows and goose targets remain valid on multi-monitor layouts.

## Important Notes

- This is not the original source release.
- The code was reconstructed from a decompiled build and then cleaned up.
- Several sections were refactored for readability and maintainability.
- In some places, behavior and implementation details were changed during refactoring and modernization.

## Pat Sounds

The original `Pat` sound assets were lost/corrupted in the recovered resources and had to be replaced.

## Credit

All credit for the original Desktop Goose app goes to **Samson**, who originally created it.

## Copyright / License Contact

If any copyright or license issue occurs regarding this repository, please contact:

`contact at exil dot dev`
