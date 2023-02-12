using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kutil;

[SelectionBase]
public class TerrainGen : MonoBehaviour {

    public VolGraph graph;
    public TileGraph tileGraph;

    private TileModel[] allTiles;
    public TileModel[] AllTiles => allTiles;

    [AddButton(nameof(FindTiles), "Find Tiles", null, AddButtonAttribute.ButtonLayout.REPLACE)]
    int dummytiles;
    [AddButton(nameof(StartGen), "Start Gen", null, AddButtonAttribute.ButtonLayout.REPLACE)]
    int dummygen;

    [ReadOnly]
    bool isGenerating;

    private void Start() {
    }
    void FindTiles() {
        allTiles = GetAllSOEditor.GetAllInstances<TileModel>();
    }
    void ClearGen() {

    }
    void StartGen() {
        if (isGenerating) {
            Debug.LogWarning("Already generating!");
            return;
        }
        ClearGen();
        isGenerating = true;
        // create the tilegraph from the volume graph


        isGenerating = false;
    }

    void Observe() {
        // Find a wave element with the minimal nonzero entropy. 
        // If there is no such elements (if all elements have zero or undefined entropy) then break
        // int minEntropy = allTiles.Length + 1;
        // List<VolNode> minimumNodes = new();
        // graph.ForEachNode(node => {
        //     if (node.allPossibleTileModels.Count < minEntropy) {
        //         minimumNodes.Clear();
        //         minEntropy = node.allPossibleTileModels.Count;
        //     }
        //     if (node.allPossibleTileModels.Count == minEntropy) {
        //         minimumNodes.Add(node);
        //     }
        // });
        // if (minEntropy==0){

        // }


        //Collapse this element into a definite state according to its coefficients and the distribution of NxN patterns in the input.
    }

}