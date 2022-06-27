using Leopotam.EcsLite;
using Logic.Components;
using UnityEngine;
using Views.Components;

namespace Views.Systems
{
    public class SpawnMoveTargetView : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<MoveTarget>().Exc<View>().End();
            var moveTargets = world.GetPool<MoveTarget>();
            var moveTargetsViews = world.GetPool<View>();
            var config = systems.GetShared<Config>();
            foreach (var entity in filter)
            {
                ref var moveTarget = ref moveTargets.Get(entity);
                var marker = Object.Instantiate(config.MoveTargetMarker);
                ref var moveTargetView = ref moveTargetsViews.Add(entity);
                moveTargetView.Target = marker;
            }
        }
    }
}
