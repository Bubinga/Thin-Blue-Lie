using DataAccessLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLieB.Helper.Algorithms
{
    public partial class PairVersions
    {
        public List<Tuple<DBOfficer, DBOfficer>> PairOfficers(List<DBOfficer> OldOfficers, List<DBOfficer> NewOfficers)
        {
            List<Tuple<DBOfficer, DBOfficer>> Officers = new List<Tuple<DBOfficer, DBOfficer>>();
            List<int> pairedIds = new List<int>();
            foreach (var officer in NewOfficers.Union(OldOfficers))
            {
                if (!pairedIds.Contains(officer.IdOfficer))
                {
                    List<DBOfficer> pair = NewOfficers.Union(OldOfficers).Where(o => o.IdOfficer == officer.IdOfficer).ToList();
                    bool isPaired = false;
                    Tuple<DBOfficer, DBOfficer> pairing;
                    for (int i = 0; i < pair.Count; i++)
                    {
                        if (pair[i] != officer)
                        {
                            if (NewOfficers.Contains(pair[i]))
                            {
                                pairing = new Tuple<DBOfficer, DBOfficer>(officer, pair[i]);
                            }
                            else //if oldOfficer contains it
                            {
                                pairing = new Tuple<DBOfficer, DBOfficer>(pair[i], officer);
                            }
                            Officers.Add(pairing);
                            isPaired = true;
                            pairedIds.AddRange(new int[] { officer.IdOfficer, pair[i].IdOfficer });
                        }
                    }
                    if (isPaired == false)
                    {
                        if (NewOfficers.Contains(officer))
                        {
                            pairing = new Tuple<DBOfficer?, DBOfficer?>(null, officer);
                        }
                        else //if oldOfficer contains it
                        {
                            pairing = new Tuple<DBOfficer?, DBOfficer?>(officer, null);
                        }
                        Officers.Add(pairing);
                        pairedIds.Add(officer.IdOfficer);
                    }
                }
            }
            return Officers;
        }
    }
}
