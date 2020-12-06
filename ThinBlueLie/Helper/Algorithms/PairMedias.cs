using DataAccessLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLie.Helper.Algorithms
{
    public partial class PairVersions
    {
        public List<Tuple<Media, Media>> PairMedia(List<Media> OldMedias, List<Media> NewMedias)
        {
            List<Tuple<Media, Media>> Medias = new List<Tuple<Media, Media>>();
            List<int> pairedIds = new List<int>();
            foreach (var Media in NewMedias.Union(OldMedias))
            {
                if (!pairedIds.Contains(Media.IdMedia))
                {
                    List<Media> pair = NewMedias.Union(OldMedias).Where(o => o.IdMedia == Media.IdMedia).ToList();
                    bool isPaired = false;
                    Tuple<Media, Media> pairing;
                    for (int i = 0; i < pair.Count; i++)
                    {
                        if (pair[i] != Media)
                        {
                            if (NewMedias.Contains(pair[i]))
                            {
                                pairing = new Tuple<Media, Media>(Media, pair[i]);
                            }
                            else //if oldMedia contains it
                            {
                                pairing = new Tuple<Media, Media>(pair[i], Media);
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
                            pairing = new Tuple<Media?, Media?>(null, Media);
                        }
                        else //if oldMedia contains it
                        {
                            pairing = new Tuple<Media?, Media?>(Media, null);
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
