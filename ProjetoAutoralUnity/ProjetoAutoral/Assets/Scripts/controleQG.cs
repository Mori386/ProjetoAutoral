﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controleQG : MonoBehaviour
{
    // Start is called before the first frame update
    
    public static string FailQG()
    {
        return "... I can't find HQ's signal.";
    }
    public static string puzzle1QG(int intParte)
    {
        switch(intParte)
        {
            default:
            case 0:
            /*Teste*/case 1: return "faladoQG";
            /*Procurando as duas chaves da cabine*/case 2: return "This is HQ, please inform your current situation... if you come across some furniture... that is locked, try to find what could open it. In past experiments, we were able to note how... the house hides its objects in the most peculiar places - some of them even being... inaccessible, depending on your luck. However, with this pocket watch, maybe you can find... new paths by travelling to the future and back. While the house might stay upright for... thousands of years, it's furniture will suffer the inevitable effects of time. You may also... have situations where you will need one item to get another.";
            /*Usar desentupidor para desentupir a privada*/ case 3: return "This is HQ, please inform your current situation... this plunger that you got must have... some use somewhere in the house. Is there anything that needs to be unclogged?";
        }
    }
    public static string puzzle2QG(int intParte)
    {
        switch (intParte)
        {
            default:
            case 0:
            /*Teste*/ case 1: return "faladoQG";
            /*Esquentar cubo de gelo com fosforo na lareira*/ case 2: return "This is HQ, please inform your current situation...  in some cases, even though... furniture doesn't seem to have an item, it can be crucial to solve a problem. Also, there's... situations where only one object won't be enough to move on forward.";
            /*Com fósforo sem gelo*/ case 3: return "This is HQ, please inform your current situation...  this match can be usefull. There... were test subjects that ignited the fireplace and snoozed for hours on end. Since they... weren't progressing with their obligation, we neutralized them. So, our friendly advice is... that, if you want to use the fireplace, use it for the experiment.";
            /*Com gelo sem fósforo*/ case 4: return "This is HQ, please inform your current situation...  well, you need to find a way to... get the object that is inside this ice block. It seems to be to hard to break, so you need... something to weaken it's structure - maybe, if it got warmed up?";
            /*Com ambos gelo e fósforo*/ case 5: return "This is HQ, please inform your current situation...  now, try to find a way to use those matches to melt the cube of ice, so it can be breakable.";
            /*Usar estilete pra cortas as vinhas*/ case 6: return "This is HQ, please inform your current situation...  we're glad you've escaped. No... other test subjects ever came across this creature - strangely enough, it was wearing... the same clothes you're wearing, we'll need to investigate this further, afterwards. Maybe... something in the future is responsible for such creature. Regarding the a box cutter, there's... probably something you could cut out with it.";
            /*Pescar chave*/ case 7: return "This is HQ, please inform your current situation...  if there's any object that you can't... reach with your own hands, maybe there is an item in the house that can give you some... extra reach.";
        }
    }

    public static string puzzle3QG(int intParte)
    {
        switch (intParte)
        {
            default:
            case 0:
            /*Teste*/ case 1: return "faladoQG";
            /*Com grampo sem palito*/ case 2: return "This is HQ, please inform your current situation...  this hairclip reminds us of one time... where one of these was used to open the lock of a furniture at the house. However, there was... also another item used in conjunction with it.";
            /*Com palito sem grampo*/ case 3: return "This is HQ, please inform your current situation...  this box of toothpicks reminds us... of one time where one of these was used to open the lock of a furniture at the house. However... there was also another item used in conjunction with it.";
            /*Com palito e grampo*/ case 4: return "This is HQ, please inform your current situation...  try to use these items to pick something that is locked.";
            /*Se interagir com o ventilador*/ case 5: return "This is HQ, please inform your current situation...  other test subjects used the... ceiling fan to forcefully break open an object, you could try to use it as well.";
            /*Apos obter código do cadeado*/ case 6: return "This is HQ, please inform your current situation...  since you got a code, try searching the house to find the padlock where you could use it.";
        }
    }
}
