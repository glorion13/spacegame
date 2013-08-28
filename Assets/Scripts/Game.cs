using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

    public GameObject PlayerPrefab;
    public GameObject BuildingPrefab;
    public GameObject UnitPrefab;
    public GameObject EnergyPrefab;

    public GameObject Players;
    public GameObject Map;

    public GameObject ActivePlayer;

	// This is the initialisation of the entire game
	void Start () {
        // TODO: Initialise network
        // TODO: Initialise network players

	    // Initialise the players
        var player1 = (GameObject) Instantiate(PlayerPrefab);
        var player2 = (GameObject) Instantiate(PlayerPrefab);
        player1.transform.parent = Players.transform;
        player2.transform.parent = Players.transform;

        // Set current player (from client's point of view)
        ActivePlayer = player1;
        player2.GetComponentInChildren<Camera>().enabled = false;// disable the camera of the non-owned Player;
        player2.GetComponentInChildren<AudioListener>().enabled = false; // Disables AudioListener of non-owned Player - prevents multiple AudioListeners from being present in scene.
	    //var currentPlayer = (player1.networkView.isMine) ? player1 : player2;

        // Initialise the grid
	    var gridCreator = transform.GetComponent<HexagonGridCreator>();
	    var mapGrid = Map.GetComponent<MapGrid>();
        gridCreator.SetSizes(mapGrid.HexagonSize, GameObject.FindGameObjectWithTag("MapMesh").renderer.bounds.size.x, GameObject.FindGameObjectWithTag("MapMesh").renderer.bounds.size.z);
        gridCreator.InitialiseGrid();

        // Initialise map grid
	    mapGrid.GridWorldPositions = gridCreator.GetWorldPositionsGrid();

        // Initialise player visibility grids
	    player1.GetComponent<Player>().VisibilityGrid.GetComponent<VisibilityGrid>().InitialiseVisibilityGrid(gridCreator.GetWorldPositionsGrid());
        player2.GetComponent<Player>().VisibilityGrid.GetComponent<VisibilityGrid>().InitialiseVisibilityGrid(gridCreator.GetWorldPositionsGrid());

        // Initialise the player starting buildings and units
	    var player1Building = (GameObject) Instantiate(BuildingPrefab);
        player1Building.GetComponent<Building>().X = 0;
        player1Building.GetComponent<Building>().Y = 0;
        var player2Building = (GameObject) Instantiate(BuildingPrefab);
        player2Building.GetComponent<Building>().X = 20;
        player2Building.GetComponent<Building>().Y = 0;
	    player1Building.transform.parent = player1.GetComponent<Player>().Buildings.transform;
        player2Building.transform.parent = player2.GetComponent<Player>().Buildings.transform;
        player1Building.GetComponent<Building>().FinishBuilding();
        player2Building.GetComponent<Building>().FinishBuilding();

        // Set player visibility texture on grid
        Map.GetComponentInChildren<MeshRenderer>().material.SetTexture("_VisibilityTex", player1.GetComponent<Player>().VisibilityGrid.GetComponent<VisibilityGrid>().VisibilityTexture);

        // Initialise energy patches
        var energy1 = (GameObject) Instantiate(EnergyPrefab);
        energy1.transform.parent = this.transform;
	}
}
