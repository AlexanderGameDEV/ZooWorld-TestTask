using System;
using UnityEngine;
using ZooWorld.Common;
using ZooWorld.Data;
using ZooWorld.Gameplay.FoodChain;
using ZooWorld.Gameplay.Movement;

namespace ZooWorld.Gameplay.Animals
{
    public sealed class Animal : IFixedTickTarget
    {
        private readonly AnimalView _view;
        private readonly IMovementStrategy _movement;
        private readonly ScreenBounds _bounds;
        private readonly AnimalDefinition _definition;
        private readonly AnimalFoodChainService _foodChain;
        private readonly IHealth _health;
        private readonly AnimalStateMachine _stateMachine;
        private readonly IAnimalState _movingState;
        private readonly IAnimalState _returningState;
        private readonly IAnimalState _deadState;
        private readonly int _id;

        public Animal(
            AnimalView view,
            IMovementStrategy movement,
            ScreenBounds bounds,
            AnimalDefinition definition,
            AnimalFoodChainService foodChain)
        {
            _view = view;
            _movement = movement;
            _bounds = bounds;
            _definition = definition;
            _foodChain = foodChain;
            _health = new HealthComponent(definition.Hp);
            _stateMachine = new AnimalStateMachine();
            _movingState = new MovingState(this);
            _returningState = new ReturningToScreenState(this, bounds);
            _deadState = new DeadState(view.Body);
            _id = view.Body.GetInstanceID();
            _view.ObstacleHit += OnObstacleHit;
            _view.AnimalHit += OnAnimalHit;
        }

        public event Action<Animal> Died;

        public AnimalDefinition Definition => _definition;
        public AnimalRole Role => _definition.Role;
        public AnimalView View => _view;
        public Rigidbody Body => _view.Body;
        public int Id => _id;
        public bool IsAlive => _health.IsAlive;
        public Vector3 Position => _view.Body.position;

        public void Activate()
        {
            _health.Reset();
            _stateMachine.Change(_movingState);
        }

        public void Deactivate()
        {
            _view.ObstacleHit -= OnObstacleHit;
            _view.AnimalHit -= OnAnimalHit;
        }

        public void FixedTick(float deltaTime)
        {
            _stateMachine.Tick(deltaTime);
        }

        public void Steer(Vector3 direction)
        {
            _movement.SetDirection(direction);
        }

        public void MoveTick(float deltaTime)
        {
            _movement.Tick(_view.Body, deltaTime);
        }

        public void Push(Vector3 direction, float force)
        {
            _view.Body.AddForce(direction * force, ForceMode.Impulse);
        }

        public bool IsInsideBounds()
        {
            return _bounds.Contains(Position);
        }

        public void EnterMoving()
        {
            _stateMachine.Change(_movingState);
        }

        public void EnterReturning()
        {
            _stateMachine.Change(_returningState);
        }

        public void Damage(int amount)
        {
            if (!_health.IsAlive)
            {
                return;
            }

            _health.Damage(amount);
            if (_health.IsAlive)
            {
                return;
            }

            Die();
        }

        public void Kill()
        {
            Damage(int.MaxValue);
        }

        private void OnObstacleHit(Vector3 awayDirection)
        {
            if (!IsAlive)
            {
                return;
            }

            if (Vector3.Dot(_movement.CurrentDirection, awayDirection) >= 0f)
            {
                return;
            }

            Steer(RandomDirection.AwayFrom(awayDirection));
        }

        private void OnAnimalHit(Rigidbody otherBody)
        {
            if (!IsAlive)
            {
                return;
            }

            _foodChain.ResolveCollision(this, otherBody);
        }

        private void Die()
        {
            _stateMachine.Change(_deadState);
            Died?.Invoke(this);
        }
    }
}
