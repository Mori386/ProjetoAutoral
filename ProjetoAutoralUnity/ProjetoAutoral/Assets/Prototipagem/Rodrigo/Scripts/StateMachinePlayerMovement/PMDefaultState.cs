using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMDefaultState : PMBaseState
{
    public override void EnterState(PMStateManager Manager)
    {

    }
    public override void UpdateState(PMStateManager Manager)
    {
        Manager.rawInputMove.x = Input.GetAxisRaw("Horizontal");//adiciona variaveis baseadas no input de teclas(de 0 a 1,baseado no tempo pressionado, quanto mais tempo, mais proximo de 1 e vice versa)
        Manager.rawInputMove.y = Input.GetAxisRaw("Vertical");// mesma coisa que o de cima so que para os botoes de mover na vertical
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Manager.TravelTime();
        }
    }
    public override void FixedUpdateState(PMStateManager Manager)
    {
        Manager.rb.MovePosition(Manager.rb.position + Manager.regulatorDirection(Manager.rawInputMove) * Manager.moveSpeed * Time.fixedDeltaTime);
    }
}
