﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using System.Reflection;
namespace GitGetter2
{
    class GitHubGetter
    {

        private static readonly HttpClient client = Program.getClient();


        public static Boolean getMainTree(String projectId)
        {
            String address = "https://api.github.com/repos/" + projectId + "/contents";
            //String address = "https://api.github.com/repos/GokuMohandas/practicalAI/contents"; // Testing address
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            Console.WriteLine(address);

            // The part where the request is actually made
            try
            {
                Console.WriteLine("Sending request. V2");
                client.DefaultRequestHeaders.Add("User-Agent", "C# App");

                HttpResponseMessage response = client.GetAsync(address).GetAwaiter().GetResult() ;
                
                Console.WriteLine("response has not been made");
                String responseString;
                if (response!=null) { responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult(); }
                else { return false; }

                Console.WriteLine("ResponseString has been made");

                List<HubObject> noPathFolder = makeHubList(responseString);

                //Makes a directory for this project
                String dirPath = Program.mainFolderGetter() + "/Middleware/Gitgetter/FileStorer/" + projectId;

                System.IO.Directory.CreateDirectory(dirPath);
                File.WriteAllText (dirPath + Program.getFiletype(), "");

                foreach (HubObject tree in noPathFolder)
                {
                    FolderNavigator(projectId, tree);
                }
                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong with HTTP request :Defaultdance");
                Console.WriteLine(e);
                //Write stuff incase the git repository was not found. 
                return false;
            }



        }

        private static Boolean getDirTree(String url, String projectId)
        {
            String address = url; // It is possible that I'll have to encode it or parts of it for the address to work. NOT TESTED YET.
            Console.WriteLine(address);
            // String address = "https://gitlab.com/api/v4/projects/dit341%2Fexpress-template/repository/tree"; 
            // The part where the request is actually made
            try
            {
                Console.WriteLine("Sending request");
                Task<String> responseTask = client.GetStringAsync(address);
                while (responseTask.IsCompleted != true) { }

                String responseString = responseTask.Result;

                List<HubObject> noPathFolder = makeHubList(responseString);

                foreach (HubObject tree in noPathFolder)
                {
                    FolderNavigator(projectId, tree);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong with HTTP request :Defaultdance");
                Console.WriteLine(e);
                return false;
                //Write stuff incase the git repository was not found. 
            }
            return true;

        }

        private static Boolean getFile(String downloadUrl, String projectId)
        {
            String address = downloadUrl; // It is possible that I'll have to encode it or parts of it for the address to work. NOT TESTED YET.
            Console.WriteLine(address);

            // The part where the request is actually made
            try
            {
                Console.WriteLine("Sending request");
                Task<String> responseTask = client.GetStringAsync(address);
                while (responseTask.IsCompleted == false)
                {
                }
                Console.WriteLine("Request done");
                String responseString = "";
                try
                {
                    responseString = responseTask.Result;
                    Console.WriteLine("Response string has been made");

                }
                catch (AggregateException e)
                {
                    Console.WriteLine(e.Message);
                };

                try
                {
                    String dirPath = Program.mainFolderGetter() + "/Middleware/GitGetter/FileStorer/" + projectId;
                    dirPath = dirPath.Replace(" ", "");

                    Console.WriteLine("Here is where the file should be: "+ dirPath);
                    //File.WriteAllText(dirPath+"/"+ name, responseString); // Creates a seperate file for each code
                    File.AppendAllText(dirPath + Program.getFiletype(), responseString +Environment.NewLine);  // Should create a single file with the contents of all the code files.
                }
                catch (Exception e)
                {
                    Console.WriteLine("Making file went wrong"); Console.WriteLine(e);
                    return false;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong with HTTP request :Defaultdance");
                Console.WriteLine(e);
                return false;
                //Write stuff incase the git repository was not found. 
            }

            return true;

        }

        private static Boolean FolderNavigator(String projectID, HubObject hubObject)
        {
            if (hubObject.type == "dir")
            { // If the object is a folder and potentially has stuff in it
                return getDirTree(hubObject.url, projectID);

            }
            else if (hubObject.type == "file" && hubObject.name.Contains(/*Program.getFiletype()*/ ".txt"))
            { // If the object is a actual file
                return getFile(hubObject.download_url, projectID);
            }
            else{
                return false;
            }



        } 


        private static List<HubObject> makeHubList(String populationMaker)
        {
            String populationString = populationMaker;
            //Console.WriteLine(populationString);

            List<HubObject> tempHubObject = new List<HubObject>();

            JsonConvert.PopulateObject(populationString, tempHubObject);

            return tempHubObject;

        }

    }
}
