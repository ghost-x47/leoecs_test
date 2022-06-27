using Leopotam.EcsLite;
using Logic.Components;
using UnityEngine;
using Views.Components;
using Views.Helpers;

namespace Views.Systems
{
    public class UpdateViewPosition : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<Position>().Inc<View>().End();
            var positions = world.GetPool<Position>();
            var views = world.GetPool<View>();

            foreach (var entity in filter)
            {
                ref var position = ref positions.Get(entity);
                ref var view = ref views.Get(entity);
                view.Target.transform.position = position.ToVec3();
            }
        }
    }
}
