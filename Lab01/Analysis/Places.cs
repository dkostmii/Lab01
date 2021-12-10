using System;
using System.Linq;

namespace Lab01.Analysis
{
    public class Place
    {
        public string Tag { get; private set; }

        public Place(string place)
        {
            Tag = place;
        }

        public override bool Equals(Object obj)
        {
            Place another = obj as Place;
            if (another == null)
            {
                throw new Exception("Invalid PlaceTag provided in Equals()");
            }

            return Tag.Equals(another.Tag);
        }

        public override int GetHashCode()
        {
            return Tag.GetHashCode();
        }

        public override string ToString()
        {
            return Tag;
        }
    }

    public static class Places
    {
        public static readonly Place WestGermany = new Place("west-germany");
        public static readonly Place USA = new Place("usa");
        public static readonly Place France = new Place("france");
        public static readonly Place UK = new Place("uk");
        public static readonly Place Canada = new Place("canada");
        public static readonly Place Japan = new Place("japan");

        public static Place[] All
        {
            get
            {
                return new Place[] { WestGermany, USA, France, UK, Canada, Japan };
            }
        }

        public static bool CheckTag(Place place)
        {
            return All.Any(p => p.Equals(place));
        }

        public static bool CheckTag(string tag)
        {
            return All.Any(t => t.Tag.Equals(tag));
        }
    }
}
