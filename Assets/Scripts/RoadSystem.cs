using System.Collections.Generic;
using UnityEngine;

public class RoadSystem : MonoBehaviour
{
    public static RoadSystem Instance { get; private set; }

    private List<CityBuilder.GridObject> roadGridObjectList;

    [SerializeField] private Transform roadDashVisual;
    [SerializeField] private Transform roadBareVisual;

    private void Awake() {
        Instance = this;
        roadGridObjectList = new List<CityBuilder.GridObject>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CityBuilder.Instance.OnBuildingPlaced += CityBuilder_OnBuildingPlaced;
    }

    private void CityBuilder_OnBuildingPlaced(object sender, CityBuilder.OnBuildingPlacedEventArgs e) {
        if (e.gridObject.GetBuildingTypeSO() == CityBuilderGameAssets.Instance.buildingTypeListSO.road ||
            e.gridObject.GetBuildingTypeSO() == CityBuilderGameAssets.Instance.buildingTypeListSO.roadCrossing) {
            roadGridObjectList.Add(e.gridObject);
            RefreshRoads();
        }
    }

    private void RefreshRoads() {
        GridXZ<CityBuilder.GridObject> grid = CityBuilder.Instance.GetGrid();
        foreach (CityBuilder.GridObject roadGridObject in roadGridObjectList) {
            bool hasRoadRight = false;
            bool hasRoadLeft = false;
            bool hasRoadUp = false;
            bool hasRoadDown = false;
            Vector2Int gridPosition = roadGridObject.GetXZVector2Int();
            hasRoadRight = HasRoad.Instance.IsHasRoadVector(grid, gridPosition, new Vector2Int(+1, +0));
            hasRoadLeft = HasRoad.Instance.IsHasRoadVector(grid, gridPosition, new Vector2Int(-1, 0));
            hasRoadUp = HasRoad.Instance.IsHasRoadVector(grid, gridPosition, new Vector2Int(+0, +1));
            hasRoadDown = HasRoad.Instance.IsHasRoadVector(grid, gridPosition, new Vector2Int(+0, -1));

            if ((hasRoadRight || hasRoadLeft) && !(hasRoadUp || hasRoadDown)) {
                roadGridObject.RotateVisual(90);
            }
            if ((hasRoadUp || hasRoadDown) && !(hasRoadRight || hasRoadLeft)) {
                roadGridObject.RotateVisual(0);
            }
            if ((hasRoadRight || hasRoadLeft) && (hasRoadUp || hasRoadDown)) {
                roadGridObject.SetVisual(roadBareVisual);
            }
        }
    }
    public List<CityBuilder.GridObject> GetNeighbourRoadList(Vector2Int gridPosition) {
        GridXZ<CityBuilder.GridObject> grid = CityBuilder.Instance.GetGrid();
        bool hasRoadRight = false;
        bool hasRoadLeft = false;
        bool hasRoadUp = false;
        bool hasRoadDown = false;
        hasRoadRight = HasRoad.Instance.IsHasRoadVector(grid, gridPosition, new Vector2Int(+1, +0));
        hasRoadLeft = HasRoad.Instance.IsHasRoadVector(grid, gridPosition, new Vector2Int(-1, 0));
        hasRoadUp = HasRoad.Instance.IsHasRoadVector(grid, gridPosition, new Vector2Int(+0, +1));
        hasRoadDown = HasRoad.Instance.IsHasRoadVector(grid, gridPosition, new Vector2Int(+0, -1));

        List<CityBuilder.GridObject> neighbourRoadList = new List<CityBuilder.GridObject>();
        if (hasRoadRight) {
            neighbourRoadList.Add(grid.GetGridObject(gridPosition + new Vector2Int(+1, +0)));
        }
        if (hasRoadLeft) {
            neighbourRoadList.Add(grid.GetGridObject(gridPosition + new Vector2Int(-1, +0)));
        }
        if (hasRoadUp) {
            neighbourRoadList.Add(grid.GetGridObject(gridPosition + new Vector2Int(+0, +1)));
        }
        if (hasRoadDown) {
            neighbourRoadList.Add(grid.GetGridObject(gridPosition + new Vector2Int(+0, -1)));
        }
        return neighbourRoadList;
    }

    public int GetRoadCount() {
        return roadGridObjectList.Count;
    }
}
