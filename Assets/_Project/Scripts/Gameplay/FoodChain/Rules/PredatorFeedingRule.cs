using ZooWorld.Common;
using ZooWorld.Gameplay.Animals;

namespace ZooWorld.Gameplay.FoodChain
{
    public abstract class PredatorFeedingRule : ICollisionRule
    {
        private readonly ITastyLabelSpawner _labelSpawner;

        protected PredatorFeedingRule(ITastyLabelSpawner labelSpawner)
        {
            _labelSpawner = labelSpawner;
        }

        public abstract bool CanApply(Animal first, Animal second);
        public abstract void Apply(Animal first, Animal second);

        protected void Feed(Animal predator, Animal victim)
        {
            victim.Kill();
            _labelSpawner.Show(predator.Position);
        }
    }
}
