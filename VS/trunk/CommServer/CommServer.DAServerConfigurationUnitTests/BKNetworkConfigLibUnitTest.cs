﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAS.NetworkConfigLib;

namespace CAS.CommServer.DAServerConfiguration.UnitTests
{
  [TestClass]
  public class BKNetworkConfigLibUnitTest
  {
    [TestMethod]
    public void CommServerConfigurationMainCreatorTest()
    {
      using (CommServerConfigurationMain _new = new CommServerConfigurationMain())
      {
        using (ComunicationNet _defaultCommunicationNet = _new.Configuartion)
        {
          Assert.IsNotNull(_defaultCommunicationNet);
          Assert.IsNotNull(_defaultCommunicationNet.Channels);
          Assert.IsNotNull(_defaultCommunicationNet.DataBlocks);
          Assert.IsNotNull(_defaultCommunicationNet.Groups);
          Assert.IsNotNull(_defaultCommunicationNet.Interfaces);
          Assert.IsNotNull(_defaultCommunicationNet.ItemPropertiesTable);
          Assert.IsNotNull(_defaultCommunicationNet.Protocol);
          Assert.IsNotNull(_defaultCommunicationNet.Relations);
          Assert.IsNotNull(_defaultCommunicationNet.Relations);
          Assert.IsNotNull(_defaultCommunicationNet.Segments);
          Assert.IsNotNull(_defaultCommunicationNet.SerialSetings);
          Assert.IsNotNull(_defaultCommunicationNet.Station);
          Assert.IsNotNull(_defaultCommunicationNet.TagBit);
          Assert.IsNotNull(_defaultCommunicationNet.Tags);
        }
        Assert.IsNull(_new.Container);
        Assert.AreEqual<string>(@"CommServerConfiguration", _new.DefaultFileName);
        Assert.IsNotNull(_new.Menu);
        Assert.IsNotNull(_new.OpenFileDialog);
      }
    }
  }
}
