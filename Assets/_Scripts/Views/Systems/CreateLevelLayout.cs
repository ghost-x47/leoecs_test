using Leopotam.EcsLite;
using Logic.Components;
using Views.Helpers;

namespace Views.Systems
{
    internal class CreateLevelLayout : IEcsInitSystem
    {
        private readonly SceneData _sceneData;

        public CreateLevelLayout(SceneData sceneData)
        {
            _sceneData = sceneData;
        }

        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();
            foreach (var wall in _sceneData.Walls)
            {
                var entity = world.NewEntity();
                var positions = world.GetPool<Position>();
                ref var position = ref positions.Add(entity);
                position = wall.transform.position.ToPos();
            }
        }
    }
}
