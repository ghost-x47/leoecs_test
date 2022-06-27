using Leopotam.EcsLite;
using Logic.Components;
using Views.Helpers;

namespace Views.Systems
{
    public class UpdatePlayerInput : IEcsRunSystem
    {
        private readonly StandardInput _input;

        public UpdatePlayerInput(StandardInput input)
        {
            _input = input;
        }

        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<PlayerInput>().End();
            var inputs = world.GetPool<PlayerInput>();
            foreach (var entity in filter)
            {
                ref var input = ref inputs.Get(entity);
                input.Canceled = _input.Cancel();
                input.Touched = _input.Touch();
                input.Position = _input.Point().ToPos();
            }
        }
    }
}
