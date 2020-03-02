//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using System;
using System.Windows.Forms;

namespace CAS.CommServer.DA.ItemDescriberEditor
{
  internal static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      DoApplicationRun( Application.Run);
    }
    /// <summary>
    /// Does the application run.
    /// </summary>
    /// <remarks>Added for the purpose of unit testing</remarks>
    /// <param name="applicationRunAction">The application run action.</param>
    internal static void DoApplicationRun( Action<Form> applicationRunAction)
    {
      applicationRunAction(new MainFormItemDescriber());
    }
  }
}
