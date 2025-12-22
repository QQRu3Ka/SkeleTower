using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _startHealth;
    [SerializeField] private int _health;
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

    public void GiveHealth(int health)
    {
        _health += health;
        if(_health > _startHealth) _health = _startHealth;
        _textMeshProUGUI.text = "’œ: " + _health;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        _textMeshProUGUI.text = "’œ: " + _health;
        if ( _health <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }
}
