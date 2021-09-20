using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CustomYarnCommands : MonoBehaviour
{
    public DialogueRunner DialogueRunner;
    public DialogueUI DialogueUI;

    void Awake()
    {
        DialogueRunner.AddCommandHandler("StopAndGo", StopAndGo);
    }

    public void StopAndGo(string[] parameters, System.Action onComplete)
    {
        if(float.TryParse(parameters[0], out float seconds))
        {
            StartCoroutine(DoWait(seconds, onComplete));
            DialogueUI.MarkLineComplete();
        }
    }

    private IEnumerator DoWait(float seconds, System.Action onComplete)
    {
        yield return new WaitForSeconds(seconds);

        onComplete();
    }
}
