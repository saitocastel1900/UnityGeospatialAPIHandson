using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlacedObjectDataBase", menuName = "ScriptableObject/PlacedObjectDataBase")]
public class PlacedObjectDataBase : ScriptableObject
{
    /// <summary>
    /// 設置するオブジェクトデータの配列
    /// </summary>
    [SerializeField] List<PlacedObjectData> _placedObjectData = new List<PlacedObjectData>();
    
    /// <summary>
    /// 設置するオブジェクトを取得
    /// </summary>
    public PlacedObjectData GetPlacedObjectData(string id)
    {
        var placedObjectData = _placedObjectData.FirstOrDefault(placedObject=>placedObject.Id == id);
        return placedObjectData;
    }
}