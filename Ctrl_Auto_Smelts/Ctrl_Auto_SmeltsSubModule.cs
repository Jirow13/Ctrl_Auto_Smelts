using System;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade;
using System.Linq;
using System.Text;
using HarmonyLib;
using System.Windows;
using TaleWorlds.CampaignSystem.ViewModelCollection.Craft.Smelting;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.InputSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Craft.Refinement;

namespace Ctrl_Auto_Smelts {
    public class Ctrl_Auto_SmeltsSubModule : MBSubModuleBase {
        protected override void OnSubModuleLoad() {
            base.OnSubModuleLoad();
            try {
                Harmony patcher = new Harmony("Reworked_SkillsSubModulePatcher");
                patcher.PatchAll();
            } catch (Exception exception1) {
                string message;
                Exception exception = exception1;
                string str = exception.Message;
                Exception innerException = exception.InnerException;
                if (innerException != null) {
                    message = innerException.Message;
                } else {
                    message = null;
                }
                MessageBox.Show(string.Concat("Reworked_SkillsSubModule Error patching:\n", str, " \n\n", message));
            }
        }
    }



    [HarmonyPatch(typeof(SmeltingVM), "SmeltSelectedItems", typeof(Hero))]
    public class Patch1 {
        private static bool repeating;

        static void Postfix(SmeltingVM __instance, Hero currentCraftingHero, ref SmeltingItemVM ____currentSelectedItem, ref ICraftingCampaignBehavior ____smithingBehavior) {
            if (repeating) {
                return;
            }
            if (____currentSelectedItem != null && ____smithingBehavior != null && Input.IsKeyDown(InputKey.LeftControl)) {
                repeating = true;
                int maxfailsafe = 100;
                while (maxfailsafe-- > 0 && ____currentSelectedItem != null && ____smithingBehavior != null) {
                    __instance.SmeltSelectedItems(currentCraftingHero);
                }
                repeating = false;
            }
        }
    }    
    [HarmonyPatch(typeof(RefinementVM), "ExecuteSelectedRefinement", typeof(Hero))]
    public class Patch2 {
        private static bool repeating;

        static void Postfix(RefinementVM __instance, Hero currentCraftingHero, ref RefinementActionItemVM ____currentSelectedAction) {
            if (repeating) {
                return;
            }
            if (____currentSelectedAction != null && Input.IsKeyDown(InputKey.LeftControl)) {
                repeating = true;
                int maxfailsafe = 100;
                while (maxfailsafe-- > 0 && ____currentSelectedAction != null) {
                    __instance.ExecuteSelectedRefinement(currentCraftingHero);
                }
                repeating = false;
            }
        }
    }
}
