using Microsoft.Graph;
namespace HFA.Models
{
    public class Files
    {
        public string Name { get; set; }
        public string LastModifiedByUser { get; set; }
        public string DateModifed { get; set; }
        public string FileType { get; set; }

        public static readonly Dictionary<string, string> MimeToFileTypeMap = new Dictionary<string, string>
        {
            { "application/zip","Zip Folder"},
            { "application/pdf","PDF Document"},
            { "application/vnd.openxmlformats-officedocument.wordprocessingml.document","Word Document"},
            { "application/vnd.openxmlformats-officedocument.presentationml.presentation","PowerPoint Slides"},
            { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","Excel Spreadsheet"},
            { "application/octet-stream","URL"}
        };

        private readonly GraphServiceClient _graphServiceClient;
        private string _groupId;
        public Files(GraphServiceClient graphServiceClient, IConfiguration iConfig)
        {
            _graphServiceClient = graphServiceClient;
            _groupId = iConfig.GetValue<string>("CustomConfiguration:GroupId");
        }

        public async Task<List<DriveItem>> GetFilesFromSearchAsync(string searchValue)
        {
            var driveItems = new List<DriveItem>();

            try
            {
                var search = await _graphServiceClient.Groups[_groupId].Drive.Root.Search().Request().GetAsync();


                foreach (var item in search)
                {
                    if (item.Name.ToUpper().Contains(searchValue.ToUpper()))
                    {
                        driveItems.Add(item);

                    }
                }

            }
            catch (ServiceException)
            {
                return null;
            }
            return driveItems;

        }

        public async Task<List<DriveItem>> GetFilesFromFolderAsync(string folderId)
        {
            var driveItems = new List<DriveItem>();

            try
            {
                var children = await _graphServiceClient.Groups[_groupId].Drive.Items[folderId].Children.Request().GetAsync();

                foreach (var item in children)
                {
                    driveItems.Add(item);
                }

            }
            catch (ServiceException)
            {
                return null;
            }

            return driveItems;
        }


        public async Task<DriveItem> GetCurrentFolder(string folderId)
        {

            var currentDrive = new DriveItem();
            try
            {
                currentDrive = await _graphServiceClient.Groups[_groupId].Drive.Items[folderId].Request().GetAsync();
            }
            catch (ServiceException)
            {
                return null;
            }

            return currentDrive;
        }



        public async Task<List<DriveItem>> GetFilesAsync()
        {
            var driveItems = new List<DriveItem>();

            try
            {
                var children = await _graphServiceClient.Groups[_groupId].Drive.Root.Children.Request().GetAsync();

                foreach (var item in children)
                {
                    driveItems.Add(item);
                }
            }
            catch (ServiceException)
            {
                return null;
            }


            return driveItems;
        }

        public static string GetFileType(string mimeType)
        {
            if (MimeToFileTypeMap.TryGetValue(mimeType, out string? fileType))
            {
                return fileType;
            }

            return mimeType;
        }
    }
}





