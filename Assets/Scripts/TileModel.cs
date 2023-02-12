using UnityEngine;


[CreateAssetMenu(fileName = "TileModel", menuName = "graph wfc test/TileModel", order = 0)]
public class TileModel : ScriptableObject {

    public GameObject model;
    public MaterialDirections matDir;
    public int priority = 1;
    public bool zRotationsOnly = true;
    public bool noWarping = false;
}

[System.Serializable]
public struct MaterialDirections {
    [System.Serializable]
    public struct MatDir {
        public Angle3D dir;
        public MaterialCon mat;
    }
    public MatDir[] dirs;

    public MatDir FindNearestMatch(MatDir dir) {
        return default;
    }
}