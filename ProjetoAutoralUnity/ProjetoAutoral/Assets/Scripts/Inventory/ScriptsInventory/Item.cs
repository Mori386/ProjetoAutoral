using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{

    public enum ItemTimeline
    {
        Present, Future
    }
    public enum ItemType
    {
        Flashlight, Placeholder,
        KeyCabine1, KeyCabine2, Cloth, Plunger, KeyExit1, //itens do puzzle1
        KeyEscritCongela, KeyEscrit, Matches, ClothesHanger, Stilleto, KeyExit2, //itens do puzzle2
        HairClip, ToothPick, LoginPass, SecretPass, Screwdriver, KeyExit3

    }
    public ItemType itemType;
    public int amount;
    public bool active;
    public bool isAged;
    [System.NonSerialized] public ItemTimeline itemTimeline;

    [System.NonSerialized] public Item itemInFuture;
    [System.NonSerialized] public ItemWorld itemWorld;
    public string getItemName(ItemType itemType, bool isAged)
    {
        switch (MenuConfigs.getLanguage())
        {
            default:
            case "English":
                string itemName;
                switch (itemType)
                {
                    default:
                    case ItemType.Flashlight:
                        itemName = "Flashlight";
                        if (isAged) itemName += " (aged)";
                        return itemName;
                    //puzzle1
                    #region
                    case ItemType.KeyCabine1:
                        itemName = "Sink Drawer Key 1";
                        if (isAged) itemName += " (aged)";
                        return itemName;
                    case ItemType.KeyCabine2:
                        itemName = "Sink Drawer Key 2";
                        if (isAged) itemName += " (aged)";
                        return itemName;
                    case ItemType.Cloth:
                        itemName = "Piece of Cloth";
                        if (isAged) itemName += " (aged)";
                        return itemName;
                    case ItemType.Plunger:
                        itemName = "Plunger";
                        if (isAged) itemName += " (aged)";
                        return itemName;
                    case ItemType.KeyExit1:
                        itemName = "Exit Key";
                        if (isAged) itemName += " (aged)";
                        return itemName;
                    #endregion
                    //puzzle 2
                    #region
                    case ItemType.KeyEscrit:
                        itemName = "Office Room Key";
                        if (isAged) itemName += " (aged)";
                        return itemName;
                    case ItemType.KeyEscritCongela:
                        itemName = "Frozen Office Room Key";
                        if (isAged) itemName += " (aged)";
                        return itemName;
                    case ItemType.Matches:
                        itemName = "Box of Matches";
                        if (isAged) itemName += " (aged)";
                        return itemName;
                    case ItemType.ClothesHanger:
                        itemName = "Clothes Hanger";
                        if (isAged) itemName += " (aged)";
                        return itemName;
                    case ItemType.Stilleto:
                        itemName = "Box Cutter";
                        if (isAged) itemName += " (aged)";
                        return itemName;
                    case ItemType.KeyExit2:
                        itemName = "Exit Key";
                        if (isAged) itemName += " (aged)";
                        return itemName;
                    #endregion
                    //puzzle 3
                    #region
                    case ItemType.HairClip:
                        itemName = "Hair Clip";
                        if (isAged) itemName += " (aged)";
                        return itemName;
                    case ItemType.ToothPick:
                        itemName = "Box of Toothpicks";
                        if (isAged) itemName += " (aged)";
                        return itemName;
                    case ItemType.LoginPass:
                        itemName = "Login Password";
                        if (isAged) itemName += " (aged)";
                        return itemName;
                    case ItemType.SecretPass:
                        itemName = "Padlock Code";
                        if (isAged) itemName += " (aged)";
                        return itemName;
                    case ItemType.Screwdriver:
                        itemName = "Screwdriver";
                        if (isAged) itemName += " (aged)";
                        return itemName;
                    case ItemType.KeyExit3:
                        itemName = "Exit Key";
                        if (isAged) itemName += " (aged)";
                        return itemName;
                        #endregion
                }
            case "Português":
                switch (itemType)
                {
                    default:
                    case ItemType.Flashlight:
                        itemName = "Lanterna";
                        if (isAged) itemName += " (envelhecida)";
                        return itemName;
                    //puzzle1
                    #region
                    case ItemType.KeyCabine1:
                        itemName = "Chave Gaveta Pia 1";
                        if (isAged) itemName += " (envelhecida)";
                        return itemName;
                    case ItemType.KeyCabine2:
                        itemName = "Chave Gaveta Pia 2";
                        if (isAged) itemName += " (envelhecida)";
                        return itemName;
                    case ItemType.Cloth:
                        itemName = "Pedaço de Pano";
                        if (isAged) itemName += " (envelhecido)";
                        return itemName;
                    case ItemType.Plunger:
                        itemName = "Desentupidor";
                        if (isAged) itemName += " (envelhecido)";
                        return itemName;
                    case ItemType.KeyExit1:
                        itemName = "Chave de Saída";
                        if (isAged) itemName += " (envelhecida)";
                        return itemName;
                    #endregion
                    //puzzle 2
                    #region
                    case ItemType.KeyEscrit:
                        itemName = "Chave do Escritório";
                        if (isAged) itemName += " (envelhecida)";
                        return itemName;
                    case ItemType.KeyEscritCongela:
                        itemName = "Chave Congelada do Escritório";
                        if (isAged) itemName += " (envelhecida)";
                        return itemName;
                    case ItemType.Matches:
                        itemName = "Caixa de Fósforos";
                        if (isAged) itemName += " (envelhecida)";
                        return itemName;
                    case ItemType.ClothesHanger:
                        itemName = "Cabide";
                        if (isAged) itemName += " (envelhecido)";
                        return itemName;
                    case ItemType.Stilleto:
                        itemName = "Estilete";
                        if (isAged) itemName += " (envelhecido)";
                        return itemName;
                    case ItemType.KeyExit2:
                        itemName = "Chave de Saída";
                        if (isAged) itemName += " (envelhecida)";
                        return itemName;
                    #endregion
                    //puzzle 3
                    #region
                    case ItemType.HairClip:
                        itemName = "Grampo de Cabelo";
                        if (isAged) itemName += " (envelhecido)";
                        return itemName;
                    case ItemType.ToothPick:
                        itemName = "Palitos de Dente";
                        if (isAged) itemName += " (envelhecidos)";
                        return itemName;
                    case ItemType.LoginPass:
                        itemName = "Senha de Login";
                        if (isAged) itemName += " (envelhecida)";
                        return itemName;
                    case ItemType.SecretPass:
                        itemName = "Senha de Cadeado";
                        if (isAged) itemName += " (envelhecida)";
                        return itemName;
                    case ItemType.Screwdriver:
                        itemName = "Chave de Fenda";
                        if (isAged) itemName += " (envelhecida)";
                        return itemName;
                    case ItemType.KeyExit3:
                        itemName = "Chave de Saída";
                        if (isAged) itemName += " (envelhecida)";
                        return itemName;
                        #endregion
                }
        }
    }
    public bool isDestroyedOnUse()
    {
        switch (itemType)
        {
            default:
            case ItemType.Flashlight:
            case ItemType.KeyCabine1:
            case ItemType.KeyCabine2:
            case ItemType.Cloth:
            case ItemType.Plunger:
            case ItemType.KeyExit1:
            case ItemType.ClothesHanger:
            case ItemType.KeyEscrit:
            case ItemType.KeyEscritCongela:
            case ItemType.Matches:
            case ItemType.Stilleto:
            case ItemType.KeyExit2:
            case ItemType.HairClip:
            case ItemType.ToothPick:
            case ItemType.LoginPass:
            case ItemType.SecretPass:
            case ItemType.Screwdriver:
            case ItemType.KeyExit3:
                return false;
                //return true;
        }
    }
    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.Flashlight:
            case ItemType.Cloth:
            case ItemType.KeyCabine1:
            case ItemType.KeyCabine2:
            case ItemType.ClothesHanger:
            case ItemType.KeyEscrit:
            case ItemType.KeyEscritCongela:
            case ItemType.Matches:
            case ItemType.Stilleto:
            case ItemType.HairClip:
            case ItemType.ToothPick:
            case ItemType.LoginPass:
            case ItemType.SecretPass:
            case ItemType.Screwdriver:
            case ItemType.KeyExit3:
                return false;
            case ItemType.Placeholder:
                return true;
        }
    }
    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Flashlight:
                if (isAged) return ItemAssets.Instance.flashlightFutureSprite;
                else return ItemAssets.Instance.flashlightPresentSprite;

            case ItemType.Placeholder: return ItemAssets.Instance.placeholderSprite;
            //puzzle1
            #region
            case ItemType.KeyCabine1:
                if (isAged) return ItemAssets.Instance.chaveCab1FutureSprite;
                else return ItemAssets.Instance.chaveCab1PresentSprite;
            case ItemType.KeyCabine2:
                if (isAged) return ItemAssets.Instance.chaveCab2FutureSprite;
                else return ItemAssets.Instance.chaveCab2PresentSprite;
            case ItemType.Cloth:
                if (isAged) return ItemAssets.Instance.panoFutureSprite;
                else return ItemAssets.Instance.panoPresentSprite;
            case ItemType.Plunger:
                if (isAged) return ItemAssets.Instance.desentupidorFutureSprite;
                else return ItemAssets.Instance.desentupidorPresentSprite;
            case ItemType.KeyExit1:
                if (isAged) return ItemAssets.Instance.chaveSaidaFutureSprite;
                else return ItemAssets.Instance.chaveSaidaPresentSprite;
            #endregion
            //puzzle2
            #region
            case ItemType.KeyEscrit:
                if (isAged) return ItemAssets.Instance.chaveEscritFutureSprite;
                else return ItemAssets.Instance.chaveEscritPresentSprite;
            case ItemType.KeyEscritCongela:
                if (isAged) return ItemAssets.Instance.chaveCongeladaPresentSprite;
                else return ItemAssets.Instance.chaveCongelaFutureSprite;
            case ItemType.Matches:
                if (isAged) return ItemAssets.Instance.fosforoFutureSprite;
                else return ItemAssets.Instance.fosforoPresentSprite;
            case ItemType.ClothesHanger:
                if (isAged) return ItemAssets.Instance.cabideFutureSprite;
                else return ItemAssets.Instance.cabidePresentSprite;
            case ItemType.Stilleto:
                if (isAged) return ItemAssets.Instance.estileteFutureSprite;
                else return ItemAssets.Instance.estiletePresentSprite;
            case ItemType.KeyExit2:
                if (isAged) return ItemAssets.Instance.chaveSaida2FutureSprite;
                else return ItemAssets.Instance.chaveSaida2PresentSprite;
            #endregion
            //puzzle3
            #region
            case ItemType.HairClip:
                if (isAged) return ItemAssets.Instance.grampoFutureSprite;
                else return ItemAssets.Instance.grampoPresentSprite;
            case ItemType.ToothPick:
                if (isAged) return ItemAssets.Instance.palitoFutureSprite;
                else return ItemAssets.Instance.palitoPresentSprite;
            case ItemType.LoginPass:
                if (isAged) return ItemAssets.Instance.senhaLoginFutureSprite;
                else return ItemAssets.Instance.senhaLoginPresentSprite;
            case ItemType.SecretPass:
                if (isAged) return ItemAssets.Instance.senhaCompartimentoFutureSprite;
                else return ItemAssets.Instance.senhaCompartimentoPresentSprite;
            case ItemType.Screwdriver:
                if (isAged) return ItemAssets.Instance.chaveFendaFutureSprite;
                else return ItemAssets.Instance.chaveFendaPresentSprite;
            case ItemType.KeyExit3:
                if (isAged) return ItemAssets.Instance.chaveSaida3FutureSprite;
                else return ItemAssets.Instance.chaveSaida3PresentSprite;
                #endregion
        }
    }
}