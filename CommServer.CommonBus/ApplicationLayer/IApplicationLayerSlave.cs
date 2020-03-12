//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

namespace CAS.Lib.CommonBus.ApplicationLayer
{

  /// <summary>
  /// Interface which must be implemented  by any data provider playing the role on the field network of the slave 
  /// station, i.e. responding to the master station. It is responsible for processing commands received from a master station.
  /// </summary>
  public interface IApplicationLayerSlave : IConnectionManagement
  {
    /// <summary>
    /// Read command from a master station.
    /// </summary>
    /// <param name="frame">Received frame</param>
    /// <returns>
    ///   ALRes_Success: Operation accomplished successfully 
    ///   ALRes_DatTransferErrr: Data transfer is impossible because of a communication error – loss of 
    ///      communication with a station
    ///   ALRes_DisInd: Disconnect indication – connection has been shut down remotely or lost because of 
    ///      communication error. Data is unavailable
    /// </returns>
    AL_ReadData_Result ReadCMD(out IReadCMDValue frame);
    /// <summary>
    /// Send negative acknowledge.
    /// </summary>
    /// <returns>
    ///   ALRes_Success: Operation accomplished successfully 
    ///   ALRes_DatTransferErrr: Data transfer is impossible because of a communication error – loss of 
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
    ///   ALRes_DatTransferErrr: Data transfer is impossible because of a communication error – loss of 
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
    IResponseValue GetEmptySendDataBuffor(IBlockDescription block, int address);
    /// <summary>
    /// Send response.
    /// </summary>
    /// <param name="data">Response to be sent.</param>
    /// <returns>
    ///   ALRes_Success: Operation accomplished successfully 
    ///   ALRes_DatTransferErrr: Data transfer is impossible because of a communication error – loss of 
    ///      communication with a station
    ///   ALRes_DisInd: Disconnect indication – connection has been shut down remotely or lost because of 
    ///      communication error. Data is unavailable
    /// </returns>
    AL_ReadData_Result SendData(IResponseValue data);

  } //IApplicationLayerSlave
}


