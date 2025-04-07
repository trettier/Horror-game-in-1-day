using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI indicator;
    [SerializeField] private GameObject player;
    [SerializeField] private Slider staminaSlider;
    private Player playerController;
    private int mirrorShapesCount = 0;

    void Start()
    {
        playerController = player.GetComponent<Player>();
        //playerController.OnDeathEvent += OnDeathEvent;
        //playerController.OnCollectEvent += OnCollectEvent;
    }

    void FixedUpdate()
    {
        staminaSlider.value = playerController.GetComponent<IStaminaController>().GetStaminaValue();
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
            player.GetComponent<PlayerEscapeSkill>().FixPortal();
        }
    }
}