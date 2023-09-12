namespace SortDownloadFolder
{
    internal class Program
    {

        public const string DOWNLOAD_PATH = @"C:\Users\domin\Downloads";
        static void Main(string[] args)
        {
            // Alle Dateien im Download-Ordner abrufen
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
        { ".gif", "images" },
        { ".mp3", "media" },
        { ".mp4", "media" }

    };

            // Schleife über alle Dateien
            foreach (var file in filesInDirs)
            {
                // Dateierweiterung und Dateiname extrahieren
                string fileExtension = Path.GetExtension(file);
                string fileName = Path.GetFileName(file);

                // Überprüfen, ob die Dateierweiterung in unserem Dictionary ist
                if (extensionFolders.ContainsKey(fileExtension))
                {
                    // Ordnername basierend auf der Erweiterung holen
                    string folderName = extensionFolders[fileExtension];
                    // Pfad zum Hauptordner erstellen
                    string extensionFolder = Path.Combine(DOWNLOAD_PATH, folderName);
                    // Pfad zum Unterordner für die Dateiendung erstellen
                    string extensionSubfolder = Path.Combine(extensionFolder, fileExtension.TrimStart('.'));
                    // Zielpfad für die Datei erstellen
                    string destinationFilePath = Path.Combine(extensionSubfolder, fileName);

                    // Überprüfen, ob der Unterordner existiert
                    if (Directory.Exists(extensionSubfolder))
                    {
                        Console.WriteLine("Unterordner existiert bereits: " + extensionSubfolder);
                        // Datei in den Unterordner verschieben
                        File.Move(file, destinationFilePath);
                    }
                    else
                    {
                        // Den Unterordner erstellen und die Datei verschieben
                        Directory.CreateDirectory(extensionSubfolder);
                        File.Move(file, destinationFilePath);
                        Console.WriteLine("Unterordner erstellt: " + extensionSubfolder);
                    }
                }
                else
                {
                    // Wenn die Erweiterung nicht in extensionFolders ist
                    // Die Datei in den "other"-Ordner verschieben
                    string otherFolder = Path.Combine(DOWNLOAD_PATH, "other");
                    string extensionSubfolder = Path.Combine(otherFolder, fileExtension.TrimStart('.'));
                    string destinationFilePath = Path.Combine(extensionSubfolder, fileName);

                    // Überprüfen, ob der Unterordner existiert und wenn nicht, erstellen
                    if (!Directory.Exists(extensionSubfolder))
                    {
                        Directory.CreateDirectory(extensionSubfolder);
                    }

                    // Datei in den Unterordner mit der Erweiterung verschieben
                    File.Move(file, destinationFilePath);
                    Console.WriteLine("Datei in den 'other'-Ordner verschoben: " + fileName);
                }
            }

            // Nachdem alle Dateien verschoben wurden, leere Ordner löschen
            DeleteEmptyFolders(DOWNLOAD_PATH);
        }

        // Methode zum Löschen leerer Ordner
        static void DeleteEmptyFolders(string path)
        {
            foreach (var directory in Directory.GetDirectories(path))
            {
                // Rekursiv durch alle Ordner gehen
                DeleteEmptyFolders(directory);

                // Überprüfen, ob der Ordner leer ist
                if (Directory.GetFiles(directory).Length == 0 && Directory.GetDirectories(directory).Length == 0)
                {
                    // Den leeren Ordner löschen
                    Directory.Delete(directory);
                    Console.WriteLine("Ordner gelöscht: " + directory);
                }
            }
        }
    }
}
