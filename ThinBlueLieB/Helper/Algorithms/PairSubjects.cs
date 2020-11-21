using DataAccessLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLieB.Helper.Algorithms
{
    public partial class PairVersions
    {
        public List<Tuple<DBSubject, DBSubject>> PairSubjects(List<DBSubject> OldSubjects, List<DBSubject> NewSubjects)
        {
            List<Tuple<DBSubject, DBSubject>> Subjects = new List<Tuple<DBSubject, DBSubject>>();
            List<int> pairedIds = new List<int>();
            foreach (var subject in NewSubjects.Union(OldSubjects))
            {
                if (!pairedIds.Contains(subject.IdSubject))
                {
                    List<DBSubject> pair = NewSubjects.Union(OldSubjects).Where(o => o.IdSubject == subject.IdSubject).ToList();
                    bool isPaired = false;
                    Tuple<DBSubject, DBSubject> pairing;
                    for (int i = 0; i < pair.Count; i++)
                    {
                        if (pair[i] != subject)
                        {
                            if (NewSubjects.Contains(pair[i]))
                            {
                                pairing = new Tuple<DBSubject, DBSubject>(subject, pair[i]);
                            }
                            else //if oldOfficer contains it
                            {
                                pairing = new Tuple<DBSubject, DBSubject>(pair[i], subject);
                            }
                            Subjects.Add(pairing);
                            isPaired = true;
                            pairedIds.AddRange(new int[] { subject.IdSubject, pair[i].IdSubject });
                        }
                    }
                    if (isPaired == false)
                    {
                        if (OldSubjects.Contains(subject))
                        {
                            pairing = new Tuple<DBSubject?, DBSubject?>(null, subject);
                        }
                        else //if oldOfficer contains it
                        {
                            pairing = new Tuple<DBSubject?, DBSubject?>(subject, null);
                        }
                        Subjects.Add(pairing);
                        pairedIds.Add(subject.IdSubject);
                    }
                }
            }
            return Subjects;
        }

    }
}
