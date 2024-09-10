using System.Collections.Generic;
using UnityEngine;

public class SidewalkPosition : MonoBehaviour
{
    [SerializeField] private List<SidewalkPosition> connectedSidewalkPositionList;
    [SerializeField] private bool isStartPoint;

    private List<SidewalkPosition> dynamicConnectedSidewalkPositionList = new List<SidewalkPosition>();

    private void Awake() {
        transform.name = "SW_" + Random.Range(10000, 99999);
    }
    void Start()
    {
        WalkingNPCManager.Instance.RegisterSidewalkPosition(this);
    }

    public void ClearDynamicList() {
        dynamicConnectedSidewalkPositionList = new List<SidewalkPosition>();
    }

    public void AddToDynamicList(SidewalkPosition sidewalkPosition) {
        dynamicConnectedSidewalkPositionList.Add(sidewalkPosition);
    }

    public List<SidewalkPosition> GetCompleteConnectedSidewalkPositionListCopy() {
        List<SidewalkPosition> completeList = new List<SidewalkPosition>(connectedSidewalkPositionList);
        completeList.AddRange(dynamicConnectedSidewalkPositionList);
        return completeList;
    }
    public bool IsStartPoint() => isStartPoint;

    public override string ToString() {
        return transform.name;
    }
}
