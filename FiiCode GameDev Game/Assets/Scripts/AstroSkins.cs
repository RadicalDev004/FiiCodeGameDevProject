using UnityEngine;

public class AstroSkins : MonoBehaviour
{
    public Material[] Skin0, Skin1, Skin2, Skin3;
    public SkinnedMeshRenderer Astro;
    public int a;

    private void Update()
    {
        a = PlayerPrefs.GetInt("SelectedSkin");
        switch (a)
        {
            case 0:

                Astro.materials = Skin0;

                break;
            case 1:

                Astro.materials = Skin1;

                break;
            case 2:

                Astro.materials = Skin2;

                break;
            case 3:

                Astro.materials = Skin3;

                break;
        }

    }
}


