using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float defaultSpeed;
    private Rigidbody2D rigidbody;
    //private TrailRenderer trailRenderer;    

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        //trailRenderer = GetComponent<TrailRenderer>();
        defaultSpeed = 5f;
    }

    public void Copy()
    {
        GameObject rightBall = GameManager.Instance.CreateBalls();
        GameObject leftBall = GameManager.Instance.CreateBalls();

        Vector2 upwardDirection = new Vector2(0, 1);
        Vector2 rightDirection = Quaternion.Euler(0, 0, 45) * upwardDirection; // ������ �������� 45�� ȸ��
        Vector2 leftDirection = Quaternion.Euler(0, 0, -45) * upwardDirection; // ���� �������� 45�� ȸ��

        rightBall.transform.position = this.transform.position;
        leftBall.transform.position = this.transform.position;

        Rigidbody2D rightRb = rightBall.GetComponent<Rigidbody2D>();
        Rigidbody2D leftRb = leftBall.GetComponent<Rigidbody2D>();
        rightRb.velocity = rightDirection.normalized * rigidbody.velocity.magnitude;
        leftRb.velocity = leftDirection.normalized * rigidbody.velocity.magnitude;
    }

    public void Destroyed()
    {
        GameManager.Instance.ObjectPool.ReturnObject(this.gameObject);
        GameManager.Instance.DestroyBalls();
    }

    void OnCollisionEnter2D(Collision2D collision) //TODO ::  switch������ ��ġ��
    {
        rigidbody.velocity = rigidbody.velocity.normalized * defaultSpeed;
        int bottomLayer = LayerMask.NameToLayer("Bottom");
        int blockLayer = LayerMask.NameToLayer("Block");
        int paddleLayer = LayerMask.NameToLayer("Paddle");

        if (collision.gameObject.layer == paddleLayer)
        {
            ProcessPaddleCollision(collision);
        }
        if (collision.gameObject.layer == blockLayer)
        {
            //Block�νô·���
            Debug.Log("Hit");
        }
        if (collision.gameObject.layer == bottomLayer)
        {
            Destroyed();
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
    }

    private bool IsLayerMatched(int value, int layer)
    {
        return value == (value | 1 << layer);
    }
}
