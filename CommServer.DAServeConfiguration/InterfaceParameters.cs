//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using System;
using static UAOOI.ProcessObserver.Configuration.ComunicationNet;

namespace UAOOI.ProcessObserver.Configuration
{
  /// <summary>
  /// Parameters of the interface
  /// </summary>
  public struct InterfaceParameters
  {
    /// <summary>
    /// The inactivity time - after this time the segment will be disconnected
    /// </summary>
    public readonly TimeSpan InactivityTime;

    /// <summary>
    /// The inactivity after failure time.
    /// </summary>
    public readonly TimeSpan InactivityAfterFailureTime;

    /// <summary>
    /// The name
    /// </summary>
    public string Name;

    /// <summary>
    /// The address - address represented by <see cref="ushort"/> if applicable
    /// </summary>
    public ushort Address;

    /// <summary>
    /// The interface number
    /// </summary>
    public byte InterfaceNumber;

    /// <summary>
    /// Gets the interface number max value.
    /// </summary>
    /// <value>The interface number max value.</value>
    public static byte InterfaceNumberMaxValue => 1;

    /// <summary>
    /// Initializes a new instance of the <see cref="InterfaceParameters"/> structure.
    /// </summary>
    /// <param name="interfacesRow">The interfaces row.</param>
    public InterfaceParameters(InterfacesRow interfacesRow)
    {
      InactivityTime = TimeSpan.FromMilliseconds(interfacesRow.InactTime);
      InactivityAfterFailureTime = TimeSpan.FromMilliseconds(interfacesRow.InactTimeAFailure);
      Name = interfacesRow.Name;
      Address = (ushort)Math.Min(interfacesRow.Address, ushort.MaxValue);
      InterfaceNumber = (byte)Math.Min(interfacesRow.InterfaceNum, byte.MaxValue);
      if (InterfaceNumber > InterfaceNumberMaxValue)
        throw new ArgumentOutOfRangeException("InterfaceNumber > InterfaceNumberMaxValue");
    }
  }
}