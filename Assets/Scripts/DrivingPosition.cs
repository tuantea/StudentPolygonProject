using System.Collections.Generic;
using UnityEngine;

public class DrivingPosition : MonoBehaviour
{

    [SerializeField] private List<DrivingPosition> connectedDrivingPositionList;

    private bool isStartingPoint;

    private void Awake() {
        transform.name = "DR_" + Random.Range(10000, 99999);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        
    }

    public void SetIsStartingPoint(bool isStartingPoint) {
        this.isStartingPoint = isStartingPoint;
    }
    public bool IsStartingPoint() => isStartingPoint;

    public List<DrivingPosition> GetConnectedDrivingPositionList()=> connectedDrivingPositionList;

    public override string ToString() {
        return transform.name;
    }
}
