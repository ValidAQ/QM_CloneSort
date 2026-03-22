#if MCM_PRESENT
using ModConfigMenu;
using ModConfigMenu.Contracts;
using ModConfigMenu.Implementations;
using ModConfigMenu.Objects;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace QM_CloneSort
{
	internal static class McmIntegration
	{
		const string manualOnlySortName = "Manual";
		const string manualAndAutoSortName = "Manual and Auto";

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void RegisterIfPresent()
		{
			foreach (var asm in System.AppDomain.CurrentDomain.GetAssemblies())
			{
				if (asm.GetName().Name == "MCM")
				{
					Register();
					return;
				}
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void Register()
		{
			var sortModeOptions = new List<object> { manualOnlySortName, manualAndAutoSortName };

			var configValues = new List<IConfigValue>
			{
				new DropdownConfig(
					key: "SortingMode",
					value: Plugin.Config.SortingMode == SortingModeOptions.ManualOnly ? manualOnlySortName : manualAndAutoSortName,
					header: "Sorting",
					defaultValue: manualAndAutoSortName,
					tooltip: $"In {manualOnlySortName} mode, the button locks/unlocks the list order. In {manualAndAutoSortName} mode, the button cycles through all enabled sort modes.",
					label: "Sort Mode",
					orderedDropdownOptions: sortModeOptions),

				new ConfigValue(
					key: "EnableNameSort",
					value: Plugin.Config.EnableNameSort,
					header: "Sorting",
					defaultValue: true,
					tooltip: $"Enable sorting by name (A-Z and Z-A) in {manualAndAutoSortName} mode.",
					label: "Enable Name Sorting"),

				new ConfigValue(
					key: "EnableRankSort",
					value: Plugin.Config.EnableRankSort,
					header: "Sorting",
					defaultValue: true,
					tooltip: $"Enable sorting by rank in {manualAndAutoSortName} mode.",
					label: "Enable Rank Sorting"),
			};

			ModConfigMenuAPI.RegisterModConfig(
				modName: "Clone Sort",
				configData: configValues,
				OnConfigSaved: (Dictionary<string, object> currentConfig, out string feedback) =>
				{
					feedback = null;
					try
					{
						if (currentConfig.TryGetValue("SortingMode", out var smVal))
						{
							string smStr = smVal?.ToString() ?? "";
							Plugin.Config.SortingMode = smStr == manualOnlySortName
								? SortingModeOptions.ManualOnly
								: SortingModeOptions.ManualAndAuto;
						}
						if (currentConfig.TryGetValue("EnableNameSort", out var ensVal))
							Plugin.Config.EnableNameSort = Convert.ToBoolean(ensVal);
						if (currentConfig.TryGetValue("EnableRankSort", out var ersVal))
							Plugin.Config.EnableRankSort = Convert.ToBoolean(ersVal);

						CloneSortState.ResetToValidMode();
						Plugin.Config.Save(Plugin.ConfigDirectories.ConfigPath);
						return true;
					}
					catch (Exception ex)
					{
						Plugin.Logger.LogException(ex);
						feedback = "Error saving config. See log for details.";
						return false;
					}
				});

			Plugin.Logger.Log("Registered with Mod Configuration Menu.");
		}
	}
}
#endif
