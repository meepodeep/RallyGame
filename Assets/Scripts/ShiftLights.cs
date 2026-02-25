using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ShiftLights : MonoBehaviour
{
    public CarControl cc;
    public Image shiftImage;
    public Sprite[] sprites;
    private float pc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprites[0] = Resources.Load<Sprite>("Light1");
        sprites[1] = Resources.Load<Sprite>("Light2");
        sprites[2] = Resources.Load<Sprite>("Light3");
        sprites[3] = Resources.Load<Sprite>("Light4");
        sprites[4] = Resources.Load<Sprite>("Light5");
        sprites[5] = Resources.Load<Sprite>("Light6");
        
    }

    // Update is called once per frame
    void Update()
    {
        pc = cc.engineRpm;
        if (pc<2000f){ shiftImage.sprite = sprites[0];}
        else if (pc<3500f){ shiftImage.sprite = sprites[1];}
        else if (pc<4000f){ shiftImage.sprite = sprites[2];}
        else if (pc<6000f){ shiftImage.sprite = sprites[3];}
        else if (pc<6500f){ shiftImage.sprite = sprites[4];}
        else if (pc<7500f){ shiftImage.sprite = sprites[5];}
    }
}
