using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAS.CommServer.Scripts.UnitTest
{
  [TestClass]
  public class CreateTagPrototyping
  {
    [TestMethod]
    public void CreateTagTest()
    {
      string CASCommServerVersion = "2.01.1061";

      #region Copy to script
      string _rel = CASCommServerVersion.Replace(".", "_");
      string _repositoryUrl = @"svn://svnserver.hq.cas.com.pl/VS/";
      string _solutionPath = @"/CommServer";
      string _trunkPath = $@"{_repositoryUrl}trunk{_solutionPath}/";
      string _tagPath = $@"{_repositoryUrl}tags{_solutionPath}.rel_{_rel}";
      #endregion

      Assert.AreEqual("svn://svnserver.hq.cas.com.pl/VS/trunk/CommServer/", _trunkPath);
      Assert.AreEqual("svn://svnserver.hq.cas.com.pl/VS/tags/CommServer.rel_2_01_1061", _tagPath);
    }
  }

}
