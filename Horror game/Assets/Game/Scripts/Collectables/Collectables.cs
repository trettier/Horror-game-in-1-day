using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    public float amplitude = 0.5f;  // ������ ���������
    public float frequency = 1f;   // �������� ���������
    public float damping = 0.1f;   // ����������� ���������� (������ = ��������� ���������)

    private Rigidbody2D rb;
    private float initialY;
    private float velocity = 0f; // �������� ��� SmoothDamp

    [SerializeField] private CollectableType collectableType;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // ��������, ��� Rigidbody2D �������� �� �������������� ���������
        rb.bodyType = RigidbodyType2D.Kinematic;

        // ��������� ��������� ���������
        initialY = transform.position.y;
    }

    void FixedUpdate()
    {
        // ������� ������� �������� �� ������ ���������
        float targetY = initialY + amplitude * Mathf.Sin(Time.time * frequency * 2 * Mathf.PI);

        // ������� �������� � �����������
        float newY = Mathf.SmoothDamp(transform.position.y, targetY, ref velocity, damping);

        // ������������� ����� ������� ����� Rigidbody2D
        rb.MovePosition(new Vector2(transform.position.x, newY));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && collectableType == CollectableType.MirrorShape)
        {
            other.gameObject.GetComponent<PlayerController>().Collect(gameObject);
        }
    }
}

public enum CollectableType
{
    MirrorShape
}