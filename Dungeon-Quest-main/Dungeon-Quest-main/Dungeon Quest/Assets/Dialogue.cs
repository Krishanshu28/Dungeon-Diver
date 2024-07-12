using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour, IInteractable
{
    public TextMeshProUGUI dialogue, NameText;
    public GameObject dialogueCanvas,fairyposition;
    public GameObject ShopCanvas;
    public string[] dialogues,parts;
    public string Name;
    public bool alreadyInteracting = false, firsttime = false, lichDied = false; 
    public bool hasShop;
    public int finalDialogues,normalDialogues;
    public float textSpeed;
    private int i=0, j=0;
    public GameObject player;
    

    public void Interact()
    {
        player.GetComponent<RPlayer>().canDo = false;
        if (!alreadyInteracting)
        {
            alreadyInteracting = true;
            gameObject.GetComponent<Signs>().sign.SetActive(false);
            lichDied = player.GetComponent<RPlayer>().lichDied;
            player.GetComponent<RPlayer>().LockMovement();
            j = 0;
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = "";
            }
            NameText.SetText(Name);
            Cursor.visible = true;
            dialogue.text = string.Empty;
            dialogueCanvas.SetActive(true);
            StartTalking();
        }
    }

    public IEnumerator printLine(string str)
    {

        dialogue.text = string.Empty;
        foreach (char c in str.ToCharArray()) 
        {
            dialogue.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        
    }
    public void StartTalking()
    {
        FindObjectOfType<AudioManager>().Play("Talk");
        if (firsttime)
        {
            i = 0;

            foreach (char c in dialogues[i])
            {
                if (c != '#')
                {
                    parts[j] += c;
                }
                else
                {
                    j++;
                    
                }
            }
            j = 0;
            StartCoroutine(printLine(parts[j]));

            firsttime = false;

        }
        else if (!lichDied)
        {
            i = (int)Random.Range(1, finalDialogues);
            foreach (char c in dialogues[i])
            {
                if (c != '#')
                {
                    parts[j] += c;
                }
                else
                {
                    j++;
                }
            }
            j = 0;
            StartCoroutine(printLine(parts[j]));
        }
        else
        {
            i = (int)Random.Range(normalDialogues, dialogues.Length);
            foreach (char c in dialogues[i])
            {
                if (c != '#')
                {
                    parts[j] += c;
                }
                else
                {
                    j++;
                }
            }
            j = 0;
            StartCoroutine(printLine(parts[j]));
        }
    }
    public void NextLine()
    {
        if (parts[j+1] != "")
        {
            StopAllCoroutines();
            j++;
            StartCoroutine(printLine(parts[j]));
        }
        else
        {
            StopAllCoroutines();
            dialogueCanvas.SetActive(false);
            player.GetComponent<RPlayer>().canDo = true; 
            player.GetComponent<RPlayer>().UnLockMovememt();
            alreadyInteracting = false;
            if (hasShop)
            {
                alreadyInteracting = true;
                ShopCanvas.SetActive(true);
                gameObject.GetComponent<Shop>().Activate();

            }
            for(int i =0; i < parts.Length;i++)
            {
                parts[i] = "";
            }
        }
    }
}
