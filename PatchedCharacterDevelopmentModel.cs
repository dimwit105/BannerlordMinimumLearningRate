using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace LearningLimitPatch
{
    public class PatchedCharacterDevelopmentModel : DefaultCharacterDevelopmentModel
    {
        public override ExplainedNumber CalculateLearningRate(
            IReadOnlyPropertyOwner<CharacterAttribute> characterAttributes,
            int focusValue,
            int skillValue,
            SkillObject skill,
            bool includeDescriptions = false)
        {
            ExplainedNumber vanilla = base.CalculateLearningRate(characterAttributes, focusValue, skillValue, skill, includeDescriptions);
            float attributelevels = 0F;
            float focusContribution = 0.1f + 0.9f * MathF.Pow(focusValue / 5f, 1.35f);
            foreach (CharacterAttribute attribute in skill.Attributes)
            {
                attributelevels += (float) characterAttributes.GetPropertyValue(attribute);
            }

            float newminimum = attributelevels * 0.1F / skill.Attributes.Length * focusContribution;
            vanilla.LimitMin(newminimum);
            return vanilla;
        }
    }
}