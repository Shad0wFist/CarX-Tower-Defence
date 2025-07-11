using UnityEngine;
using System.Collections;

public class ProjectileParticleController : MonoBehaviour
{
    [SerializeField] private ParticleSystem smokePrefab = null;

    [SerializeField, Min(0f)] private float minInterval = 0.1f;
    [SerializeField, Min(0f)] private float maxInterval = 1f;

    private ParticleSystem smokeSystem;
    private float timer;
    private int insideCount;

    private static Transform _globalSmokeContainer;

    void Awake()
    {
        // Find or create a container
        if (_globalSmokeContainer == null)
        {
            var existing = GameObject.Find("SmokeSystemsContainer");
            if (existing != null)
                _globalSmokeContainer = existing.transform;
            else
            {
                var go = new GameObject("SmokeSystemsContainer");
                _globalSmokeContainer = go.transform;
            }
        }

        smokeSystem = Instantiate(smokePrefab, _globalSmokeContainer);
        smokeSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    void OnEnable()
    {
        insideCount = 0;
        timer = Random.Range(minInterval, maxInterval);
    }

    void Update()
    {
        if (insideCount > 0)
            return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            EmitOne();
            timer = Random.Range(minInterval, maxInterval);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        insideCount++;
    }

    void OnTriggerExit(Collider other)
    {
        insideCount = Mathf.Max(0, insideCount - 1);
        // If leave the last collider, emit a particle
        if (insideCount == 0)
        {
            EmitOne();
            timer = Random.Range(minInterval, maxInterval);
        }
    }
    
    private void EmitOne()
    {
        var emitParams = new ParticleSystem.EmitParams
        {
            position = transform.position,
            applyShapeToPosition = false
        };
        smokeSystem.Emit(emitParams, 1);
    }
}
