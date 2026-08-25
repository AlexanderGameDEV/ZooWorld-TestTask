using ZooWorld.Gameplay.Animals;

namespace ZooWorld.Gameplay.FoodChain
{
    public interface ICollisionRule
    {
        bool CanApply(Animal first, Animal second);
        void Apply(Animal first, Animal second);
    }
}
