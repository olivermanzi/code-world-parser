﻿using Newtonsoft.Json;
using System;
using System.Xml;
using System.IO;
using System.Diagnostics;

[Serializable]

class Program
{
    protected static void Main(string[] args)
    {
        /*Start benchmark*/
        Stopwatch benchmark = new Stopwatch();
        benchmark.Start();
        Console.WriteLine("started project: " + benchmark.Elapsed);

        XmlDocument srcML, gameObjects;

        /*Create an xml doc for existing file*/
        srcML = new XmlDocument();

        /*projects*/
        string[] projects = new string[5];
        projects[0] = "/../../../../../globalAssets/tests/official/k9.xml";
        projects[1] = "/../../../../../globalAssets/tests/official/bitcoin.xml";
        projects[2] = "/../../../../../globalAssets/tests/sample/reuxProject.xml";
        projects[3] = "/../../../../../globalAssets/tests/sample/omniProject.xml";
        projects[4] = "/../../../../../globalAssets/tests/sample/databaseProjectProject.xml";
        String inbox = "/../../../../../globalAssets/inbox/srcML.xml";

        /*set the project here*/
        String project = projects[0];

        /*load project(s)*/
        String srcMLPath = AppDomain.CurrentDomain.BaseDirectory + project;
        srcML.Load(srcMLPath);

        /*create a namspace for xpath querying*/
        XmlNamespaceManager namespaceManager = new XmlNamespaceManager(srcML.NameTable);
        namespaceManager.AddNamespace("src", "http://www.srcML.org/srcML/src");

        /*retrieve data*/
        parser.SrcMLReader reader = new parser.SrcMLReader(); // parser for srcMl
        XmlElement[] jClasses = reader.GetClasses(srcML, namespaceManager); // retrieves srcML data
        XmlElement classes = srcML.CreateElement("JavaProject");

        Console.WriteLine(project);
        Console.WriteLine("num of classes: "+jClasses.Length);

        foreach (XmlElement jClass in jClasses)
        {
            classes.AppendChild(jClass);
        }

        /*import xml info*/
        gameObjects = new XmlDocument();
        XmlNode data = gameObjects.CreateElement("JavaProject");
        data = ImportNode(classes, gameObjects);
        gameObjects.AppendChild(data);

        /*filter the xml in json format*/
        parser.JsonReader jsonReader = new parser.JsonReader(); // custom parser for xml
        String json = jsonReader.XmlToJson(gameObjects); // pops root node out of xml document

        /*place json into outbox*/
        ExportJson(json);

        /*Close Benchmark*/
        benchmark.Stop();
        Console.WriteLine("Benchmark: " + benchmark.Elapsed);
    }

    //----------------------------------------------------------------------------------------------------------------
    //
    //Methods 
    //
    //----------------------------------------------------------------------------------------------------------------

    public static XmlNode ImportNode(XmlNode node, XmlDocument destinationDoc)
    {
        //you cant really just reference a node from one doc to another so you need to import them first

        return destinationDoc.ImportNode(node, true);
    }

    public static String XmlToJson(XmlDocument xmlFile)
    {
        return JsonConvert.SerializeXmlNode(xmlFile);
    }

    public static void ExportJson(String jsonString)
    {
        Boolean success = false;
        try
        {
            String path = System.AppDomain.CurrentDomain.BaseDirectory + "/../../../../globalAssets/outbox/xml2json.json";
            File.WriteAllText(path, jsonString);
            success = true;
        }
        catch (Exception u)
        {
            Console.WriteLine("error");
            Console.WriteLine(u.ToString());
            success = false;
        }
        Console.WriteLine(Yay(success));
    }

    public static String Yay(Boolean reality)
    {
        String res;
        if (reality)
        {
            res = "────────────────────────────────────────\n────────────────────────────────────────\n───────────████──███────────────────────\n──────────█████─████────────────────────\n────────███───███───████──███───────────\n────────███───███───██████████──────────\n────────███─────███───████──██──────────\n─────────████───████───███──██──────────\n──────────███─────██────██──██──────────\n──────██████████────██──██──██──────────\n─────████████████───██──██──██──────────\n────███────────███──██──██──██──────────\n────███─████───███──██──██──██──────────\n───████─█████───██──██──██──██──────────\n───██████───██──────██──────██──────────\n─████████───██──────██─────███──────────\n─██────██───██─────────────███──────────\n─██─────███─██─────────────███──────────\n─████───██████─────────────███──────────\n───██───█████──────────────███──────────\n────███──███───────────────███──────────\n────███────────────────────███──────────\n────███───────────────────███───────────\n─────████────────────────███────────────\n──────███────────────────███────────────\n────────███─────────────███─────────────\n────────████────────────██──────────────\n──────────███───────────██──────────────\n──────────████████████████──────────────\n──────────████████████████──────────────\n────────────────────────────────────────\n────────────────────────────────────────";
        }
        else
        {
            res = "░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░\n░░░░░░░░░░░░░░░░▄▄███▄▄▄░▄▄██▄░░░░░░░\n░░░░░░░░░██▀███████████████▀▀▄█░░░░░░\n░░░░░░░░█▀▄▀▀▄██████████████▄░░█░░░░░\n░░░░░░░█▀▀░▄██████████████▄█▀░░▀▄░░░░\n░░░░░▄▀░░░▀▀▄████████████████▄░░░█░░░\n░░░░░▀░░░░▄███▀░░███▄████░████░░░░▀▄░\n░░░▄▀░░░░▄████░░▀▀░▀░░░░░░██░▀▄░░░░▀▄\n░▄▀░░░░░▄▀▀██▀░░░░░▄░░▀▄░░██░░░▀▄░░░░\n█░░░░░█▀░░░██▄░░░░░▀▀█▀░░░█░░░░░░█░░░\n█░░░▄▀░░░░░░██░░░░░▀██▀░░█▀▄░░░░░░▀▀▀\n▀▀▀▀░▄▄▄▄▄▄▀▀░█░░░░░░░░░▄█░░█▀▀▀▀▀█░░\n░░░░█░░░▀▀░░░░░░▀▄░░░▄▄██░░░█░░░░░▀▄░\n░░░░█░░░░░░░░░░░░█▄▀▀▀▀▀█░░░█░░░░░░█░\n░░░░▀░░░░░░░░░░░░░▀░░░░▀░░░░▀░░░░░░░░\n░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░";
        }
        return res;
    }
}