//<summary>
//  Title   : Base implementation of the IConnectionManagement 
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MPOstol 03-04-2007: created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using CAS.Lib.CommonBus.CommunicationLayer;

namespace CAS.Lib.CommonBus.ApplicationLayer
{
  /// <summary>
  /// Base implementation of IConnectionManagement interface.
  /// </summary>
  public abstract class ApplicationLayerCommon: IConnectionManagement
  {
    #region private
    private ICommunicationLayer m_CommLayer;
    #endregion
    #region IApplicationLayerManagement Members
    /// <summary>
    /// Connect Request, this fuction is used for establishing the connection
    /// </summary>
    /// <param name="remoteAddress">address of the remote unit</param>
    /// <returns>
    /// Result of the operation
    /// </returns>
    TConnectReqRes IConnectionManagement.ConnectReq( IAddress remoteAddress )
    {
      return m_CommLayer.ConnectReq( remoteAddress );
    }
    /// <summary>
    /// Disconnect indication – Check if there is a connection accepted to the remote address. 
    /// The result is returned immediately – it will not block;
    /// </summary>
    /// <param name="pRemoteAddress">
    /// The remote address we are waiting for connection from. Null if we are waiting for any connection.
    /// </param>
    /// <param name="pTimeOutInMilliseconds">
    /// How long the client is willing to wait for an incoming connection in ms.
    /// </param>
    /// <returns>
    /// Result of the operation
    /// </returns>
    TConnIndRes IConnectionManagement.ConnectInd( IAddress pRemoteAddress, int pTimeOutInMilliseconds )
    {
      return m_CommLayer.ConnectInd( pRemoteAddress, pTimeOutInMilliseconds );
    }
    /// <summary>
    /// Disconnect Request - Unconditionally disconnect the connection if any.
    /// </summary>
    void IConnectionManagement.DisReq()
    {
      m_CommLayer.DisReq();
    }
    /// <summary>
    /// true if the layer is connected for connection oriented communication or ready for communication 
    /// for connectionless communication.
    /// </summary>
    bool IConnectionManagement.Connected { get { return m_CommLayer.Connected; } }
    #endregion
    #region IDisposable Members
    private bool disposed = false;
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <remarks>
    /// Dispose(bool disposing) executes in two distinct scenarios. If disposing equals true, the method has been called directly
    /// or indirectly by a user's code. Managed and unmanaged resources can be disposed. If disposing equals false, 
    /// the method has been called by the runtime from inside the finalizer and you should not reference other objects. 
    /// Only unmanaged resources can be disposed.
    /// </remarks>
    /// <param name="disposing">
    /// If disposing equals true, the method has been called directly or indirectly by a user's code.
    /// </param>
    protected virtual void Dispose( bool disposing )
    {
      // Check to see if Dispose has already been called.
      if ( this.disposed )
        return;
      if ( disposing )
      {
        // If disposing equals true, dispose all managed resources.
      }
      // Release unmanaged resources. If disposing is false, only the following code is executed.
      ( (IDisposable)m_CommLayer ).Dispose();
      // Note that this is not thread safe.Another thread could start disposing the object
      // after the managed resources are disposed,
      // but before the disposed flag is set to true.
      // If thread safety is necessary, it must be
      // implemented by the client.
      disposed = true;
    }
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. 
    /// </summary>
    void IDisposable.Dispose()
    {
      Dispose( true );
      // Take yourself off the Finalization queue to prevent finalization code for this object from executing a second time.
      GC.SuppressFinalize( this );
    }
    /// <summary>
    /// Calling Dispose(false) is optimal in terms of readability and maintainability.
    /// </summary>
    ~ApplicationLayerCommon() { Dispose( false ); }
    #endregion
    #region creator
    /// <summary>
    /// ApplicationLayerCommon creator
    /// </summary>
    /// <param name="commChannel">
    /// Communication layer to be used to transfer data to/from data provider
    /// </param>
    /// <exception cref="ApplicationException">If commChannel is null.</exception>
    public ApplicationLayerCommon( ICommunicationLayer commChannel )
    {
      if ( commChannel == null)
        throw new ArgumentNullException("Communication layer cannot be null");
      m_CommLayer = commChannel;
    }
    #endregion
  }
}
