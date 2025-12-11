using EasyButtons;
using UnityEngine;

public class PointSystemTest : MonoBehaviour
{
    [SerializeField] private PointSystem _pointSystem;

    [Button]
    public void TestAddPoints(int points)
    {
        _pointSystem.AddPoints(points);
    }

    [Button]
    public void TestAddOnePoint()
    {
        _pointSystem.AddOnePoint();
    }
}
