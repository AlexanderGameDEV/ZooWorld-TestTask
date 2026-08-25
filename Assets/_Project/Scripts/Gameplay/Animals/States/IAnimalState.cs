namespace ZooWorld.Gameplay.Animals
{
    public interface IAnimalState
    {
        void Enter();
        void Tick(float deltaTime);
        void Exit();
    }
}
