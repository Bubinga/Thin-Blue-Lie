using DataAccessLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLie.Models;

namespace ThinBlueLie.Helper.Algorithms
{
    public partial class PairVersions
    {
        public List<Tuple<ViewMedia, ViewMedia>> PairMedia(List<ViewMedia> OldMedias, List<ViewMedia> NewMedias)
        {
            List<Tuple<ViewMedia, ViewMedia>> Medias = new List<Tuple<ViewMedia, ViewMedia>>();
            List<int> pairedIds = new List<int>();
            foreach (var Media in NewMedias.Union(OldMedias))
            {
                if (!pairedIds.Contains(Media.IdMedia))
                {
                    List<ViewMedia> pair = NewMedias.Union(OldMedias).Where(o => o.IdMedia == Media.IdMedia).ToList();
                    bool isPaired = false;
                    Tuple<ViewMedia, ViewMedia> pairing;
                    for (int i = 0; i < pair.Count; i++)
                    {
                        if (pair[i] != Media)
                        {
                            if (NewMedias.Contains(pair[i]))
                            {
                                pairing = new Tuple<ViewMedia, ViewMedia>(Media, pair[i]);
                            }
                            else //if oldMedia contains it
                            {
                                pairing = new Tuple<ViewMedia, ViewMedia>(pair[i], Media);
                            }
                            Medias.Add(pairing);
                            isPaired = true;
                            pairedIds.AddRange(new int[] { Media.IdMedia, pair[i].IdMedia });
                        }
                    }
                    if (isPaired == false)
                    {
                        if (NewMedias.Contains(Media))
                        {
                            pairing = new Tuple<ViewMedia?, ViewMedia?>(null, Media);
                        }
                        else //if oldMedia contains it
                        {
                            pairing = new Tuple<ViewMedia?, ViewMedia?>(Media, null);
                        }
                        Medias.Add(pairing);
                        //pairedIds.Add(Media.IdMedia);
                    }
                }
            }
            return Medias;
        }
    }
}
