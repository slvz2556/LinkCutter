using SLVZDB;

namespace LinkCutter.Components.LogicAndModel;

public class DataBaseContext : DbContext<LinkModel>
{
    public override void Configuration()
    {
        SetConfig(Path.Combine(AppContext.BaseDirectory,"DB.slvz"), "ID");
    }
}
