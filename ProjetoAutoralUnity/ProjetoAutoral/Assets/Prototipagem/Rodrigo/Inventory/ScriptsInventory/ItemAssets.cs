using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public Transform pfItemWorld;
    public Sprite flashlightPresentSprite, flashlightFutureSprite;
    public Sprite placeholderSprite;
    //puzzle1
    #region
    public Sprite panoPresentSprite, panoFutureSprite,
        chaveCab1PresentSprite, chaveCab1FutureSprite,
        chaveCab2PresentSprite, chaveCab2FutureSprite,
        desentupidorPresentSprite, desentupidorFutureSprite,
        chaveSaidaPresentSprite, chaveSaidaFutureSprite;
    #endregion
}
