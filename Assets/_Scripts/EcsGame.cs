using Leopotam.EcsLite;
using Logic.Systems;
using UnityEngine;
using Views.Helpers;
using Views.Systems;

public class EcsGame : MonoBehaviour
{
    [SerializeField]
    private SceneData _sceneData;

    [SerializeField]
    private Config _config;

    private EcsSystems _updateSystems;

    private void Start()
    {
        var world = new EcsWorld();
        InitSystems(world);
        UpdateSystems(world);
    }

    private void Update() => _updateSystems.Run();

    private void OnDestroy() => _updateSystems?.Destroy();

    private void UpdateSystems(EcsWorld world)
    {
        _updateSystems = new EcsSystems(world, _config);
        _updateSystems.Add(new UpdatePlayerInput(new StandardInput()));
        _updateSystems.Add(new ClickToMoveInputSystem(_sceneData.PlayerCamera));
        _updateSystems.Add(new SpawnMoveTargetView());

        _updateSystems.Add(new MovementSystem());
        _updateSystems.Add(new TriggerActivateSystem());
        _updateSystems.Add(new DoorSpeedCalculationSystem());

        _updateSystems.Add(new DoorUpdateProgressSystem());
        _updateSystems.Add(new DoorUpdatePositionSystem());
        _updateSystems.Add(new UpdateViewPosition());
        _updateSystems.Init();
    }

    private void InitSystems(EcsWorld world)
    {
        var init = new EcsSystems(world, _config);
        init.Add(new CreateLevelLayout(_sceneData));
        init.Add(new CreateDoorsAndTriggers(_sceneData));
        init.Add(new CreatePlayer(_sceneData));
        init.Init();
        init.Destroy();
    }
}
