using System;
using System.IO;
using System.IO.Compression;

namespace GenCSPRO
{
    class Project
    {
        public string project;
        public string file;
        public string directory;

        public Project()
        { }
    }

    static class Program
    {
        #region Files
        static Project name = new Project();
        static Project sln = new Project();
        static Project csproj = new Project();
        static Project appConfig = new Project();
        static Project assemblyInfo = new Project();
        #endregion

        #region Directories
        static Project solution = new Project();
        static Project project = new Project();
        static Project bin = new Project();
        static Project obj = new Project();
        static Project properties = new Project();
        #endregion

        static void Main(string[] args)
        {
            Console.Write("Name your project: ");
            name.project = Console.ReadLine();

            ReadFiles();
            CreateDirectories();
            CreateFiles();
            CompressDependencies();

        }

        static void ReadFiles()
        {
            using (FileStream streamSln = new FileStream(@"./GenCSPRO.sln", FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(streamSln))
                {
                    string data = reader.ReadToEnd();
                    sln.file = data.Replace("GenCSPRO", name.project);
                }
            }

            using (FileStream streamCsproj = new FileStream(@"./GenCSPRO/GenCSPRO.csproj", FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(streamCsproj))
                {
                    string data = reader.ReadToEnd();
                    csproj.file = data.Replace("GenCSPRO", name.project);
                }
            }

            using (FileStream streamAppConfig = new FileStream(@"./GenCSPRO/App.config", FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(streamAppConfig))
                {
                    appConfig.file = reader.ReadToEnd();
                }
            }

            using (FileStream streamAssemblyInfo = new FileStream(@"./GenCSPRO/Properties/AssemblyInfo.cs", FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(streamAssemblyInfo))
                {
                    string data = reader.ReadToEnd();
                    assemblyInfo.file = data.Replace("GenCSPRO", name.project);
                }
            }
        }

        static void CreateDirectories()
        {
            solution.directory = $@"./{name.project}";
            project.directory = $@"{solution.directory}/{name.project}";
            bin.directory = $@"{project.directory}/bin";
            obj.directory = $@"{project.directory}/obj";
            properties.directory = $@"{project.directory}/Properties";

            Directory.CreateDirectory(solution.directory);
            Directory.CreateDirectory(project.directory);
            Directory.CreateDirectory(bin.directory);
            Directory.CreateDirectory(obj.directory);
            Directory.CreateDirectory(properties.directory);
        }

        static void CreateFiles()
        {
            using (FileStream streamSln = new FileStream($@"{solution.directory}/{name.project}.sln", FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(streamSln))
                {
                    writer.Write(sln.file);
                }
            }

            using (FileStream streamCsproj = new FileStream($@"{project.directory}/{name.project}.csproj", FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(streamCsproj))
                {
                    writer.Write(csproj.file);
                }
            }

            using (FileStream streamAppConfig = new FileStream($@"{project.directory}/App.config", FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(streamAppConfig))
                {
                    writer.Write(appConfig.file);
                }
            }

            using (FileStream streamAssemblyInfo = new FileStream($@"{properties.directory}/AssemblyInfo.cs", FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(streamAssemblyInfo))
                {
                    writer.Write(assemblyInfo.file);
                }
            }
        }

        private static void CompressDependencies()
        {
            string compressPath = $@"./{name.project}";
            string zipArchive = $@"./{name.project}.zip";

            try
            {
                ZipFile.CreateFromDirectory(compressPath, zipArchive);
                Console.WriteLine("Project {0} succefully created!", name.project);
            }
            catch (IOException e)
            {
                Console.WriteLine("Error! Project {0} already exists! Message = {1}", name.project, e.Message);
            }
            finally
            {
                Console.Write("Press Enter to exit...");
                Console.ReadKey();
            }
        }
    }
}
