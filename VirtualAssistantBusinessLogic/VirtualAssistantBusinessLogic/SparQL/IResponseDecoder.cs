using System.Collections.Generic;
using System.IO;

namespace VirtualAssistantBusinessLogic.SparQL
{
    public interface IResponseDecoder
    {
        Dictionary<string, Dictionary<string, List<string>>> Decode(Stream stream);
    }
}
