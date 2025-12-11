using UnityEngine;

public class PointSystem : MonoBehaviour
{
    [SerializeField] private int _points;
    public int Points
    {
        get { return _points; }
        set { _points = value; }
    }

    public void AddPoints(int points)
    {
        _points += points;
    }

    public void AddOnePoint()
    {
        _points++;
    }

    public void RevokePoints(int points)
    {
        _points -= points;
    }
}
