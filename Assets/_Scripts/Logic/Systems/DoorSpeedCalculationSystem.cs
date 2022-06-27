using Leopotam.EcsLite;
using Logic.Components;

namespace Logic.Systems
{
    public class DoorSpeedCalculationSystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<DoorTriggerLink>().End();
            var doorFilter = world.Filter<Door>().End();
            var links = world.GetPool<DoorTriggerLink>();
            var triggers = world.GetPool<DoorTrigger>();
            var doors = world.GetPool<Door>();
            var activeTriggers = world.GetPool<Triggered>();

            foreach (var entity in doorFilter)
            {
                ref var door = ref doors.Get(entity);
                door.Speed = 0;
            }
            foreach (var entity in filter)
            {
                ref var link = ref links.Get(entity);
                link.Trigger.Unpack(world, out var triggerEntity);
                link.Door.Unpack(world, out var doorEntity);
                ref var door = ref doors.Get(doorEntity);
                ref var trigger = ref triggers.Get(triggerEntity);
                if (activeTriggers.Has(triggerEntity))
                {
                    door.Speed = trigger.Speed;
                }
            }
        }
    }
}
