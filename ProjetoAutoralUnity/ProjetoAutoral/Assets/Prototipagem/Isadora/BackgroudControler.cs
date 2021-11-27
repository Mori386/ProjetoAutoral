using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroudControler : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Animator animator;
    private IEnumerator Fade()
    {
        while (((byte)(image.color.a * 255)) <= 230)
        {
            image.color = new Color32((byte)(image.color.r*255), (byte)(image.color.g * 255), (byte)(image.color.b * 255), (byte)((image.color.a* 255)+10));
            yield return new WaitForSeconds(0.1f);
        }

    }
    public void StartFade()
    {
        StartCoroutine(Fade());
    }
}
