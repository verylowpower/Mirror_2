// using UnityEngine;
// using UnityEditor;
// using System.IO;
// using System.Collections.Generic;

// public class QuestImporter
// {
//     [MenuItem("Tools/Import Quests From CSV")]
//     public static void Import()
//     {
//         string path = "Assets/quests.csv";

//         if (!File.Exists(path))
//         {
//             Debug.LogError("CSV file not found at " + path);
//             return;
//         }

//         string folder = "Assets/Resources/Quests";

//         if (!Directory.Exists(folder))
//             Directory.CreateDirectory(folder);

//         string[] lines = File.ReadAllLines(path);

//         Dictionary<string, QuestData> questDict = new();
//         Dictionary<string, string> nextQuestMap = new();

//         for (int i = 1; i < lines.Length; i++)
//         {
//             if (string.IsNullOrWhiteSpace(lines[i]))
//                 continue;

//             string[] data = lines[i].Split(',');

//             if (data.Length < 8)
//             {
//                 Debug.LogError($"CSV line {i + 1} is invalid");
//                 continue;
//             }

//             string questId = data[0].Trim();
//             string questName = data[1].Trim();
//             string description = data[2].Trim();
//             string enemyType = data[3].Trim();
//             int killCount = int.Parse(data[4].Trim());
//             string rewardSkill = data[5].Trim();
//             string nextQuestId = data[6].Trim();
//             bool autoStart = bool.Parse(data[7].Trim());

//             QuestData quest = ScriptableObject.CreateInstance<QuestData>();

//             quest.questId = questId;
//             quest.questName = questName;
//             quest.description = description;

//             quest.conditions = new List<QuestCondition>()
//             {
//                 new QuestCondition()
//                 {
//                     targetId = enemyType,
//                     requiredAmount = killCount
//                 }
//             };

//             quest.rewardSkillId = rewardSkill;
//             quest.autoStartNext = autoStart;

//             string assetPath = $"{folder}/{questId}.asset";

//             if (File.Exists(assetPath))
//                 AssetDatabase.DeleteAsset(assetPath);

//             AssetDatabase.CreateAsset(quest, assetPath);

//             questDict.Add(questId, quest);
//             nextQuestMap.Add(questId, nextQuestId);
//         }
//         foreach (var pair in nextQuestMap)
//         {
//             if (string.IsNullOrEmpty(pair.Value))
//                 continue;

//             if (questDict.ContainsKey(pair.Value))
//             {
//                 questDict[pair.Key].nextQuest = questDict[pair.Value];
//                 EditorUtility.SetDirty(questDict[pair.Key]);
//             }
//         }

//         string[] skillGuids = AssetDatabase.FindAssets("t:SkillNodeData");

//         foreach (string guid in skillGuids)
//         {
//             string skillPath = AssetDatabase.GUIDToAssetPath(guid);
//             SkillNodeData skill = AssetDatabase.LoadAssetAtPath<SkillNodeData>(skillPath);

//             foreach (var quest in questDict.Values)
//             {
//                 if (skill.skillId == quest.rewardSkillId)
//                 {
//                     skill.requiredQuestId = quest.questId;
//                     EditorUtility.SetDirty(skill);
//                 }
//             }
//         }

//         AssetDatabase.SaveAssets();
//         AssetDatabase.Refresh();

//         Debug.Log("Quest Import Finished");
//     }
// }