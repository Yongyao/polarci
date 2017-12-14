using System;
using System.Collections.Generic;
using System.Linq;

namespace GeoSearch
{
    /// <summary>
    /// Custom class to support multivalued facets sorting
    /// </summary>
    public class MyStringList : List<string>, IComparable
    {
        #region Implementation of IComparable
        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj"/> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj"/>.Greater than zero This instance follows <paramref name="obj"/> in the sort order.
        /// </returns>
        /// <param name="obj">An object to compare with this instance. </param><exception cref="T:System.ArgumentException"><paramref name="obj"/> is not the same type as this instance. </exception>
        public int CompareTo(object obj)
        {
            //PUT YOUR SORTING LOGIC HERE
            //var stringList = obj as MyStringList;
            //if (stringList == null || this == null)
            //    return 0;
            //if (this.Count == 0 || stringList.Count == 0)
            //    return 0;
            //else
            //{
            //    if (this.Count == 0 || stringList.Count == 0)
            //    {
            //        if (this.Count == 0 && stringList.Count > 0)
            //            return 1;
            //        else if (this.Count > 0 && stringList.Count == 0)
            //            return -1;
            //    }
            //    else
            //    {
            //        foreach (string a in stringList)
            //        {
            //            if (this.Contains(a))
            //                return 0;
            //        }
            //        string a1 = (stringList.ToArray())[0];
            //        string b1 = (this.ToArray())[0];
            //        return b1.CompareTo(a1);
            //    }
            //}
            //return -1;
            return 0;
        }
        #endregion
    }
}