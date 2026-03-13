using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KnifeScript : MonoBehaviour
{
    [SerializeField]
    private Vector2 throwForce;

    private bool isActive = true;

    private Rigidbody2D rb;
    private BoxCollider2D knifeCollider;

    [Header("Sound Effects")]
    [SerializeField]
    private AudioClip hitSound;
    [SerializeField]
    private AudioClip bounceSound;
    private AudioSource audioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        knifeCollider = GetComponent<BoxCollider2D>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isActive && !PauseMenu.IsInteractingWithUI())
        {
            ThrowKnife();
        }
    }

    private void ThrowKnife()
    {
        rb.AddForce(throwForce, ForceMode2D.Impulse);
        rb.gravityScale = 1;
        GameController.Instance.GameUi.DecrementDisplayedKnifeCount();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isActive)
            return;

        isActive = false;

        if (collision.collider.CompareTag("Log"))
        {
            if (hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }

            GetComponent<ParticleSystem>().Play();
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            transform.SetParent(collision.collider.transform);

            knifeCollider.offset = new Vector2(knifeCollider.offset.x, -0.4f);
            knifeCollider.size = new Vector2(knifeCollider.size.x, 1.2f);

            GameController.Instance.OnSuccessfulKnifeHit();
        }
        else if (collision.collider.CompareTag("Knife"))
        {
            if (bounceSound != null)
            {
                audioSource.PlayOneShot(bounceSound);
            }
            rb.velocity = new Vector2(rb.velocity.x, -2);
            GameController.Instance.StartGameOverSequence(false);
        }
    }
}