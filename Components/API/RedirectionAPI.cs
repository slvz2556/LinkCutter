using LinkCutter.Components.LogicAndModel;
using Microsoft.AspNetCore.Mvc;

namespace LinkCutter.Components.API;

public class RedirectionAPI : ControllerBase
{
    [Route("/r/{id}")]
    public IActionResult Redirection(string id)
    {
        try
        {
            DataBaseContext db = new DataBaseContext();

            var model = db.Get(id);
            if (model is not null)
            {
                model.Seen++;
                db.Update(model);
                return Redirect(model.Url);
            }
            else
                return Redirect("https://slvz.dev");
        }
        catch
        {
            return Redirect("https://slvz.dev");
        }
    }
}
