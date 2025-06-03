using UnityEngine;
using System;
using NicoUtilities;

[Serializable] public class Sentence
{
    [TextArea(3, 10)] public string dialogue;
    public AudioClip audioclip;
}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public int dialogueID;
    public string characterName;
    public AudioClip dialogueClip;

    public string DialogueText => DialogueCSVLoader.Instance.GetDialogueText(dialogueID);
}
