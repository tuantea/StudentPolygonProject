using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class DrivingNPCManager : MonoBehaviour
{
    public static DrivingNPCManager Instance { get; private set; }

    private List<DrivingPosition> drivingPositionList;
    private float spawnCarTimer = 0f;

    private void Awake() {
        Instance = this;
        drivingPositionList = new List<DrivingPosition>();
    }
    public void RegisterDrivingPosition(DrivingPosition drivingPosition) {
        drivingPositionList.Add(drivingPosition);
        for (int i = 0; i < drivingPositionList.Count; i++) {
            if (drivingPositionList[i] == null) {
                drivingPositionList.RemoveAt(i);
                i--;
            }
        }
        foreach (DrivingPosition drivingPosition1 in drivingPositionList) {
            drivingPosition1.SetIsStartingPoint(false);
            CityBuilder.Instance.GetGrid().GetXZ(drivingPosition1.transform.position, out int x, out int z);
            if (RoadSystem.Instance.GetNeighbourRoadList(new Vector2Int(x, z)).Count == 1 &&
                drivingPosition1.GetConnectedDrivingPositionList().Count > 0) {
                Collider[] colliderArray = Physics.OverlapSphere(drivingPosition1.transform.position, 1f);
                List<DrivingPosition> nearbyDrivingPositionList = new List<DrivingPosition>();
                foreach (Collider collider in colliderArray) {
                    if (collider.TryGetComponent(out DrivingPosition nearbyDrivingPosition) &&
                        nearbyDrivingPosition != drivingPosition1) {
                        nearbyDrivingPositionList.Add(nearbyDrivingPosition);
                    }
                }
                if (nearbyDrivingPositionList.Count == 0) {
                    drivingPosition1.SetIsStartingPoint(true);
                }
            }
        }
        CalculatePath();
    }

    private List<DrivingPosition> CalculatePath() {
        List<DrivingPosition> path = new List<DrivingPosition>();

        List<DrivingPosition> validStartingDrivingPositionList = new List<DrivingPosition>();
        foreach (DrivingPosition drivingPosition in drivingPositionList) {
            if (drivingPosition.IsStartingPoint()) {
                validStartingDrivingPositionList.Add(drivingPosition);
            }
        }
        if (validStartingDrivingPositionList.Count > 0) {
            DrivingPosition currentDrivingPositon = validStartingDrivingPositionList[Random.Range(0, validStartingDrivingPositionList.Count)];
            path.Add(currentDrivingPositon);
            path.AddRange(currentDrivingPositon.GetConnectedDrivingPositionList());

            int safety = 0;
            while (true && safety < 100) {
                safety++;
                DrivingPosition lastDrivingPosition = path[path.Count - 1];
                Collider[] colliderArray = Physics.OverlapSphere(lastDrivingPosition.transform.position, 1f);
                List<DrivingPosition> nearbyDrivingPositionList = new List<DrivingPosition>();
                foreach (Collider collider in colliderArray) {
                    if (collider.TryGetComponent(out DrivingPosition nearbyDrivingPosition) &&
                        nearbyDrivingPosition != lastDrivingPosition &&
                        nearbyDrivingPosition.GetConnectedDrivingPositionList().Count > 0) {
                        nearbyDrivingPositionList.Add(nearbyDrivingPosition);
                    }
                }
                if (nearbyDrivingPositionList.Count > 0) {
                    currentDrivingPositon = nearbyDrivingPositionList[Random.Range(0, nearbyDrivingPositionList.Count)];
                    path.Add(currentDrivingPositon);
                    path.AddRange(currentDrivingPositon.GetConnectedDrivingPositionList());
                } else {
                    break;
                }
            }
        }

        return path;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            spawnCarTimer = 0f;
        }
        spawnCarTimer -= Time.deltaTime;
        if (spawnCarTimer < 0f) {
            float amountNormalized = Mathf.Min(RoadSystem.Instance.GetRoadCount(), 30) / 30f;
            float spawnNPCTimerMax = 3f - 2.8f * amountNormalized;
            spawnCarTimer = spawnNPCTimerMax;
            List<DrivingPosition> path = CalculatePath();
            if (path.Count > 0) {
                Transform carTransform = Instantiate(CityBuilderGameAssets.Instance.carPrefab, path[0].transform.position, path[0].transform.rotation);
                carTransform.GetComponent<CityCar>().Setup(path);
            }
        }

    }
}
