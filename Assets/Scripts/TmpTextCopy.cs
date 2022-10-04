using UnityEngine;
using TMPro;

/// <summary>
/// This class' intended purpose is to give a string the ability to 
/// replicate the forefront color's text. It is an extention created after 
/// the first release for aesthetic purposes.
/// </summary>
public class TmpTextCopy : MonoBehaviour
{

    // The original text object to copy from
    public TMP_Text textToCopy;

    // Attached to the text object that will do the copying
    private TMP_Text thisText;

    /// <summary>
    ///     Checks to see if there is text to copy, otherwise 
    ///     the component will render useless.
    /// </summary>
    void Start()
    {
        if (textToCopy == null) {
            throw new System.Exception("There is a TmpTextCopy component not copying anything!");
        }
        thisText = GetComponent<TMP_Text>();
    }

    // Updates the text.
    void Update()
    {
        thisText.text = textToCopy.text;
    }
}
