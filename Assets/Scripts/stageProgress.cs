using UnityEngine;
using UnityEngine.UI;

public class stageProgress : MonoBehaviour
{
    public Transform finish;
    public Image player;
    public float maxDiffH;
    private float pacenoteCount = 22;
    private float currentNote=0;
    private float iconPos;
    private Vector2 anchoredPos;
    private CarControl cc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player.rectTransform.anchoredPosition = new Vector2(-850,300);
        cc = FindAnyObjectByType<CarControl>();
    }

    // Update is called once per frame
    void Update()
    {
        anchoredPos = player.rectTransform.anchoredPosition;
        iconPos = Mathf.Lerp(-300f,300f, 1-(currentNote/pacenoteCount));
        Debug.Log((anchoredPos.y-300).ToString()+"anchoredpos");
        Debug.Log((iconPos-300).ToString()+"iconPos");
        if (iconPos-300 < anchoredPos.y-300)
        {
            player.rectTransform.anchoredPosition -= new Vector2(0,1*Time.deltaTime*Mathf.Abs(cc.realSpeed));
        }
        
        
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Hairpin Right":
               currentNote +=1;
                break;
            case "Hairpin Left":
                currentNote +=1;
                break;
            case "1 Right":
                currentNote +=1;
                break;
            case "1 Left":
                currentNote +=1;
                break;
            case "2 Right":
                currentNote +=1;
                break;
            case "2 Left":
                currentNote +=1;
                break;
            case "3 Right":
                currentNote +=1;
                break;
            case "3 Left":
                currentNote +=1;
                break;
            case "4 Right":
                currentNote +=1;
                break;
            case "4 Left":
               currentNote +=1;
                break;
            case "5 Right":
                currentNote +=1;
                break;
            case "5 Left":
                currentNote +=1;
                break;
            case "6 Right":
                currentNote +=1;
                break;
            case "6 Left":
                currentNote +=1;
                break;
            case "Flat Right":
                currentNote +=1;
                break;
            case "Flat Left":
                currentNote +=1;
                break;
            case "Finish":
                currentNote +=1;
                break;
            default:
                Debug.Log("Triggered by something else");
                break;
        }
    }
}
