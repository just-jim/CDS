using CDS.Application.Common.Interfaces.Models;

namespace CDS.Application.Common.Models;

public class BriefingDomainBriefing : IMessage{
    public string Name { get; set; }
    public string Description { get; set; }
    public string CreatedBy { get; set; }
    public string CreatedDate { get; set; }
    
    string IMessage.Id() {
        return Name;
    }
}