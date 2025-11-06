using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class EchoGhost : MonoBehaviour
{
    private List<Vector3> path;
    private List<Quaternion> rots;
    private float speed = 1f;
    private Material mat;
    private Color baseColor;

    public void Initialize(List<Vector3> p, List<Quaternion> r, float playbackSpeed = 1f)
    {
        path = p; rots = r; speed = Mathf.Max(0.01f, playbackSpeed);
        mat = GetComponent<Renderer>().material;
        baseColor = mat.color;
        // start transparent
        mat.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0f);
        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        // quick fade-in
        yield return Fade(0f, 0.35f, 0.2f);

        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector3 a = path[i], b = path[i + 1];
            Quaternion ra = rots[i], rb = rots[i + 1];
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(a, b, t);
                transform.rotation = Quaternion.Slerp(ra, rb, t);
                yield return null;
            }
        }

        // fade-out and destroy
        yield return Fade(0.35f, 0f, 0.3f);
        Destroy(gameObject);
    }

    private IEnumerator Fade(float fromA, float toA, float time)
    {
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(fromA, toA, t / time);
            mat.color = new Color(baseColor.r, baseColor.g, baseColor.b, a);
            yield return null;
        }
    }
}
