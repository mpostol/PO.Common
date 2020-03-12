//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using UAOOI.ProcessObserver.RealTime.Processes;

namespace CAS.Lib.CommonBus.CommunicationLayer
{

  /// <summary>
  /// Interface used by communication library to get access to serialized data transmitted over a physical medium.
  /// </summary>
  public interface ISesDBuffer : IDBuffer, IEnvelope { }

}