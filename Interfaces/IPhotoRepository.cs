public interface IPhotoRepository
{
    void Add(Photo photo);
    void Remove(Photo photo);
    Task<bool> SaveAllAsync();
}