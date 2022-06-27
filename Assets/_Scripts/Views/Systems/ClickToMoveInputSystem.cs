using Leopotam.EcsLite;
using Logic.Components;
using UnityEngine;
using Views.Components;
using Views.Helpers;

namespace Views.Systems
{
    public class ClickToMoveInputSystem : IEcsRunSystem
    {
        private readonly Camera _camera;
        private int? _entity;
        private Plane _plane;

        public ClickToMoveInputSystem(Camera camera)
        {
            _camera = camera;
            _plane = new Plane(Vector3.up, Vector3.zero);
        }

        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();

            var playerInputs = world.GetPool<PlayerInput>();
            var moveTargets = world.GetPool<MoveTarget>();
            var filter = world.Filter<Player>().Inc<PlayerInput>().End();
            foreach (var entity in filter)
            {
                ref var input = ref playerInputs.Get(entity);
                var mouseRay = _camera.ScreenPointToRay(input.Position.ToVec3());
                if (input.Touched)
                {
                    UpdateMoveTarget(world, systems.GetShared<Config>(), mouseRay, moveTargets, entity);
                }
                else if (input.Canceled)
                {
                    RemoveMoveTarget(world, moveTargets, entity);
                }
            }
        }

        private static void RemoveMoveTarget(EcsWorld world, EcsPool<MoveTarget> moveTargets, int entity)
        {
            if (!moveTargets.Has(entity))
            {
                return;
            }

            ref var target = ref moveTargets.Get(entity);
            if (target.Value.Unpack(world, out var targetEntity))
            {
                var views = world.GetPool<View>();
                ref var view = ref views.Get(targetEntity);
                Object.Destroy(view.Target);
                world.DelEntity(targetEntity);
            }

            moveTargets.Del(entity);
        }

        private void UpdateMoveTarget(
            EcsWorld world, Config config, Ray mouseRay
          , EcsPool<MoveTarget> moveTargets, int entity
        )
        {
            if (!_plane.Raycast(mouseRay, out var distance))
            {
                return;
            }
            var positions = world.GetPool<Position>();
            var views = world.GetPool<View>();
            var point = mouseRay.GetPoint(distance);
            if (!moveTargets.Has(entity))
            {
                ref var target = ref moveTargets.Add(entity);
                var targetEntity = world.NewEntity();

                ref var position = ref positions.Add(targetEntity);
                ref var view = ref views.Add(targetEntity);
                position = point.ToPos();
                view.Target = Object.Instantiate(config.MoveTargetMarker);
                target.Value = world.PackEntity(targetEntity);
            }
            else
            {
                ref var target = ref moveTargets.Get(entity);
                if (target.Value.Unpack(world, out var targetEntity))
                {
                    ref var position = ref positions.Get(targetEntity);
                    position = point.ToPos();
                }
            }
        }
    }
}
