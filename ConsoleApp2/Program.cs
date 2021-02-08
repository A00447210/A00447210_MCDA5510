using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;
using System.Linq;
using System.Diagnostics;
namespace ConsoleApp2
{
    public class SimpleCSVParser
    {


        public static int goodrow = 0; 
        public static int badrow = 0;
        public static int flag = 0;
        public static int finalgoodrow = 0;
        public static int header_length = 0;
        public static List<string> items = new List<string>();
        public static void Main(String[] args)
               {
                    var watch = new System.Diagnostics.Stopwatch();
                    watch.Start();
            //SimpleCSVParser parser = new SimpleCSVParser();
            //parser.parse(@"\\Mac\Home\Documents\GitHub\MCDA5510_Assignments\Assignment1\Assignment1\sampleFile.csv");
                    DirWalker fw = new DirWalker();
                    fw.walk(@"\\Mac\Home\Downloads\MCDA5510_Assignments\MCDA5510_Assignments\Sample Data");

                    watch.Stop();
            //Console.WriteLine(stopwatch.ElapsedMilliseconds.ToString());
            var log = OpenStream(@"\\Mac\Home\Downloads\output2.txt");
                    log.WriteLine("Number of good rows"+finalgoodrow);
                    log.WriteLine("Number of bad rows" + badrow);
                    log.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

                    log.Close();


        }
        

        public void parse(String fileName)
        {
            var sw = OpenStream(@"\\Mac\Home\Downloads\output2.csv");
            try
            {
                int whilecount = 0;
                using (TextFieldParser parser = new TextFieldParser(fileName))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    parser.HasFieldsEnclosedInQuotes = true;
                    while (!parser.EndOfData)
                    {


                        //Processing row
                        flag = 0;
                        string[] fields = parser.ReadFields();
        

                            foreach (string field in fields)
                        {
                            //to eliminate headers
                            if (whilecount == 0)
                            {
                                whilecount = whilecount + 1;
                                header_length = fields.Length;
                                continue;
                            }

                            if (String.IsNullOrEmpty(field))
                            {
                                badrow = badrow + 1;
                                //Console.WriteLine("Bad one" + field);
                                flag = 1;
                                break;
                            }
                            else 
                            {
                                goodrow = goodrow + 1;
                                
                                //Console.WriteLine(goodrow + "good row count");
                            }

                        }
                        //these are the good rows
                        
  
                        if (goodrow == header_length)
                        {
                            //Console.WriteLine(fields.ToArray());
                            foreach (string field in fields)
                            {
                                //Console.Write(field + ",");
                                sw.Write(field+",");
                            }
                            //Console.WriteLine();
                            sw.WriteLine();
                            finalgoodrow = finalgoodrow + 1;

                        }
                        
                        
                        goodrow = 0;
                        

                    }
                        
    
                    }
                sw.Close();

            }

            
            catch (IOException ioe)
            {
                Console.WriteLine(ioe.StackTrace);
            }

        }
        static StreamWriter OpenStream(string path)
        {
            if (path is null)
            {
                Console.WriteLine("You did not supply a file path.");
                return null;
            }

            try
            {

                    var fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                return new StreamWriter(fs);

            }
            catch (FileNotFoundException)
            {


                Console.WriteLine("The file or directory cannot be found.");

            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("The file or directory cannot be found.");
            }
            catch (DriveNotFoundException)
            {
                Console.WriteLine("The drive specified in 'path' is invalid.");
            }
            catch (PathTooLongException)
            {
                Console.WriteLine("'path' exceeds the maxium supported path length.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("You do not have permission to create this file.");
            }
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32)
            {
                Console.WriteLine("There is a sharing violation.");
            }
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 80)
            {
                Console.WriteLine("The file already exists.");
            }
            catch (IOException e)
            {
                Console.WriteLine($"An exception occurred:\nError code: " +
                                  $"{e.HResult & 0x0000FFFF}\nMessage: {e.Message}");
            }
            return null;
        }
    }

    public class DirWalker
    {

        public void walk(String path)
        {
            try
            {


                string[] list = Directory.GetDirectories(path);


                if (list == null) return;

                foreach (string dirpath in list)
                // foreach (String f : list)
                {
                    if (Directory.Exists(dirpath))
                    {
                        walk(dirpath);
                        //Console.WriteLine("Dir:" + dirpath);
                    }
                }
                string[] fileList = Directory.GetFiles(path);
                foreach (string filepath in fileList)
                {
                    if (filepath.Contains(".csv") == true)
                    {
                        //Console.WriteLine("File:" + filepath + "\n");
                        SimpleCSVParser parser = new SimpleCSVParser();
                        //parser.parse(@"\\Mac\Home\Downloads\MCDA5510_Assignments\MCDA5510_Assignments\Assignment1\Assignment1\sampleFile.csv");
                        parser.parse(filepath);
                    }


                }
            }
            catch (FileNotFoundException)
            {


                Console.WriteLine("The file or directory cannot be found.");

            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("The file or directory cannot be found.");
            }
            catch (DriveNotFoundException)
            {
                Console.WriteLine("The drive specified in 'path' is invalid.");
            }
            catch (PathTooLongException)
            {
                Console.WriteLine("'path' exceeds the maxium supported path length.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("You do not have permission to create this file.");
            }
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32)
            {
                Console.WriteLine("There is a sharing violation.");
            }
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 80)
            {
                Console.WriteLine("The file already exists.");
            }
            catch (IOException e)
            {
                Console.WriteLine($"An exception occurred:\nError code: " +
                                  $"{e.HResult & 0x0000FFFF}\nMessage: {e.Message}");
            }

        }


    }


}


