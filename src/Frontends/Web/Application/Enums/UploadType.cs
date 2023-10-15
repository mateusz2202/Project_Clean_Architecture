using System.ComponentModel;

namespace BlazorApp.Application.Enums
{
    public enum UploadType : byte
    {
        [Description(@"Images\ProfilePictures")]
        ProfilePicture,
        
        [Description(@"Images\Products")]
        Product,       

        [Description(@"Documents")]
        Document
    }
}