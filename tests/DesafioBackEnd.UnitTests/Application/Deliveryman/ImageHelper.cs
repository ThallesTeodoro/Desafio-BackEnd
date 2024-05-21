using Microsoft.AspNetCore.Http;

namespace DesafioBackEnd.UnitTests.Application.Deliveryman;

public static class ImageHelper
{
    public static IFormFile CreateImage()
    {
         //Setup mock file using a memory stream
        var fileName = "test.jpg";
        var stream = new MemoryStream();
        stream.Position = 0;

        //create FormFile with desired data
        return new FormFile(stream, 0, stream.Length, "id_from_form", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/jpeg"
        };
    }
}
