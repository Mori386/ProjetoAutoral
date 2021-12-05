using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMiniBoss : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] AudioClip miniBossPreAttack;
    [SerializeField] AudioClip miniBossDash;
    private Vector3 deltaPos;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Flashlights")
        {
            if(audioSource.isPlaying)audioSource.Stop();
            audioSource.clip = miniBossPreAttack;
            audioSource.Play();
            animator.SetTrigger("Preparation");
        }
        else if (EnragedDashing)
        {
            if (collision.CompareTag("Player"))
            {
                PMStateManager.Instance.TakeDamage(1);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Flashlights")
        {
            animator.SetTrigger("CancelPreparation");
            if(!EnragedDashing) audioSource.Stop();
        }
    }
    bool EnragedDashing;
    public void startEnragedDash()
    {
        GetComponent<CapsuleCollider2D>().isTrigger = true;
        if (audioSource.isPlaying) audioSource.Stop();
        audioSource.clip = miniBossDash;
        audioSource.Play();
        StartCoroutine(EnragedDash());
    }
    private IEnumerator EnragedDash()
    {
        EnragedDashing = true;
        CapsuleCollider2D capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        CapsuleCollider2D player = PMStateManager.Instance.GetComponent<CapsuleCollider2D>();
        while (Vector2.Distance(capsuleCollider2D.bounds.center, player.bounds.center) < 15)
        {
            transform.position += deltaPos * moveSpeed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }
    public void DefineDeltaPos()
    {
        deltaPos = Vector3.Normalize(PMStateManager.Instance.GetComponent<CapsuleCollider2D>().bounds.center - GetComponent<CapsuleCollider2D>().bounds.center);
    }
}
