using Leopotam.EcsLite;
using Logic.Components;
using Views.Components;
using Views.Helpers;

namespace Views.Systems
{
    public class CreatePlayer : IEcsInitSystem
    {
        private readonly SceneData _sceneData;

        public CreatePlayer(SceneData sceneData)
        {
            _sceneData = sceneData;
        }

        public void Init(EcsSystems systems)
        {
            var playerCollider = _sceneData.Player;
            var world = systems.GetWorld();
            var config = systems.GetShared<Config>();
            var playerPool = world.GetPool<Player>();
            var views = world.GetPool<View>();
            var playerInputs = world.GetPool<PlayerInput>();
            var playerMovementPool = world.GetPool<Movement>();
            var positions = world.GetPool<Position>();
            var entity = world.NewEntity();
            ref var view = ref views.Add(entity);
            view.Target = playerCollider.gameObject;
            ref var position = ref positions.Add(entity);
            position = playerCollider.transform.position.ToPos();
            playerPool.Add(entity);
            ref var input = ref playerInputs.Add(entity);
            input.Canceled = false;
            input.Touched = false;
            input.Position = new Position(0, 0, 0);
            ref var movement = ref playerMovementPool.Add(entity);
            movement.Speed = config.PlayerSpeed;
        }
    }
}
