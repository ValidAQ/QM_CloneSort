# Quasimorph Clone Sort

QOL UI mod that lets you reorder your clone operative list.

## Features

* Adds sorting modes to operative selection screens, including a manual mode that allows you to customize the order to your preference.
* In **Manual** mode, each list row shows an up-arrow icon button to move that operative up in the list.

## Sort Modes

| Mode       | Description                                           |
| ---------- | ----------------------------------------------------- |
| `Manual`   | User-customizable order                               |
| `NameAsc`  | Alphabetical A -> Z by display name.                  |
| `NameDesc` | Alphabetical Z -> A by display name.                  |
| `RankDesc` | Highest rank first, then A -> Z within the same rank. |

# Config File
Manual operative order is persisted across sessions in `%AppData%/../LocalLow/Magnum Scriptum Ltd/Quasimorph_ModConfigs/QM_CloneSort/manual_order.txt` and is restored when switching back from another sort mode.

## Mod Compatibility

This mod patches the following game classes. Other mods that patch the same methods may conflict.

| Class                        | Method                | Patch type       |
| ---------------------------- | --------------------- | ---------------- |
| `MGSC.SelectMercenaryScreen` | `OnEnable`            | Prefix & Postfix |
| `MGSC.SelectMercenaryScreen` | `PanelOnSelected`     | Prefix           |
| `MGSC.MercenariesScreen`     | `OnEnable`            | Prefix & Postfix |
| `MGSC.MercenariesScreen`     | `PanelOnIconSelected` | Prefix           |

# Source Code
Source code is available on GitHub at https://github.com/validaq/QM_CloneSort

# Credits
Thanks to nbk_redspy, whose mod sources were very valuable for learning how to mod the game properly.

# Changelog

## 1.0.0
* Initial release.