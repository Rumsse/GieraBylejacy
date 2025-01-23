using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;


public class TriggerTile : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase triggerTile;
    public TileBase healthTile;
    public TileBase trigger2Tile;
    public GameObject player;
    public GameObject batMate;
    public Vector3Int playerTilePosition;
    public Vector3Int batMateTilePosition;

    public CharactersData characterDataPlayer;
    public CharactersData characterDataBatmate;

    private HashSet<Vector3Int> TriggeredTiles = new HashSet<Vector3Int>();
    private HashSet<Vector3Int> HealingTiles = new HashSet<Vector3Int>();



    void Update()
    {
        CheckTrigger();
        CheckTrigger2();
        CheckHealthTilePlayer();
        CheckHealthTileBatmate();
    }

    void CheckTrigger()
    {
        if (IsPlayerOnTriggerTile(player) && IsPlayerOnTriggerTile(batMate))
        {
            OpenGate();
        }

    }

    private bool IsPlayerOnTriggerTile(GameObject player)
    {
        Vector3Int tilePosition = tilemap.WorldToCell(player.transform.position);
        return tilemap.GetTile(tilePosition) == triggerTile;
    }


    void OpenGate()
    {
        //Animator
        //unlockedGateAnimation.SetTrigger("Unlocked");
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
    }

    private bool IsPlayerOnHealthTile(GameObject player)
    {
        Vector3Int tilePosition = tilemap.WorldToCell(player.transform.position);
        return tilemap.GetTile(tilePosition) == healthTile;
    }

    void CheckHealthTilePlayer()
    {
        if (IsPlayerOnHealthTile(player))
        {
            characterDataPlayer.Heal(100);
        }
    }

    void CheckHealthTileBatmate()
    {
        if (IsPlayerOnHealthTile(batMate))
        {
            characterDataBatmate.Heal(40);
        }
    }

    void CheckTrigger2()
    {
        if (IsPlayerOnTrigger2Tile(player) && IsPlayerOnTrigger2Tile(batMate))
        {
            NextScene();
        }

    }

    private bool IsPlayerOnTrigger2Tile(GameObject player)
    {
        Vector3Int tilePosition = tilemap.WorldToCell(player.transform.position);
        return tilemap.GetTile(tilePosition) == trigger2Tile;
    }

    void NextScene()
    {
        SceneManager.LoadScene(6);
    }



}
