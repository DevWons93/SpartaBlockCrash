using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private Animator animator;


    public void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(BossAttackRoutine());

    }

    private IEnumerator BossAttackRoutine()
    {
        Debug.Log("�ִϸ��̼ǽ���");
        while (true)
        {
            yield return new WaitForSeconds(4f);

            int randomValue = Random.Range(0, 4);
            if (randomValue == 0)
            {
                Debug.Log("����1");
                animator.SetTrigger("Attack1");
            }
            else if (randomValue == 1)
            {
                Debug.Log("����2");
                animator.SetTrigger("Attack2");
            }
            else if (randomValue == 2)
            {
                Debug.Log("����3");
                animator.SetTrigger("Attack3");
            }
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹�� ������Ʈ�� ������ Ȯ��
        if (collision.gameObject.CompareTag("Ball"))
        {
            Debug.Log("��Ʈ");
            animator.SetTrigger("BossHit");
        }
        // �÷��̾�� �浹�ߴ��� Ȯ��
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("�÷��̾�ų");
            Destroy(collision.gameObject); // �÷��̾� ������Ʈ�� �ı�
        }
    }
}
