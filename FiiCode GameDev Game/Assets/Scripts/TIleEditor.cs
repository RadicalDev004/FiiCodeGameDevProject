using UnityEngine;

[ExecuteInEditMode]
public class TileEditor : MonoBehaviour
{
    //Gives Easy acces to editing tiles in the Editor (for creating levels)


    public bool shouldWork = true;

    private Tile tile;
    void Start()
    {
        tile = transform.GetComponent<Tile>();
    }

    void Update()
    {
        if (!shouldWork) return; 

        if (tile.isEnd)
            gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = Color.green;

        else if (tile.isChest)
            gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = Color.yellow;

        else if (tile.isRock)
            gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = Color.black;

        else if (tile.isImmutable)
            gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = Color.red;
        else
            gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = new Color(1, 1, 1, 0.25f);
    }
}
