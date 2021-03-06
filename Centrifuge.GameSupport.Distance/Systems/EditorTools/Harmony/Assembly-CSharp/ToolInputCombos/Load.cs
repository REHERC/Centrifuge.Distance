﻿using Centrifuge.Distance.EditorTools.Attributes;
using Centrifuge.Distance.Systems.ExportedTypes;
using HarmonyLib;
using LevelEditorTools;
using System;

namespace Centrifuge.Distance.Systems.EditorTools.Harmony
{
    [HarmonyPatch(typeof(ToolInputCombos), "Load")]
    internal static class ToolInputCombos__Load
    {
        [HarmonyPostfix]
        internal static void Postfix(ref ToolInputCombos __result, ref string fileName)
        {
            switch (fileName)
            {
                case "BlenderToolInputCombos":  // Scheme A
                    AddCustomHotkeys(ref __result, 'A');
                    break;
                case "UnityToolInputCombos":    // Scheme B
                    AddCustomHotkeys(ref __result, 'B');
                    break;
            }
        }

        internal static void AddCustomHotkeys(ref ToolInputCombos __result, char scheme)
        {
            foreach (Type toolType in TypeExportManager.GetTypesOfType(typeof(LevelEditorTool)))
            {
                LevelEditorTool instance = Activator.CreateInstance(toolType) as LevelEditorTool;

                if (toolType.HasAttribute<EditorToolAttribute>() && toolType.GetAttribute(out KeyboardShortcutAttribute attribute, false))
                {
                    __result.Add(attribute.Get(scheme).ToString(), instance.Info_.Name_);
                }
            }
        }
    }
}