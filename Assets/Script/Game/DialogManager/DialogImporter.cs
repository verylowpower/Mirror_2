using UnityEngine;
using UnityEditor;
using System.IO;

public class DialogImporter
{
    [MenuItem("Tools/Import Dialogs From CSV")]
    public static void Import()
    {
        string path = "Assets/dialogs.csv";

        if (!File.Exists(path))
        {
            Debug.LogError("Dialog CSV not found");
            return;
        }

        string rootFolder = "Assets/Resources/Dialogs/Fairy";

        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");

        if (!AssetDatabase.IsValidFolder("Assets/Resources/Dialogs"))
            AssetDatabase.CreateFolder("Assets/Resources", "Dialogs");

        if (!AssetDatabase.IsValidFolder(rootFolder))
            AssetDatabase.CreateFolder("Assets/Resources/Dialogs", "Fairy");


        string[] lines = File.ReadAllLines(path);

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            string[] data = lines[i].Split(',');

            if (data.Length < 4)
            {
                Debug.LogWarning($"Invalid line {i}");
                continue;
            }

            string questId = data[0].Trim();
            string state = data[1].Trim();
            string boolRaw = data[2].Trim();
            string sentenceRaw = data[3];

            bool startQuest = false;
            bool.TryParse(boolRaw, out startQuest);

            string[] sentences = sentenceRaw.Split('|');

            DialogData dialog = ScriptableObject.CreateInstance<DialogData>();

            dialog.sentences = sentences;
            if (!System.Enum.TryParse(state, out DialogQuestState questState))
            {
                Debug.LogWarning($"Invalid state '{state}' at line {i}");
                continue;
            }

            dialog.questState = questState;
            dialog.startQuestOnEnd = startQuest;

            string[] questGuids = AssetDatabase.FindAssets($"t:QuestData {questId}");

            if (questGuids.Length > 0)
            {
                string questPath = AssetDatabase.GUIDToAssetPath(questGuids[0]);
                QuestData quest = AssetDatabase.LoadAssetAtPath<QuestData>(questPath);
                dialog.relatedQuest = quest;
            }

            string assetName = $"{questId}_{state}.asset";
            string assetPath = $"{rootFolder}/{assetName}";
            if (File.Exists(assetPath))
            {
                AssetDatabase.DeleteAsset(assetPath);
            }

            AssetDatabase.CreateAsset(dialog, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Dialog Import Finished");
    }
}