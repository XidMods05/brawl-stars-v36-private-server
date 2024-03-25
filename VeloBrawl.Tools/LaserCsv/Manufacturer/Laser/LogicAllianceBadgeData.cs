using VeloBrawl.Tools.LaserCsv.Data;
using VeloBrawl.Tools.LaserCsv.Portal;

namespace VeloBrawl.Tools.LaserCsv.Manufacturer.Laser;

public class LogicAllianceBadgeData(CsvRow row, LogicDataTable table) : LogicData(row, table)
{
    private string _category = null!;
    private string _iconSwf = null!;

    // LogicAllianceBadgeData.

    public override void CreateReferences()
    {
        _iconSwf = GetValue("IconSWF", 0);
        _category = GetValue("Category", 0);
    }

    public string GetIconSwf()
    {
        return _iconSwf;
    }

    public string GetCategory()
    {
        return _category;
    }
}