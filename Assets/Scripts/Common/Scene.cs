using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
    public bool finished;

    public Camera SceneCamera;

    public GameObject sound;

    public string interactionText;

    public Dictionary<string, string> texts = new Dictionary<string, string>();

    public Scene()
    {
        finished = false;
    }

    public void PushMessageToMaster(string message)
    {
        GameController.Master.messages.Add(message);
    }

    public void PushSceneMessageToMaster(string message)
    {
        GameController.Master.WriteSceneMessage(message);
    }

    public void WriteTextToDreamJournalMaster(string message)
    {
        GameController.Master.WriteInDreamJournal(message);
    }

    public void ToggleKeybinds(bool state)
    {
        GameController.Master.Keybinds.gameObject.SetActive(state);
    }


    public void ReduceTime(int x)
    {
        GameController.Master.updateTimer(x);
    }

    public void IncreaseTime(int x)
    {
        GameController.Master.IncreaseTimer(x);
    }

    public void RemoveFromInteractables()
    {
        PlayerController._PlayerController.interactables.Remove(this.gameObject);
    }

    virtual public void OnPlayerInteract() { }

    public virtual void Keybinds()
    {
    }

    public virtual void Keybinds(int mode)
    {
    }

    public void ToggleSound(bool state)
    {
        sound.SetActive(state);
    }
}
