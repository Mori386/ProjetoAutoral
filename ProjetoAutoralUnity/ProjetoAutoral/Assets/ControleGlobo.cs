using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleGlobo : MonoBehaviour
{
    [SerializeField]
    private GameObject globo;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject==globo)
        {
            ChangeIsMovable(collision, false);
            globo.GetComponent<MovableObjectStateManager>().enabled = false;
            globo.GetComponent<IOStateManager>().enabled = true;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        ChangeIsMovable(collision, true);
    }
    private void ChangeIsMovable(Collider2D collision, bool isMovableBool)
    {
        if (collision.isTrigger == false)
        {
            MovableObjectStateManager movableObjectStateManager = collision.GetComponent<MovableObjectStateManager>();
            if (movableObjectStateManager != null)
            {
                movableObjectStateManager.isMovable = isMovableBool;
                if (movableObjectStateManager.isMovable)
                {
                    foreach (BoxCollider2D box in movableObjectStateManager.GetComponents<BoxCollider2D>())
                    {
                        if (box.isTrigger == true) box.enabled = true;
                    }
                }
                else
                {
                    foreach (BoxCollider2D box in movableObjectStateManager.GetComponents<BoxCollider2D>())
                    {
                        if (box.isTrigger == true) box.enabled = false;
                    }
                }
                if (movableObjectStateManager.isMovable) movableObjectStateManager.SwitchState(movableObjectStateManager.movableState);
                else movableObjectStateManager.SwitchState(movableObjectStateManager.notMovableState);

            }
        }

    }
}
