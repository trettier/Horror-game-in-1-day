using UnityEngine;

public class Lighter : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f; // Скорость вращения

    void Update()
    {
        // Получаем позицию курсора в мировых координатах
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Обнуляем Z-координату для 2D

        // Вычисляем направление от фонарика к курсору
        Vector3 direction = (mousePosition - transform.position).normalized;
        
        // Вычисляем угол поворота в градусах
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Создаем целевой поворот
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90); // -90 чтобы фонарик смотрел вверх при угле 0

        // Плавно поворачиваем фонарик к целевому повороту
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}