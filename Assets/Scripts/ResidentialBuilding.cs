using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ResidentialBuilding : MonoBehaviour,IPlaceBuilding
{
    public static event EventHandler OnAnyPlaced;

    [SerializeField] private Transform visualTransform;
    [SerializeField] private Material[] materialArray;
    [SerializeField] private Transform[] childTransformArray;
    
    private GridXZ<CityBuilder.GridObject> grid;
    private CityBuilder.GridObject gridObject;

    public void Setup(GridXZ<CityBuilder.GridObject> grid, CityBuilder.GridObject gridObject) {
        this.grid = grid;
        this.gridObject = gridObject;
        Material selectedMaterial = materialArray[UnityEngine.Random.Range(0, materialArray.Length)];
        MeshRenderer[] meshRendererArray = visualTransform.GetComponentsInChildren<MeshRenderer>(true);
        foreach (MeshRenderer meshRenderer in meshRendererArray) {
            meshRenderer.material = selectedMaterial;
        }
        foreach (Transform childTransform in childTransformArray) {
            childTransform.gameObject.SetActive(false);
        }
        childTransformArray[UnityEngine.Random.Range(0, childTransformArray.Length)].gameObject.SetActive(true);
        bool hasRoadRight = false;
        bool hasRoadLeft = false;
        bool hasRoadUp = false;
        bool hasRoadDown = false;
        Vector2Int gridPosition = gridObject.GetXZVector2Int();

        hasRoadRight = IsHasRoadVector(grid,gridPosition,new Vector2Int(+1,+0));
        hasRoadLeft = IsHasRoadVector(grid, gridPosition, new Vector2Int(-1, 0));
        hasRoadUp = IsHasRoadVector(grid, gridPosition, new Vector2Int(+0,+1));
        hasRoadDown = IsHasRoadVector(grid, gridPosition, new Vector2Int(+0,-1));

        if (hasRoadRight) { visualTransform.eulerAngles = new Vector3(0, +90, 0); }
        if (hasRoadLeft) { visualTransform.eulerAngles = new Vector3(0,-90,0); }
        if (hasRoadUp) { visualTransform.eulerAngles = new Vector3(0,180,0); }
        if (hasRoadDown) { visualTransform.eulerAngles = new Vector3(0, 0, 0); }

        OnAnyPlaced?.Invoke(this,EventArgs.Empty);
    }

    private bool IsHasRoadVector(GridXZ<CityBuilder.GridObject> grid,Vector2Int gridPosition ,Vector2Int vector) {
        return grid.IsValidGridPosition(gridPosition + vector) && grid.GetGridObject(gridPosition.x + vector.x, gridPosition.y + vector.y).HasRoadBuilding();
    }
}
