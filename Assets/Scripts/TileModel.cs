using UnityEngine;


[CreateAssetMenu(fileName = "TileModel", menuName = "graph wfc test/TileModel", order = 0)]
public class TileModel : ScriptableObject {

    [System.Serializable]
    public struct MaterialDirections {
        public Vector3 dir;
        public MaterialCon mat;
    }
    public GameObject model;
    public MaterialDirections matDir;
    public int priority = 1;
    public bool zRotationsOnly = true;
    public bool noWarping = false;
}