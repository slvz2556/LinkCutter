using LinkCutter.Components.LogicAndModel;
using Microsoft.AspNetCore.Components.Web;
using QRCoder;

namespace LinkCutter.Components.Pages;

public partial class Home
{
    List<LinkModel> Models = new List<LinkModel>();

    DataBaseContext db = new DataBaseContext();

    string search = "";

    protected override void OnInitialized()
    {
        Models = db.Get().OrderByDescending(x => x.Date).ToList();
    }

    //Set data
    private void SetData(string id)
    {
        db.Update(Models.Find(x => x.ID == id));        
    }

    //Delete link
    private void DeleteLink(string id)
    {
        db.Remove(id);

        var mdl = Models.Find(x => x.ID == id);
        Models.Remove(mdl);
        if (File.Exists(Path.Combine(AppContext.BaseDirectory, "wwwroot", "Images", mdl.Qr)))
            File.Delete(Path.Combine(AppContext.BaseDirectory, "wwwroot", "Images", mdl.Qr));

        StateHasChanged();
    }


    private void CreateNew()
    {
        var ml = new LinkModel();

        //Create new id
        ml.ID = IDGenerator.CreateShortLinkID();

        //Set date time
        ml.Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm");

        //Create direcorties if they are not exist
        Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "wwwroot")!);
        Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "wwwroot", "Images")!);

        //Find a random filename that hasn't been used
        string filename;
        do filename = Path.GetRandomFileName();
        while (File.Exists(Path.Combine(AppContext.BaseDirectory, "wwwroot", "Images", $"{filename}.png")));

        //Generate QRcode
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode($"{Navigation.BaseUri}r/{ml.ID}", QRCodeGenerator.ECCLevel.Q);
        var qrCode = new QRCoder.PngByteQRCode(qrCodeData);

        //Save image
        byte[] image = qrCode.GetGraphic(20);
        File.WriteAllBytes(Path.Combine(AppContext.BaseDirectory, "wwwroot", "Images", $"{filename}.png"), image);

        ml.Qr = $"{filename}.png";

        //Add model to database
        db.Append(ml);
        
        //Insert model into models
        Models.Insert(0, ml);

        //Rerender the scene
        StateHasChanged();
    }


    private void OnEnter(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
            StateHasChanged();
    }
}
