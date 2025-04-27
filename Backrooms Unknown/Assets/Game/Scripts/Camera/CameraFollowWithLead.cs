using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class CameraFollowWithLead : MonoBehaviour
{
    public float moveSpeed = 5f;     // Скорость перемещения камеры
    public float leadDistance = 2f;  // Дистанция упреждения в направлении движения
    public float smoothTime = 0.3f;  // Время сглаживания (для SmoothDamp)

    private Vector3 velocity = Vector3.zero;  // Вектор скорости
    private Vector3 targetPos;        // Целевая позиция для камеры
    private float timer;
    [SerializeField] private bool flag = false;
    private GameObject followTarget;  // Теперь приватное, назначается через поиск локального игрока

    void FixedUpdate()
    {
        if (followTarget == null) return; // Если нет цели, ничего не делаем

        // Получаем ввод для расчета упреждения
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 leadOffset = new Vector3(horizontalInput, verticalInput, 0).normalized * leadDistance;
        targetPos = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z) + leadOffset;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }



    public void SetTarget(GameObject target)
    {
        // Добавляем проверку isLocalPlayer при ручном назначении цели
        if (target != null && target.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            followTarget = target;
        }
    }
}