using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;       // DateTimeを使うために必要
using System.IO;    // CSV保存をするために必要

public class FileOutput : MonoBehaviour
{
  void Start()
  {
    // sampleDataを作って、CSVSaveの関数に引数として渡す
    // var sampleData = "SampleText";
    // CSVSave(sampleData, "sampleFile");
  }

  // CSV形式で保存するための関数
  public void CSVSave(string data, string fileName)
  {
    StreamWriter sw;
    FileInfo fi;
    DateTime now = DateTime.Now;

    fileName = fileName + "_" + now.Year.ToString() + "_" + now.Month.ToString() + "_" + now.Day.ToString() + "__" + now.Hour.ToString() + "_" + now.Minute.ToString() + "_" + now.Second.ToString();
    fi = new FileInfo(Application.persistentDataPath + "/" + fileName + ".csv");
    sw = fi.AppendText();
    sw.WriteLine(data);
    sw.Flush();
    sw.Close();
    Debug.Log("Save Completed");
  }
}