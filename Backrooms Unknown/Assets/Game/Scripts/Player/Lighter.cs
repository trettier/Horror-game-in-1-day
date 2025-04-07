using UnityEngine;

public class Lighter : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f; // Скорость вращения
    [SerializeField] private float followSpeed = 5f; // Скорость следования за родителем
    [SerializeField] private float offsetDistance = 1f; // Дистанция от родителя

    void Update()
    {
        if (transform.parent == null) return; // Проверка наличия родителя

        // Получаем позицию курсора
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // Вычисляем направление
        Vector3 direction = (mousePosition - transform.parent.position).normalized;

        // Вычисляем угол поворота
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90);

        // Применяем вращение
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Вычисляем целевую позицию (на расстоянии offsetDistance от родителя)
        Vector3 targetPosition = transform.parent.position + direction * offsetDistance / 4 * 3;

        // Плавное перемещение
        transform.position = Vector3.Lerp(transform.position, targetPosition , followSpeed * Time.deltaTime);
    }
}