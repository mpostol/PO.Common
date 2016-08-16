//<summary>
//  Title   : COMMUNICATIONS LIBRARY - Protocols Application layer interface
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MZbrzezny 2007-01-31
//    usuwanie mechanizmu bazujacego na porcie 
//    w application layer i communication layer
//    MPostol - 13-06-2003:
//    Description: Created (start point (SB_SBAPP.DEF )
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Lib.RTLib.Processes;

namespace CAS.Lib.CommonBus.ApplicationLayer
{
  using CommunicationLayer;
  /// <summary>
  /// Interface which must be implemented  by any data provider playing the role on the field network of the slave 
  /// station, i.e. responding to the master station. It is responsible for processing commands received from a master station.
  /// </summary>
  public interface IApplicationLayerSlave: IConnectionManagement
  {
    /// <summary>
    /// Read command from a master station.
    /// </summary>
    /// <param name="frame">Received frame</param>
    /// <returns>
    ///   ALRes_Success: Operation accomplished successfully 
    ///   ALRes_DatTransferErrr: Data transfer is imposible because of a communication error – loss of 
    ///      communication with a station
    ///   ALRes_DisInd: Disconnect indication – connection has been shut down remotely or lost because of 
    ///      communication error. Data is unavailable
    /// </returns>
    AL_ReadData_Result ReadCMD( out IReadCMDValue frame );
    /// <summary>
    /// Send negative acknowledge.
    /// </summary>
    /// <returns>
    ///   ALRes_Success: Operation accomplished successfully 
    ///   ALRes_DatTransferErrr: Data transfer is imposible because of a communication error – loss of 
    ///      communication with a station
    ///   ALRes_DisInd: Disconnect indication – connection has been shut down remotely or lost because of 
    ///      communication error. Data is unavailable
    /// </returns>
    AL_ReadData_Result SendNAKRes();
    /// <summary>
    /// Send acknowledge.
    /// </summary>
    /// <returns>
    ///   ALRes_Success: Operation accomplished successfully 
    ///   ALRes_DatTransferErrr: Data transfer is imposible because of a communication error – loss of 
    ///      communication with a station
    ///   ALRes_DisInd: Disconnect indication – connection has been shut down remotely or lost because of 
    ///      communication error. Data is unavailable
    /// </returns>
    AL_ReadData_Result SendACKRes();
    /// <summary>
    /// Get empty, but prepared to be send as response frame, data buffer.
    /// </summary>
    /// <param name="block">Data block description to be read.</param>
    /// <param name="address">Address of the remote station connected to the common field bus.</param>
    /// <returns>Response to be sent.</returns>
    IResponseValue GetEmptySendDataBuffor( IBlockDescription block, int address );
    /// <summary>
    /// Send response.
    /// </summary>
    /// <param name="data">Response to be sent.</param>
    /// <returns>
    ///   ALRes_Success: Operation accomplished successfully 
    ///   ALRes_DatTransferErrr: Data transfer is imposible because of a communication error – loss of 
    ///      communication with a station
    ///   ALRes_DisInd: Disconnect indication – connection has been shut down remotely or lost because of 
    ///      communication error. Data is unavailable
    /// </returns>
    AL_ReadData_Result SendData( IResponseValue data );
  } //IApplicationLayerSlave
}


