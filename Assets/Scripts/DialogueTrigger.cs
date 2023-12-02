using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private bool hasTriggered = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            if (!PlayerPrefs.HasKey("DialogueTriggered_" + gameObject.name))
            {
                TriggerDialogue();
            }
        }
    }

    public void TriggerDialogue()
    {
        if (dialogue != null)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            hasTriggered = true;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
