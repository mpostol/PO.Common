//<summary>
//  Title   : Data provider identifying interface – provides general information about Item Default Settings.
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//  20081006 mzbrzezny: ItemAccessRights are moved to RtLibComm
//  20081003: mzbrzezny: created
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Lib.RTLib;

namespace CAS.Lib.CommonBus
{
  /// <summary>
  /// general information about Item Default Settings.
  /// </summary>
  public interface IItemDefaultSettings
  {
    /// <summary>
    /// Gets the default name.
    /// </summary>
    /// <value>The name.</value>
    string Name { get; }
    /// <summary>
    /// Gets the type of the default.
    /// </summary>
    /// <value>The type of the default.</value>
    System.Type DefaultType { get; }
    /// <summary>
    /// Gets the availiable types.
    /// </summary>
    /// <value>The availiable types.</value>
    System.Type[] AvailiableTypes { get; }
    /// <summary>
    /// Gets the access rights.
    /// </summary>
    /// <value>The access rights.</value>
    ItemAccessRights AccessRights { get; }
  }

}
