using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Experimental.GraphView.GraphView;

public class TriggerTile : MonoBehaviour
{
    public Tilemap triggerTilemap;
    public TileBase triggerTile;
    public GameObject player;
    public GameObject batMate;
    public Vector3Int playerTilePosition;
    public Vector3Int batMateTilePosition;

    private HashSet<Vector3Int> TriggeredTiles = new HashSet<Vector3Int>();



    void Update()
    {
        CheckTrigger();
    }

    void CheckTrigger()
    {
        if (IsPlayerOnTriggerTile(player) && IsPlayerOnTriggerTile(batMate))
        {
            OpenDoor();
        }
    }

    private bool IsPlayerOnTriggerTile(GameObject player)
    {
        Vector3Int tilePosition = triggerTilemap.WorldToCell(player.transform.position);
        return triggerTilemap.GetTile(tilePosition) == triggerTile;
    }


    void OpenDoor()
    {
        //Animator
        //unlockedDoorAnimation.SetTrigger("Unlocked");
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2f); 
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }



}
