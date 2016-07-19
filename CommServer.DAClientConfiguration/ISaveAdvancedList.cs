//<summary>
//  Title   : List of ISaveAdvanced
//  System  : Microsoft Visual C# .NET 2008
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  20081224: mzbrzezny: ISaveAdvancedList: function that returns array of string is added
//  20080516: mzbrzezny: created
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.Collections;
using System.Collections.Generic;

namespace CAS.DataPorter.Configurator
{
  /// <summary>
  /// List of ISaveAdvanced
  /// </summary>
  public class ISaveAdvancedList: IEnumerable
  {
    #region public
    private List<ISaveAdvanced> list = new List<ISaveAdvanced>();
    /// <summary>
    /// Adds the specified save advanced item.
    /// </summary>
    /// <param name="ISaveAdvancedItem">The I save advanced item.</param>
    public void Add( ISaveAdvanced ISaveAdvancedItem )
    {
      list.Add( ISaveAdvancedItem );
    }
    /// <summary>
    /// Removes the specified save advanced item.
    /// </summary>
    /// <param name="ISaveAdvancedItem">The I save advanced item.</param>
    public void Remove( ISaveAdvanced ISaveAdvancedItem )
    {
      list.Remove( ISaveAdvancedItem );
    }
    /// <summary>
    /// Gets the save advanced by last DB identifier.
    /// </summary>
    /// <param name="LastDBIdentifier">The last DB identifier.</param>
    /// <returns></returns>
    public ISaveAdvanced GetISaveAdvancedByLastDBIdentifier( long LastDBIdentifier )
    {
      for ( int idx = 0; idx < list.Count; idx++ )
        if ( list[ idx ].GetLastDBIdentifier == LastDBIdentifier )
          return list[ idx ];
      return null;
    }
    /// <summary>
    /// Gets the  array of ISaveAdvanced elements on the list .
    /// </summary>
    /// <returns></returns>
    public ISaveAdvanced[] GetISaveAdvancedArray()
    {
      ISaveAdvanced[] retarray = new ISaveAdvanced[ list.Count ];
      int idx = 0;
      foreach ( ISaveAdvanced isa in list )
        retarray[ idx++ ] = isa;
      return retarray;
    }
    /// <summary>
    /// Gets the string (representation of each element) array.
    /// </summary>
    /// <param name="AddNoneElementToList">if set to <c>true</c> [add none element to list].</param>
    /// <returns></returns>
    public string[] GetStringArray( bool AddNoneElementToList )
    {
      string[] retarray;
      int idx = 0;
      if ( AddNoneElementToList )
      {
        retarray = new string[ list.Count + 1 ];
        retarray[ idx++ ] = "(none)";
      }
      else
        retarray = new string[ list.Count ];
      foreach ( ISaveAdvanced isa in list )
        retarray[ idx++ ] = isa.ToString();
      return retarray;
    }
    #endregion
    #region IEnumerable Members
    IEnumerator IEnumerable.GetEnumerator()
    {
      return ( (IEnumerable)list ).GetEnumerator();
    }

    #endregion
  }
}
