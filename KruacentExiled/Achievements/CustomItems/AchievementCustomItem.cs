using Exiled.API.Features;
using KruacentExiled.CustomItems.API.Features;
using ServerAchievementFramework.API.Feature;
using Exiled.API.Features.Items;

namespace KruacentExiled.Achievements.CustomItems
{
    public abstract class AchievementCustomItem : AchievementItem
    {

        public abstract string CustomItemName { get; }


        protected override bool CheckItem(Item item)
        {
            if (KECustomItem.TryGet(CustomItemName, out KECustomItem ci))
            {
                return ci.Check(item);
            }
            else
            {
                Log.Warn(CustomItemName + " not found");
            }
            return false;
        }
    }
}
