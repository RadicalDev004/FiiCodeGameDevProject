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
        if (tile.isEnd)
            gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = new Color32(0, 75, 255, 175);

        else if (tile.isChest)
            gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = new Color32(255, 200, 0, 175);

        else if (tile.isRock)
            gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = new Color32(0, 0, 0, 175);

        else if (tile.isImmutable)
            gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = new Color32(255, 0, 0, 175);
    }
}
