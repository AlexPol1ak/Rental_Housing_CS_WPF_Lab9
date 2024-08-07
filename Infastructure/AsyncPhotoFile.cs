
using CS_WPF_Lab9_Rental_Housing.Domain.Entities;
using System.IO;

namespace CS_WPF_Lab9_Rental_Housing.Infastructure
{
    /// <summary>
    /// A class of asynchronous operations on photo files.
    /// </summary>
    public static class AsyncPhotoFile
    {
        public static readonly string PhotosDir =
            Path.Combine(Directory.GetCurrentDirectory(), "Images");

        /// <summary>
        /// Asynchronously deletes photo files from the directory.
        /// </summary>
        /// <param name="photos">Photos collection</param>
        static public async Task<bool> DeletePhotoFileAsync(IEnumerable<Photo> photos)
        {
            bool result = false;

            List<Task> tasks = new();
            if (photos != null && photos.Count() > 0)
            {
                foreach (Photo photo in photos)
                {
                    FileInfo photoFile = new FileInfo(Path.Combine(PhotosDir, photo.PhotoName));
                    if (photoFile.Exists)
                    {
                        Task task = Task.Run(() => photoFile.Delete());
                        tasks.Add(task);
                        result = true;
                    }
                }
                await Task.WhenAll(tasks);
            }
            return result;
        }

        /// <summary>
        /// Asynchronously deletes apartment photo files from the directory.
        /// </summary>
        static public async Task<bool> DeletePhotoFileAsync(Apartment apartment)
        {
            bool result = false;
            if (apartment != null && apartment.Photos != null)
            {
                result = await DeletePhotoFileAsync(apartment.Photos);
            }
            return result;
        }

        /// <summary>
        /// Asynchronously deletes photo files of all apartments in the transferred building.
        /// </summary>
        /// <param name="house">House object</param>
        static public async Task<bool> DeletePhotoFileAsync(House house)
        {
            bool result = false;

            List<Task<bool>> tasks = new List<Task<bool>>();
            if (house != null && house.Apartments != null)
            {
                foreach (Apartment ap in house.Apartments)
                {
                    var task = DeletePhotoFileAsync(ap);
                    tasks.Add(task);
                }
                bool[] tasksResult = await Task.WhenAll(tasks);
                result = tasksResult.Any();
            }
            return result;
        }
    }
}
