using UnityEngine;
using TMPro;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
public class PaceNotes : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMP_Text PaceNote;
    private Sprite[] sprites;
    public UnityEngine.UI.Image uiImage;
    void Start()
    {
        sprites = Resources.LoadAll<Sprite>("Pacenotes");
    }
    private void OnTriggerEnter(Collider other)
    {
        //Check if the collider of the other GameObject involved in the collision is tagged "Enemy"
        switch (other.tag)
        {
            case "Hairpin Right":
                Debug.Log("Hairpin Right");
                PaceNote.text = "Hairpin Right";
                uiImage.sprite = sprites[7];
                break;
            case "Hairpin Left":
                Debug.Log("Hairpin Left");
                PaceNote.text = "Hairpin Left";
                uiImage.sprite = sprites[15];
                break;
            case "1 Right":
                Debug.Log("1 Right");
                uiImage.sprite = sprites[6];
                PaceNote.text = "1 Right";
                break;
            case "1 Left":
                Debug.Log("1 Left");
                uiImage.sprite = sprites[14];
                PaceNote.text = "1 Left";
                break;
            case "2 Right":
                Debug.Log("2 Right");
                uiImage.sprite = sprites[5];
                PaceNote.text = "2 Right";
                break;
            case "2 Left":
                Debug.Log("2 Left");
                uiImage.sprite = sprites[13];
                PaceNote.text = "2 Left";
                break;
            case "3 Right":
                Debug.Log("3 Right");
                uiImage.sprite = sprites[4];
                PaceNote.text = "3 Right";
                break;
            case "3 Left":
                Debug.Log("3 Left");
                uiImage.sprite = sprites[12];
                PaceNote.text = "3 Left";
                break;
            case "4 Right":
                Debug.Log("4 Right");
                uiImage.sprite = sprites[3];
                PaceNote.text = "4 Right";
                break;
            case "4 Left":
                Debug.Log("4 Left");
                uiImage.sprite = sprites[11];
                PaceNote.text = "4 Left";
                break;
            case "5 Right":
                Debug.Log("5 Right");
                uiImage.sprite = sprites[2];
                PaceNote.text = "6 Right";
                break;
            case "5 Left":
                Debug.Log("5 Left");
                uiImage.sprite = sprites[10];
                PaceNote.text = "5 Left";
                break;
            case "6 Right":
                Debug.Log("6 Right");
                uiImage.sprite = sprites[1];
                break;
            case "6 Left":
                Debug.Log("6 Left");
                uiImage.sprite = sprites[9];
                PaceNote.text = "6 Left";
                break;
            case "Flat Right":
                uiImage.sprite = sprites[0];
                Debug.Log("Flat Right");
                PaceNote.text = "Flat Right";
                break;
            case "Flat Left":
                uiImage.sprite = sprites[8];
                Debug.Log("Flat Left");
                PaceNote.text = "Flat Left";
                break;
            case "Finish":
                Debug.Log("Finish");
                PaceNote.text = "Finish";
                break;
            default:
                Debug.Log("Triggered by something else");
                break;
        }
    }
}
