using System;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    public event EventHandler OnSelectedBuildingTypeChanged;

    private BuildingTypeSO buildingTypeSO;

    private void Awake() {
        Instance = this;
    }

    public BuildingTypeSO GetSelectedBuildingTypeSO() {
        return buildingTypeSO;
    }
    public void SetActiveBuildingTypeSO(BuildingTypeSO buildingTypeSO) {
        this.buildingTypeSO = buildingTypeSO;
        OnSelectedBuildingTypeChanged?.Invoke(this, EventArgs.Empty);
    }
}
