using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kutil;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class VolGraph : MonoBehaviour {
    public VolNode rootNode;

    public List<VolNode> allNodes;


    public TileGraph GetTileGraph(){
        TileGraph tileGraph = new TileGraph();
        if (rootNode.tileNodes.Count>0){
            tileGraph.rootNode = rootNode.tileNodes[0];
        }
        return tileGraph;
    }

    [ContextMenu("update")]
    public void UpdateNodes() {
        allNodes = GetAllNodes();
    }

    public List<VolNode> GetAllNodes() {
        List<VolNode> searched = new();
        Queue<VolNode> frontier = new();
        frontier.Enqueue(rootNode);
        while (frontier.Count > 0) {
            VolNode current = frontier.Dequeue();
            searched.Add(current);
            foreach (var nei in current.connectedNodes) {
                if (!searched.Contains(nei) && !frontier.Contains(nei)) {
                    frontier.Enqueue(nei);
                }
            }
        }
        return searched;
    }

    /// <summary>
    /// Breadth first search on all nodes
    /// </summary>
    /// <param name="action"></param>
    public void ForEachNode(System.Action<VolNode> action) {
        List<VolNode> searched = new();
        Queue<VolNode> frontier = new();
        frontier.Enqueue(rootNode);
        while (frontier.Count > 0) {
            VolNode current = frontier.Dequeue();
            action?.Invoke(current);
            searched.Add(current);
            if (current == null) continue;
            foreach (var nei in current.connectedNodes) {
                if (!searched.Contains(nei) && !frontier.Contains(nei)) {
                    frontier.Enqueue(nei);
                }
            }
        }
    }
    public void ForEachNodeDFS(System.Action<VolNode> action) {
        void ForEachDFS(System.Action<VolNode> action, VolNode node) {
            action?.Invoke(node);
            foreach (var nei in node.connectedNodes) {

            }
        }
        ForEachDFS(action, rootNode);
    }

    // show connections
    private void OnDrawGizmos() {
        if (allNodes == null) return;
#if UNITY_EDITOR
        bool selected = Selection.activeGameObject == gameObject;
        if (rootNode == null) return;
        using (new Handles.DrawingScope()) {
            foreach (var node in allNodes) {
                // }
                // ForEachNode(node => {
                // bool selected = node == rootNode;
                float distn = Vector3.Distance(rootNode.pos, node.pos) / 5;
                Color lineCol = Color.Lerp(Color.green, Color.black, distn);
                Handles.color = lineCol;
                foreach (var con in node.connectedNodes) {
                    Handles.DrawLine(node.pos, con.pos);
                }
            };
        }
#endif
    }
}

/// <summary>
/// This node is created manually and represents a solid material
/// </summary>
[System.Serializable]
public class VolNode {
    public Vector3 pos;
    public float radius;
    public MaterialCon mat;
    [SerializeReference]
    // [SerializeField]
    List<VolNode> _connectedNodes;// = new List<Node>(8);
    [ReadOnly]
    [SerializeReference]
    // [SerializeField]
    List<TileNode> _tileNodes;


    public List<TileNode> tileNodes => _tileNodes;
    public List<VolNode> connectedNodes => _connectedNodes;

    public VolNode() {
        // }
        // public void Init() {
        _connectedNodes = new List<VolNode>(10);
        _tileNodes = new List<TileNode>(10);
    }

    public bool IsConnectedTo(VolNode node) {
        return _connectedNodes.Contains(node);
    }
    public void ConnectTo(VolNode node) {
        _connectedNodes.Add(node);
        // todo instead, reference the tilegraph and tell it to fix stuff?
        TileNode tileNode = new TileNode(this, node);
        tileNode.AddConnections(tileNodes);
        tileNode.AddConnections(node.tileNodes);
        foreach (var ntn in node.tileNodes.Where(tn => tn.node1 == this || tn.node2 == this)) {
            ntn.AddConnections(tileNode.InNewArray());
        }
        _tileNodes.Add(tileNode);
        // UpdateTileNodeConnectivity();// todo fix
    }
    public void DisconnectFrom(VolNode node) {
        _connectedNodes.Remove(node);
        for (int i = 0; i < _tileNodes.Count; i++) {
            TileNode tileNode = _tileNodes[i];
            if ((tileNode.node1 == this && tileNode.node2 == node) ||
                (tileNode.node2 == this && tileNode.node1 == node)) {
                tileNode.Remove();
                _tileNodes.RemoveAt(i);
                break;
            }
        }
        UpdateTileNodeConnectivity();
    }
    public void UpdateTileNodeConnectivity(bool andNeighbors = true) {
        foreach (TileNode tileNode in _tileNodes) {
            if (tileNode == null) {
                Debug.LogWarning(this + " null tilenode " + tileNodes.ToStringFull());
                continue;
            }
            if (andNeighbors) {
                tileNode.ClearConnections();
            }
            tileNode.AddConnections(_tileNodes);
            tileNode.UpdatePos();
        }
        if (andNeighbors) {
            foreach (var vNode in _connectedNodes) {
                vNode.UpdateTileNodeConnectivity(false);
            }
        }
    }

    public override string ToString() {
        return $"TileNode pos {pos} mat {mat} r{radius} con{_connectedNodes.Count}";
    }

}
