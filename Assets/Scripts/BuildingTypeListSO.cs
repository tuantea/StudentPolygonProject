using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BuildingTypeListSO : ScriptableObject
{
    public List<BuildingTypeSO> buildingTypeSOList;

    public BuildingTypeSO road;
    public BuildingTypeSO roadCrossing;
    public BuildingTypeSO residential;
    public BuildingTypeSO office;
    public BuildingTypeSO park;

}
