//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.Lib.CodeProtect;
using System.Data;
using System.IO;

namespace UAOOI.ProcessObserver.Configuration
{
  /// <summary>
  /// XML to DataSet IO helper (read file, write file)
  /// </summary>
  public static class XML2DataSetIO
  {
    /// <summary>
    /// Reads the XML file.
    /// File is opened Read/Write, file share is Read
    /// </summary>
    /// <param name="myData">My data.</param>
    /// <param name="filename">The filename.</param>
    public static void readXMLFile(DataSet myData, string filename)
    {
      readXMLFile(myData, filename, false);
    }

    /// <summary>
    /// Reads the XML file.
    /// </summary>
    /// <param name="myData">My data.</param>
    /// <param name="filename">The filename.</param>
    /// <param name="open_readonly">if set to <c>true</c> file is opened as read-only.</param>
    public static void readXMLFile(DataSet myData, string filename, bool open_readonly)
    {
      //Processes.EventLogMonitor.WriteToEventLogInfo( "Reading: " + filename + " for CommServer configuration" );
      System.IO.FileStream myFileStream;
      FileInfo fi = new FileInfo(filename);
      if (!fi.Exists)
        fi = RelativeFilePathsCalculator.GetAbsolutePathToFileInApplicationDataFolder(filename);
      if (open_readonly)
        myFileStream = new System.IO.FileStream(fi.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
      else
        myFileStream = new System.IO.FileStream(fi.FullName, System.IO.FileMode.Open);
      System.Xml.XmlTextReader myXmlReader;
      //Create an XmlTextReader with the fileStream.
      myXmlReader = new System.Xml.XmlTextReader(myFileStream);
      myData.ReadXml(myXmlReader);
      myXmlReader.Close();
    }

    /// <summary>
    /// Writes the XML file.
    /// </summary>
    /// <param name="myData">My data to be written .</param>
    /// <param name="filename">The filename.</param>
    public static void writeXMLFile(DataSet myData, string filename)
    {
      System.IO.FileStream myFileStream = new System.IO.FileStream(filename, System.IO.FileMode.Create);
      System.Xml.XmlTextWriter myXmlWriter;
      //Create an XmlTextWriter with the fileStream.
      myXmlWriter = new System.Xml.XmlTextWriter(myFileStream, System.Text.Encoding.Unicode)
      {
        Formatting = System.Xml.Formatting.Indented
      };
      myData.WriteXml(myXmlWriter);
      myXmlWriter.Close();
    }
  }
}