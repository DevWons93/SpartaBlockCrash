using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int highScore;
    private int currentScore;
    private int blockCount;
    private int stageLevel;

    public ObjectPool ObjectPool { get; private set; }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
        
        ObjectPool = GetComponent<ObjectPool>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ��� �ı� �� 
    public void DestroyBlock(int score)
    {

    }
        

    // ��� �߰� ������
    public void AddLife()
    {

    }

    // �� ���� ������
    public void CreateBalls()
    {

    }
}
