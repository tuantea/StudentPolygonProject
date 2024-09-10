using UnityEngine;

public class HasRoad : MonoBehaviour
{
    public static HasRoad Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public bool IsHasRoadVector(GridXZ<CityBuilder.GridObject> grid, Vector2Int gridPosition, Vector2Int vector) {
        return grid.IsValidGridPosition(gridPosition + vector) && grid.GetGridObject(gridPosition.x + vector.x, gridPosition.y + vector.y).HasRoadBuilding();
    }
}
