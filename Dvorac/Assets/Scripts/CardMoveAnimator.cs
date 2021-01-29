using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMoveAnimator : MonoBehaviour
{
    public IEnumerator AnimateCardMove(GameObject card, Vector2 from, Vector2 to, float duration)
    {
        float timeElapsed = 0.0f;
        float t = 0.0f;

        while (t < 1.0f)
        {
            timeElapsed += Time.deltaTime;
            t = timeElapsed / duration;
            card.transform.position = Vector2.Lerp(from, to, t);
            yield return null;
        }
    }
}
