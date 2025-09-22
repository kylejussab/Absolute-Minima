using UnityEngine;

public class AllDialogue : MonoBehaviour
{
    public DialogueManager dialogueManager;

    void Start()
    {
        Message[] messages = new Message[3];
        messages[0] = new Message { actor = ActorType.Linea, message = "What is this place?" };
        messages[1] = new Message { actor = ActorType.Animator, message = "This is my domain, Linea." };
        messages[2] = new Message { actor = ActorType.Linea, message = "I’m not afraid of you." };

        dialogueManager.OpenDialogue(messages);
    }
}
