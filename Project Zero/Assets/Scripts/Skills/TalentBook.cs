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

            //Add in All tag
            if (talentBook.ContainsKey(SkillTag.All))
            {
                talentBook[SkillTag.All].Add(talent);
            }
            else
            {
                talentBook.Add(SkillTag.All, new List<Talent>());
                talentBook[SkillTag.All].Add(talent);
            }
        }
    }

    public List<Talent> GetSkills(SkillTag skillTag)
    {
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