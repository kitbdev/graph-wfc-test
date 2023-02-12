using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kutil;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class TileGraph {
    public TileNode rootNode;
}

/// <summary>
/// This node is generated based on VolNodes.
/// tiles are instantiated on these
/// </summary>
[System.Serializable]
public class TileNode {
    [SerializeReference]
    public VolNode node1;
    [SerializeReference]
    public VolNode node2;
    public Vector3 pos;
    [SerializeReference]
    public List<TileNode> connectedNodes;// = new List<Node>(8);

    public MaterialDirections connectivityMap;

    [SerializeReference]
    public List<TileModel> allPossibleTileModels;
    public TileModel choosenTileModel;
    public GameObject activeGameObject;

    public TerrainGen terrain;

    public TileNode(VolNode node1, VolNode node2) {
        this.node1 = node1;
        this.node2 = node2;
        UpdatePos();
        ResetTile();
    }
    public void Remove() {
        ClearGO();
    }
    public void ClearGO() {
        if (activeGameObject == null) return;
        activeGameObject.DestroySafe();
    }
    public void ResetTile() {
        ClearGO();
        choosenTileModel = null;
        if (terrain != null) allPossibleTileModels = terrain.AllTiles.ToList();
    }
    public void AddConnections(IEnumerable<TileNode> nodes) {
        connectedNodes.AddRange(nodes.Distinct().Except(connectedNodes.Append(this)));
    }
    public void ClearConnections() {
        connectedNodes.Clear();
    }

    public void UpdatePos() {
        pos = (node1.pos + node2.pos) / 2f;
    }
}