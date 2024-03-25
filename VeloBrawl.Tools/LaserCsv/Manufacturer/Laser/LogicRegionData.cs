using VeloBrawl.Tools.LaserCsv.Data;
using VeloBrawl.Tools.LaserCsv.Portal;

namespace VeloBrawl.Tools.LaserCsv.Manufacturer.Laser;

public class LogicRegionData(CsvRow row, LogicDataTable table) : LogicData(row, table)
{
    private string _displayName = null!;
    private bool _isCountry;

    // LogicRegionData.

    public override void CreateReferences()
    {
        _displayName = GetValue("DisplayName", 0);
        _isCountry = GetBooleanValue("IsCountry", 0);
    }

    public string GetDisplayName()
    {
        return _displayName;
    }

    public bool GetIsCountry()
    {
        return _isCountry;
    }
}