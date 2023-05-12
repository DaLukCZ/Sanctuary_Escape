using UnityEditor;

[CustomEditor(typeof(Interactable),true)]
public class InteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Interactable interactable = (Interactable)target;
        if (target.GetType() == typeof(Interactor))
        {
            interactable.promptMessage = EditorGUILayout.TextField("Promp Message", interactable.promptMessage);
            EditorGUILayout.HelpBox("EventOnlyInteract can ONLY use UnityEvents.", MessageType.Info);
            if (interactable.GetComponent<Interactor>() == null)
            {
                interactable.useEvents = true;
                interactable.gameObject.AddComponent<Interactable>();
            }
        }
        else
        {
            base.OnInspectorGUI();
            if (interactable.useEvents)
            {
                if (interactable.GetComponent<Interactable>() == null)
                    interactable.gameObject.AddComponent<Interactable>();
            }
            else
            {
                if (interactable.GetComponent<Interactable>() != null)
                    DestroyImmediate(interactable.GetComponent<Interactor>());
            }
        }
    }
}
