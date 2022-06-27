using Leopotam.EcsLite;
using Logic.Components;
using UnityEngine;
using Views.Components;
using Views.Helpers;

namespace Views.Systems
{
    public class CreateDoorsAndTriggers : IEcsInitSystem
    {
        private readonly SceneData _sceneData;

        public CreateDoorsAndTriggers(SceneData sceneData)
        {
            _sceneData = sceneData;
        }

        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();
            foreach (var doorTransform in _sceneData.Doors)
            {
                var config = systems.GetShared<Config>();
                var door = CreateDoor(world, doorTransform);
                foreach (var (triggerTransform, speed) in _sceneData.GetTriggers(doorTransform))
                {
                    var trigger = CreateTrigger(world, config, triggerTransform, config.DoorSpeed * speed);
                    CreateLink(world, world.NewEntity(), door, trigger);
                }
            }
        }

        private static int CreateLink(
            EcsWorld world, int linkEntity, int doorEntity
          , int triggerEntity
        )
        {
            var links = world.GetPool<DoorTriggerLink>();
            ref var linkData = ref links.Add(linkEntity);
            linkData.Door = world.PackEntity(doorEntity);
            linkData.Trigger = world.PackEntity(triggerEntity);
            return linkEntity;
        }

        private static int CreateTrigger(
            EcsWorld world, Config config, Transform transform
          , float speed
        )
        {
            var triggerEntity = world.NewEntity();
            var triggers = world.GetPool<DoorTrigger>();
            var positions = world.GetPool<Position>();

            ref var trigger = ref triggers.Add(triggerEntity);
            trigger.Speed = speed;
            ref var position = ref positions.Add(triggerEntity);
            position = transform.position.ToPos();
            trigger.Radius = config.TriggerRadius * transform.localScale.magnitude;

            return triggerEntity;
        }

        private static int CreateDoor(EcsWorld world, Transform transform)
        {
            var doorEntity = world.NewEntity();
            var doors = world.GetPool<Door>();
            var positions = world.GetPool<Position>();
            var views = world.GetPool<View>();
            ref var position = ref positions.Add(doorEntity);
            position = transform.position.ToPos();
            doors.Add(doorEntity);
            ref var view = ref views.Add(doorEntity);
            view.Target = transform.gameObject;
            return doorEntity;
        }
    }
}
