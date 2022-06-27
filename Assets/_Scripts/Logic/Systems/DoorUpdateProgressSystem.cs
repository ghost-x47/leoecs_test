using System;
using Leopotam.EcsLite;
using Logic.Components;
using Logic.Helpers;

namespace Logic.Systems
{
    public class DoorUpdateProgressSystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<Door>().End();
            var doors = world.GetPool<Door>();
            var config = systems.GetShared<ILogicConfig>();
            foreach (var entity in filter)
            {
                ref var door = ref doors.Get(entity);
                door.Progress = Math.Clamp(door.Progress + door.Speed * config.DeltaTime, 0, 1f);
            }
        }
    }
}
