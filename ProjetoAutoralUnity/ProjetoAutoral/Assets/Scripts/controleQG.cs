using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controleQG : MonoBehaviour
{
    // Start is called before the first frame update
    
    public static string FailQG()
    {
        return "fail";
    }
    public static string puzzle1QG(int intParte)
    {
        switch(intParte)
        {
            default:
            case 0:
            /*Teste*/case 1: return "faladoQG";
            /*Procurando as duas chaves da cabine*/case 2: return "This is HQ, please inform your current situation... if you come across some furniture that is locked, try to find what could open it. In past experiments, we were able to observe how the house can hide its objects in the most peculiar places, and these items can even be inaccessible, depending on your luck. However, with this pocket watch, maybe you can find new things by travelling to the future and back. While the house will stay unharmed, it's furnitures will suffer the inevitable effects of time. You may also have situations where you will need one item to get another.";
            /*Usar desentupidor para desentupir a privada*/ case 3: return "This is HQ, please inform your current situation... this plunger that you got must have some use somewhere in the house. Is there anything that needs to be unclogged?";
        }
    }
    public static string puzzle2QG(int intParte)
    {
        switch (intParte)
        {
            default:
            case 0:
            /*Teste*/ case 1: return "faladoQG";
            /*Esquentar cubo de gelo com fosforo na lareira*/ case 2: return "This is HQ, please inform your current situation...  in some cases, even though furniture doesn't seem to have an item, it can be crucial to solve a problem. Also, there's moments where only one object won't be enough to move on forward.";
            /*Com fósforo sem gelo*/ case 3: return "This is HQ, please inform your current situation...  this match can be usefull. There were test subject that ignited the fireplace and snoozed for hours on end. Since weren't progressing with their obligation, they were neutralized. So, our friendly advice is that, if you want to use the fireplace, use it for the experiment.";
            /*Com gelo sem fósforo*/ case 4: return "This is HQ, please inform your current situation...  well, you need to find a way to get the object that is inside this ice block. It seems to be to hard to break, so you need something to weaken its structure - maybe, if got warmer?";
            /*Com ambos gelo e fósforo*/ case 5: return "This is HQ, please inform your current situation...  now, try to find a way to use those matches to melt the cube of ice, so it can be breakable.";
            /*Usar estilete pra cortas as vinhas*/ case 6: return "This is HQ, please inform your current situation...  we're glad you've escaped. No other test subject ever came across this creature (strangely enough, it was wearing test subjects clothes, we need to investigate that afterward). Maybe something in future is responsible for the appearence of such creature. Regardin the a box cutter, there' probably something you could cut with it.";
            /*Pescar chave*/ case 7: return "This is HQ, please inform your current situation...  if there is any object that you can't reach with your own hands, maybe there is an item in the house that can give you some extra reach.";
        }
    }

    public static string puzzle3QG(int intParte)
    {
        switch (intParte)
        {
            default:
            case 0:
            /*Teste*/ case 1: return "faladoQG";
            /*Com grampo sem palito*/ case 2: return "This is HQ, please inform your current situation...  this hairclip reminds us of one time where one of these was used to open the lock of a furniture at the house. However, there was also another item used in conjunction with it.";
            /*Com palito sem grampo*/ case 3: return "This is HQ, please inform your current situation...  this box of toothpicks reminds us of one time where one of these was used to open the lock of a furniture at the house. However, there was also another item used in conjunction with it.";
            /*Com palito e grampo*/ case 4: return "This is HQ, please inform your current situation...  try to use these items to pick something that is locked.";
            /*Se interagir com o ventilador*/ case 5: return "This is HQ, please inform your current situation...  other test subjects used the fan to force break open an object, you could try to use it as well.";
            /*Apos obter código do cadeado*/ case 6: return "This is HQ, please inform your current situation...  since you got a code, try searching the house to find the padlock where you could use it.";
        }
    }
}
