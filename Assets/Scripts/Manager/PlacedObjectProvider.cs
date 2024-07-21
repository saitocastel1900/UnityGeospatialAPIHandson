using UnityEngine;

public class PlacedObjectProvider : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private PlacedObjectDataBase _placedObjectDataBase;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private string _id;

    /// <summary>
    /// 
    /// </summary>
    public PlacedObjectData PlacedObject => _placedObjectDataBase.GetPlacedObjectData(_id);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public GameObject CreatePlacedObject()
    {
        return Instantiate(_placedObjectDataBase.GetPlacedObjectData(_id).PlacedObject);
    }
}