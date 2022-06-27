using Logic.Helpers;
using UnityEngine;
using ILogger = Logic.Helpers.ILogger;

[CreateAssetMenu(menuName = "New Config")]
public class Config : ScriptableObject, ILogicConfig, ILogger
{
    [SerializeField]
    private float _playerSpeed = 1f;

    [SerializeField]
    private float _doorSpeed = 1f;

    [SerializeField]
    private float _triggerRadius = 1f;

    [SerializeField]
    private GameObject _marker;

    public float PlayerSpeed => _playerSpeed;
    public float TriggerRadius => _triggerRadius;
    public GameObject MoveTargetMarker => _marker;
    public float DoorSpeed => _doorSpeed;
    public void Log(string message) => Debug.Log(message);
    public float DeltaTime => Time.deltaTime;
}
