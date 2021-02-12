using System;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;
namespace Asnmnt01
{
    class Program
    {
        public static int goodrow = 0;
        public static int badrow = 0;
        public static int flag = 0;
        public static int finalgoodrow = 0;
        public static int header_length = 0;
        public static void Main(string[] args)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            DirWalker fw = new DirWalker();
            fw.walk(@"\\Mac\Home\Downloads\MCDA5510_Assignments\MCDA5510_Assignments\Sample Data");
            watch.Stop();
            var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
            log.WriteLine("Number of good rows" + finalgoodrow);
            log.WriteLine("Number of bad rows" + badrow);
            log.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

            log.Close();
        }


        public  void Logic(String filename)
        {
            
            string fileContents = File.ReadAllText(filename);
            var sw = OpenStream(@"\\Mac\Home\Documents\jericho.csv");
            int i = -1;
            try
            {
                foreach (string lines in fileContents.Split("\n"))
                {
                    i = i + 1;
                    //string[] result = Regex.Split((fileContents.Split("\n")[i]), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                    if (i == 0 || i == fileContents.Split("\n").Length - 1)
                    {
                        //skipping the header row
                        header_length = fileContents.Split("\n")[0].Split(",").Length;
                        continue;
                    }
                    else
                    {
                        flag = 0;

                        string[] result = Regex.Split((lines), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                        foreach (string field in result)
                        {
                            //main login is to check if each field is empty or has invertd commas  or check if they have NON ASCII Characters
                            //if (String.IsNullOrEmpty(field) == true || field=="\"\"" || Regex.IsMatch(field, @".*[^\u0000-\u007F].*") == true)
                            if (String.IsNullOrEmpty(field) == true || field == "\"\"" )
                            {
                                badrow = badrow + 1;
                                //Console.WriteLine("Bad one" + lines);
                                flag = 1;
                   
                                break;//immedietely break to save computational speed
                            }
                            else
                            {
                                goodrow = goodrow + 1;
                            }
           
          

                        }
                        //these are the good rows

                        //check if all the columns are parsed and if flag is zero, they are never flagged and are good rows
                      
                        if (goodrow==header_length)
                        {
                            //Console.WriteLine(fields.ToArray());

                            //Console.Write(field + ",");
                            sw.WriteLine(lines);

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
                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                log.WriteLine(ioe.StackTrace);
                log.Close();
            }
        }
        
    //Streamwriter to append data into csv
    static StreamWriter OpenStream(string path)
    {
        if (path is null)
        {


             var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
             log.WriteLine("You did not supply a file path.");
             log.Close();
                return null;
        }

        try
        {

            var fs = new FileStream(path, FileMode.Append, FileAccess.Write);
            return new StreamWriter(fs);

        }
        catch (FileNotFoundException)
        {

                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                Console.WriteLine("The file or directory cannot be found.");
                log.Close();

        }
        catch (DirectoryNotFoundException)
        {
                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                log.WriteLine("The file or directory cannot be found.");
                log.Close();
        }
        catch (DriveNotFoundException)
        {
                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                log.WriteLine("The drive specified in 'path' is invalid.");
                log.Close();
        }
        catch (PathTooLongException)
        {
                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                log.WriteLine("'path' exceeds the maxium supported path length.");
                log.Close();
        }
        catch (UnauthorizedAccessException)
        {
                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                log.WriteLine("You do not have permission to create this file.");
                log.Close();
        }
        catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32)
        {
                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                log.WriteLine("There is a sharing violation.");
                log.Close();
        }
        catch (IOException e) when ((e.HResult & 0x0000FFFF) == 80)
        {
                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                log.WriteLine("The file already exists.");
                log.Close();
        }
        catch (IOException e)
        {
                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                log.WriteLine($"An exception occurred:\nError code: " +
                              $"{e.HResult & 0x0000FFFF}\nMessage: {e.Message}");
                log.Close();
            }
        return null;
    }
}


    //this is the class which traverses each directory
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

                        Program parser = new Program();

                        parser.Logic(filepath);
                    }


                }
            }
            catch (FileNotFoundException)
            {

                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");

                Console.WriteLine("The file or directory cannot be found.");
                log.Close();

            }
            catch (DirectoryNotFoundException)
            {
                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                Console.WriteLine("The file or directory cannot be found.");
                log.Close();
            }
            catch (DriveNotFoundException)
            {
                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                Console.WriteLine("The drive specified in 'path' is invalid.");
                log.Close();
            }
            catch (PathTooLongException)
            {
                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                Console.WriteLine("'path' exceeds the maxium supported path length.");
                log.Close();
            }
            catch (UnauthorizedAccessException)
            {
                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                Console.WriteLine("You do not have permission to create this file.");
                log.Close();
            }
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32)
            {
                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                Console.WriteLine("There is a sharing violation.");
                log.Close();
            }
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 80)
            {
                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                Console.WriteLine("The file already exists.");
                log.Close();
            }
            catch (IOException e)
            {
                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                Console.WriteLine($"An exception occurred:\nError code: " +
                                  $"{e.HResult & 0x0000FFFF}\nMessage: {e.Message}");
                log.Close();
            }

        }
        static StreamWriter OpenStream(string path)
        {
            if (path is null)
            {


                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                log.WriteLine("You did not supply a file path.");
                log.Close();
                return null;
            }

            try
            {

                var fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                return new StreamWriter(fs);

            }
            catch (FileNotFoundException)
            {

                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                Console.WriteLine("The file or directory cannot be found.");
                log.Close();

            }
            catch (DirectoryNotFoundException)
            {
                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                log.WriteLine("The file or directory cannot be found.");
                log.Close();
            }
            catch (DriveNotFoundException)
            {
                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                log.WriteLine("The drive specified in 'path' is invalid.");
                log.Close();
            }
            catch (PathTooLongException)
            {
                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                log.WriteLine("'path' exceeds the maxium supported path length.");
                log.Close();
            }
            catch (UnauthorizedAccessException)
            {
                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                log.WriteLine("You do not have permission to create this file.");
                log.Close();
            }
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32)
            {
                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                log.WriteLine("There is a sharing violation.");
                log.Close();
            }
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 80)
            {
                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                log.WriteLine("The file already exists.");
                log.Close();
            }
            catch (IOException e)
            {
                var log = OpenStream(@"\\Mac\Home\Documents\output2.txt");
                log.WriteLine($"An exception occurred:\nError code: " +
                              $"{e.HResult & 0x0000FFFF}\nMessage: {e.Message}");
                log.Close();
            }
            return null;
        }


    }


}




    

