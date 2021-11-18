using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controleQG : MonoBehaviour
{
    // Start is called before the first frame update
    
    public static string puzzle1QG(int intParte)
    {
        switch(intParte)
        {
            default:
            case 0:
            /*Teste*/case 1: return "faladoQG";
            /*Procurando as duas chaves da cabine*/case 2: return "Aqui é o QG, informe sua situação atual... caso você se depare com algum móvel que esteja trancado, procure encontrar o que poderia abri-lo. Em experimentos passados, pudemos observar como a casa pode esconder seus objetos nos lugares mais peculiares, e esses itens podem até ficar inacessíveis, dependendo da sua sorte. No entanto, com esse relógio de bolso, quem sabe você consiga encontrar coisas novas. Enquanto a casa em si se mante erguida, seus móveis ainda sofrem os efeitos inevitáveis do tempo. Também pode ter situações em que você vai precisar de um item para pode adquirir outro.";
            /*Usar desentupidor para desentupir a privada*/ case 3: return "Aqui é o QG, informe sua situação atual...  esse desentupidor que você adquiriu deve ter utilidade em algum lugar da casa. Há alguma coisa que precisa ser desentupida?";
        }
    }
    public static string puzzle2QG(int intParte)
    {
        switch (intParte)
        {
            default:
            case 0:
            /*Teste*/ case 1: return "faladoQG";
            /*Esquentar cubo de gelo com fosforo na lareira*/ case 2: return "Aqui é o QG, informe sua situação atual...  esse fósforo pode ser útil. Houveram cobaias que acendiam a lareira e se aconchegavam horas afim. Mas, como não estavam progredindo no experimento, foram neutralizados. Um conselho amigável: se for usar a lareira, que seja pra prosseguir no experimento.";
            /*Com fósforo sem gelo*/ case 3: return "Aqui é o QG, informe sua situação atual...  esse fósforo pode ser útil. Houveram cobaias que acendiam a lareira e se aconchegavam horas afim. Mas, como não estavam progredindo no experimento, foram neutralizados. Um conselho amigável: se for usar a lareira, que seja pra prosseguir no experimento.";
            /*Com gelo sem fósforo*/ case 4: return "Aqui é o QG, informe sua situação atual...  você vai precisar pegar o objeto que está dentro desse bloco de alguma forma. Como ele parece ser duro o suficiente pra não ser quebrado, é preciso de algo que possa enfraquecer sua estrutura. Quem sabe, se der uma esquentada nele.";
            /*Com ambos gelo e fósforo*/ case 5: return "Aqui é o QG, informe sua situação atual...  agora, tente encontrar uma forma de usar os fósforos pra descongelar esse cubo de gelo até que ele possa ser quebrável.";
            /*Usar estilete pra cortas as vinhas*/ case 6: return "Aqui é o QG, informe sua situação atual...  ainda bem que você escapou. Nunca antes alguém se deparou com uma coisa dessas durante o experimento (o que mais nos estranhou foi o fato dela usar as roupagens de cobaia, teremos de investigar isso mais pra frente). Talvez tenha algo no futuro que seja responsável pelo surgimento dessa criatura. Como você tem um estilete, provavelmente há algo na casa que precisa ser cortado.";
            /*Pescar chave*/ case 7: return "Aqui é o QG, informe sua situação atual...  se tiver algum objeto que você não consiga alcançar pra pegar com as próprias mãos, talvez tenha um item na casa que posse lhe dar um apoio.";
        }
    }

    public static string puzzle3QG(int intParte)
    {
        switch (intParte)
        {
            default:
            case 0:
            /*Teste*/ case 1: return "faladoQG";
            /*Com grampo sem palito*/ case 2: return "Aqui é o QG, informe sua situação atual...  esse grampo nos lembram de uma vez que um desses foi utilizado durante o experimento pra abrir a fechadura de um dos móveis da casa. No entanto, foi utilizado mais um item para ajudar a abrir a fechadura.";
            /*Com palito sem grampo*/ case 3: return "Aqui é o QG, informe sua situação atual...  estes palitos de dente nos lembram de uma vez que um desses foi utilizado durante o experimento pra abrir a fechadura de um dos móveis da casa. No entanto, foi utilizado mais um item para ajudar a abrir a fechadura.”";
            /*Com palito e grampo*/ case 4: return "Aqui é o QG, informe sua situação atual...  tente utilizar esses dois itens para abrir algo da casa que esteja trancado.";
            /*Se interagir com o ventilador*/ case 5: return "Aqui é o QG, informe sua situação atual...  ...Outros cobaias usaram o ventilador para abrir um objeto a força, talvez possa funcionar novamente.";

        }
    }
}
