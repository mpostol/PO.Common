//<summary>
//  Title   : CAPI_PLCIPortManager
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    <Author> - <date>:
//    <description>
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.com.pl
//  http:\\www.cas.eu
//</summary>
namespace CAS.Lib.CommonBus.CommunicationLayer.CAPI.Session
{
  using System;
  /// <summary>
  /// Manager for PLCIPortMeneger class
  /// </summary>
  class CAPI_PLCIPortManager
  {
    #region PUBLIC
    /// <summary>
    /// Checks if there are any free ports and assignes a new one if possible
    /// </summary>
    /// <returns>Port object if any free found, otherwise null</returns>
    internal C_PLCIState NewPLCIPort()
    {
      lock ( this )
      {
        byte currPort = 0;
        while ( !PLCI_StatesArr[ currPort ].PortAcquire )
        {
          currPort++;
          if ( currPort == numOfPorts )
            return null;
        }
        PLCI_StatesArr[ currPort ].PLCI = currPort;
        return PLCI_StatesArr[ currPort ];
      }
    }
    /// <summary>
    /// Searches for port
    /// </summary>
    /// <param name="portNum">Number of port to retrieve</param>
    /// <returns>Found port object</returns>
    internal C_PLCIState FindPortNum( int portNum )
    {
      return PLCI_StatesArr[ portNum - Byte.MaxValue ];
    }
    /// <summary>
    /// Finds port with indicated PLCI
    /// </summary>
    /// <param name="plci">Physical Link Connection Information</param>
    /// <returns>Port object if any free found, otherwise null</returns>
    internal C_PLCIState FindPLCIPort( byte plci )
    {
      lock ( this )
      {
        int currPort = 0;
        while
          ( PLCI_StatesArr[ currPort ].IsDisconnected | !( PLCI_StatesArr[ currPort ].PLCI == plci ) )
        {
          currPort++;
          if ( currPort == numOfPorts )
            return null;
        }
        return PLCI_StatesArr[ currPort ];
      }
    }
    /// <summary>
    /// Finds port, which is waiting for connection
    /// </summary>
    /// <returns>Port object waiting for connection</returns>
    internal C_PLCIState FindWaitingFConnIndPort()
    {
      lock ( this )
      {
        int currPort = 0;
        while ( !PLCI_StatesArr[ currPort ].IsWaitingFConnIndication )
        {
          currPort++;
          if ( currPort == numOfPorts )
            return NewPLCIPort();
        }
        return PLCI_StatesArr[ currPort ];
      }
    }
    /// <summary>
    /// Clears and releases port
    /// </summary>
    /// <param name="port">Port object to close</param>
    internal void ClosePLCIPort( ref C_PLCIState port )
    {
      port.Release();
      port = null;
    }
    internal CAPI_PLCIPortManager()
    {
      for ( int i = 0; i < numOfPorts; i++ )
        PLCI_StatesArr[ i ] = new PLCIPortMeneger();
    }
    #endregion
    #region PRIVATE
    /// <summary>
    /// Class that represents the logical connection
    /// </summary>
    private class PLCIPortMeneger: C_PLCIState
    {
      /// <summary>
      /// Used acquire port if free
      /// </summary>
      internal bool PortAcquire
      {
        get
        {
          lock ( this )
          {
            if ( inUse ) { return false; }
            else
            {
              inUse = true;
              return true;
            }
          }
        }
      }
      /// <summary>
      /// Clears the port and sets the starting conditions
      /// </summary>
      internal override void Release()
      {
        lock ( this )
        {
          inUse = false;
          base.Release();

        }
      }
      private bool inUse = false;
    }
    private const int numOfPorts = 3;
    private PLCIPortMeneger[] PLCI_StatesArr = new PLCIPortMeneger[ numOfPorts ];
    #endregion
  }
}