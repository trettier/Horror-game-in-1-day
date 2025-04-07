using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEscapeSkill : MonoBehaviour
{
    [SerializeField] private AudioSource deny;
    [SerializeField] private bool IsCollect = false;

    public void TryToEscape()
    {
        if (IsCollect)
        {
            SceneManager.LoadScene("Final");
        }
        else
        {
            deny.Play();
        }
    }

    public void FixPortal()
    {
        IsCollect = true;
    }
}
