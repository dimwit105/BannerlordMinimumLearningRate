using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace LearningLimitPatch
{
    public class LearningLimitPatchSubmodule : MBSubModuleBase
    {
        private Harmony _harmony;
        private const string HarmonyID = "learninglimitpatch";
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            _harmony = new Harmony(HarmonyID);
            Harmony.DEBUG = true;
            _harmony.PatchAll();
            
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();
            _harmony.UnpatchAll(HarmonyID);
        }
    }
    [HarmonyPatch]
    public static class LearningLimitPatch
    {
        [HarmonyPatch(typeof(DefaultCharacterDevelopmentModel), "CalculateLearningRate")]
        [HarmonyPostfix]
        public static void Postfix_CalculateLearningRate(
            IReadOnlyPropertyOwner<CharacterAttribute> characterAttributes,
            int focusValue,
            int skillValue,
            SkillObject skill,
            bool includeDescriptions,
            ref ExplainedNumber __result)
        {
            float attributelevels = 0F;
            foreach (CharacterAttribute attribute in skill.Attributes)
            {
                attributelevels += (float) characterAttributes.GetPropertyValue(attribute);
            }
            __result.LimitMin(attributelevels*0.1F);
        }
    }
}