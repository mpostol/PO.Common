//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.Lib.CodeProtect;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using UAOOI.ProcessObserver.RealTime.Processes;

namespace UAOOI.ProcessObserver.Configuration.ItemDescriber
{
  /// <summary>
  /// Opens and reads configuration from XML file.
  /// </summary>
  public class XMLManagement
  {
    private readonly string itemdscHasNotBeenSet = "The location of the item_dsc.xml file has not been set in the config file";
    private readonly string itemdscDoesNotExists = "The item description file {0} does not exist";
    private readonly string itemdscCannotBeOpened = "Item_dsc.xml file cannot be opened";

    /// <summary>
    /// reading of configuration XML file
    /// </summary>
    /// <param name="myData">target data set</param>
    /// <param name="filename">filename</param>
    public void readXMLFile(DataSet myData, string filename)
    {
      if (string.IsNullOrEmpty(filename))
      {
        EventLogMonitor.WriteToEventLogInfo(itemdscHasNotBeenSet, 39);
        return;
      }
      else if (filename == "item_dsc.xml")
      {
        FileInfo fi = RelativeFilePathsCalculator.GetAbsolutePathToFileInApplicationDataFolder(filename);
        string itemdscPath = fi.FullName;
        if (!new FileInfo(itemdscPath).Exists)
        {
          EventLogMonitor.WriteToEventLog(itemdscDoesNotExists, EventLogEntryType.Warning);
          return;
        }
        else
          filename = itemdscPath;
      }
      else if (!new FileInfo(filename).Exists)
      {
        EventLogMonitor.WriteToEventLog(string.Format(itemdscDoesNotExists, filename), EventLogEntryType.Warning);
        return;
      }
      myData.Clear();
      try
      {
        myData.ReadXml(filename, XmlReadMode.IgnoreSchema);
      }
      catch (Exception)
      {
        EventLogMonitor.WriteToEventLog(itemdscCannotBeOpened, EventLogEntryType.Warning);
      }
    }
    /// <summary>
    /// writing of configuration data set
    /// </summary>
    /// <param name="myData">dataset to be written</param>
    /// <param name="filename">target filename</param>
    public void writeXMLFile(DataSet myData, string filename)
    {
      FileStream _FileStream = new FileStream(filename, FileMode.Create);
      //Create an XmlTextWriter with the fileStream.
      XmlTextWriter _XmlWriter = new XmlTextWriter(_FileStream, Encoding.Unicode)
      {
        Formatting = Formatting.Indented
      };
      myData.WriteXml(_XmlWriter);
      _XmlWriter.Close();
    }
  }
}
