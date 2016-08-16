//<summary>
//  Title   : COMMUNICATIONS LIBRARY - Protocols Application Layer Interface
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MZbrzezny 2007-01-31
//    - usuwanie mechanizmu bazujacego na porcie 
//    w application layer i communication layer
//    - dodano ApplicationLayerResults
//
//    MPostol - 13-06-2003:
//    Description: Created (start point (SB_SBAPP.DEF )
//    MPostol - 06-10-2003:
//      RWOperationCouters was added to interface
//    MPostol - 03-11-2003:
//      DgReadCounters was added to interface - interface management needs to be cleaned up.
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.u
//  http://www.cas.eu
//</summary>

namespace CAS.Lib.CommonBus.ApplicationLayer
{
  /// <summary>
  /// Result of the data read operation returned by ReadData
  /// </summary>
  public enum AL_ReadData_Result
  {
    /// <summary>
    /// Operation accomplished successfully 
    /// </summary>
    ALRes_Success,
    /// <summary>
    /// Data transfer is imposible because of a communication error – loss of communication with a station
    /// </summary>
    ALRes_DatTransferErrr,
    /// <summary>
    /// Disconnect indication – connection has been shut down remotely or lost because of communication error.
    /// Data is unavailable
    /// </summary>
    ALRes_DisInd
  }
  /// <summary>
  /// Interface which must be implemented  by any data provider playing the role on the field network of the master station,
  /// i.e. responsible for pooling all remote station to acquire date from them.
  /// </summary>
  public interface IApplicationLayerMaster: IConnectionManagement
  {
    /// <summary>
    /// Reads process data from the selected location and device resources.
    /// </summary>
    /// <param name="pBlock"><see cref="IBlockDescription"/> selecting the resource containing the data block to be read.</param>
    /// <param name="pStation">Address of the remote station connected to the common field bus. –1 if not applicable.</param>
    /// <param name="pData">The buffer <see cref="IReadValue"/> containing the requested data.</param>
    /// <param name="pRetries">Number of retries to get data.</param>
    /// <returns>Result of the operation</returns>
    AL_ReadData_Result ReadData( IBlockDescription pBlock, int pStation, out IReadValue pData, byte pRetries );
    /// <summary>
    /// Gets a buffer from a pool and initiates. After filling it up with the data can be send to the data provider remote
    /// unit by the <see cref="WriteData"/>.
    /// </summary>
    /// <param name="block"><see cref="IBlockDescription"/> selecting the resource where the data block is to be written.</param>
    /// <param name="station">Address of the remote station connected to the common field bus. –1 if not applicable.</param>
    /// <returns>
    /// A buffer <see cref="IWriteValue"/> ready to be filled up with the data and written down by the <see cref="WriteData"/>
    /// to the destination – remote station.
    /// </returns>
    IWriteValue GetEmptyWriteDataBuffor( IBlockDescription block, int station );
    /// <summary>
    /// Writes process data down to the selected location and device resources.
    /// </summary>
    /// <param name="data">Data to be send. Always null after return. Data buffer must be returned to the pool.</param>
    /// <param name="retries">Number of retries to wrtie data.</param>
    /// <returns><see cref="AL_ReadData_Result"/> result of the operation.</returns>
    AL_ReadData_Result WriteData( ref IWriteValue data, byte retries );
  } //IApplicationLayerMaster
}

