using DataAccessLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLie.Helper.Extensions;

namespace ThinBlueLie.Helper.Algorithms
{
    public partial class PairVersions
    {
        public List<Tuple<Officer, Officer>> PairOfficers(List<Officer> OldOfficers, List<Officer> NewOfficers)
        {
            List<Tuple<Officer, Officer>> Officers = new List<Tuple<Officer, Officer>>();
            List<int> pairedIds = new List<int>();
            foreach (var officer in NewOfficers.Union(OldOfficers))
            {
                if (!pairedIds.Contains(officer.IdOfficer)) //if not already paired
                {
                    List<Officer> pair = NewOfficers.Union(OldOfficers).Where(o => (o.IdOfficer == officer.IdOfficer) && o.IdOfficer != 0).ToList();
                    bool isPaired = false;
                    Tuple<Officer, Officer> pairing;
                    for (int i = 0; i < pair.Count; i++)
                    {
                        if (pair[i] != officer) //if not comparing to self
                        {
                            if (NewOfficers.Contains(pair[i]))
                            {
                                pairing = new Tuple<Officer, Officer>(officer, pair[i]);
                            }
                            else //if oldOfficer contains it
                            {
                                pairing = new Tuple<Officer, Officer>(pair[i], officer);
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
                            pairing = new Tuple<Officer?, Officer?>(null, officer);
                        }
                        else //if oldOfficer contains it
                        {
                            pairing = new Tuple<Officer?, Officer?>(officer, null);
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
