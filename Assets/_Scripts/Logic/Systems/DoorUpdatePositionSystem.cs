using Leopotam.EcsLite;
using Logic.Components;

namespace Logic.Systems
{
    public class DoorUpdatePositionSystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<Door>().Inc<Position>().End();
            var doors = world.GetPool<Door>();
            var positions = world.GetPool<Position>();
            foreach (var entity in filter)
            {
                ref var door = ref doors.Get(entity);
                ref var position = ref positions.Get(entity);
                position.Y = door.Progress;
            }
        }
    }
}
