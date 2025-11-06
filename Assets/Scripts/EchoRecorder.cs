using System.Collections.Generic;
using UnityEngine;

public class EchoRecorder : MonoBehaviour
{
    [Header("Record")]
    public Transform target;            // assign Player (capsule)
    public float sampleInterval = 0.1f; // seconds
    public int maxSamples = 600;

    [Header("Ghost")]
    public EchoGhost ghostPrefab;       // assign prefab below
    public float playbackSpeed = 1.0f;  // 1 = realtime

    private readonly List<Vector3> positions = new();
    private readonly List<Quaternion> rotations = new();
    private float t;

    void Update()
    {
        // sample target transform
        t += Time.deltaTime;
        if (t >= sampleInterval && target != null)
        {
            t = 0f;
            if (positions.Count >= maxSamples) { positions.RemoveAt(0); rotations.RemoveAt(0); }
            positions.Add(target.position);
            rotations.Add(target.rotation);
        }

        // press R to spawn a replay ghost
        if (Input.GetKeyDown(KeyCode.R)) SpawnGhost();
    }

    public void SpawnGhost()
    {
        if (ghostPrefab == null || positions.Count < 4) return;
        var g = Instantiate(ghostPrefab, positions[0], rotations[0]);
        g.Initialize(new List<Vector3>(positions), new List<Quaternion>(rotations), playbackSpeed);
    }
}
