using ZooWorld.Common;
using ZooWorld.Gameplay.Animals;

namespace ZooWorld.Gameplay.FoodChain
{
    public sealed class PredatorEatsPreyRule : PredatorFeedingRule
    {
        public PredatorEatsPreyRule(ITastyLabelSpawner labelSpawner) : base(labelSpawner)
        {
        }

        public override bool CanApply(Animal first, Animal second)
        {
            return first.Role != second.Role;
        }

        public override void Apply(Animal first, Animal second)
        {
            Animal predator = first.Role == AnimalRole.Predator ? first : second;
            Animal prey = first.Role == AnimalRole.Prey ? first : second;
            Feed(predator, prey);
        }
    }
}
