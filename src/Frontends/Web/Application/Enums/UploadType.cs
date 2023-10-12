using System.ComponentModel;

namespace BlazorHero.CleanArchitecture.Application.Enums
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