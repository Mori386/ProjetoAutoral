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
        float delay = 0.025f;
        while (((byte)(image.color.a * 255)) <= 230)
        {
            image.color = new Color32((byte)(image.color.r*255), (byte)(image.color.g * 255), (byte)(image.color.b * 255), (byte)((image.color.a* 255)+10));
            yield return new WaitForSeconds(delay);
        }
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("MenuPresenteDefaultAni"))
        {
            animator.SetTrigger("Pr2Ft");
        }
        else
        {
            animator.SetTrigger("Ft2Pr");
        }
        while (((byte)(image.color.a * 255)) >0)
        {
            image.color = new Color32((byte)(image.color.r * 255), (byte)(image.color.g * 255), (byte)(image.color.b * 255), (byte)((image.color.a * 255) - 10));
            yield return new WaitForSeconds(delay);
        }
    }
    public void StartFade()
    {
        StartCoroutine(Fade());
    }
}
