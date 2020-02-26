﻿//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAS.DataPorter.Configurator;

namespace CAS.CommServer.DAClientConfiguration.UnitTests
{
  [TestClass]
  public class DataPorterConfigLibUnitTest
  {
    [TestMethod]
    public void ConfigurationManagementCreatorMethod1()
    {
      bool _disposedRaised = false;
      using (ConfigurationManagement _newConfiguration = new ConfigurationManagement())
      {
        _newConfiguration.Disposed += (x, y) => _disposedRaised = true; 
        Assert.IsNotNull(_newConfiguration.Configuartion);
        Assert.AreEqual<string>("OPCViewerSession", _newConfiguration.DefaultFileName);
        Assert.IsNotNull(_newConfiguration.Menu);
        Assert.IsNotNull(_newConfiguration.OpenFileDialog);
        Assert.IsFalse(_disposedRaised);
      }
      Assert.IsTrue(_disposedRaised);
    }
  }
}
