using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _walls;

    [SerializeField]
    private List<Transform> _openTriggers;

    [SerializeField]
    private List<Transform> _closeTriggers;

    [SerializeField]
    private List<Transform> _doors;

    [SerializeField]
    private Transform _player;

    [SerializeField]
    private Camera _playerCamera;

    public IEnumerable<Transform> Walls => _walls;
    public IEnumerable<Transform> Doors => _doors;
    public Transform Player => _player;
    public Camera PlayerCamera => _playerCamera;

    public IEnumerable<(Transform, float)> GetTriggers(Transform door)
    {
        var index = _doors.IndexOf(door);
        yield return (_openTriggers[index], 1f);
        yield return (_closeTriggers[index], -1f);
    }
}
