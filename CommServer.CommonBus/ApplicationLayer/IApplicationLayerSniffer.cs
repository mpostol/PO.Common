//<summary>
//  Title   : COMMUNICATIONS LIBRARY - Protocols Sniffer Application layer interface
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MPostol 2007-01-12:
//      Moved to a separate file.
//    MZbrzezny - 06-07-2005:
//      Description: Created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace CAS.Lib.CommonBus.ApplicationLayer
{
  /// <summary>
  /// Interface which must be implemented  by any data provider playing the role on the field network of the passive 
  /// reader station, i.e. responsible for reading master station commands and slave station responses and gathering the data from coupled pairs.
  /// </summary>
  public interface IApplicationLayerSniffer: IConnectionManagement
  {
    /// <summary>
    /// Interface of the passive reader station, i.e. responsible for reading master station commands and slave station responses 
    /// and gathering the data from coupled pairs.
    /// </summary>
    /// <param name="pBlock">Data block description.</param>
    /// <param name="pData">Message containing obtained data.</param>
    /// <returns>
    ///   ALRes_Success: 
    ///     Operation accomplished successfully 
    ///   ALRes_DatTransferErrr: Data transfer is imposible because of a communication error – loss of 
    ///      communication with a station
    ///   ALRes_DisInd: Disconnect indication – connection has been shut down remotely or lost because of 
    ///      communication error. Data is unavailable
    /// </returns>
    AL_ReadData_Result ReadData( out IBlockDescription pBlock, out IReadValue pData );
  } //IApplicationLayerSniffer
}

