using ZooWorld.Common;
using ZooWorld.Gameplay.Animals;

namespace ZooWorld.Gameplay.FoodChain
{
    public sealed class PredatorVsPredatorRule : PredatorFeedingRule
    {
        public PredatorVsPredatorRule(ITastyLabelSpawner labelSpawner) : base(labelSpawner)
        {
        }

        public override bool CanApply(Animal first, Animal second)
        {
            return first.Role == AnimalRole.Predator && second.Role == AnimalRole.Predator;
        }

        public override void Apply(Animal first, Animal second)
        {
            Feed(second, first);
        }
    }
}
