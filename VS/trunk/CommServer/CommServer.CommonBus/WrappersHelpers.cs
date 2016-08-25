//<summary>
//  Title   : DataSetHelpers
//  System  : Microsoft Visual C# .NET 
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//  20081105: mzbrzezny: WrappersHelpers: additional check in GetName
//  20081006: mzbrzezny: AddressSpaceDescriptor and Item Default Settings are implemented.
//  20081003: mzbrzezny: class is marked as internal
//  20081003: mzbrzezny: AddressSpaceDescriptor implementation
//  Tomasz Siwecki - February 2007 Add some comment and reformat code
//  Tomasz Siwecki - October 2006
//  Created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections;
using System.Collections.Generic;
using CAS.Lib.CommonBus;

namespace CAS.CommServer.CommonBus
{
  /// <summary>
  /// Class contains static methods helping management of the <see cref="IAddressSpaceDescriptor"/> collections.
  /// </summary>
  public static class WrappersHelpers
  {
    #region Methods
    /// <summary>
    /// Returns the name of indexed by <paramref name="index"/> of an object implementing <see cref="IAddressSpaceDescriptor"/>. 
    /// If the object is not present <paramref name="index"/> is converted to string.
    /// </summary>
    /// <param name="table">table of indexed <see cref="IAddressSpaceDescriptor"/></param>
    /// <param name="index">Index of the <see cref="IAddressSpaceDescriptor"/> to retrieve its name.</param>
    /// <returns>Name of the <see cref="IAddressSpaceDescriptor"/> indexed by the <paramref name="index"/>, or "N/A" if <paramref name="table"/> is null.</returns>
    public static string GetName(SortedList<short, IAddressSpaceDescriptor> table, short? index)
    {
      string name = "N/A";
      if (index.HasValue && table != null)
      {
        try
        {
          return table[(short)index].Name;
        }
        catch (Exception)
        {
          if (index.HasValue)
          {
            return index.ToString();
          }
        }
      }
      else
      {
        if (table == null && index.HasValue)
          return index.ToString();
      }
      return name;
    }
    /// <summary>
    /// Returns the id related to specified name
    /// </summary>
    /// <param name="table">has table with  enum </param>
    /// <param name="name">Name that will be changed to id</param>
    /// <returns>Integer related to the specified name</returns>
    public static short? GetID(SortedList<short, IAddressSpaceDescriptor> table, string name)
    {
      if (table != null)
      {
        // znaczy sie jest tablica zawierajaca dane
        foreach (KeyValuePair<short, IAddressSpaceDescriptor> kvpIAddressSpaceDescriptor in table)
        {
          if (kvpIAddressSpaceDescriptor.Value.Name == name)
            return kvpIAddressSpaceDescriptor.Value.Identifier;
        }
      }
      // probujemy dokonac konwersji name do int, jesli sie uda to konfiguracja jest dobra
      // jesli nie to znczy ze zostawiamy null
      try
      {
        return System.Convert.ToInt16(name);
      }
      catch (Exception)
      {
        return null;
      }
    }
    /// <summary>
    /// Returns the id related to specified name
    /// </summary>
    /// <param name="table">has table with  enum </param>
    /// <param name="name">Name that will be changed to id</param>
    /// <returns>Integer related to the specified name</returns>
    public static short? GetID(IAddressSpaceDescriptor[] table, string name)
    {
      if (table != null)
      {
        // znaczy sie jest tablica zawierajaca dane
        foreach (IAddressSpaceDescriptor myIAddressSpaceDescriptor in table)
        {
          if (myIAddressSpaceDescriptor.Name == name)
            return myIAddressSpaceDescriptor.Identifier;
        }
      }
      // probujemy dokonac konwersji name do int, jesli sie uda to konfiguracja jest dobra
      // jesli nie to znczy ze zostawiamy null
      try
      {
        return System.Convert.ToInt16(name);
      }
      catch (Exception)
      {
        return null;
      }
    }
    /// <summary>
    /// returns an array of string that are keys of the <see cref="Hashtable"/>
    /// </summary>
    /// <param name="array">The array of address space descriptors.</param>
    /// <returns>all keys</returns>
    public static string[] GetNames(SortedList<short, IAddressSpaceDescriptor> array)
    {
      int count = 256;
      if (array != null)
        count = array.Keys.Count;
      string[] return_array = new string[count];
      int idx = 0;
      if (array != null)
      {
        foreach (IAddressSpaceDescriptor AddressSpaceDescriptor in array.Values)
        {
          return_array[idx++] = AddressSpaceDescriptor.Name;
        }
      }
      else
        for (int i = 0; i < 256; i++)
          return_array[i] = i.ToString();
      return return_array;
    }
    #endregion
  }
}
