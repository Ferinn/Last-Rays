using System.Collections.Generic;
using UnityEngine;

public class GunModulator
{
    private const int MAX_MODULES = 3;

    private GunStats modifiedStats;
    private List<GunModule> installedModules = new List<GunModule>(MAX_MODULES);
    private int uniqueModulesCount = 0;

    public GunModulator(GunData baseData, GunStats stats)
    {
        this.modifiedStats = stats;
    }

    public bool ApplyModule(GunModule module, GunModule targetMod = null)
    {
        if (installedModules.Count >= MAX_MODULES)
        {
            Debug.Log("Max capacity of modules reached!");
            return false;
        }
        if (module.type == ModuleType.Unique && uniqueModulesCount > 0)
        {
            Debug.Log("Only one unique module allowed!");
            return false;
        }
        if (installedModules.Contains(module))
        {
            Debug.Log($"Module {module.name} already installed!");
            return false;
        }

        if (module.type == ModuleType.Modifier)
        {
            if (targetMod == null)
            {
                Debug.Log("Modifier module requires a target module!");
                return false;
            }
            ApplyModifierModule(module, targetMod);
        }
        else
        {
            modifiedStats += module;
        }

        installedModules.Add(module);
        if (module.type == ModuleType.Unique) uniqueModulesCount++;

        return true;
    }

    public bool RemoveModule(GunModule module)
    {
        if (!installedModules.Contains(module))
        {
            Debug.Log($"Module {module.name} not found!");
            return false;
        }

        modifiedStats -= module;
        installedModules.Remove(module);
        if (module.type == ModuleType.Unique) uniqueModulesCount--;

        return true;
    }

    private void ApplyModifierModule(GunModule modifier, GunModule target)
    {
        Debug.Log($"Applying modifier {modifier.name} to {target.name}");
        // Custom logic for modifier modules
    }
}