using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using TUNING;
using Zolibrary.Logging;

namespace Zolibrary.Utilities
{
    public static class BuildingUtils
    {
        public static void AddStrings(string ID, string Name, string Description, string Effect)
        {
            Strings.Add(new string[]{
                    "STRINGS.BUILDINGS.PREFABS." + ID.ToUpperInvariant() + ".NAME", UI.FormatAsLink(Name, ID)
            });

            Strings.Add(new string[]{
                    "STRINGS.BUILDINGS.PREFABS." + ID.ToUpperInvariant() + ".DESC",Description
            });

            Strings.Add(new string[]{
                    "STRINGS.BUILDINGS.PREFABS." + ID.ToUpperInvariant() + ".EFFECT", Effect
            });
        }

        public static void AddToPlanning(HashedString Category, string ID, string beforeID)
        {
            int catIndex = TUNING.BUILDINGS.PLANORDER.FindIndex((PlanScreen.PlanInfo y) => y.category == Category);

            if (catIndex < 0)
            {
                LogManager.LogError("Error adding building (" + ID + ") to planning list. Category doesn't exist :" + Category);
                return;
            }

            IList<string> list = TUNING.BUILDINGS.PLANORDER[catIndex].data as IList<string>;

            int index = -1;

            foreach (string s in list)
                if (s.Equals(beforeID))
                    index = list.IndexOf(s);

            if (index == -1)
            {
                LogManager.LogError("Error adding building (" + ID + ") to planning list. ID doesn't exist :" + beforeID);
                return;
            }

            list.Insert(index + 1, ID);
        }

        public static void AddToTechnology(string Tech, string ID)
        {
            List<string> list = new List<string>(Techs.TECH_GROUPING[Tech]) { ID };
            Techs.TECH_GROUPING[Tech] = list.ToArray();
        }
    }
}

