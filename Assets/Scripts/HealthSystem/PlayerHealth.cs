using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _startHealth;
    [SerializeField] private int _health;
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _firstButton;
    [SerializeField] private GameObject _secondButton;

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
            _canvas.SetActive(true);
            _firstButton.SetActive(false);
            _secondButton.SetActive(true);
            StartCoroutine(KillSteletons());
        }
    }

    private IEnumerator KillSteletons()
    {
        yield return new WaitForSeconds(3f);
        var skeletons = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var skeleton in skeletons)
        {
            Destroy(skeleton);
        }
    }
}
