using UnityEngine;

public class ShopButton : MonoBehaviour
{
    public int Nr;
    public int Cost;
    public GameObject Lock;
    public Shop Shop;

    private void Start()
    {
        if (IsValid()) Lock.SetActive(false);
        Shop = GetComponentInParent<Shop>();
        gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Select);
    }

    private void Update()
    {
        if (IsSelected()) GetComponent<UnityEngine.UI.Image>().sprite = Shop.Selected;
        else GetComponent<UnityEngine.UI.Image>().sprite = Shop.UnSelected;
    }

    private bool IsValid() => FindObjectOfType<Menu>().SCount > Cost;
    private bool IsSelected() => PlayerPrefs.GetInt("SelectedSkin") == Nr;
    private void Select() => PlayerPrefs.SetInt("SelectedSkin", Nr);

}
