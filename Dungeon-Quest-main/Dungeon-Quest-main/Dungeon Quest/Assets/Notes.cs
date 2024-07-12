using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;

public class Notes : MonoBehaviour,IInteractable
{
    public TextMeshProUGUI Note;
    public string note;
    public GameObject NoteCanvas;
    public void Interact()
    {
        FindObjectOfType<RPlayer>().canDo = false;
        FindObjectOfType<AudioManager>().Play("Note");
        Time.timeScale = 0;
        Note.SetText(note);
        Cursor.visible = true;
        NoteCanvas.SetActive(true);
    }
    public void Back()
    { 
        FindObjectOfType<RPlayer>().canDo = true;
        Cursor.visible = false;
        Time .timeScale = 1;
        NoteCanvas.SetActive (false);
    }
}
