//<summary>
//  Title   : Remote Access Server interface
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    mzbrzezny, 2007: created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace CAS.Lib.DeviceSimulator
{
  /// <summary>
  /// Remote Access Server interface
  /// </summary>
  public interface IRemoteAccessServer
  {
    /// <summary>
    /// Starts the server.
    /// </summary>
    void Start();
    /// <summary>
    /// Stops the server.
    /// </summary>
    void Stop();
  }
}
