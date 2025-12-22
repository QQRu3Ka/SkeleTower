using TMPro;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    [SerializeField] private int _points;
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    public int Points
    {
        get { return _points; }
        set { _points = value; }
    }

    public void AddPoints(int points)
    {
        _points += points;
        _textMeshPro.text = "Очков: " + _points;
    }

    public void AddOnePoint()
    {
        _points++;
        _textMeshPro.text = "Очков: " + _points;
    }

    public void RevokePoints(int points)
    {
        _points -= points;
        _textMeshPro.text = "Очков: " + _points;
    }
}
