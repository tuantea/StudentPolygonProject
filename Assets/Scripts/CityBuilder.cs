using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CityBuilder : MonoBehaviour {
    public static CityBuilder Instance { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public event EventHandler<OnBuildingPlacedEventArgs> OnBuildingPlaced;

    public class OnBuildingPlacedEventArgs : EventArgs {
        public GridObject gridObject;
    }
    public event EventHandler OnMoneyAmountChanged;
    public event EventHandler OnPeopleAmountChanged;
    public event EventHandler OnHappinessChanged;

    private GridXZ<GridObject> grid;
    private GridObject startClickGridObject;
    private int moneyAmout;
    private int peopleAmount;
    private float happiness;

    private void Awake() {
        Instance = this;
        grid = new GridXZ<GridObject>(40, 20, 10f, Vector3.zero, (GridXZ<GridObject> grid, int x, int y) => new GridObject(grid, x, y));
    }
    void Start() {
        OfficeBuilding.OnAnyGeneratedMoney += OfficeBuilding_OnAnyGeneratedMoney;
        ResidentialBuilding.OnAnyPlaced += ResidentialBuilding_OnAnyPlaced;
        happiness = .5f;
    }

    private void ResidentialBuilding_OnAnyPlaced(object sender, EventArgs e) {
        peopleAmount += UnityEngine.Random.Range(5, 26);
        OnPeopleAmountChanged?.Invoke(this, EventArgs.Empty);
        happiness = UnityEngine.Random.Range(.3f, .8f);
        OnHappinessChanged?.Invoke(this, EventArgs.Empty);
    }

    private void OfficeBuilding_OnAnyGeneratedMoney(object sender, OfficeBuilding.OnGeneratedMoneyEventArgs e) {
        Transform transform = (sender as OfficeBuilding).transform;
        Popup.Create(transform.position + new Vector3(10, 27, 10), CityBuilderGameAssets.Instance.dollarSprite, e.amount.ToString());
        moneyAmout += e.amount;
        OnMoneyAmountChanged?.Invoke(this, EventArgs.Empty);
    }
    public void GetStats(out int moneyAmount, out int peopleAmount, out float happiness) {
        moneyAmount = this.moneyAmout;
        peopleAmount = this.peopleAmount;
        happiness = this.happiness;
    }

    // Update is called once per frame
    void Update() {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        if (Input.GetMouseButton(0)) {
            BuildingTypeSO buildingTypeSO = BuildingManager.Instance.GetSelectedBuildingTypeSO();
            if (buildingTypeSO == null) {
                return;
            }
            Debug.Log(Mouse3D.GetMouseWorldPosition());
            if (grid.GetGridObject(Mouse3D.GetMouseWorldPosition()).HasBuilding()) {
                return;
            }
            grid.GetGridObject(Mouse3D.GetMouseWorldPosition()).SpawnBuilding(buildingTypeSO);
            OnBuildingPlaced?.Invoke(this, new OnBuildingPlacedEventArgs() {
                gridObject = grid.GetGridObject(Mouse3D.GetMouseWorldPosition()),
            });
        }
    }
    public GridXZ<GridObject> GetGrid() {
        return grid;
    }
    public class GridObject {

        private GridXZ<GridObject> grid;
        private int x;
        private int z;
        private BuildingTypeSO buildingTypeSO;
        private Transform spawnedTransform;

        public GridObject(GridXZ<GridObject> grid, int x, int z) {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }
        public void GetXZ(out int x, out int z) {
            x = this.x;
            z = this.z;
        }
        public Vector2Int GetXZVector2Int() {
            return new Vector2Int(x, z);
        }
        public BuildingTypeSO GetBuildingTypeSO() {
            return buildingTypeSO;
        }
        public bool HasBuilding() {
            return buildingTypeSO != null;
        }
        public bool HasRoadBuilding() {
            return HasBuilding() && 
                (buildingTypeSO == CityBuilderGameAssets.Instance.buildingTypeListSO.road||
                 buildingTypeSO ==CityBuilderGameAssets.Instance.buildingTypeListSO.roadCrossing);
        }
        public void SpawnBuilding(BuildingTypeSO buildingTypeSO) {
            this.buildingTypeSO = buildingTypeSO;
            spawnedTransform = Instantiate(buildingTypeSO.prefab, grid.GetWorldPosition(x, z), Quaternion.identity);
            if (spawnedTransform.TryGetComponent(out IPlaceBuilding placeBuilding)) {
                placeBuilding.Setup(grid, this);
            }
        }
        public void RotateVisual(float eulerY) {
            spawnedTransform.GetChild(0).eulerAngles = new Vector3(0, eulerY, 0);
        }
        public void SetVisual(Transform visualPrefab) {
            Destroy(spawnedTransform.GetChild(0).gameObject);
            Instantiate(visualPrefab, spawnedTransform);
        }
    }
}
