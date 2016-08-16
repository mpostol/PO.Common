//<summary>
//  Title   : Base class for all x_to_Serial classes providing common functionality, e.g. tracing.
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MPostol 15-04-2007: created.
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Diagnostics;

namespace CAS.Lib.CommonBus.CommunicationLayer
{
  using CommonBus;
  internal class Medium_to_Serial: IDisposable
  {
    #region private
    private static uint m_InstanceNum = 0;
    private string m_traceName;
    private TraceSource m_TraceSource;
    private TraceException m_LastException = null;
    private class TraceException: Exception
    {
      internal readonly TraceEventType type;
      internal readonly int id;
      internal readonly string message;
      internal readonly Exception ex;
      internal TraceException( string m, TraceEventType t, int i, Exception e )
      {
        message = m;
        type = t;
        id = i;
        ex = e;
      }
    }
    private LayerState m_CurrLState = LayerState.Disconnected;
    #endregion
    #region protected
    protected const TraceEventType TraceFlow = TraceEventType.Verbose;
    protected const TraceEventType TraceCommError = TraceEventType.Information;
    protected const TraceEventType TraceProgError = TraceEventType.Error;
    /// <summary>
    /// Mark underling layer exception
    /// </summary>
    /// <param name="m">The trace message to write.</param>
    /// <param name="t">One of the <see cref="System.Diagnostics.TraceEventType"/> values that specifies 
    /// the event type of the trace data.</param>
    /// <param name="i">A numeric identifier for the event.</param>
    /// <param name="e">Exception couch by the underling layer </param>
    protected void MarkException( string m, TraceEventType t, int i, Exception e )
    {
      LastLayerError = new TraceException( m, t, i, e );
    }
    /// <summary>
    /// Writes a trace event message to the trace listeners in the System.Diagnostics.TraceSource.Listeners collection 
    /// using the specified event type and event identifier.
    /// </summary>
    /// <param name="pEventType">
    /// One of the System.Diagnostics.TraceEventType values that specifies the event type of the trace data.
    /// </param>
    /// <param name="pId">A numeric identifier for the event.</param>
    /// <param name="pMessage">The trace message to write.</param>
    [Conditional( "TRACE" )]
    protected void TraceEvent( TraceEventType pEventType, int pId, string pMessage )
    {
      m_TraceSource.TraceEvent( pEventType, pId, m_traceName + pMessage );
    }
    /// <summary>
    ///Writes a trace event to the trace listeners in the System.Diagnostics.TraceSource.Listeners 
    ///collection using the specified event type, event identifier, and argument array and format.
    /// </summary>
    /// <param name="pEventType">One of the System.Diagnostics.TraceEventType values that specifies the event
    /// type of the trace data.</param>
    /// <param name="pId">A numeric identifier for the event.</param>
    /// <param name="pFormat">A format string that contains zero or more format items, which correspond
    /// to objects in the args array.</param>
    /// <param name="pArgs">An object array containing zero or more objects to format.</param>
    [Conditional( "TRACE" )]
    protected void TraceEvent( TraceEventType pEventType, int pId, string pFormat, params object[] pArgs )
    {
      m_TraceSource.TraceEvent( pEventType, pId, m_traceName + pFormat, pArgs );
    }
    public enum LayerState { Connected, Disconnected, Fail };
    private readonly object CurrentLayerStateLocker = new object();
    /// <summary>
    /// Get the current state of the layer.
    /// </summary>
    public LayerState CurrentLayerState
    {
      get
      {
        lock ( CurrentLayerStateLocker )
        {
          return m_CurrLState;
        }
      }
      protected set
      {
        lock ( CurrentLayerStateLocker )
        {
          TraceEventType ct = TraceEventType.Verbose;
          if ( ( value != m_CurrLState ) && ( ( value == LayerState.Fail ) || ( m_CurrLState == LayerState.Fail ) ) )
            ct = TraceEventType.Information;
          m_CurrLState = value;
          TraceEvent( ct, 618, "CurrentLayerState: Layer state has been changed :" + value.ToString() );
          if ( StatisticChanged != null )
            StatisticChanged();
        }
      }
    }
    /// <summary>
    /// Gets the last exception caught in the layer.
    /// </summary>
    public Exception LastLayerError
    {
      get { return m_LastException.ex; }
      private set
      {
        Debug.Assert( value != null );
        TraceException lx = (TraceException)value;
        m_LastException = lx;
        TraceEvent( lx.type, lx.id, lx.message );
        CurrentLayerState = LayerState.Fail;
      }
    }
    public delegate void StatisticHandler();
    /// <summary>
    /// Is activated after changing any statistic value in the layer;
    /// </summary>
    public event StatisticHandler StatisticChanged;
    #endregion
    #region IDisposable Members
    /// <summary>
    /// Track whether Dispose has been called.
    /// </summary>
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
      if ( this.disposed )
        return;
      if ( disposing ) { }
      // Release unmanaged resources. Set large fields to null.
      disposed = true;
    }
    public void Dispose()
    {
      Dispose( true );
      // Take yourself off the Finalization queue to prevent finalization code for this object from executing a second time.
      //GC.SuppressFinalize( this );
    }
    #endregion
    #region creator
    public Medium_to_Serial( string pTraceName, CommonBusControl pParent )
    {
      m_traceName = pTraceName + ": " + ( (uint)m_InstanceNum++ ).ToString();
      m_TraceSource = pParent.GetTraceSource;
    }
    public Medium_to_Serial( object[] param )
      : this( (string)param[ 0 ], (CommonBusControl)param[ 1 ] )
    {
    }
    #endregion
  }
}
