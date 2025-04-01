using UnityEngine;

public class Lighter : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f; // �������� ��������

    void Update()
    {
        // �������� ������� ������� � ������� �����������
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // �������� Z-���������� ��� 2D

        // ��������� ����������� �� �������� � �������
        Vector3 direction = (mousePosition - transform.position).normalized;
        
        // ��������� ���� �������� � ��������
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ������� ������� �������
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90); // -90 ����� ������� ������� ����� ��� ���� 0

        // ������ ������������ ������� � �������� ��������
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}