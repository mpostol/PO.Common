//<summary>
//  Title   : address space in data provider identifying interface – provides general information about Address Space.
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//   20081003: mzbrzezny: created
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;

namespace CAS.Lib.CommonBus
{
  /// <summary>
  /// Address space in data provider identifying interface – provides general information about Address Space.
  /// </summary>
  public interface IAddressSpaceDescriptor
  {
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>The name.</value>
    string Name { get; }
    /// <summary>
    /// Gets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    short Identifier { get; }
    /// <summary>
    /// Gets the start address.
    /// </summary>
    /// <value>The start address.</value>
    long StartAddress { get; }
    /// <summary>
    /// Gets the end address.
    /// </summary>
    /// <value>The end address.</value>
    long EndAddress { get; }
  }
  /// <summary>
  /// Class that represets a simple couple of start and edn address
  /// </summary>
  public struct StartEndPair
  {
    long startaddress;
    long endaddress;
    /// <summary>
    /// Gets or sets the start address.
    /// </summary>
    /// <value>The start address.</value>
    public long StartAddress
    {
      get { return startaddress; }
      set { startaddress = value; }
    }
    /// <summary>
    /// Gets or sets the end address.
    /// </summary>
    /// <value>The end address.</value>
    public long EndAddress
    {
      get { return endaddress; }
      set { endaddress = value; }
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="StartEndPair"/> struct.
    /// </summary>
    /// <param name="_StartAddress">The start address.</param>
    /// <param name="_EndAddress">The end address.</param>
    public StartEndPair( long _StartAddress, long _EndAddress )
    {
      startaddress = _StartAddress;
      endaddress = _EndAddress;
    }
  }
  /// <summary>
  /// Default AddressSpace descriptor class, it can be used by dataproviders or dataprovider can implement own descriptor class
  /// </summary>
  public class AddressSpaceDescriptor: IAddressSpaceDescriptor
  {
    #region private
    string name;
    short identifier;
    StartEndPair startendpair;
    #endregion private
    #region IAddressSpaceDescriptor Members
    string IAddressSpaceDescriptor.Name
    {
      get { return name; }
    }
    short IAddressSpaceDescriptor.Identifier
    {
      get { return identifier; }
    }
    long IAddressSpaceDescriptor.StartAddress
    {
      get { return startendpair.StartAddress; }
    }
    long IAddressSpaceDescriptor.EndAddress
    {
      get { return startendpair.EndAddress; }
    }
    #endregion
    /// <summary>
    /// Initializes a new instance of the <see cref="AddressSpaceDescriptor"/> class.
    /// </summary>
    /// <param name="_Name">Name of the address space.</param>
    /// <param name="_Identifier">The identifier.</param>
    /// <param name="_StartAddress">The start address.</param>
    /// <param name="_EndAddress">The end address.</param>
    public AddressSpaceDescriptor( string _Name, short _Identifier, long _StartAddress, long _EndAddress )
    {
      name = _Name;
      identifier = _Identifier;
      startendpair = new StartEndPair( _StartAddress, _EndAddress );
    }
    /// <summary>
    /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
    /// </returns>
    public override string ToString()
    {
      return String.Format( "name: {0}, ID: {1}, (Start,End)=({2},{3})",
        name, identifier, startendpair.StartAddress, startendpair.EndAddress );
    }
  }
}
