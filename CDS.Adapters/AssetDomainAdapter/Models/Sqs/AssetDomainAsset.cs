using CDS.Adapters.Interfaces;

namespace CDS.Adapters.AssetDomainAdapter.Models.Sqs;

public class AssetDomainAsset : IMessage {
    public string AssetId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string FileFormat { get; set; }
    public string FileSize { get; set; }
    public string Path { get; set; }

    string IMessage.Id() {
        return AssetId;
    }
}