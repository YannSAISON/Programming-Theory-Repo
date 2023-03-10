using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetsSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _targets;
    [SerializeField] private List<SpawningZone> _spawningZones;

    [SerializeField] private float _waitTime;
    [SerializeField] private float _minInterval;
    [SerializeField] private float _maxInterval;

    private float _timer;

    private void Awake()
    {
        _timer = _waitTime;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            int targetIndex = Random.Range(0, _targets.Count);
            GameObject target = Instantiate(_targets[targetIndex]);

            target.GetComponent<Target>().SpawningZones = _spawningZones.ToArray();

            _timer = Random.Range(_minInterval, _maxInterval);
        }
    }
}
