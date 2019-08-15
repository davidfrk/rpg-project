using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Skills;
using Rpg.ProgressionSystem;

public class TalentBook : MonoBehaviour
{
    public static TalentBook instance;

    public List<Talent> talents;

    public SkillTagDictionary talentBook = new SkillTagDictionary();

    void Awake()
    {
        instance = this;

        foreach (Talent talent in talents)
        {
            foreach (SkillTag skillTag in talent.skillTags)
            {
                if (talentBook.ContainsKey(skillTag))
                {
                    talentBook[skillTag].Add(talent);
                }
                else
                {
                    talentBook.Add(skillTag, new List<Talent>());
                    talentBook[skillTag].Add(talent);
                }
            }
        }
    }

    void Start()
    {
        
    }

    public List<Talent> GetSkills(SkillTag skillTag)
    {
        //Debug.Log("SkillBookContainsKey " + skillTag + " " + skillBook.ContainsKey(skillTag));
        if (talentBook.ContainsKey(skillTag))
        {
            return talentBook[skillTag];
        }
        else
        {
            return null;
        }
    }
}

public class SkillTagDictionary : Dictionary<SkillTag, List<Talent>> { }

/*
[System.Serializable]
public class SerializableSkillTagDictionary : SerializableDictionary<SkillTag, List<Skill>> { }

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    // save the dictionary to lists
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // load dictionary from lists
    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
            throw new System.Exception(string.Format("there are 0 keys and 1 values after deserialization. Make sure that both key and value types are serializable."));

        for (int i = 0; i < keys.Count; i++)
            this.Add(keys[i], values[i]);
    }
}
*/
