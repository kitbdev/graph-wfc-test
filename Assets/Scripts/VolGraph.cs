using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class VolGraph : MonoBehaviour {
    public Node rootNode;



    public void ForEachNode(System.Action<Node> action) {
        List<Node> searched = new();
        Queue<Node> frontier = new();
        frontier.Enqueue(rootNode);
        while (frontier.Count > 0) {
            Node current = frontier.Dequeue();
            action?.Invoke(current);
            searched.Add(current);
            if (current.connectedNodes==null) continue;
            foreach (var nei in current.connectedNodes) {
                if (!searched.Contains(nei) && !frontier.Contains(nei)) {
                    frontier.Enqueue(nei);
                }
            }
        }
    }

    // show connections
    private void OnDrawGizmos() {
#if UNITY_EDITOR
        // bool selected = Selection.activeGameObject == gameObject;
        if (rootNode.connectedNodes == null) return;
        using (new Handles.DrawingScope()) {
            ForEachNode(node => {
                // bool selected = node == rootNode;
                float distn = Vector3.Distance(rootNode.pos, node.pos) / 5;
                Color lineCol = Color.Lerp(Color.green, Color.black, distn);
                Handles.color = lineCol;
                foreach (var con in node.connectedNodes) {
                    Handles.DrawLine(node.pos, con.pos);
                }
            });
        }
#endif
    }
}

[System.Serializable]
public class Node {
    public Vector3 pos;
    public float radius;
    public MaterialCon mat;
    [SerializeReference]
    public List<Node> connectedNodes;// = new List<Node>(8);


    // private Node[] _connectedNodes;
    // public Node[] connectedNodes => _connectedNodes;

    // public void ClearConnections() {
    //     _connectedNodes = new Node[0];
    // }
    // public void AddConnections(Node[] newConnections) {
    //     var oldCN = _connectedNodes;
    //     _connectedNodes = new Node[_connectedNodes.Length + newConnections.Length];
    //     for (int i = 0; i < _connectedNodes.Length; i++) {
    //         _connectedNodes[i] = oldCN[i];
    //     }
    //     for (int i = 0; i < newConnections.Length; i++) {
    //         _connectedNodes[_connectedNodes.Length + i] = newConnections[i];
    //     }
    // }
    // public void RemoveConnections(Node[] existingConnections) {
    //     var oldCN = _connectedNodes;
    // }
}