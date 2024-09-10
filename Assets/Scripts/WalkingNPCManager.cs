using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WalkingNPCManager : MonoBehaviour
{
    public static WalkingNPCManager Instance { get; private set; }
    private List<SidewalkPosition> sidewalkPositionList;
    private bool hasValidStartPosition;
    private float spawnNPCTimer;

    private void Awake() {
        Instance = this;
        sidewalkPositionList = new List<SidewalkPosition>();
    }

    public void RegisterSidewalkPosition(SidewalkPosition sidewalkPosition) {
        sidewalkPositionList.Add(sidewalkPosition);
        if (sidewalkPosition.IsStartPoint()) {
            hasValidStartPosition = true;
        }
        for (int i = 0; i < sidewalkPositionList.Count; i++) {
            if (sidewalkPositionList[i] == null) {
                sidewalkPositionList.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < sidewalkPositionList.Count; i++) {
            sidewalkPositionList[i].ClearDynamicList();
        }
        for (int i = 0; i < sidewalkPositionList.Count; i++) {
            SidewalkPosition sidewalkPositionA = sidewalkPositionList[i];
            for (int j = 0; j < sidewalkPositionList.Count; j++) {
                SidewalkPosition sidewalkPositionB = sidewalkPositionList[j];
                float connectedDistance = 1.2f;
                if (Vector3.Distance(sidewalkPositionA.transform.position, sidewalkPositionB.transform.position) < connectedDistance) {
                    sidewalkPositionA.AddToDynamicList(sidewalkPositionB);
                }
            }
        }
    }

    private bool CanCalculatePath() {
        return hasValidStartPosition&& sidewalkPositionList.Count>10; 
    }

    public List<SidewalkPosition> GetSidewalkPath() {
        List<SidewalkPosition> validStartSidewalkPositionList = new List<SidewalkPosition>();
        foreach (SidewalkPosition sidewalkPosition in sidewalkPositionList) {
            if (sidewalkPosition.IsStartPoint()) {
                validStartSidewalkPositionList.Add(sidewalkPosition);
            }
        }
        SidewalkPosition currentSidewalkPosition = validStartSidewalkPositionList[Random.Range(0, validStartSidewalkPositionList.Count)];
        List<SidewalkPosition> path = new List<SidewalkPosition>(){
            currentSidewalkPosition,
        };
        SidewalkPosition lastSidewalkPosition = null;
        int safety = 0;
        while (currentSidewalkPosition != null && safety < 1000) {
            safety++;
            List<SidewalkPosition> connectedSidewalkPositionList = currentSidewalkPosition.GetCompleteConnectedSidewalkPositionListCopy();
            connectedSidewalkPositionList.Remove(currentSidewalkPosition);
            connectedSidewalkPositionList.Remove(lastSidewalkPosition);
            if (connectedSidewalkPositionList.Count == 0) {
                currentSidewalkPosition = null;
            } else {
                lastSidewalkPosition = currentSidewalkPosition;
                currentSidewalkPosition = connectedSidewalkPositionList[Random.Range(0, connectedSidewalkPositionList.Count)];
                path.Add(currentSidewalkPosition);
            }
        }
        return path;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            spawnNPCTimer = 0f;
        }
        spawnNPCTimer -= Time.deltaTime;
        if (spawnNPCTimer < 0f) {
            float amountNormalized = Mathf.Min(RoadSystem.Instance.GetRoadCount(), 30) / 30f;
            float spawnNPCTimerMax = 3f - 2.95f * amountNormalized;
            spawnNPCTimer = spawnNPCTimerMax;
            if (CanCalculatePath()) {
                List<SidewalkPosition> path = GetSidewalkPath();
                Transform npcTransform =
                    Instantiate(CityBuilderGameAssets.Instance.npcPrefab, path[0].transform.position, path[0].transform.rotation);
                npcTransform.GetComponent<CityNPC>().Setup(path);
            }
        }
    }
}
