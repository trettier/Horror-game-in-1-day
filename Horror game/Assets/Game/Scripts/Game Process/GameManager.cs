using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI indicator;
    [SerializeField] private GameObject player;
    [SerializeField] private Environment portal;
    private int mirrorShapesCount = 0;

    void Start()
    {
        player.GetComponent<PlayerController>().OnDeathEvent += OnDeathEvent;
        player.GetComponent<PlayerController>().OnCollectEvent += OnCollectEvent;
    }

    void OnDeathEvent()
    {
        SceneManager.LoadScene("Menu");
    }

    void OnCollectEvent()
    {
        mirrorShapesCount++;
        indicator.text = mirrorShapesCount + " из 5";
        if (mirrorShapesCount == 5)
        {
            portal.FixPortal();
        }
    }
}