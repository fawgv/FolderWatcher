using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FolderWatcher
{
    class Program
    {
        public static void Main()
        {
            Run(); 
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void Run()
        {
            string[] args = System.Environment.GetCommandLineArgs();

            //// If a directory is not specified, exit program.
            //if (args.Length != 2)
            //{
            //    // Display the proper way to call the program.
            //    Console.WriteLine("Usage: Watcher.exe (directory)");
            //    return;
            //}

            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            //watcher.Path = args[1];
            watcher.Path = Properties.Resources.watcherPath;
            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            watcher.Filter = "*.mp4";

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);
            //watcher.WaitForChanged(WatcherChangeTypes.Created);
            watcher.IncludeSubdirectories = true;

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            // Wait for the user to quit the program.
            Console.WriteLine("Press \'q\' to quit the sample.");
            while (Console.Read() != 'q') ;
        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            string filePath = e.FullPath;
            bool notreadyToRemove = true;
            while (notreadyToRemove)
            {
                try
                {
                    using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        //Console.WriteLine("файл свободен");
                        ////Thread.Sleep(10000);
                        ////Console.WriteLine("файл свободен1");
                        //notreadyToRemove = false;
                    }
                }
                //catch (IOException ioex)
                //{
                //    Console.WriteLine("файл занят");
                //}
                catch (Exception)
                {
                    try
                    {
                        using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                        {
                            //Thread.Sleep(1000);
                            Console.WriteLine("файл свободен");
                            //Thread.Sleep(10000);
                            //Console.WriteLine("файл свободен1");
                            notreadyToRemove = false;
                        }
                    }
                    catch (IOException ioex)
                    {
                        //Console.WriteLine("файл занят");
                    }
                    catch (Exception)
                    {
                        //Console.WriteLine("файл занят");
                    }
                }
            }
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }

        private void CreateFileUsersAD()
        {
            try
            {
                ProcessStartInfo runRobocopy = new ProcessStartInfo();
                runRobocopy.FileName = "cmd.exe";
                //string fileName = System.IO.Path.Combine(System.IO.Directory..{ipaddr}List.txt");
                string pathVideoSource = Properties.Resources.watcherPath;
                //string nbtscan = System.IO.Path.Combine(Environment.CurrentDirectory, @"SupportTools\nbtscan.exe");
                //if (File.Exists(fileName))
                //    File.Delete(fileName);

                runRobocopy.Arguments = $"/c start /b robocopy \"{Properties.Resources.watcherPath}\" {Properties.Resources.destinationFolder}%COMPUTERNAME% *.mp4 /MOT:1";

                //Process.Start(runNBTScan);
                Process proc = new Process();
                proc.StartInfo = runRobocopy;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc.Start();
            }
            catch (Exception ex)
            {
                //Bindings.StatusBarText = ex.Message;
            }
        }
    }
}
