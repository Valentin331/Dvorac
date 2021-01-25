using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    private float duration = .085f;

    public IEnumerator ChangeDZColor(string mode)
    {
        float timeElapsed = 0.0f;
        float t = 0.0f;
        if (mode == "on")
        {
            while (t < 1.0f)
            {
                timeElapsed += Time.deltaTime;
                t = timeElapsed / duration;
                GetComponent<Image>().color = Color.Lerp(new Color32(255, 255, 255, 0), new Color32(255, 255, 255, 35), t);
                yield return null;
            }
        }
        else if (mode == "off")
        {
            while (t < 1.0f)
            {
                timeElapsed += Time.deltaTime;
                t = timeElapsed / duration;
                GetComponent<Image>().color = Color.Lerp(new Color32(255, 255, 255, 35), new Color32(255, 255, 255, 0), t);
                yield return null;
            }
        }
        
    }
}
