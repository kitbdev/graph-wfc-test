using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kutil;

[SelectionBase]
public class WaveFunctionCollapse : MonoBehaviour {

    public VolGraph graph;
    public TileModel[] tiles;

    [AddButton(nameof(FindTiles), "Find Tiles", null, AddButtonAttribute.ButtonLayout.REPLACE)]
    int dummytiles;
    [AddButton(nameof(StartGen), "Start Gen", null, AddButtonAttribute.ButtonLayout.REPLACE)]
    int dummygen;

    private void Start() {
    }
    void FindTiles(){
        tiles = GetAllSOEditor.GetAllInstances<TileModel>();
    }
    void StartGen(){

    }

}