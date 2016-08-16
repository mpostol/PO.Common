//<summary>
//  Title   : Data buffor pool engine
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MPostol = 30-09-03 created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Lib.RTLib.Processes;

namespace CAS.Lib.CommonBus.CommunicationLayer
{
  /// <summary>
  /// Interface used by communication library to get access to serialized data transmited over a physical medium.
  /// </summary>
  public interface ISesDBuffer: IDBuffer, IEnvelope{}
}