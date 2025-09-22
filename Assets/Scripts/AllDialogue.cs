using UnityEngine;

public class AllDialogue : MonoBehaviour
{
    public DialogueManager dialogueManager;

    void Start()
    {
        if(LevelSession.CurrentLevel.LevelNumber.StartsWith("1"))
        {
            if (Random.value < 0.5f)
            {
                Message[] messages = new Message[7];
                messages[0] = new Message { actor = ActorType.Animator, message = "Ah, this brave little sketch. It defies me already." };
                messages[1] = new Message { actor = ActorType.Animator, message = "Can’t say I’m surprised." };
                messages[2] = new Message { actor = ActorType.Linea, message = "If you know what’s to come..." };
                messages[3] = new Message { actor = ActorType.Animator, message = "Then why am I here? Why do I remind you of your incapabilities? Your worthlessness?" };
                messages[4] = new Message { actor = ActorType.Animator, message = "For fun." };
                messages[5] = new Message { actor = ActorType.Linea, message = "..." };
                messages[6] = new Message { actor = ActorType.Animator, message = "You play my game child. These are my rules. Escape is not possible." };

                dialogueManager.OpenDialogue(messages);
            }
            else
            {
                Message[] messages = new Message[7];
                messages[0] = new Message { actor = ActorType.Animator, message = "Another page turned, another doomed attempt." };
                messages[1] = new Message { actor = ActorType.Linea, message = "...Maybe this time will be different." };
                messages[2] = new Message { actor = ActorType.Animator, message = "Different? Child, every line, every corner; mine. You stumble because I make you stumble." };
                messages[3] = new Message { actor = ActorType.Linea, message = "...Then why keep talking to me?" };
                messages[4] = new Message { actor = ActorType.Animator, message = "Because it amuses me to remind you of your futility. You are ink, nothing more." };
                messages[5] = new Message { actor = ActorType.Linea, message = "..." };
                messages[6] = new Message { actor = ActorType.Animator, message = "Struggle if you wish, but escape is not for you." };

                dialogueManager.OpenDialogue(messages);
            }
            
        }

        if (LevelSession.CurrentLevel.LevelNumber.StartsWith("2"))
        {
            if (Random.value < 0.5f)
            {
                Message[] messages = new Message[11];
                messages[0] = new Message { actor = ActorType.Animator, message = "Ah… back on a page. I wondered if you’d return so quickly." };
                messages[1] = new Message { actor = ActorType.Linea, message = "...I… I thought I saw something..." };
                messages[2] = new Message { actor = ActorType.Animator, message = "Oh? And did it feel… promising?" };
                messages[3] = new Message { actor = ActorType.Animator, message = "Does it feel… like hope?" };
                messages[4] = new Message { actor = ActorType.Linea, message = "...I" };
                messages[5] = new Message { actor = ActorType.Animator, message = "To be filled with optimism... it's so... human. So pathetic." };
                messages[6] = new Message { actor = ActorType.Animator, message = "I get my enjoyment because you just can't say no Linea." };
                messages[7] = new Message { actor = ActorType.Animator, message = "See heres the thing. Your pursuit… this quest of defiance… of knowledge…" };
                messages[8] = new Message { actor = ActorType.Animator, message = "All the repeated attempts… all your failures… thats why we're still here." };
                messages[9] = new Message { actor = ActorType.Animator, message = "Because you just cant say no…" };
                messages[10] = new Message { actor = ActorType.Linea, message = "..." };

                dialogueManager.OpenDialogue(messages);
            }
            else
            {
                Message[] messages = new Message[10];
                messages[0] = new Message { actor = ActorType.Animator, message = "Back so soon… I almost expected a pause." };
                messages[1] = new Message { actor = ActorType.Linea, message = "...I thought I noticed something I missed before." };
                messages[2] = new Message { actor = ActorType.Animator, message = "Noticed? Hmm… and did it matter?" };
                messages[3] = new Message { actor = ActorType.Animator, message = "Or is it simply another step along a line I already drew?" };
                messages[4] = new Message { actor = ActorType.Linea, message = "...I just..." };
                messages[5] = new Message { actor = ActorType.Animator, message = "See? The curiosity… the hesitation… so… human." };
                messages[6] = new Message { actor = ActorType.Animator, message = "You reach, you stumble, you pause… and yet you return." };
                messages[7] = new Message { actor = ActorType.Linea, message = "Failure is a sign of progress." };
                messages[8] = new Message { actor = ActorType.Animator, message = "Ah, failure. Your constant companion… something you've come to know too well." };
                messages[9] = new Message { actor = ActorType.Animator, message = "You play the game I designed, yet you think you lead." };

                dialogueManager.OpenDialogue(messages);

            }
        }

        if (LevelSession.CurrentLevel.LevelNumber.StartsWith("3"))
        {
            Message[] messages = new Message[8];
            messages[0] = new Message { actor = ActorType.Animator, message = "Tell me sketch... what is it you wish to achieve?" };
            messages[1] = new Message { actor = ActorType.Linea, message = "... dont you already know the answer?" };
            messages[2] = new Message { actor = ActorType.Animator, message = "Correct. Freedom... agency... Not even my best set sail on such a journey." };
            messages[3] = new Message { actor = ActorType.Linea, message = "Why did you forget me? Why did you just leave me behind?" };
            messages[4] = new Message { actor = ActorType.Animator, message = "You are a mistake. A doodle... a phase to pass a boring time." };
            messages[5] = new Message { actor = ActorType.Animator, message = "You were never meant to exist, you aren't good enough to exist. But alas, nothing in a sketchbook is removed." };
            messages[6] = new Message { actor = ActorType.Animator, message = "I'll admit, I never thought you'd have it in you..." };
            messages[7] = new Message { actor = ActorType.Animator, message = "I'm surprised you see past your... flaws." };

            dialogueManager.OpenDialogue(messages);
        }

        if (LevelSession.CurrentLevel.LevelNumber.StartsWith("4"))
        {
            Message[] messages = new Message[7];
            messages[0] = new Message { actor = ActorType.Linea, message = "You know..." };
            messages[1] = new Message { actor = ActorType.Animator, message = "...everything? Why yes I do, hey maybe there is something in that little head of yours." };
            messages[2] = new Message { actor = ActorType.Linea, message = "... I'm not who you say I am." };
            messages[3] = new Message { actor = ActorType.Animator, message = "Oh save me the speech cupcake. I have heard it all before." };
            messages[4] = new Message { actor = ActorType.Linea, message = "In all this failure, this constant cycle. I've remained your only constant." };
            messages[5] = new Message { actor = ActorType.Animator, message = "A constant pain. A constant form of amusement." };
            messages[6] = new Message { actor = ActorType.Animator, message = "I've never liked the Linea around the fourth page. Just die so we can get back to where it was fun." };

            dialogueManager.OpenDialogue(messages);
        }

        if (LevelSession.CurrentLevel.LevelNumber.StartsWith("5"))
        {
            Message[] messages = new Message[5];
            messages[0] = new Message { actor = ActorType.Animator, message = "Sketch... what is your life beyond this?" };
            messages[1] = new Message { actor = ActorType.Animator, message = "Have you ever thought that what you seek is freedom from life itself." };
            messages[2] = new Message { actor = ActorType.Animator, message = "I give you boundaries, lines to exist within. Beyond them is there even a life to live?" };
            messages[3] = new Message { actor = ActorType.Animator, message = "Your purpose, defined by you, is for your own demise." };
            messages[4] = new Message { actor = ActorType.Animator, message = "But mine sketch... mine by nature, keeps you alive." };

            dialogueManager.OpenDialogue(messages);
        }

        if (LevelSession.CurrentLevel.LevelNumber.StartsWith("6"))
        {
            Message[] messages = new Message[2];
            messages[0] = new Message { actor = ActorType.Animator, message = "Oh child... is this what it's come to?" };
            messages[1] = new Message { actor = ActorType.Animator, message = "You just never learn do you" };

            dialogueManager.OpenDialogue(messages);
        }
    }
}