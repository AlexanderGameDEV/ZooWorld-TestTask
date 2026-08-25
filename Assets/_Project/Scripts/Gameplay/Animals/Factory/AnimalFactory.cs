using UnityEngine;
using ZooWorld.Common;
using ZooWorld.Data;
using ZooWorld.Gameplay.FoodChain;
using ZooWorld.Gameplay.Movement;

namespace ZooWorld.Gameplay.Animals
{
    public sealed class AnimalFactory
    {
        private readonly MovementStrategyFactory _movementFactory;
        private readonly ScreenBounds _screenBounds;
        private readonly AnimalFoodChainService _foodChain;

        public AnimalFactory(MovementStrategyFactory movementFactory, ScreenBounds screenBounds, AnimalFoodChainService foodChain)
        {
            _movementFactory = movementFactory;
            _screenBounds = screenBounds;
            _foodChain = foodChain;
        }

        public Animal Create(AnimalView view, AnimalDefinition definition, Vector3 position, Vector3 direction)
        {
            view.PlaceAt(position);
            view.ApplyAppearance(definition);

            IMovementStrategy movement = _movementFactory.Create(definition.MovementConfig);
            movement.SetDirection(direction);

            var animal = new Animal(view, movement, _screenBounds, definition, _foodChain);
            animal.Activate();
            return animal;
        }
    }
}
