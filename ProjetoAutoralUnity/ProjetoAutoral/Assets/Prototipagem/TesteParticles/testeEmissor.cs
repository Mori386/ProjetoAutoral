using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testeEmissor : MonoBehaviour
{
    private ParticleSystem partSys;
    // Update is called once per frame
    private void Start()
    {
        partSys = GetComponent<ParticleSystem>();
    }
    void Update()
    {
        if (Input.GetKeyDown("e")) partSys.Play(true);
        else partSys.Stop(true);

    }
}
