using UnityEngine;
using TMPro;
public class PaceNotes : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMP_Text PaceNote;

    private void OnTriggerEnter(Collider other)
    {
        //Check if the collider of the other GameObject involved in the collision is tagged "Enemy"
        switch (other.tag)
        {
            case "Hairpin Right":
                Debug.Log("Hairpin Right");
                PaceNote.text = "Hairpin Right";
                break;
            case "Hairpin Left":
                Debug.Log("Hairpin Left");
                PaceNote.text = "Hairpin Left";
                break;
            case "1 Right":
                Debug.Log("1 Right");
                PaceNote.text = "1 Right";
                break;
            case "1 Left":
                Debug.Log("1 Left");
                PaceNote.text = "1 Left";
                break;
            case "2 Right":
                Debug.Log("2 Right");
                PaceNote.text = "2 Right";
                break;
            case "2 Left":
                Debug.Log("2 Left");
                PaceNote.text = "2 Left";
                break;
            case "3 Right":
                Debug.Log("3 Right");
                PaceNote.text = "3 Right";
                break;
            case "3 Left":
                Debug.Log("3 Left");
                PaceNote.text = "3 Left";
                break;
            case "4 Right":
                Debug.Log("4 Right");
                PaceNote.text = "4 Right";
                break;
            case "4 Left":
                Debug.Log("4 Left");
                PaceNote.text = "4 Left";
                break;
            case "5 Right":
                Debug.Log("5 Right");
                PaceNote.text = "6 Right";
                break;
            case "5 Left":
                Debug.Log("5 Left");
                PaceNote.text = "5 Left";
                break;
            case "6 Right":
                Debug.Log("6 Right");
                break;
            case "6 Left":
                Debug.Log("6 Left");
                PaceNote.text = "6 Left";
                break;
            case "Flat Right":
                Debug.Log("Flat Right");
                PaceNote.text = "Flat Right";
                break;
            case "Flat Left":
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
