using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lastDoorEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("boss"))
       {
            Destroy(transform.Find("Porta1").gameObject);
            Destroy(transform.Find("Porta2").gameObject);
            GetComponent<SpriteRenderer>().enabled = false;
            transform.Find("finalLight").gameObject.SetActive(true);
        }
    }
}
