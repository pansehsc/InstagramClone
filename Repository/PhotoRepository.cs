using API.Data;

public class PhotoRepository(AppDbContext context)
    : IPhotoRepository
{
    public void Add(Photo photo)
    {
        context.Photo.Add(photo);
    }

    public void Remove(Photo photo)
    {
        context.Photo.Remove(photo);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}