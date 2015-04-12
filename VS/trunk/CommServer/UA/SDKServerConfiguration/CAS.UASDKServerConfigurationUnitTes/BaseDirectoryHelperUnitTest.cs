using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CAS.UA.SDK.ServerConfigurationBase.UnitTes
{
  [TestClass]
  public class Common
  {

    [TestMethod]
    [TestCategory("CAS.UA.SDK")]
    public void BaseDirectoryHelperTestMethod()
    {
      BaseDirectoryHelper _bd = BaseDirectoryHelper.Instance;
      Assert.IsNotNull(_bd);
      Assert.IsTrue(String.IsNullOrEmpty(_bd.GetBaseDirectory()));
      _bd.SetBaseDirectoryProvider(new BaseDirectoryProvider());
      Assert.AreEqual<string>("BaseDirectoryProvider", _bd.GetBaseDirectory());
    }
  }
  //private definitions.
  internal class BaseDirectoryProvider : IBaseDirectoryProvider
  {
    public string GetBaseDirectory()
    {
      return typeof(BaseDirectoryProvider).Name;
    }
  }
}
