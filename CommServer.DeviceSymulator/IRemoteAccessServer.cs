//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

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
