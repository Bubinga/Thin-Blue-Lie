using DataAccessLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ThinBlueLie.Helper.Algorithms
{
    public partial class PairVersions
    {
        public List<Tuple<Subject, Subject>> PairSubjects(List<Subject> OldSubjects, List<Subject> NewSubjects)
        {
            List<Tuple<Subject, Subject>> Subjects = new List<Tuple<Subject, Subject>>();
            List<int> pairedIds = new List<int>();
            foreach (var subject in NewSubjects.Union(OldSubjects))
            {
                if (!pairedIds.Contains(subject.IdSubject))
                {
                    List<Subject> pair = NewSubjects.Union(OldSubjects).Where(o => o.IdSubject == subject.IdSubject).ToList();
                    bool isPaired = false;
                    Tuple<Subject, Subject> pairing;
                    for (int i = 0; i < pair.Count; i++)
                    {
                        if (pair[i] != subject)
                        {
                            if (NewSubjects.Contains(pair[i]))
                            {
                                pairing = new Tuple<Subject, Subject>(subject, pair[i]);
                            }
                            else //if oldOfficer contains it
                            {
                                pairing = new Tuple<Subject, Subject>(pair[i], subject);
                            }
                            Subjects.Add(pairing);
                            isPaired = true;
                            pairedIds.AddRange(new int[] { subject.IdSubject, pair[i].IdSubject });
                        }
                    }
                    if (isPaired == false)
                    {
                        if (NewSubjects.Contains(subject))
                        {
                            pairing = new Tuple<Subject?, Subject?>(null, subject);
                        }
                        else //if oldOfficer contains it
                        {
                            pairing = new Tuple<Subject?, Subject?>(subject, null);
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
