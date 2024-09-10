using UnityEngine;

public interface IPlaceBuilding 
{
    public void Setup(GridXZ<CityBuilder.GridObject> grid,CityBuilder.GridObject gridObject);
}
