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
                    DirWalker fw = new DirWalker();
                    fw.walk(@"\\Mac\Home\Downloads\MCDA5510_Assignments\MCDA5510_Assignments\Sample Data");
                    watch.Stop();
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
                            //to skip headers
                            if (whilecount == 0)
                            {
                                whilecount = whilecount + 1;
                                header_length = fields.Length;//take a note of the header length which will be useful later
                                continue;
                            }
                            //check if the value of field is empty
                            if (String.IsNullOrEmpty(field))
                            {
                                badrow = badrow + 1;
                                //Console.WriteLine("Bad one" + field);
                                //flag = 1; //Initially i thought to use flag concept, but not relvant. This line can be commented as its not used
                                break;//immedietely break to save computational speed
                            }
                            else 
                            {
                                goodrow = goodrow + 1;
                                //keep a count of the good column values. Please dont get confused with good row. Here goodrow means good columns count
                                //Console.WriteLine(goodrow + "good row count");
                            }

                        }
                        //these are the good rows
                        
                        //check if all the columns are parsed which is equal to the column header length
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
                            finalgoodrow = finalgoodrow + 1;//here i'm calculating the nubmer of good rows

                        }
                        
                        
                        goodrow = 0;
                        //this is the good column counter for each row which needs to be reset. My goodrow counter is above mentioned as finalgoodrow

                    }
                        
    
                    }
                sw.Close();

            }

            
            catch (IOException ioe)
            {
                Console.WriteLine(ioe.StackTrace);
            }

        }


        //Streamwriter to append data into csv
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


    //this is the class to traverse through each directory
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
 
                        SimpleCSVParser parser = new SimpleCSVParser();
           
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


