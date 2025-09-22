using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum ActorType
{
    Linea,
    Animator
}

[System.Serializable]
public class Message
{
    public ActorType actor;
    public string message;
}

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI actorName;
    public TextMeshProUGUI messageText;

    [Header("Portraits")]
    public Image lineaPortrait;
    public Image animatorPortrait;

    [Header("Dialogue Settings")]
    public float dialogueSpeed = 0.02f;

    private Message[] currentMessages;
    private int activeMessage = 0;

    private bool isWritingMessage = false;

    void Start()
    {
        dialoguePanel.SetActive(true);
    }

    void Update()
    {
        if (!LevelSession.inConversation) return;

        if (isWritingMessage && Input.GetKeyDown(KeyCode.Space))
        {
            // Skip typewriter effect
            dialogueSpeed = 0f;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isWritingMessage)
        {
            dialogueSpeed = 0.02f;
            NextMessage();
        }
    }

    public void OpenDialogue(Message[] messages)
    {
        currentMessages = messages;
        activeMessage = 0;
        LevelSession.inConversation = true;
        dialoguePanel.SetActive(true);

        DisplayMessage();
    }

    private void DisplayMessage()
    {
        Message messageToDisplay = currentMessages[activeMessage];

        messageText.text = "";
        StartCoroutine(WriteSentence(messageToDisplay.message));

        if (messageToDisplay.actor == ActorType.Linea)
        {
            actorName.text = "Linea";
            lineaPortrait.gameObject.SetActive(true);
            animatorPortrait.gameObject.SetActive(false);
        }
        else
        {
            actorName.text = "The Animator";
            lineaPortrait.gameObject.SetActive(false);
            animatorPortrait.gameObject.SetActive(true);
        }
    }

    private void NextMessage()
    {
        activeMessage++;
        if (activeMessage < currentMessages.Length)
        {
            DisplayMessage();
        }
        else
        {
            LevelSession.inConversation = false;
            dialoguePanel.SetActive(false);
        }
    }

    private IEnumerator WriteSentence(string message)
    {
        isWritingMessage = true;

        foreach (char c in message.ToCharArray())
        {
            messageText.text += c;
            yield return new WaitForSeconds(dialogueSpeed);
        }

        isWritingMessage = false;
    }
}