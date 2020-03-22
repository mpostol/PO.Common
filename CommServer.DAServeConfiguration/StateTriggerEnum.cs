//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

namespace UAOOI.ProcessObserver.Configuration
{
  /// <summary>
  /// State Triggers Enum
  /// </summary>
  public enum StateTrigger : sbyte
  {
    /// <summary>
    /// None trigger
    /// </summary>
    None = 0,

    /// <summary>
    /// State high trigger
    /// </summary>
    StateHigh = 1,

    /// <summary>
    /// State low trigger
    /// </summary>
    StateLow = 2
  }
}