//<summary>
//  Title   : TraceEvent in CAS.Lib.CommonBus
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    20080505: mzbrzezny: created
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>


namespace CAS.Lib.CommonBus
{
  /// <summary>
  /// class responsible for tracing inside BaseStation - please use TraceSource( "CAS.Lib.CommonBus.TraceEvent" )
  /// </summary>
  public class TraceEvent
  {
    private static CAS.Lib.RTLib.Processes.TraceEvent m_traceevent_internal =
      new CAS.Lib.RTLib.Processes.TraceEvent( typeof( TraceEvent ).ToString() );
    /// <summary>
    /// Gets the tracer.
    /// </summary>
    /// <value>The tracer.</value>
    public static CAS.Lib.RTLib.Processes.TraceEvent Tracer
    {
      get
      {
        return m_traceevent_internal;
      }
    }
  }
}
