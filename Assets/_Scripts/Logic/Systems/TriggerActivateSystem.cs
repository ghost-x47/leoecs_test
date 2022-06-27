using System;
using System.Runtime.CompilerServices;
using Leopotam.EcsLite;
using Logic.Components;

namespace Logic.Systems
{
    public class TriggerActivateSystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var triggers = world.GetPool<DoorTrigger>();
            var positions = world.GetPool<Position>();
            var activeTriggers = world.GetPool<Triggered>();
            var activatorsFilter = world.Filter<Player>().Inc<Position>().End();
            var inactiveTriggersFilter = world.Filter<DoorTrigger>().Inc<Position>().Exc<Triggered>().End();
            var activeTriggersFilter = world.Filter<DoorTrigger>().Inc<Position>().Inc<Triggered>().End();

            foreach (var activatorEntity in activatorsFilter)
            {
                ref var activatorPosition = ref positions.Get(activatorEntity);
                ActivateInactiveTriggers(inactiveTriggersFilter, triggers, positions, activatorPosition
                  , activeTriggers);
                DeactivateActiveTriggers(activeTriggersFilter, triggers, positions, activatorPosition, activeTriggers);
            }
        }

        private static void DeactivateActiveTriggers(
            EcsFilter activeTriggersFilter, EcsPool<DoorTrigger> triggers, EcsPool<Position> positions
          , Position activatorPosition, EcsPool<Triggered> activeTriggers
        )
        {
            foreach (var triggerEntity in activeTriggersFilter)
            {
                ref var trigger = ref triggers.Get(triggerEntity);
                ref var triggerPosition = ref positions.Get(triggerEntity);
                var playerOutsideTrigger = Distance(activatorPosition, triggerPosition) >=
                    trigger.Radius;

                if (playerOutsideTrigger)
                {
                    activeTriggers.Del(triggerEntity);
                }
            }
        }

        private static void ActivateInactiveTriggers(
            EcsFilter inactiveTriggersFilter, EcsPool<DoorTrigger> triggers, EcsPool<Position> positions
          , Position activatorPosition, EcsPool<Triggered> activeTriggers
        )
        {
            foreach (var triggerEntity in inactiveTriggersFilter)
            {
                ref var trigger = ref triggers.Get(triggerEntity);
                ref var triggerPosition = ref positions.Get(triggerEntity);
                var playerOnTrigger = Distance(activatorPosition, triggerPosition) <
                    trigger.Radius;

                if (playerOnTrigger)
                {
                    activeTriggers.Add(triggerEntity);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float Distance(Position a, Position b)
        {
            var num1 = a.X - b.X;
            var num2 = a.Y - b.Y;
            var num3 = a.Z - b.Z;
            return (float) Math.Sqrt(num1 * (double) num1 + num2 * (double) num2 + num3 * (double) num3);
        }
    }
}
