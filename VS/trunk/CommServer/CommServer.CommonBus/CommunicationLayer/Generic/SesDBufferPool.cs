//<summary>
//  Title   : Data buffor pool engine generic version
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MPostol: 02-04-2007: created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Lib.RTLib.Processes;

namespace CAS.Lib.CommonBus.CommunicationLayer.Generic
{
  /// <summary>
  /// Interface allowing to return data buffer to the pool.
  /// </summary>
  public interface IBufferLink
  {
    /// <summary>
    /// Return the data buffer to the common pool. 
    /// </summary>
    /// <param name="mess">Buffer to be returneed. After returning is assigned null.
    /// </param>
    void ReturnEmptyISesDBuffer( ref ISesDBuffer mess );
  }
  /// <summary>
  /// Data buffor pool engine
  /// </summary>
  /// <typeparam name="TSesDBuffer">Type managed by the buffer.</typeparam>
  public abstract class SesDBufferPool<TSesDBuffer>: IBufferLink where TSesDBuffer: ISesDBuffer
  {
    #region PRIVATE
    private EnvelopePool myPool;
    private IEnvelope NewSesDBuffer( EnvelopePool source )
    {
      TSesDBuffer newBuff = CreateISesDBuffer();
      return newBuff;
    }
    #endregion
    #region PUBLIC
    /// <summary>
    /// Abstract method used to create new envelope. New envelope is created each time 
    /// GetEmptyEnvelope is called while the pool is emty.
    /// </summary>
    protected abstract TSesDBuffer CreateISesDBuffer();
    /// <summary>
    /// It gets an empty envelope from the common pool, or if empty creates ones.
    /// </summary>
    public TSesDBuffer GetEmptyISesDBuffer()
    {
      return (TSesDBuffer)myPool.GetEmptyEnvelope();
    }
    /// <summary>
    /// Returns an empty envelope to the common pool.
    /// </summary>
    /// <param name="mess">Envelope to return</param>
    public void ReturnEmptyISesDBuffer( ref ISesDBuffer mess )
    {
      IEnvelope ieMess = mess;
      mess = null;
      myPool.ReturnEmptyEnvelope( ref ieMess );
    }
    /// <summary>
    /// Creator of the SesDBufferPool
    /// </summary>
    public SesDBufferPool()
    {
      myPool = new EnvelopePool( new EnvelopePool.CreateEnvelope( this.NewSesDBuffer ) );
    }
    #endregion
  }
}

