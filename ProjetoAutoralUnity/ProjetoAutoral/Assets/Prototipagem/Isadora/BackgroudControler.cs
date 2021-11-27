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
        Debug.Log((image.color.a <= (230 / 255)));
        while (image.color.a<=(230/255))
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, (image.color.a + (10 / 255)));
            yield return new WaitForSeconds(0.1f);
        }
        
    
    }
    public void StartFade()
    {
        StartCoroutine(Fade());
    }

    
}
