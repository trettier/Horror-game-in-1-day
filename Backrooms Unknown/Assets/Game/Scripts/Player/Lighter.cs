using UnityEngine;

public class Lighter : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f; // �������� ��������
    [SerializeField] private float followSpeed = 5f; // �������� ���������� �� ���������
    [SerializeField] private float offsetDistance = 1f; // ��������� �� ��������

    void Update()
    {
        if (transform.parent == null) return; // �������� ������� ��������

        // �������� ������� �������
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // ��������� �����������
        Vector3 direction = (mousePosition - transform.parent.position).normalized;

        // ��������� ���� ��������
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90);

        // ��������� ��������
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // ��������� ������� ������� (�� ���������� offsetDistance �� ��������)
        Vector3 targetPosition = transform.parent.position + direction * offsetDistance / 4 * 3;

        // ������� �����������
        transform.position = Vector3.Lerp(transform.position, targetPosition , followSpeed * Time.deltaTime);
    }
}