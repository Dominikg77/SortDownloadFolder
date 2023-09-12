namespace SortDownloadFolder
{
    internal class Program
    {

        public const string DOWNLOAD_PATH = @"C:\Users\domin\Downloads";
        static void Main(string[] args)
        {
            var filesInDirs = Directory.GetFiles(DOWNLOAD_PATH);

            // Dictionary, um Erweiterungen und zugehörige Ordner zu speichern
            Dictionary<string, string> extensionFolders = new Dictionary<string, string>
    {
        { ".pdf", "documents" },
        { ".txt", "documents" },
        { ".doc", "documents" },
        { ".xml", "documents" },
        { ".jpg", "images" },
        { ".png", "images" },
        { ".gif", "images" }
    };

            foreach (var file in filesInDirs)
            {
                string fileExtension = Path.GetExtension(file);
                string fileName = Path.GetFileName(file); // Den Dateinamen extrahieren

                if (extensionFolders.ContainsKey(fileExtension))
                {
                    string folderName = extensionFolders[fileExtension];
                    string extensionFolder = Path.Combine(DOWNLOAD_PATH, folderName);
                    string extensionSubfolder = Path.Combine(extensionFolder, fileExtension.TrimStart('.')); // Unterordner für die Dateiendung
                    string destinationFilePath = Path.Combine(extensionSubfolder, fileName);

                    if (Directory.Exists(extensionSubfolder))
                    {
                        Console.WriteLine("Unterordner existiert bereits: " + extensionSubfolder);
                        File.Move(file, destinationFilePath); // Datei in den Unterordner verschieben
                    }
                    else
                    {
                        Directory.CreateDirectory(extensionSubfolder);
                        File.Move(file, destinationFilePath); // Datei in den neu erstellten Unterordner verschieben
                        Console.WriteLine("Unterordner erstellt: " + extensionSubfolder);
                    }
                }
                else
                {
                    // Wenn die Erweiterung nicht in extensionFolders enthalten ist, verschiebe die Datei in den "other"-Ordner
                    string otherFolder = Path.Combine(DOWNLOAD_PATH, "other");
                    string extensionSubfolder = Path.Combine(otherFolder, fileExtension.TrimStart('.'));
                    string destinationFilePath = Path.Combine(extensionSubfolder, fileName);

                    if (!Directory.Exists(extensionSubfolder))
                    {
                        Directory.CreateDirectory(extensionSubfolder);
                    }

                    File.Move(file, destinationFilePath); // Datei in den Unterordner mit der Erweiterung verschieben
                    Console.WriteLine("Datei in den 'other'-Ordner verschoben: " + fileName);
                }
            }

            // Lösche alle leeren Ordner rekursiv
            DeleteEmptyFolders(DOWNLOAD_PATH);
        }

        // Methode zum Löschen der leeren Ordner
        static void DeleteEmptyFolders(string path)
        {
            foreach (var directory in Directory.GetDirectories(path))
            {
                DeleteEmptyFolders(directory);

                if (Directory.GetFiles(directory).Length == 0 && Directory.GetDirectories(directory).Length == 0)
                {
                    Directory.Delete(directory); // Lösche den leeren Ordner
                    Console.WriteLine("Ordner gelöscht: " + directory);
                }
            }
        }
    }
}
