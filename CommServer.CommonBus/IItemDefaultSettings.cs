//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using UAOOI.ProcessObserver.RealTime;

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
    /// Gets the available types.
    /// </summary>
    /// <value>The available types.</value>
    System.Type[] AvailiableTypes { get; }
    /// <summary>
    /// Gets the access rights.
    /// </summary>
    /// <value>The access rights.</value>
    ItemAccessRights AccessRights { get; }
  }

}
