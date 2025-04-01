using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration && Time.timeScale == 1)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, 0) + transform.localPosition;

            elapsed += Time.deltaTime;

            yield return null;
        }
    }
}
