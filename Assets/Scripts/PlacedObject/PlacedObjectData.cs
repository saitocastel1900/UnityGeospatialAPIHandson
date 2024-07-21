using UnityEngine;

[System.Serializable]
public class PlacedObjectData 
{
    /// <summary>
    /// Id
    /// </summary>
    public string Id;
    
    /// <summary>
    /// 設置するオブジェクト
    /// </summary>
    public GameObject PlacedObject;
    
    /// <summary>
    /// 設置する緯度
    /// </summary>
    public double Latitude;
    
    /// <summary>
    /// 設置する軽度
    /// </summary>
    public double Longitude;
    
    /// <summary>
    /// 設置する高度
    /// </summary>
    public double Altitude;
    
    /// <summary>
    /// /オブジェクトの方位
    /// </summary>
    public double Heading;
}