using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class BallController : MonoBehaviour
{
    public float defaultSpeed;
    private Rigidbody2D rigidbody;
    private TrailRenderer trailRenderer;


    // �� Copy���� ����� ����
    private float minRotationAngle = 30f; 
    private float maxRotationAngle = 50f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();        
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(Mathf.Sign(rigidbody.velocity.x) * Mathf.Abs(defaultSpeed), rigidbody.velocity.y);
    }

    public void Copy()
    {
        GameObject rightBall = GameManager.Instance.CreateBalls();
        GameObject leftBall = GameManager.Instance.CreateBalls();

        Vector2 currentVelocity = rigidbody.velocity;
        float rotationAngle = Random.Range(minRotationAngle, maxRotationAngle);

        Vector2 rightDirection = Quaternion.Euler(0, 0, rotationAngle) * currentVelocity.normalized;
        Vector2 leftDirection = Quaternion.Euler(0, 0, -rotationAngle) * currentVelocity.normalized;

        rightBall.transform.position = this.transform.position;
        leftBall.transform.position = this.transform.position;

        Rigidbody2D rightRb = rightBall.GetComponent<Rigidbody2D>();
        Rigidbody2D leftRb = leftBall.GetComponent<Rigidbody2D>();

        rightRb.velocity = rightDirection * currentVelocity.magnitude;
        leftRb.velocity = leftDirection * currentVelocity.magnitude;
    }
 

    void OnCollisionEnter2D(Collision2D collision) //TODO ::  switch������ ��ġ��
    {
        string collisionLayerName = LayerMask.LayerToName(collision.gameObject.layer);

        switch (collisionLayerName)
        {
            case "Player":
                ProcessPaddleCollision(collision);
                break;
            case "Block":
                ProcessBlockCollision(collision);
                break;
            case "Bottom":
                Destroyed();
                break;
            default:
                break;
        }
    }

    private void ProcessPaddleCollision(Collision2D collision)
    {
        Vector2 collisionPoint = collision.contacts[0].point; // �е鿡 �����ġ�� �浹�� �Ͼ���� Ȯ�� �ϱ����� Vector
        Vector2 paddleCenter = collision.transform.position; // �е� �߽� ������������ Vector

        // �浹 ������ �е� �߽ɺ��� ���ʿ� �ִ��� Ȯ��
        bool isLeftCollision = collisionPoint.x < paddleCenter.x;

        // X�� ������ �ӵ��� ���� �Ǵ� ���������� ����
        float direction = isLeftCollision ? -1f : 1f;
        rigidbody.velocity = new Vector2(direction * Mathf.Abs(rigidbody.velocity.x), rigidbody.velocity.y);
        Debug.Log("�Ǵµ�");
    }
    private void ProcessBlockCollision(Collision2D collision)
    {
        BlockHandler blockHandler = collision.gameObject.GetComponent<BlockHandler>();
        if (blockHandler != null)
        {
            blockHandler.TakeDamage(1); // ����� HP�� ����.
        }
        else
        {
            Debug.Log("BlockHandler�� null�Դϴ�.");
        }
    }
    public void Destroyed()
    {
        GameManager.Instance.ObjectPool.ReturnObject(this.gameObject);
        GameManager.Instance.DestroyBalls();
    }

    private bool IsLayerMatched(int value, int layer)
    {
        return value == (value | 1 << layer);
    }
}
