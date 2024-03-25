using VeloBrawl.Titan.DataStream;

namespace VeloBrawl.Logic.Environment.LaserMessage.Sepo.Home.Laser.Laser;

public class LogicQuests(List<QuestData> questDataList)
{
    public void Encode(ByteStream byteStream)
    {
        byteStream.WriteVInt(questDataList.Count);
        {
            if (questDataList.Count <= 0) return;

            foreach (var questData in questDataList) questData.Encode(byteStream);
        }
    }
}