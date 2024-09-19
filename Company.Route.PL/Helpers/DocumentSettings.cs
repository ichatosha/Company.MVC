namespace Company.Route.PL.Helpers
{
    public class DocumentSettings
    {



        // Upload 
        public static string UploadFile(IFormFile file , string fileName)
        {
            // 1. Get Location Folder Path
            //string folderPath = $"C:\\Users\\HESHAM\\source\\repos\\Company.Route.Solution\\Company.Route.PL\\wwwroot\\Media\\{fileName}";
            // GetCurrentDirectory == C:\\Users\\HESHAM\\source\\repos\\Company.Route.Solution\\Company.Route.PL\
            string folderPath = Path.Combine(Directory.GetCurrentDirectory() + @"\wwwroot\Media\\" + fileName);

            // 2. Get File Name Make it Unique
            string fileNameGuid = $"{Guid.NewGuid()}{file.FileName}";

            // 3. Ge File Path --> FolderPath + FileName 
            string filePath = Path.Combine(folderPath, fileNameGuid);

            // 4. Save File as Stream : Data Per Time 
            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream);
            return fileNameGuid;
        }


        // Delete

        public static void DeleteFile(string fileName , string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory() + @"\wwwroot\Media\\" + folderName + "\\" + fileName);

            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }
           

        }



    }
}
