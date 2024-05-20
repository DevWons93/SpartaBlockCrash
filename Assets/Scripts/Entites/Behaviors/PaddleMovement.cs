using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    private PlayerController controller;
    private Rigidbody2D rigidbody;    

    private float size;
    private float speed = 2f;    
    private Vector3 direction;

    private void Awake()
    {
        controller = GetComponentInParent<PlayerController>();
        rigidbody = GetComponent<Rigidbody2D>();             
    }

    private void Start()
    {
        controller.OnMoveEvent += Move;
        controller.OnFireEvent += Fire;        
    }

    private void Update()
    {        
        rigidbody.velocity = direction;
    }

    public void Move(float input)
    {
        if (input == 0) rigidbody.constraints |= RigidbodyConstraints2D.FreezePositionX;
        else rigidbody.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        direction = new Vector2(input, 0);
        direction = direction * speed;     
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //������Ʈ �浹 �� ȿ�� ����
        if (collision.gameObject.layer == 9)
        {
            ItemController collisionController = collision.gameObject.GetComponent<ItemController>();
            //������ Ÿ�� �к� �� ȿ�� ����
            ApplyItem(collisionController.SoItem.itemType);
            //������ ���� �� �ı�
            Destroy(collision.gameObject);
        }
    }

    public void Fire()
    {
        GameManager.Instance.Copyballs();
    }

    public void ApplyItem(EItemType itemType)
    {
        if (itemType == EItemType.SIZE)
        {
            ChangeSize();
        }
        else if (itemType == EItemType.SPEED)
        {
            ChangeSpeed();
        }
        else if (itemType == EItemType.LIFE)
        {
            GameManager.Instance.AddLife();
        }
        else if (itemType == EItemType.COPY)
        {
            GameManager.Instance.Copyballs();
        }
    }

    public void ChangeSize()
    {
        //������ �����Ͽ��� ������ �ȵ�
        float randomsize = Random.Range(-1f, 2f);

        size += randomsize;

    }

    public void ChangeSpeed()
    {
        float changespeed = Random.Range(-2f, 3f);

        speed += changespeed;
    }
}

