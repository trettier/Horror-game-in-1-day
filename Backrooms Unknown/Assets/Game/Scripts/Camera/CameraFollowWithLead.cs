using UnityEngine;
using UnityEngine.SceneManagement;  // �� ������ ���������� ���� namespace ��� ������ � �������

public class CameraFollowWithLead : MonoBehaviour
{
    public GameObject followTarget;  // ������ ������
    public float moveSpeed = 5f;     // �������� ���������� ������
    public float leadDistance = 2f;  // ��������� ���������� � ������� ��������
    public float smoothTime = 0.3f;  // ����� ����������� (��� ������, ��� ������� �������)

    private Vector3 velocity = Vector3.zero;  // �������� ������
    private Vector3 targetPos;        // ������� ��� ������
    private float timer;
    [SerializeField] private bool flag = false;

    private void Start()
    {
        timer = Time.time;
    }

    void FixedUpdate()
    {
        if (!flag || Time.time - timer < 1)
        {
            flag = true;
        }
        else
        {
            if (followTarget == null)
            {
                followTarget = GameObject.FindWithTag("Player");
                return;
            }

            // �������� ���� � ���������� ��� ����������� ����������� ��������
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // ���������� �������� ��� ������ � ������� ��������
            Vector3 leadOffset = new Vector3(horizontalInput, verticalInput, 0).normalized * leadDistance;

            // ������������ ������� ������� ������, �������� ��������
            targetPos = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z) + leadOffset;

            //if (SceneManager.GetActiveScene().name == "Roof")
            //{
            //    targetPos.y = Mathf.Max(targetPos.y, -4);  // ����������� ������ �� ��� Y
            //}

            // ������� ����������� ������ � �������������� SmoothDamp
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }

    }
}
