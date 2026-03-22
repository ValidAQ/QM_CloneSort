# Quasimorph Clone Sort

QOL UI mod that lets you reorder your clone operative list.

## Features

* Adds sorting modes to operative selection screens, including a manual mode that allows you to customize the order to your preference.
* In **Manual** mode, each list row shows an up-arrow icon button to move that operative up in the list.
* Optional integration with [Mod Configuration Menu (MCM)](https://steamcommunity.com/sharedfiles/filedetails/?id=3469678797) to configure sorting behaviour in-game.

## Sort Modes

| Mode       | Description                                           |
| ---------- | ----------------------------------------------------- |
| `Manual`   | User-customizable order. Up-arrow button visible on each row. |
| `Name A-Z`  | Alphabetical A → Z by display name.                   |
| `Name Z-A` | Alphabetical Z → A by display name.                   |
| `Rank` | Highest rank first, then A → Z within the same rank.  |

The sort button cycles through the modes that are enabled in config (see below).

## Configuration

Settings are stored in `%AppData%/../LocalLow/Magnum Scriptum Ltd/Quasimorph_ModConfigs/QM_CloneSort/config.json` and can also be changed in-game with [MCM](https://steamcommunity.com/sharedfiles/filedetails/?id=3469678797).

| Setting           | Default            | Description |
| ----------------- | ------------------ | ----------- |
| `SortingMode`     | `ManualAndAuto`    | **Manual** - button toggles the up-arrow button visibility in the operative list. **Manual and Auto** - button cycles through manual and any enabled automatic sort modes. |
| `EnableNameSort`  | `true`             | Include Name A-Z and Name Z-A in the cycle when in *Manual and Auto* mode. |
| `EnableRankSort`  | `true`             | Include Rank in the cycle when in *Manual and Auto* mode. |

### Manual order persistence
The manually arranged operative order is saved to `Quasimorph_ModConfigs/QM_CloneSort/manual_order.txt` and restored automatically when switching back to Manual mode.

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

## 1.1.1
* Replaced mod ID with proper mod name in MCM configuration list.

## 1.1.0
* Added localization support for sort button labels. Currently machine translated - corrections welcome.
* Added mod configuration with two configurable sorting modes (*Manual* and *Manual and Auto*) and toggles to enable/disable name and rank sorting.
* In *Manual* mode the sort button toggles the up-arrow button visibility instead of cycling to automatic modes.
* Optional in-game configuration via Mod Configuration Menu (MCM).

## 1.0.0
* Initial release.