using UnityEditor;

[CustomEditor(typeof(Interactable),true)]
public class InteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Interactable interactable = (Interactable)target;
        if (target.GetType() == typeof(EventOnlyInteractable))
        {
            interactable.promptMessage = EditorGUILayout.TextField("Promp Message", interactable.promptMessage);
            EditorGUILayout.HelpBox("EventOnlyInteract can ONLY use UnityEvents.", MessageType.Info);
            if (interactable.GetComponent<InteractionEvent>() == null)
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
                    DestroyImmediate(interactable.GetComponent<InteractionEvent>());
            }
        }
    }
}
