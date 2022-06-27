using System;
using System.Runtime.CompilerServices;
using Leopotam.EcsLite;
using Logic.Components;
using Logic.Helpers;

namespace Logic.Systems
{
    public class MovementSystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<Movement>().Inc<Position>().Inc<MoveTarget>().End();
            var targetPool = world.GetPool<MoveTarget>();
            var movementPool = world.GetPool<Movement>();
            var positions = world.GetPool<Position>();
            var config = systems.GetShared<ILogicConfig>();

            foreach (var entity in filter)
            {
                ref var target = ref targetPool.Get(entity);
                ref var movement = ref movementPool.Get(entity);
                ref var position = ref positions.Get(entity);
                var currentPosition = position;

                if (target.Value.Unpack(world, out var targetEntity))
                {
                    ref var targetPosition = ref positions.Get(targetEntity);
                    currentPosition = MoveTowards(currentPosition, targetPosition, movement.Speed * config.DeltaTime);
                    position = currentPosition;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Position MoveTowards(
            Position current,
            Position target,
            float maxDistanceDelta
        )
        {
            var num1 = target.X - current.X;
            var num2 = target.Y - current.Y;
            var num3 = target.Z - current.Z;
            var d = (float) (num1 * (double) num1 + num2 * (double) num2 + num3 * (double) num3);
            if (d == 0.0 || (maxDistanceDelta >= 0.0 && d <= maxDistanceDelta * (double) maxDistanceDelta))
            {
                return target;
            }
            var num4 = (float) Math.Sqrt(d);
            return new Position(current.X + num1 / num4 * maxDistanceDelta, current.Y + num2 / num4 * maxDistanceDelta
              , current.Z + num3 / num4 * maxDistanceDelta);
        }
    }
}
